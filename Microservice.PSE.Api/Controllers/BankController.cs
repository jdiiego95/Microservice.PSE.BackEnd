using Microservice.PSE.Api.Entities.Domain;
using Microservice.PSE.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.PSE.Api.Controllers
{
    /// <summary>
    /// Controller for managing bank operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BankController"/> class.
    /// </remarks>
    /// <param name="bankService">The bank service instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bankService"/> is null.</exception>
    [ApiController]
    [Route("api/[controller]")]
    public class BankController(IBankService bankService) : BaseApiController
    {
        /// <summary>
        /// The service instance for managing banks.
        /// </summary>
        private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));

        /// <summary>
        /// Retrieves a collection of bank responses based on a bank ID.
        /// </summary>
        /// <param name="bankId">The ID of the bank to filter by.</param>
        /// <returns>An action result containing a collection of bank responses.</returns>
        /// <response code="200">Returns the collection of bank responses.</response>
        /// <response code="400">If a business rule validation fails.</response>
        /// <response code="404">If a requested resource is not found.</response>
        /// <response code="409">If there's a conflict with the current state of the resource.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<BankResponse>> GetBanks(int? bankId)
        {
            return this.ExecuteWithErrorHandling(() => this._bankService.GetBanks(bankId));
        }

        /// <summary>
        /// Creates a new bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The bank request containing the bank information.</param>
        /// <returns>A task representing the asynchronous operation, with an action result containing a string indicating success.</returns>
        /// <response code="201">If the bank was created successfully.</response>
        /// <response code="400">If a business rule validation fails or request data is invalid.</response>
        /// <response code="409">If a bank with the same bank code already exists.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateBankAsync([FromBody] BankRequest bankRequest)
        {
            if (string.IsNullOrWhiteSpace(bankRequest.BankCode))
            {
                return BadRequest("Bank code is required.");
            }
            if (string.IsNullOrWhiteSpace(bankRequest.BankName))
            {
                return BadRequest("Bank name is required.");
            }
            if (string.IsNullOrWhiteSpace(bankRequest.ApiUrl))
            {
                return BadRequest("API URL is required.");
            }

            var result = await this.ExecuteWithErrorHandlingAsync(() => this._bankService.CreateBankAsync(bankRequest));

            if (result.Result is OkObjectResult okResult)
            {
                return StatusCode(StatusCodes.Status201Created, okResult.Value);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing bank asynchronously.
        /// </summary>
        /// <param name="bankRequest">The bank request containing updated bank information.</param>
        /// <returns>A task representing the asynchronous operation, with an action result containing a string indicating success.</returns>
        /// <response code="200">If the bank was updated successfully.</response>
        /// <response code="400">If a business rule validation fails or request data is invalid.</response>
        /// <response code="404">If the bank to update is not found.</response>
        /// <response code="409">If updating would result in a conflict with existing data.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<string>> UpdateBankAsync([FromBody] BankRequest bankRequest)
        {
            if (string.IsNullOrWhiteSpace(bankRequest.BankCode))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest("Bank code is required for update operations."));
            }
            if (string.IsNullOrWhiteSpace(bankRequest.BankName))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest("Bank name is required."));
            }
            if (string.IsNullOrWhiteSpace(bankRequest.ApiUrl))
            {
                return Task.FromResult<ActionResult<string>>(BadRequest("API URL is required."));
            }

            return this.ExecuteWithErrorHandlingAsync(() => this._bankService.UpdateBankAsync(bankRequest));
        }

        /// <summary>
        /// Deletes an existing bank asynchronously.
        /// </summary>
        /// <param name="bankId">The bank identifier.</param>
        /// <returns>A task representing the asynchronous operation, with an action result containing a string indicating success.</returns>
        /// <response code="200">If the bank was deleted successfully.</response>
        /// <response code="400">If a business rule validation fails.</response>
        /// <response code="404">If the bank to delete is not found.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpDelete("{bankId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<string>> DeleteBankAsync(int bankId)
        {
            if (bankId <= 0)
            {
                return Task.FromResult<ActionResult<string>>(BadRequest("Bank ID must be greater than zero."));
            }

            return this.ExecuteWithErrorHandlingAsync(() => this._bankService.DeleteBankAsync(bankId));
        }
    }
}