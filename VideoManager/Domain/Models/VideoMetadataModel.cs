using System;

namespace Domain.Models
{
    public class VideoMetadataModel
    {
        public string Identifier { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string VideoTitle { get; set; }

        public string VideoDescription { get; set; }

        public string PinnedComment { get; set; }

        public string VideoUrl { get; set; }
    }
}
