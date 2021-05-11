using AutoMapper;

namespace VideoManager.Application
{
    internal class ApplicationMappingProfile : Profile
    {
        /// <summary>
        /// Profile name.
        /// </summary>
        public override string ProfileName
        {
            get { return "ApplicationMappingProfile"; }
        }

        /// <summary>
        /// Create a new instance of <see cref="AutoMapperMappingProfile"/>.
        /// </summary>
        public ApplicationMappingProfile()
        {
            AllowNullCollections = false;
        }
    }
}