namespace VideoManager.Infrastructure.Airtable
{
    public sealed class AirtableConfiguration
    {
        public string ApiKey { get; set; }

        public string DatabaseId { get; set; }

        public string TableName { get; set; }

        public string ViewName { get; set; }
    }
}
