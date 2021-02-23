# Retrieving Authorization Code from Square - Proof of Concept

| ⚠️ THERE ARE INHERENT SECURITY RISKS IN USING THIS SOLUTION. Square recommends the use of a secure backend. Please see this [Sandbox 101: OAuth Best Practices video](https://www.youtube.com/watch?v=3gLqCJC6kLI) on YouTube. |
| --- |

## Background

A [feature request](https://developer.squareup.com/forums/t/retrieve-oauth-authorization-code-without-https-server/1470) was made on the [Square Developer Forums](https://developer.squareup.com/forums/) to be allow direct retrieval of an OAuth *authorization code* without the use of an HTTPS server.

In this case, the user had a desktop application and wished not to deploy a web server for this process. OAuth protocol uses a callback requires the authorization server to redirect to a known link. [Reference](https://developer.squareup.com/docs/oauth-api/how-oauth-works).

## Solution

A solution would be to encapsulate the retrieval of the authorization code in a stand-alone web application running on the desktop that could be called by a desktop application. It is implemented in [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1).

The repository consists of:

- Web application
- Console Based Sample Application

### Web Application

A minimal [Web Api Application](https://docs.microsoft.com/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio) with an [anonymous pipe](https://docs.microsoft.com/dotnet/standard/io/how-to-use-anonymous-pipes-for-local-interprocess-communication) for interprocess communication. The application is hosted in [Kestrel](https://docs.microsoft.com/aspnet/core/fundamentals/servers/?view=aspnetcore-5.0&tabs=windows#kestrel) a lightweight webserver built into .NET Core.

### Console Based Sample Application

Demonstrates how an application could implement the initial part of the Square OAuth process and retrieve the Square Authorization code.

## Getting Started

This guide assumes some familiarity with the [Square Developer Dashboard](https://developer.squareup.com/apps).

- Clone this repository.
- Configure **appsettings** for web application with Square settings from **Credentials** page. Create an *appsettings.development.json* and *appsettings.production.json* using the *appsettings.json* provided as a template.
- Set the **Redirect URL** on **OAuth** page to *<https://localhost:5001/oauth-redirect>*.
- Build and run at the root of repository. A browser window will open at the Square OAuth page.

    ``` powershell
    dotnet build
    dotnet run -p .\ConsoleApp\ 
    ```
