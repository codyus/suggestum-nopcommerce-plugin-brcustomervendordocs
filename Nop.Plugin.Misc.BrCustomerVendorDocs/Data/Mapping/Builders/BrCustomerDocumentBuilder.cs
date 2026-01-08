using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Data.Mapping.Builders;

/// <summary>
/// Represents a Brazilian customer document entity builder
/// </summary>
public class BrCustomerDocumentBuilder : NopEntityBuilder<BrCustomerDocument>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BrCustomerDocument.CustomerId)).AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.Cascade)
            .WithColumn(nameof(BrCustomerDocument.Cpf)).AsString(11).NotNullable()
            .WithColumn(nameof(BrCustomerDocument.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(BrCustomerDocument.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }

    #endregion
}

