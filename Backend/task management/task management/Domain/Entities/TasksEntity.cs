using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management.Domain.Entities
{
    public class TasksEntity
    {
        [Key]
        [Column("id")]
        public int? id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("title")]
        public string? title { get; set; }

        [Column("description", TypeName = "text")]
        public string? description { get; set; }

        [Column("due_date", TypeName = "timestamp with time zone")]
        public DateTime? due_date { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("status")]
        public string? status { get; set; }

        [Required]
        [StringLength(1)]
        [Column("state", TypeName = "character(1)")]
        public string? state { get; set; }

        [Column("create_at", TypeName = "timestamp with time zone")]
        public DateTime? create_at { get; set; }

        [Column("update_at", TypeName = "timestamp with time zone")]
        public DateTime? update_at { get; set; }
       
    }
}
