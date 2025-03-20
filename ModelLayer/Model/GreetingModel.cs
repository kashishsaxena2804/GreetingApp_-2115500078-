using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModelLayer.Models;

namespace ModelLayer.Model
{
    [Table("Greetings")]
    public class GreetingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int UserId { get; set; } // ✅ Foreign Key Relationship

        [ForeignKey("UserId")]
        public virtual User User { get; set; } // ✅ Navigation Property
    }
}
