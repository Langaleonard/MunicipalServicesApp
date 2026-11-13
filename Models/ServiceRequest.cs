namespace MunicipalServicesApp.Models
{
    public class ServiceRequest
    {
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; } = "Submitted";
        public string Priority { get; set; } = "Normal";
        public string AssignedDepartment { get; set; } = "Municipal Intake";
        public int ProgressPercent { get; set; }
        public DateTime LastUpdated { get; set; }

        public int PriorityRank => Priority switch
        {
            "Urgent" => 1,
            "High" => 2,
            "Normal" => 3,
            "Low" => 4,
            _ => 5
        };
    }
}
