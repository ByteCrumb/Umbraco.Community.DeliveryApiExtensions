# Contributing Guidelines 📝

Contributions to this package are very welcome! 🙌

## Running locally 🧑‍💻

> [!IMPORTANT]  
> **Requirements**
> - .NET 7
> - Node 20+ (using [Volta](https://volta.sh/) is recommended to ensure you always have the right version ✨)
> 

**Visual Studio**  
When using VS everything should just work, running the test website (`tests\UmbracoDeliveryApiExtensions.TestSite`) should automaticaly build both the UI and the package.

**Command line**  
Running `dotnet run` in the test website path (`tests\UmbracoDeliveryApiExtensions.TestSite`) should automaticaly build both the UI and the package.

There are also other helpful npm scripts to just run the UI, watch for file changes, among others, so check the different projects `package.json` to see what is already set up.

**Backoffice credentials**  
Username: `admin@umbraco`  
Password: `#Umbraco123!`

## Project structure

### Back-end 🦾

**.NET Library**: `src\UmbracoDeliveryApiExtensions\UmbracoDeliveryApiExtensions.csproj`

### Front-end 🌻

**Vite + Lit**: `src\UmbracoDeliveryApiExtensions.UI`  
When building the back-end library, the front-end is automatically built and its output is copied over to the back-end library `wwwroot` folder.  
If you want to make changes to the front-end it might be useful to run the test website and do `npm run watch` on the front-end folder so that you can see your changes in real time.

### Tests 🐞

For end-to-end testing, we have a [Playwright](https://playwright.dev) project set up with a few tests in `tests\UmbracoDeliveryApiExtensions.Playwright`.

#### Testing the typed swagger 🪄

In order to test changes in the typed swagger feature, we have added different client projects in `tests\clients`. Each projects uses a different client generation tool.

For the typescript projects, you can simply run `npm install` on each one of them and then `npm run start`. These are basically console apps, and the `start` command will both generate the client and run some sample code which is using it. If any step of the command fails, then something might have broken! 🤐 (or fixed, who knows 😅)

For the `nswag` one, that is also a console app but built in .NET, so you can simply run it using `dotnet run` (or through Visual Studio). 🙌
