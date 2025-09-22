using Microservice.PSE.Api.Clients.Interfaces;
using Microservice.PSE.Api.Entities.Domain;
using Microservice.PSE.Api.Entities.Model;
using Microservice.PSE.Api.Exceptions;
using Microservice.PSE.Api.Properties;
using Microservice.PSE.Api.Services.Interfaces;
using System.Globalization;
using System.Linq.Expressions;
using System.Resources;

namespace Microservice.PSE.Api.Services
{
    /// <summary>
    /// Provides services for managing banks.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BankService"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance.</param>
    /// <param name="bankClient">The bank client instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bankClient"/> is null.</exception>
    public class BankService(ILogger<BaseService> logger, IBankClient bankClient) : BaseService(logger), IBankService
    {
        /// <summary>
        /// The client used to interact with banks.
        /// </summary>
        private readonly IBankClient _bankClient = bankClient ?? throw new ArgumentNullException(nameof(bankClient));

        /// <summary>
        /// Retrieves banks based on the specified bank ID.
        /// </summary>
        /// <param name="bankId">Optional bank ID to filter the banks.</param>
        /// <returns>A list of bank responses.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
        public IEnumerable<BankResponse> GetBanks(int? bankId)
        {
            try
            {
                Expression<Func<Bank, bool>>? filter = bankId > 0 ? x => x.BankId == bankId : null;

                IEnumerable<Bank> banks = this._bankClient.GetBanks(filter);

                return GetBanksResponse(banks);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The bank request containing the details of the bank to create.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the success of the operation.</returns>
        /// <exception cref="EntityAlreadyExistsException">Thrown when a bank with the same bank code already exists.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
        public async Task<string> CreateBankAsync(BankRequest bankRequest)
        {
            try
            {
                Bank? currentBank = this._bankClient.GetBank(x => x.BankCode.Equals(bankRequest.BankCode));

                if (currentBank == null)
                {
                    Bank newBank = CreateNewBankFromBankRequest(bankRequest);

                    await this._bankClient.CreateBankAsync(newBank).ConfigureAwait(false);

                    return string.Format(CultureInfo.CurrentCulture, Resources.EntityCreatedSuccessfully, bankRequest.BankName);
                }
                else
                {
                    throw new EntityAlreadyExistsException(bankRequest.BankCode);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Delete an existing bank asynchronously.
        /// </summary>
        /// <param name="bankId">Bank identifier.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the success of the operation.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the bank does not exist.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
        public async Task<string> DeleteBankAsync(int bankId)
        {
            try
            {
                Bank? currentBank = this._bankClient.GetBank(x => x.BankId.Equals(bankId));

                if (currentBank != null)
                {
                    await this._bankClient.DeleteBankAsync(bankId).ConfigureAwait(false);

                    return Resources.EntityDeleteSuccessfully;
                }
                else
                {
                    throw new EntityNotFoundException(bankId);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The bank request containing updated information.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the success of the operation.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the bank does not exist.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
        public async Task<string> UpdateBankAsync(BankRequest bankRequest)
        {
            try
            {
                Bank? currentBank = this._bankClient.GetBank(x => x.BankCode == bankRequest.BankCode);

                if (currentBank == null)
                {
                    throw new EntityNotFoundException(bankRequest.BankCode);
                }

                currentBank.BankName = bankRequest.BankName;
                currentBank.IsActive = bankRequest.IsActive;
                currentBank.ApiUrl = bankRequest.ApiUrl;

                await this._bankClient.UpdateBankAsync(currentBank).ConfigureAwait(false);

                return string.Format(CultureInfo.CurrentCulture,
                    Resources.EntityUpdateSuccessfully,
                    bankRequest.BankName);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Creates bank response objects from a list of banks.
        /// </summary>
        /// <param name="banks">The list of banks.</param>
        /// <returns>A list of bank response objects.</returns>
        private static IEnumerable<BankResponse> GetBanksResponse(IEnumerable<Bank> banks)
        {
            return from bank in banks
                   select new BankResponse
                   {
                       BankId = bank.BankId,
                       BankCode = bank.BankCode,
                       BankName = bank.BankName,
                       IsActive = bank.IsActive,
                       ApiUrl = bank.ApiUrl,
                       CreatedDate = bank.CreatedDate
                   };
        }

        /// <summary>
        /// Creates a bank entity from a bank request.
        /// </summary>
        /// <param name="bankRequest">The bank request containing the details of the bank to create.</param>
        /// <returns>The created bank entity.</returns>
        private static Bank CreateNewBankFromBankRequest(BankRequest bankRequest)
        {
            return new Bank
            {
                BankCode = bankRequest.BankCode,
                BankName = bankRequest.BankName,
                IsActive = bankRequest.IsActive,
                ApiUrl = bankRequest.ApiUrl
            };
        }
    }
}