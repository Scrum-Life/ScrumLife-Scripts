using AutoMapper;
using Domain.Models;
using System;

namespace VideoManager.Infrastructure
{
    public class DomainMappingProfile : Profile
    {
        /// <summary>
        /// Profile name.
        /// </summary>
        public override string ProfileName
        {
            get { return "DomainMappingProfile"; }
        }

        /// <summary>
        /// Create a new instance of <see cref="AutoMapperMappingProfile"/>.
        /// </summary>
        public DomainMappingProfile()
        {
            AllowNullCollections = false;

            CreateMap<RecordModel, VideoMetadataModel>()
                //.ForMember(m => m.Identifier, opt => opt.MapFrom(m => m.Fields["Identifiant unique"]))
                .ForMember(m => m.VideoTitle, opt => opt.MapFrom(m => m.Fields["[youtube] Titre"]))
                .ForMember(m => m.VideoDescription, opt => opt.MapFrom(m => m.Fields["[youtube] Description"]))
                .ForMember(m => m.PinnedComment, opt => opt.MapFrom(m => m.Fields["[youtube] Commentaire à épingler"]))
                .ForMember(m => m.PublicationDate, opt => opt.MapFrom(m => m.Fields["Date de publication"] as DateTime?))
                .ForMember(m => m.VideoUrl, opt => opt.MapFrom(m => m.Fields["[youtube] URL"]));
        }
    }
}
