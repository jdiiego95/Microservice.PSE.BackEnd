using System.Globalization;
using Microservice.PSE.Api.Properties;

namespace Microservice.PSE.Api.Exceptions
{
    /// <summary>
    /// Base class for business-related exceptions in the PSE API.
    /// </summary>
    /// <remarks>
    /// Business exceptions represent expected error conditions that arise from
    /// business rule violations or validation failures.
    /// </remarks>
    public abstract class BusinessException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with this business exception.
        /// </summary>
        /// <value>
        /// An integer representing the appropriate HTTP status code for this exception.
        /// </value>
        public int StatusCode { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected BusinessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when attempting to create an entity that already exists.
    /// </summary>
    public class EntityAlreadyExistsException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="entityName">The name of the entity that already exists.</param>
        public EntityAlreadyExistsException(string entityName)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.EntityAlreadyExists, entityName))
        {
            StatusCode = StatusCodes.Status409Conflict;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="entityName">The name of the entity that already exists.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityAlreadyExistsException(string entityName, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.EntityAlreadyExists, entityName), innerException)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to access an entity that does not exist.
    /// </summary>
    public class EntityNotFoundException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">The identifier of the entity that was not found.</param>
        public EntityNotFoundException(object entityId)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.EntityNotExists, entityId))
        {
            StatusCode = StatusCodes.Status404NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">The identifier of the entity that was not found.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityNotFoundException(object entityId, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.EntityNotExists, entityId), innerException)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use an inactive bank.
    /// </summary>
    public class InactiveBankException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveBankException"/> class.
        /// </summary>
        /// <param name="bankCode">The code of the inactive bank.</param>
        public InactiveBankException(string bankCode)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.InactiveBankError, bankCode))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveBankException"/> class.
        /// </summary>
        /// <param name="bankCode">The code of the inactive bank.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InactiveBankException(string bankCode, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.InactiveBankError, bankCode), innerException)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    /// <summary>
    /// Exception thrown when there are issues with bank API connectivity.
    /// </summary>
    public class BankApiException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankApiException"/> class.
        /// </summary>
        /// <param name="bankCode">The code of the bank with API issues.</param>
        public BankApiException(string bankCode)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.BankApiError, bankCode))
        {
            StatusCode = StatusCodes.Status503ServiceUnavailable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BankApiException"/> class.
        /// </summary>
        /// <param name="bankCode">The code of the bank with API issues.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BankApiException(string bankCode, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.BankApiError, bankCode), innerException)
        {
            StatusCode = StatusCodes.Status503ServiceUnavailable;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use an invalid bank code.
    /// </summary>
    public class InvalidBankException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBankException"/> class.
        /// </summary>
        /// <param name="bankCode">The invalid bank code.</param>
        public InvalidBankException(string bankCode)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidBankError, bankCode))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBankException"/> class.
        /// </summary>
        /// <param name="bankCode">The invalid bank code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidBankException(string bankCode, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidBankError, bankCode), innerException)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    /// <summary>
    /// Exception thrown for general application errors that are not business-related.
    /// </summary>
    /// <remarks>
    /// This exception is typically used for unexpected system errors, infrastructure
    /// failures, or other technical issues that are not part of normal business flow.
    /// </remarks>
    public class GeneralApplicationException : Exception
    {
        /// <summary>
        /// Gets the unique error identifier for tracking purposes.
        /// </summary>
        public string ErrorId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralApplicationException"/> class.
        /// </summary>
        /// <param name="errorId">The unique error identifier for tracking purposes.</param>
        public GeneralApplicationException(string errorId)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.GeneralApplicationError, errorId))
        {
            ErrorId = errorId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralApplicationException"/> class.
        /// </summary>
        /// <param name="errorId">The unique error identifier for tracking purposes.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public GeneralApplicationException(string errorId, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.GeneralApplicationError, errorId), innerException)
        {
            ErrorId = errorId;
        }
    }

    /// <summary>
    /// Exception thrown when there are issues with database configuration.
    /// </summary>
    public class DatabaseConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfigurationException"/> class.
        /// </summary>
        public DatabaseConfigurationException()
            : base(Resources.DataBaseConfigurationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfigurationException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DatabaseConfigurationException(Exception innerException)
            : base(Resources.DataBaseConfigurationError, innerException)
        {
        }
    }
}