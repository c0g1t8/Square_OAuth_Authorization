# Retrieving Authorization Code from Square - Proof of Concept

## Background

A [feature request](https://developer.squareup.com/forums/t/retrieve-oauth-authorization-code-without-https-server/1470) was made on the [Square Developer Forums](https://developer.squareup.com/forums/) to be allow direct retrieval of an OAuth *authorization code* without the use of an HTTPS server.

In this case, the user had a desktop application and wished not to deploy a web server for this process. OAuth protocol uses a callback requires the authorization server to redirect to a known link. [Reference](https://developer.squareup.com/docs/oauth-api/how-oauth-works).

## Solution

A possible solution would be to encapsulate the retrieval of the authorization code in a stand-alone web application that could be called by a desktop application.
