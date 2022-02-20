# WrapThat.Version

##: Install

Install from nuget 

## Usage

Add this package to your main ASP.Net project, and it will add an Info endpoint with three methods, giving you the version of the main assembly in semver format.

### Get version as a string

```cs
Route:  /api/info/version

returns
1.2.3
```

### Get version as a [shields.io](https://shields.io/) structure

```cs
Route:  /api/info

returns shields io structure for use in a [shields endpoint](https://shields.io/endpoint) call.

```

You call this using shields like:

```
https://img.shields.io/endpoint?url=https://yourwebapi/api/info
```

And you get a lightgrey shields badge back, with label Version and your version number.

### Get version as a string authenticated

This is the same as the first method, except that it require authentication, if your API/app is set up for that.  The intention is that you can use this to check if the authentication is actually working.

If it is, you will be returned the version string, if not, you will get a 403 response.

```cs
Route:  /api/info/status

returns
1.2.3
```

