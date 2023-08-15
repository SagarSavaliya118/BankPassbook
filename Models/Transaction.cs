using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankPassbook.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(12)")]
        [DisplayName("Account Number")]
        [Required]
        [MaxLength(12)]
        public string AccountNumber { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Beneficiary Name")]
        [Required]
        [MaxLength(150)]
        public string BeneficiaryName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Bank Name")]
        [Required]
        [MaxLength(150)]
        public string BankName { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        [DisplayName("Swift Code")]
        [Required]
        [MaxLength(12)]
        public string SwiftCode { get; set;}

        [Required]
        public int Amount { get; set;}

        [Required]
        public DateTime Date { get; set; }

        public IdentityUser User { get; set; }
    }
}
