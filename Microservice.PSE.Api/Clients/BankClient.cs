using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microservice.PSE.Api.Clients.Interfaces;
using Microservice.PSE.Api.Entities;
using Microservice.PSE.Api.Entities.Model;

namespace Microservice.PSE.Api.Clients
{
    /// <summary>
    /// Client class for managing Bank entity operations including retrieval, creation, update and deletion
    /// </summary>
    /// <remarks>
    /// This internal client provides Bank entity operations using Entity Framework directly,
    /// utilizing execution strategies for reliable database operations
    /// </remarks>
    /// <param name="context">The Entity Framework context for database operations</param>
    /// <exception cref="ArgumentNullException">Thrown when context parameter is null</exception>
    internal class BankClient : BaseClient, IBankClient
    {
        /// <summary>
        /// Initializes a new instance of the BankClient class
        /// </summary>
        /// <param name="context">The Entity Framework context for database operations</param>
        public BankClient(MainContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a collection of banks based on the specified filter criteria
        /// </summary>
        /// <param name="filter">Optional lambda expression to filter banks. If null, returns all banks</param>
        /// <returns>An enumerable collection of Bank entities matching the filter criteria</returns>
        public IEnumerable<Bank> GetBanks(Expression<Func<Bank, bool>>? filter)
        {
            var query = _context.Banks.AsQueryable();
            return filter != null ? query.Where(filter) : query;
        }

        /// <summary>
        /// Retrieves a single bank based on the specified filter criteria
        /// </summary>
        /// <param name="filter">Lambda expression to identify the specific bank to retrieve</param>
        /// <returns>The Bank entity matching the filter criteria, or null if no match is found</returns>
        public Bank? GetBank(Expression<Func<Bank, bool>> filter)
        {
            return _context.Banks.FirstOrDefault(filter);
        }

        /// <summary>
        /// Creates a new bank entity asynchronously with resilient execution strategy
        /// </summary>
        /// <param name="bank">The Bank entity to be created in the database</param>
        /// <returns>A task containing the created Bank entity</returns>
        /// <remarks>
        /// Uses Entity Framework's execution strategy to handle transient failures during database operations
        /// </remarks>
        public async Task<Bank> CreateBankAsync(Bank bank)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                _context.Banks.Add(bank);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return bank;
            });
        }

        /// <summary>
        /// Updates an existing bank entity asynchronously
        /// </summary>
        /// <param name="bank">The Bank entity to be updated in the database</param>
        /// <returns>A task containing the updated Bank entity</returns>
        public async Task<Bank> UpdateBankAsync(Bank bank)
        {
            _context.Banks.Update(bank);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return bank;
        }

        /// <summary>
        /// Deletes an existing bank entity asynchronously
        /// </summary>
        /// <param name="bankId">The identifier of the bank to be deleted</param>
        /// <returns>A task representing the asynchronous delete operation</returns>
        public async Task DeleteBankAsync(int bankId)
        {
            var bank = await _context.Banks.FindAsync(bankId).ConfigureAwait(false);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}