using AutoMapper;
using Domain.Data;
using Domain.Models;
using Domain.Video;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace VideoManager.GUI
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly IMapper _mapper;
        private readonly IDataAccessorService _dataService;
        private readonly IVideoService _videoService;

        private IList<VideoMetadataModel> _metadataSource;

        public MainForm(ILogger<MainForm> logger, IMapper mapper, IDataAccessorService dataService, IVideoService videoService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
            InitializeComponent();
        }

        private async void GetAirtableDataBtn_Click(object sender, EventArgs e)
        {
            IReadOnlyList<RecordModel> records = await _dataService.GetRecords(10);

            _metadataSource = _mapper.ProjectTo<VideoMetadataModel>(records.AsQueryable()).ToList();

            VideoDGV.DataSource = _metadataSource;
        }

        private async void UpdateVideosBtn_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in VideoDGV.SelectedRows)
            {
                await _videoService.UpdateVideoMetadata(row.DataBoundItem as VideoMetadataModel, CancellationToken.None);
            }
        }
    }
}
