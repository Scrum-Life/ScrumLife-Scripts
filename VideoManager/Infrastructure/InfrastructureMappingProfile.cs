using AirtableApiClient;
using AutoMapper;
using Domain.Models;

namespace VideoManager.Infrastructure
{
    public class InfrastructureMappingProfile : Profile
    {
        /// <summary>
        /// Profile name.
        /// </summary>
        public override string ProfileName
        {
            get { return "InfrastructureMappingProfile"; }
        }

        /// <summary>
        /// Create a new instance of <see cref="AutoMapperMappingProfile"/>.
        /// </summary>
        public InfrastructureMappingProfile()
        {
            AllowNullCollections = false;

            CreateMap<VideoModel, Google.Apis.YouTube.v3.Data.Video>()
                .ForPath(m => m.LiveStreamingDetails.ScheduledStartTime, opt => opt.MapFrom(x => x.StartDate))
                .ForPath(m => m.Snippet.DefaultAudioLanguage, opt => opt.MapFrom(x => x.Language))
                .ForPath(m => m.Snippet.DefaultLanguage, opt => opt.MapFrom(x => x.Language))
                .ForPath(m => m.Snippet.Description, opt => opt.MapFrom(x => x.Description))
                .ForPath(m => m.Snippet.Title, opt => opt.MapFrom(x => x.Title))
                .ForPath(m => m.Snippet.Tags, opt => opt.MapFrom(x => x.Tags))
                .ForPath(m => m.Snippet.CategoryId, opt => opt.MapFrom(x => x.CategoryId))
                //TODO .ForPath(m => m.Snippet.Thumbnails, opt => opt.MapFrom(x => x.StartDate))
                .ReverseMap()
            ;

            //CreateMap<VideoMetadataModel, Google.Apis.YouTube.v3.Data.Video>()
            //    .ForPath(m => m.Snippet.Description, opt => opt.MapFrom(x => x.VideoDescription))
            //    .ForPath(m => m.Snippet.Title, opt => opt.MapFrom(x => x.VideoTitle))
            //    .ForMember(m => m.Id, opt => { 
            //        opt.MapFrom(x => x.VideoUrl);
            //        opt.AddTransform(x => x.Replace("https://www.youtube.com/watch?v=", ""));
            //    })
            //    .ReverseMap()
            //;

            CreateMap<Google.Apis.YouTube.v3.Data.VideoCategory, VideoCategoryModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(m => m.Name, opt => opt.MapFrom(x => x.Snippet.Title));

            CreateMap<AirtableRecord, RecordModel>();
        }

    }
}
