using System.ComponentModel.DataAnnotations;

namespace Microservice.PSE.Api.Entities.Domain
{
    /// <summary>
    /// Represents a request model for bank operations within the PSE system.
    /// This class encapsulates all necessary data required to create or update a bank record.
    /// </summary>
    public class BankRequest
    {
        /// <summary>
        /// The unique code that identifies the bank in the system.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string BankCode { get; set; } = string.Empty;

        /// <summary>
        /// The official name of the bank.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the bank is active and available for transactions.
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The API URL endpoint for this bank's services.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string ApiUrl { get; set; } = string.Empty;
    }
}