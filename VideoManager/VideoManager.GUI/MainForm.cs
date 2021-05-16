using AutoMapper;
using Domain.Data;
using Domain.Models;
using Domain.Video;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        private IReadOnlyList<PublicationModel> _metadataSource;

        public MainForm(ILogger<MainForm> logger, IMapper mapper, IDataAccessorService dataService, IVideoService videoService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));

            InitializeComponent();

            Text = $"Video Manager - {GetType().Assembly.GetName().Version}";
        }

        private async void GetAirtableDataBtn_Click(object sender, EventArgs e)
        {
            IReadOnlyList<PublicationModel> records = await _dataService.GetRecords(10);

            _metadataSource = records;

            VideoDGV.DataSource = records;
        }

        private async void UpdateVideosBtn_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in VideoDGV.SelectedRows)
            {
                await _videoService.UpdateVideoMetadata(row.DataBoundItem as VideoMetadataModel, CancellationToken.None);
            }
        }

        private void UpdateLiveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    _videoService.UpdateVideoMetadata(model.LiveVideo, CancellationToken.None);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("¯\\(°_o)/¯ Oups, quelque chose s'est mal passé pendant la mise à jour du live.", "Echec mise à jour du live");
                _logger.LogError(ex, "Erreur pendant la mise à jour du live");
            }
        }

        private async void UpdateAirtableForLiveDataBtn_Click(object sender, EventArgs e)
        {
            VideoMetadataModel res = await _videoService.GetUpcomingLiveAsync(CancellationToken.None);

            if (res != null && !string.IsNullOrEmpty(res.Identifier))
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    model.LiveVideo.Identifier = res.Identifier;
                    model.LiveVideo.VideoUrl = res.VideoUrl;

                    if (await _dataService.UpdateLiveVideoRecord(model.Id, model.LiveVideo))
                    {
                        MessageBox.Show("Airtable a été mis à jour");
                    }
                    else
                    {
                        MessageBox.Show("Problème pendant la mise à jour de Airtable");

                    }
                }
            }
            else
            {
                MessageBox.Show("¯\\(°_o)/¯ Attention, vidéo du live non trouvée. Airtable ne sera pas mis à jour.", "Live non trouvé");
            }
        }
    }
}
