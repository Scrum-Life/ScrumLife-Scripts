using System;
using System.Collections.Generic;
using System.IO;

namespace Domain.Models
{
    public class VideoModel
    {
        public string Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public IList<string> Tags { get; set; }

        public Stream VideoStream { get; set; }

        public DateTime? StartDate { get; set; }

        public int CategoryId { get; set; }

        //TODO thumbnails
    }
}
