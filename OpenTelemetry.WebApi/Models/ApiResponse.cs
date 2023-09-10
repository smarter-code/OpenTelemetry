using System.ComponentModel.DataAnnotations;

namespace OpenTelemetry.WebApi.Models
{
    public class ApiResponse
    {
        [Key]
        public int Id { get; set; }
        public string Payload { get; set; }
        public DateTime Date { get; set; }
    }
}
