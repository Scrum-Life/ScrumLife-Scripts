using System.ComponentModel.DataAnnotations;

namespace VideoManager.Infrastructure.YouTube
{
    /// <summary>
    /// YouTube API configuration
    /// </summary>
    public sealed class YoutubeConfiguration
    {
        /// <summary>
        /// Youtube channel username
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Application name as it will be seen by YouTube API
        /// </summary>
        public string AplicationName { get; set; }

        public string GoogleClientId { get; set; }

        public string GoogleClientSecret { get; set; }
    }
}
