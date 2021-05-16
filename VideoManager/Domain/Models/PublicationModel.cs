namespace Domain.Models
{
    public class PublicationModel
    {
        public string Id { get; set; }
        
        public VideoMetadataModel MainVideo { get; set; }

        public VideoMetadataModel LiveVideo { get; set; }


    }
}
