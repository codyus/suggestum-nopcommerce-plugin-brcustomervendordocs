using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Services;

/// <summary>
/// Brazilian vendor document service
/// </summary>
public class BrVendorDocumentService
{
    #region Fields

    private readonly IRepository<BrVendorDocument> _vendorDocumentRepository;

    #endregion

    #region Ctor

    public BrVendorDocumentService(IRepository<BrVendorDocument> vendorDocumentRepository)
    {
        _vendorDocumentRepository = vendorDocumentRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets vendor document by vendor identifier
    /// </summary>
    /// <param name="vendorId">Vendor identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the vendor document
    /// </returns>
    public async Task<BrVendorDocument> GetByVendorIdAsync(int vendorId)
    {
        return await _vendorDocumentRepository.Table
            .FirstOrDefaultAsync(doc => doc.VendorId == vendorId);
    }

    /// <summary>
    /// Gets vendor document by CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ (normalized)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the vendor document
    /// </returns>
    public async Task<BrVendorDocument> GetByCnpjAsync(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return null;

        var normalized = CnpjValidator.Normalize(cnpj);
        return await _vendorDocumentRepository.Table
            .FirstOrDefaultAsync(doc => doc.Cnpj == normalized);
    }

    /// <summary>
    /// Checks if CNPJ already exists
    /// </summary>
    /// <param name="cnpj">CNPJ to check</param>
    /// <param name="excludeVendorId">Vendor ID to exclude from check</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains true if exists, false otherwise
    /// </returns>
    public async Task<bool> CnpjExistsAsync(string cnpj, int? excludeVendorId = null)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        var normalized = CnpjValidator.Normalize(cnpj);
        var query = _vendorDocumentRepository.Table.Where(doc => doc.Cnpj == normalized);

        if (excludeVendorId.HasValue)
            query = query.Where(doc => doc.VendorId != excludeVendorId.Value);

        return await query.AnyAsync();
    }

    /// <summary>
    /// Inserts or updates vendor document
    /// </summary>
    /// <param name="vendorId">Vendor identifier</param>
    /// <param name="cnpj">CNPJ</param>
    /// <param name="allowAlphaNumeric">Whether to allow alphanumeric format</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SaveAsync(int vendorId, string cnpj, bool allowAlphaNumeric = true)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ cannot be empty", nameof(cnpj));

        var normalized = CnpjValidator.Normalize(cnpj);

        if (!CnpjValidator.IsValid(normalized, allowAlphaNumeric))
            throw new ArgumentException("Invalid CNPJ", nameof(cnpj));

        var existing = await GetByVendorIdAsync(vendorId);

        if (existing != null)
        {
            existing.Cnpj = normalized;
            existing.UpdatedOnUtc = DateTime.UtcNow;
            await _vendorDocumentRepository.UpdateAsync(existing);
        }
        else
        {
            var document = new BrVendorDocument
            {
                VendorId = vendorId,
                Cnpj = normalized,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            await _vendorDocumentRepository.InsertAsync(document);
        }
    }

    /// <summary>
    /// Deletes vendor document
    /// </summary>
    /// <param name="vendorId">Vendor identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task DeleteAsync(int vendorId)
    {
        var document = await GetByVendorIdAsync(vendorId);
        if (document != null)
            await _vendorDocumentRepository.DeleteAsync(document);
    }

    #endregion
}

