using Microservice.PSE.Api.Entities.Domain;

namespace Microservice.PSE.Api.Services.Interfaces
{
    /// <summary>
    /// Interface for services related to banks.
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Retrieves banks based on the specified bank ID.
        /// </summary>
        /// <param name="bankId">Optional bank ID to filter the banks.</param>
        /// <returns>A list of bank responses.</returns>
        IEnumerable<BankResponse> GetBanks(int? bankId);

        /// <summary>
        /// Creates a new bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The request containing details of the bank to create.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        Task<string> CreateBankAsync(BankRequest bankRequest);

        /// <summary>
        /// Updates an existing bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The request containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        Task<string> UpdateBankAsync(BankRequest bankRequest);

        /// <summary>
        /// Delete an existing bank asynchronously.
        /// </summary>
        /// <param name="bankId">Bank identifier.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the success of the operation.</returns>
        Task<string> DeleteBankAsync(int bankId);
    }
}