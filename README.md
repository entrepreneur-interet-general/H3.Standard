# H3Standard: A c# binding to Uber H3 C library

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

H3 is a geospatial indexing system using a hexagonal grid.

Documentation of the C library is available at [https://uber.github.io/h3/](https://uber.github.io/h3/).

## Nuget

This c# binding is available as a Nuget package [here](https://www.nuget.org/packages/H3/). 

```
PM > Install-Package H3 -Version 3.2.0-beta
```

### Versions

Available versions are:
- 3.2.0-beta  Bindings to Uber C library version 3.2.0

This package includes the 3.2.0 version of the native C library.

- 1.0.0 Bindings to Uber C library version 3.1.0

This package does not embed the corresponding native c library within the package.
It has to be build and added separately.

## Prerequisites
Windows: .NET Standard 2.0

## H3 3.2.0 Support

All H3 functions are available EXCEPT the experimental ones:

```
experimentalH3ToLocalIj(H3Index origin, H3Index h3, CoordIJ *out);
experimentalLocalIjToH3(H3Index origin, const CoordIJ *ij, H3Index *out);
```

## Legal and Licensing

H3 is licensed under the [Apache 2.0 License](./LICENSE).
