using FluentMigrator;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Data.Migrations;

[NopMigration("2025-01-15 00:00:00", "Misc.BrCustomerVendorDocs schema", MigrationProcessType.Installation)]
public class SchemaMigration : Migration
{
    #region Fields

    private readonly INopDataProvider _dataProvider;

    #endregion

    #region Ctor

    public SchemaMigration(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        // Create BrCustomerDocument table
        this.CreateTableIfNotExists<BrCustomerDocument>();

        // Create unique constraint on CustomerId
        var customerDocumentTableName = NameCompatibilityManager.GetTableName(typeof(BrCustomerDocument));
        var customerIdColumnName = NameCompatibilityManager.GetColumnName(typeof(BrCustomerDocument), nameof(BrCustomerDocument.CustomerId));
        var customerUniqueConstraintName = $"AK_{customerDocumentTableName}_{customerIdColumnName}";
        
        if (!Schema.Table(customerDocumentTableName).Constraint(customerUniqueConstraintName).Exists())
        {
            Create.UniqueConstraint(customerUniqueConstraintName)
                .OnTable(customerDocumentTableName)
                .Column(customerIdColumnName);
        }

        // Create BrVendorDocument table
        this.CreateTableIfNotExists<BrVendorDocument>();

        // Create unique constraint on VendorId
        var vendorDocumentTableName = NameCompatibilityManager.GetTableName(typeof(BrVendorDocument));
        var vendorIdColumnName = NameCompatibilityManager.GetColumnName(typeof(BrVendorDocument), nameof(BrVendorDocument.VendorId));
        var vendorUniqueConstraintName = $"AK_{vendorDocumentTableName}_{vendorIdColumnName}";
        
        if (!Schema.Table(vendorDocumentTableName).Constraint(vendorUniqueConstraintName).Exists())
        {
            Create.UniqueConstraint(vendorUniqueConstraintName)
                .OnTable(vendorDocumentTableName)
                .Column(vendorIdColumnName);
        }
    }

    /// <summary>
    /// Collects the DOWN migration expressions
    /// </summary>
    public override void Down()
    {
        Delete.Table(NameCompatibilityManager.GetTableName(typeof(BrCustomerDocument)));
        Delete.Table(NameCompatibilityManager.GetTableName(typeof(BrVendorDocument)));
    }

    #endregion
}

