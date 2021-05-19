namespace Domain.Models
{
    public class UploadStatusModel
    {
        public UploadStatusModel(VideoMetadataModel metadata) => Metadata = metadata;

        public VideoMetadataModel Metadata { get; private set; }

        public bool IsComplete { get; set; }

        public bool IsSuccess { get; set; }
        public string StatusText { get; set; }
        public long CompletionRate { get; set; }
    }
}
