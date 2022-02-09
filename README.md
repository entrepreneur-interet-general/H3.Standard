# H3Standard: A c# binding to Uber H3 C library

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

H3 is a geospatial indexing system using a hexagonal grid.

Documentation of the C library is available at [https://uber.github.io/h3/](https://uber.github.io/h3/).

### Version

Bindings to Uber C library version 3.7.2.1

This project does not embed the corresponding native c library.
It has to be build and added separately.

## Prerequisites
.NET Standard 2.0

A _WINDOWS pragma directive has been defined to target the right name for the underlying H3 c lib, h3 in case of *Nix platforms, h3.dll in case of Windows platforms.

## H3 - 3.7.2.1 Support

All H3 C functions are accessible via a simple wrapping, with the exact same c-style name (camel-casing).

Most of the H3 functions have a dotnet-style wrapper helper where out params and pre-array dimensioning are avoided.

## Legal and Licensing

H3 is licensed under the [Apache 2.0 License](./LICENSE).
