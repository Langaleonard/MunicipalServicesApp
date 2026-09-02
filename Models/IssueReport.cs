namespace MunicipalServicesApp.Models
{
    public class IssueReport
    {
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AttachmentPath { get; set; } = string.Empty;
        public DateTime DateReported { get; set; }
    }
}