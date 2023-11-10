# Contributing Guidelines 📝

Contributions to this package are very welcome! 🙌

## Running locally 🧑‍💻

> [!IMPORTANT]  
> **Requirements**
> - .NET 7
> - Node 20+ (if you are a Windows user, take a look at [Volta](https://volta.sh/), it's a really nice tool to manage JS tool versions, like Node ✨)
> 

First things first, let's build the front-end! 🌻  
You can do that by using your favourite terminal, going to the `src/UmbracoDeliveryApiExtensions.UI` folder and simply running `npm install --frozen-lockfile`.

In `tests\UmbracoDeliveryApiExtensions.TestSite` you will find a test Umbraco website, that is connected to our library (`src\UmbracoDeliveryApiExtensions`) that you can run (either using Visual Studio or using `dotnet run` in a terminal) and test your changes in!

**Credentials**  
Username: `admin@umbraco`  
Password: `#Umbraco123!`

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
