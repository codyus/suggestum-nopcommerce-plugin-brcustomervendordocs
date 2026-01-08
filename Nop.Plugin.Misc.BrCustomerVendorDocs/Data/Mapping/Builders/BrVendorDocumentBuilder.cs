using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Vendors;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Data.Mapping.Builders;

/// <summary>
/// Represents a Brazilian vendor document entity builder
/// </summary>
public class BrVendorDocumentBuilder : NopEntityBuilder<BrVendorDocument>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BrVendorDocument.VendorId)).AsInt32().NotNullable().ForeignKey<Vendor>(onDelete: System.Data.Rule.Cascade)
            .WithColumn(nameof(BrVendorDocument.Cnpj)).AsString(14).NotNullable()
            .WithColumn(nameof(BrVendorDocument.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(BrVendorDocument.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }

    #endregion
}

