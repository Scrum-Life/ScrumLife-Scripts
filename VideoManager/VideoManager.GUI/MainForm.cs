using AutoMapper;
using Domain.Data;
using Domain.Models;
using Domain.Video;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoManager.GUI
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly IMapper _mapper;
        private readonly IDataAccessorService _dataService;
        private readonly IVideoService _videoService;

        public MainForm(ILogger<MainForm> logger, IMapper mapper, IDataAccessorService dataService, IVideoService videoService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));

            InitializeComponent();

            Text = $"Video Manager - {GetType().Assembly.GetName().Version}";
            _logger.LogInformation($"{Text} started!");
        }

        private async void GetAirtableDataBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(GetAirtableDataBtn_Click)}");
            IReadOnlyList<PublicationModel> records = await _dataService.GetRecords(300);

            VideoDGV.DataSource = records;
        }

        private void UpdateLiveBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(UpdateLiveBtn_Click)}");
            if (!CheckSelectedRow()) return;

            try
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    _videoService.UpdateVideoMetadataAsync(model.LiveVideo, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("¯\\(°_o)/¯ Oups, quelque chose s'est mal passé pendant la mise à jour du live.", "Echec mise à jour du live");
                _logger.LogError(ex, "Erreur pendant la mise à jour du live");
            }
        }

        private async void UpdateAirtableForLiveDataBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(UpdateAirtableForLiveDataBtn_Click)}");
            if (!CheckSelectedRow()) return;

            VideoMetadataModel res = await _videoService.GetUpcomingLiveAsync(CancellationToken.None);

            if (res != null && !string.IsNullOrEmpty(res.VideoUrl))
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    //model.LiveVideo.Identifier = res.Identifier;
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

        private async void UploadMainVideoBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(UploadMainVideoBtn_Click)}");
            if (!CheckSelectedRow()) return;

            Progress<UploadStatusModel> progress = new Progress<UploadStatusModel>(status => {
                UploadProgressBar.Value = (int)status.CompletionRate;
                ProgressLabel.Text = $"{status.CompletionRate}% ({status.StatusText})";
            });

            DataGridViewRow row = VideoDGV.SelectedRows[0];
            PublicationModel publicationModel = row.DataBoundItem as PublicationModel;

            DialogResult result = VideoFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                VideoModel videoModel = new VideoModel()
                {
                    Metadata = publicationModel.MainVideo,
                    VideoStream = VideoFileDialog.OpenFile()
                };

                await Task.Run(() => { _videoService.UploadVideoAsync(videoModel, progress, CancellationToken.None); });
            }
            else
            {
                MessageBox.Show("OK on fera ça plus tard 😉");
            }
        }

        private bool CheckSelectedRow()
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(CheckSelectedRow)}");
            if (VideoDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vous devez sélectionner une ligne dans le tableau avant de continuer");
                return false;
            }

            return true;
        }

        private async void UpdateMainVideoBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(UpdateMainVideoBtn_Click)}");
            if (!CheckSelectedRow()) return;

            try
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    await _videoService.UpdateVideoMetadataAsync(model.MainVideo, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("¯\\(°_o)/¯ Oups, quelque chose s'est mal passé pendant la mise à jour de la vidéo principale.", "Echec mise à jour vidéo");
                _logger.LogError(ex, "Erreur pendant la mise à jour de la vidéo principale");
            }
        }

        private async void CommentMainVideoBtn_Click(object sender, EventArgs e)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(CommentMainVideoBtn_Click)}");
            if (!CheckSelectedRow()) return;

            try
            {
                foreach (DataGridViewRow row in VideoDGV.SelectedRows)
                {
                    PublicationModel model = row.DataBoundItem as PublicationModel;
                    await _videoService.CommentVideoAsync(model.MainVideo, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("¯\\(°_o)/¯ Oups, quelque chose s'est mal passé pendant l'ajout d'un commentaire sur la vidéo principale.", "Echec ajout commentaire");
                _logger.LogError(ex, "Erreur pendant l'ajout d'un commentaire");
            }
        }

        private async void UpdateMainVideoAirtableBtn_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow()) return;
            foreach (DataGridViewRow row in VideoDGV.SelectedRows)
            {
                PublicationModel model = row.DataBoundItem as PublicationModel;

                if (await _dataService.UpdateMainVideoRecord(model.Id, model.MainVideo))
                {
                    MessageBox.Show("Airtable a été mis à jour");
                }
                else
                {
                    MessageBox.Show("Problème pendant la mise à jour de Airtable");

                }
            }
        }
    }
}
