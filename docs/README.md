<!--IF NUGET
![Package Logo](https://raw.githubusercontent.com/ByteCrumb/Umbraco.Community.DeliveryApiExtensions/HEAD/docs/icon.png)
<!--ELSE-->
<img src="https://raw.githubusercontent.com/ByteCrumb/Umbraco.Community.DeliveryApiExtensions/HEAD/docs/icon.png" alt="Delivery Api Extensions logo" height="130" align="right">
<!--END-->

# Umbraco Delivery Api Extensions

[![Downloads](https://img.shields.io/nuget/dt/Umbraco.Community.DeliveryApiExtensions?color=cc9900)](https://www.nuget.org/packages/Umbraco.Community.DeliveryApiExtensions/)
[![NuGet](https://img.shields.io/nuget/vpre/Umbraco.Community.DeliveryApiExtensions?color=0273B3)](https://www.nuget.org/packages/Umbraco.Community.DeliveryApiExtensions)
[![GitHub license](https://img.shields.io/github/license/ByteCrumb/Umbraco.Community.DeliveryApiExtensions?color=8AB803)](../LICENSE)

Extensions for the Umbraco Delivery API.

## Features ✨

### Backoffice preview
Preview the Delivery API responses from the backoffice content/media nodes.

![Preview](https://raw.githubusercontent.com/ByteCrumb/Umbraco.Community.DeliveryApiExtensions/v13/main/docs/screenshots/api-preview.png)

### Typed swagger

> **Note:** As of v18, the typed swagger feature has been removed from this package. Umbraco 18 includes content type schemas in OpenAPI natively. See the [Umbraco documentation](https://docs.umbraco.com/umbraco-cms/develop-with-umbraco/headless-and-apis/content-delivery-api/content-type-schemas-in-openapi) for details.

## Installation 🧑‍💻

Add the package to an existing Umbraco website (v12.2+) from nuget:

```sh
dotnet add package Umbraco.Community.DeliveryApiExtensions
```

### Configuration (appsettings.json)

The following represents the default configuration, which can optionally be overriden by defining it in your own app settings.
```jsonc
{
  "DeliveryApiExtensions": {
    "Preview": {
      "Enabled": true,
      "Media": {
        "Enabled": true
      },
      "AllowedUserGroupAliases": [] // All allowed by default
    }
  }
}
```

## Contributing 🙌

Contributions to this package are most welcome! Please read the [Contributing Guidelines](https://github.com/ByteCrumb/Umbraco.Community.DeliveryApiExtensions/blob/HEAD/.github/CONTRIBUTING.md).
