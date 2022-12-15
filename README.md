# H3Standard: A c# binding to Uber H3 C library

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

H3 is a geospatial indexing system using a hexagonal grid.

Documentation of the C library is available at [https://uber.github.io/h3/](https://uber.github.io/h3/).

### Version

Bindings to Uber C library version 4.0.0.0

This project does not embed the corresponding native c library.
It has to be build and added separately.

## Prerequisites
.NET Standard 2.0 library

A _WINDOWS pragma directive has been defined to target the right name for the underlying H3 c lib, h3 in case of *Nix platforms, h3.dll in case of Windows platforms.

## H3 - 4.0.0.0 Support

All H3 C functions are accessible via a simple wrapping, with the exact same c-style name (camel-casing).

Most of the H3 functions have a dotnet-style wrapper where out params and pre-array dimensioning are managed. They are all grouped in a new H3Net static class.

Most methods of the H3 and H3Net have been tested and validated except:
- cellsToLinkedMultiPolygon;
- cellToChildPos;
- childPosToCell;
- MISCELLANEOUS methods.

In the .Net version of polygonToCells (former Polyfill), polygons with holes are not managed.

## H3 - 3.7.2.1 Support

There is a tag v3.7.2.1 on the master branch for those in need of H3 v3.7.2.1 support.

## Legal and Licensing

H3 is licensed under the [Apache 2.0 License](./LICENSE).
