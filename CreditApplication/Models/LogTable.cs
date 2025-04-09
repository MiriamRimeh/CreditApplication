namespace CreditApplication.Models
{
    public class LogTable
    {
        public int ID { get; set; }
        public string? TableName { get; set; }
        public string? ActionType { get; set; } // e.g., "Insert", "Update", "Delete"
        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}
