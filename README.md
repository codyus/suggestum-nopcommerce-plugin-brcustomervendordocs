# Brazilian Customer and Vendor Documents Plugin

A nopCommerce plugin that adds CPF (Cadastro de Pessoa Física) and CNPJ (Cadastro Nacional de Pessoa Jurídica) fields with Brazilian document validation for customers and vendors.

## Description

This plugin extends nopCommerce to support essential Brazilian documents:

- **CPF (Cadastro de Pessoa Física)**: Field for individual customers with complete validation of the check digit algorithm
- **CNPJ (Cadastro Nacional de Pessoa Jurídica)**: Field for vendors with support for numeric or alphanumeric format

The plugin includes robust validation, duplicate checking, and configurable options for displaying fields on different pages of the system.

## Features

### For Customers (CPF)
- CPF field with Brazilian algorithm validation
- Duplicate checking (one CPF per customer)
- Optional display on customer information page
- Optional display during checkout
- Configurable required field
- Automatic normalization (digits only)

### For Vendors (CNPJ)
- CNPJ field with format validation
- Support for numeric format (14 digits) or alphanumeric format (14 characters A-Z0-9)
- Duplicate checking (one CNPJ per vendor)
- Display on vendor edit page
- Configurable required field
- Automatic normalization

### General Features
- Administrative interface for configuration
- Real-time validation
- Localized error messages
- Native integration with nopCommerce permissions system
- Automatic database migrations
- Full localization support

## System Requirements

- **nopCommerce**: Version 4.90 or 5.00
- **.NET**: 9.0 or higher
- **Database**: SQL Server, MySQL or PostgreSQL

## Installation

### Method 1: Upload via Admin Panel

1. Download the plugin ZIP file
2. Access the nopCommerce admin panel
3. Navigate to **Configuration → Plugins → Local plugins**
4. Click **Upload plugin**
5. Select the plugin ZIP file
6. Wait for automatic installation
7. Click **Install** on the listed plugin

### Method 2: Manual Installation

1. Extract the ZIP file contents
2. Copy the `Misc.BrCustomerVendorDocs` folder to the `~/Plugins/` directory of your nopCommerce installation
3. Access the admin panel
4. Navigate to **Configuration → Plugins → Local plugins**
5. Locate the "Brazilian Customer and Vendor Documents" plugin
6. Click **Install**

## Configuration

After installation, configure the plugin:

1. Access **Configuration → Plugins → Local plugins**
2. Locate "Brazilian Customer and Vendor Documents"
3. Click **Configure**

### Configuration Options

#### CPF (Customer)
- **CPF Required**: Check to make CPF required during registration/checkout
- **Show CPF on Checkout**: Display the CPF field on the checkout page
- **Show CPF on Customer Info**: Display the CPF field on the customer information page

#### CNPJ (Vendor)
- **CNPJ Required**: Check to make CNPJ required for vendors
- **Show CNPJ on Vendor Edit**: Display the CNPJ field on the vendor edit page
- **Allow Alphanumeric CNPJ**: Allow alphanumeric format (14 characters A-Z0-9) in addition to the standard numeric format

## Database Structure

The plugin automatically creates the following tables:

- `BrCustomerDocument`: Stores customer CPF
- `BrVendorDocument`: Stores vendor CNPJ

Migrations are executed automatically during installation.

## Usage

### For Customers

1. During registration or checkout, the CPF field will be displayed (if configured)
2. The customer must enter the CPF (with or without formatting)
3. The system automatically validates the format and check digits
4. The CPF is normalized and stored as digits only

### For Vendors

1. When editing a vendor in the admin panel, the CNPJ field will be displayed
2. The administrator must enter the CNPJ
3. The system validates the format (numeric or alphanumeric, as configured)
4. The CNPJ is stored in the configured format

## Validation

### CPF
- Format: 11 digits (accepts formatting: 000.000.000-00)
- Check digit validation using official Brazilian algorithm
- Verification of known invalid CPFs (e.g., 111.111.111-11)
- Duplicate checking

### CNPJ
- Numeric format: 14 digits (accepts formatting: 00.000.000/0000-00)
- Alphanumeric format: 14 characters (A-Z, 0-9) - if enabled
- Format validation
- Duplicate checking

## Uninstallation

1. Access **Configuration → Plugins → Local plugins**
2. Locate "Brazilian Customer and Vendor Documents"
3. Click **Uninstall**
4. Confirm uninstallation

**Note**: Uninstallation removes the plugin settings, but **does not remove** the CPF/CNPJ data already registered. To completely remove the data, manually execute the appropriate SQL queries.

## Troubleshooting

### CPF/CNPJ field does not appear
- Verify that the plugin is installed and activated
- Check the plugin display settings
- Clear the nopCommerce cache

### Validation error
- Verify that the document format is correct
- For alphanumeric CNPJ, ensure the option is enabled
- Verify that the document is not duplicated

### Error during installation
- Verify that the nopCommerce version is compatible (4.90+ or 5.00+)
- Check write permissions in the Plugins folder
- Check nopCommerce error logs

## Changelog

### Version 1.00.0
- Initial release
- CPF support for customers
- CNPJ support for vendors
- Complete validation of Brazilian documents
- Administrative configuration interface
- Integration with checkout and customer/vendor pages

## Support

For support, bug reports or suggestions, please refer to:
- nopCommerce Marketplace
- Official nopCommerce documentation

## License

This plugin follows the same license as nopCommerce.

## Development

### Plugin Structure

```
Nop.Plugin.Misc.BrCustomerVendorDocs/
├── BrCustomerVendorDocsPlugin.cs      # Main plugin class
├── BrCustomerVendorDocsSettings.cs   # Settings
├── BrCustomerVendorDocsDefaults.cs   # Constants
├── Controllers/                       # Controllers (Admin and Public)
├── Domain/                            # Domain entities
├── Services/                          # Services and validators
├── Data/                              # Migrations and mappings
├── Models/                            # Data models
├── Views/                             # Razor views
├── Components/                        # View Components
├── Infrastructure/                    # Infrastructure configurations
├── Validators/                        # FluentValidation validators
├── plugin.json                        # Plugin metadata
└── logo.png                           # Plugin logo
```

## Useful Links

- [nopCommerce Documentation](https://docs.nopcommerce.com/)
- [nopCommerce Marketplace](https://www.nopcommerce.com/marketplace)
- [nopCommerce Forum](https://forum.nopcommerce.com/)
