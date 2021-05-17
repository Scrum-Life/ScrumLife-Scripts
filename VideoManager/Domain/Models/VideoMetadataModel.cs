using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class VideoMetadataModel
    {
        //public string Identifier { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string VideoTitle { get; set; }

        public string VideoDescription { get; set; }

        public string PinnedComment { get; set; }

        public string VideoUrl { get; set; }

        public IList<string> Tags { get; set; }

        public DateTime? StartDate { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        public override string ToString()
        {
            return $"{VideoUrl} - {VideoTitle} [{(PublicationDate.HasValue ? PublicationDate.Value.ToString("G") : "")}]";
        }
    }
}
