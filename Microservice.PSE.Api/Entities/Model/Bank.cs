using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.PSE.Api.Entities.Model
{
    public partial class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankId { get; set; }

        [Required]
        [StringLength(50)]
        public string BankCode { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string BankName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(500)]
        public string ApiUrl { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}