using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Services;

/// <summary>
/// Brazilian customer document service
/// </summary>
public class BrCustomerDocumentService
{
    #region Fields

    private readonly IRepository<BrCustomerDocument> _customerDocumentRepository;

    #endregion

    #region Ctor

    public BrCustomerDocumentService(IRepository<BrCustomerDocument> customerDocumentRepository)
    {
        _customerDocumentRepository = customerDocumentRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets customer document by customer identifier
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer document
    /// </returns>
    public async Task<BrCustomerDocument> GetByCustomerIdAsync(int customerId)
    {
        return await _customerDocumentRepository.Table
            .FirstOrDefaultAsync(doc => doc.CustomerId == customerId);
    }

    /// <summary>
    /// Gets customer document by CPF
    /// </summary>
    /// <param name="cpf">CPF (normalized)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer document
    /// </returns>
    public async Task<BrCustomerDocument> GetByCpfAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;

        var normalized = CpfValidator.Normalize(cpf);
        return await _customerDocumentRepository.Table
            .FirstOrDefaultAsync(doc => doc.Cpf == normalized);
    }

    /// <summary>
    /// Checks if CPF already exists
    /// </summary>
    /// <param name="cpf">CPF to check</param>
    /// <param name="excludeCustomerId">Customer ID to exclude from check</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains true if exists, false otherwise
    /// </returns>
    public async Task<bool> CpfExistsAsync(string cpf, int? excludeCustomerId = null)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var normalized = CpfValidator.Normalize(cpf);
        var query = _customerDocumentRepository.Table.Where(doc => doc.Cpf == normalized);

        if (excludeCustomerId.HasValue)
            query = query.Where(doc => doc.CustomerId != excludeCustomerId.Value);

        return await query.AnyAsync();
    }

    /// <summary>
    /// Inserts or updates customer document
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <param name="cpf">CPF</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SaveAsync(int customerId, string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF cannot be empty", nameof(cpf));

        var normalized = CpfValidator.Normalize(cpf);

        if (!CpfValidator.IsValid(normalized))
            throw new ArgumentException("Invalid CPF", nameof(cpf));

        var existing = await GetByCustomerIdAsync(customerId);

        if (existing != null)
        {
            existing.Cpf = normalized;
            existing.UpdatedOnUtc = DateTime.UtcNow;
            await _customerDocumentRepository.UpdateAsync(existing);
        }
        else
        {
            var document = new BrCustomerDocument
            {
                CustomerId = customerId,
                Cpf = normalized,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            await _customerDocumentRepository.InsertAsync(document);
        }
    }

    /// <summary>
    /// Deletes customer document
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteAsync(int customerId)
    {
        var document = await GetByCustomerIdAsync(customerId);
        if (document != null)
            await _customerDocumentRepository.DeleteAsync(document);
    }

    #endregion
}

