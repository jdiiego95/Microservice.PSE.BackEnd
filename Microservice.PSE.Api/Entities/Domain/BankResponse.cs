namespace Microservice.PSE.Api.Entities.Domain
{
    /// <summary>
    /// Represents a response model for bank operations within the PSE system.
    /// This class contains all the bank information returned to the client.
    /// </summary>
    public class BankResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bank.
        /// </summary>
        /// <value>An integer representing the bank ID.</value>
        public int BankId { get; set; }

        /// <summary>
        /// Gets or sets the unique code that identifies the bank in the system.
        /// </summary>
        /// <value>A string representing the bank code.</value>
        public string BankCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the official name of the bank.
        /// </summary>
        /// <value>A string representing the bank name.</value>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the bank is active and available for transactions.
        /// </summary>
        /// <value>A boolean value indicating if the bank is active.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the API URL endpoint for this bank's services.
        /// </summary>
        /// <value>A string representing the API URL.</value>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the bank record was created.
        /// </summary>
        /// <value>A DateTime representing when the bank was created.</value>
        public DateTime CreatedDate { get; set; }
    }
}