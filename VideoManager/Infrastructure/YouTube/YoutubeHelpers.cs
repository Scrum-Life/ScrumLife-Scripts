using Domain.Models;
using Google.Apis.YouTube.v3.Data;

namespace VideoManager.Infrastructure.YouTube
{
    internal static class YoutubeHelpers
    {
        public static void HydrateFromVideoModel(this Video video, VideoMetadataModel metadata)
        {
            video.Snippet = video.Snippet is null ? new VideoSnippet() : video.Snippet;
            video.Snippet.Title = metadata.VideoTitle;
            video.Snippet.Description = metadata.VideoDescription;
            video.Snippet.Tags = metadata.Tags;
            video.Status = new VideoStatus { SelfDeclaredMadeForKids = false };
        }
    }
}
