using System.Linq.Expressions;
using Microservice.PSE.Api.Entities.Model;

namespace Microservice.PSE.Api.Clients.Interfaces
{
    /// <summary>
    /// Interface for client operations related to banks.
    /// </summary>
    public interface IBankClient
    {
        /// <summary>
        /// Retrieves banks based on a filter.
        /// </summary>
        /// <param name="filter">An expression to filter the banks.</param>
        /// <returns>A collection of banks that match the filter.</returns>
        IEnumerable<Bank> GetBanks(Expression<Func<Bank, bool>>? filter);

        /// <summary>
        /// Retrieves a single bank based on a filter.
        /// </summary>
        /// <param name="filter">An expression to filter the bank.</param>
        /// <returns>The bank that matches the filter, or null if no match is found.</returns>
        Bank? GetBank(Expression<Func<Bank, bool>> filter);

        /// <summary>
        /// Creates a new bank asynchronously.
        /// </summary>
        /// <param name="bank">The bank to create.</param>
        /// <returns>A task representing the asynchronous operation, with the created bank.</returns>
        Task<Bank> CreateBankAsync(Bank bank);

        /// <summary>
        /// Updates an existing bank asynchronously.
        /// </summary>
        /// <param name="bank">The bank to update.</param>
        /// <returns>A task representing the asynchronous operation, with the updated bank.</returns>
        Task<Bank> UpdateBankAsync(Bank bank);

        /// <summary>
        /// Delete an existing bank asynchronously.
        /// </summary>
        /// <param name="bankId">Bank identifier.</param>
        Task DeleteBankAsync(int bankId);
    }
}