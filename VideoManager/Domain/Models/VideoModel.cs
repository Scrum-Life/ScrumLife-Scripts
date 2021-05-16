using System.IO;

namespace Domain.Models
{
    public class VideoModel
    {
        public Stream VideoStream { get; set; }

        public VideoMetadataModel Metadata {get;set;}
    }
}
