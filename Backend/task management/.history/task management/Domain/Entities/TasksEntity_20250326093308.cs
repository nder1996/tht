using System.ComponentModel.DataAnnotations;

namespace task_management.Domain.Entities
{
    public class TasksEntity
    {
        [Key]
        public int? id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? due_date { get; set; }
        public string? status { get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public string? state { get; set; }
    }
}
