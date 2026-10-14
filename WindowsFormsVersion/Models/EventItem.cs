namespace MunicipalServicesApp.Models
{
    public class EventItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAnnouncement { get; set; }

        public override string ToString()
        {
            return $"{EventDate:dd MMM yyyy} - {Title}";
        }
    }
}
