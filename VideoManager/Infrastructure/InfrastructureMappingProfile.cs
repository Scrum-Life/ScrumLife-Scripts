using AirtableApiClient;
using AutoMapper;
using Domain.Models;
using Google.Apis.YouTube.v3.Data;

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

            CreateMap<VideoMetadataModel, Google.Apis.YouTube.v3.Data.Video>()
                .ForPath(m => m.Status.PublishAt, opt => opt.MapFrom(x => x.StartDate))
                .ForPath(m => m.Status.PrivacyStatus, opt => { 
                    opt.Condition(x => x.Source.StartDate.HasValue); 
                    opt.MapFrom(x => "private"); //This value is required by YouTube API when PublishAt is set
                })
                .ForPath(m => m.Snippet.DefaultAudioLanguage, opt => opt.MapFrom(x => x.Language))
                .ForPath(m => m.Snippet.DefaultLanguage, opt => opt.MapFrom(x => x.Language))
                .ForPath(m => m.Snippet.Description, opt => opt.MapFrom(x => x.VideoDescription))
                .ForPath(m => m.Snippet.Title, opt => opt.MapFrom(x => x.VideoTitle))
                .ForPath(m => m.Snippet.Tags, opt => opt.MapFrom(x => x.Tags))
                .ReverseMap()
            ;

            CreateMap<SearchResult, VideoMetadataModel>()
                .ForMember(m => m.Identifier, opt => opt.MapFrom(x => x.Id.VideoId))
                .ForMember(m => m.PublicationDate, opt => opt.MapFrom(x => x.Snippet.PublishedAt))
                .ForMember(m => m.VideoTitle, opt => opt.MapFrom(x => x.Snippet.Title))
                .ForMember(m => m.VideoDescription, opt => opt.MapFrom(x => x.Snippet.Description))
            ;

            CreateMap<AirtableRecord, RecordModel>();
        }

    }
}
