# H3Standard: A c# binding to Uber H3 C library

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

H3 is a geospatial indexing system using a hexagonal grid.

Documentation of the C library is available at [https://uber.github.io/h3/](https://uber.github.io/h3/).

## Nuget

This c# binding is available as a Nuget package [here](https://www.nuget.org/packages/H3/). 

```
PM > Install-Package H3 -Version 3.2.0-alpha
```

### Versions

Available versions are:
- 3.2.0-alpha Bindings to Uber C library version 3.2.0

This package embeds the 3.2.0 H3 C library as a resource.
H3.InstanciateNativeLibrary() has to be called first. It will copy the h3lib.dll into the output directory.
This is a workaround for packing native dlls within a nuget package.

- 1.0.0 Bindings to Uber C library version 3.1.0

This package does not embed the corresponding native c library within the package.
It has to be build and added separately

## Prerequisites
Windows: .NET 4.6.1+

## H3 3.2.0 Support

All H3 functions are available EXCEPT the experimental ones:

```
experimentalH3ToLocalIj)(H3Index origin, H3Index h3, CoordIJ *out);
experimentalLocalIjToH3)(H3Index origin, const CoordIJ *ij, H3Index *out);
```

## Legal and Licensing

H3 is licensed under the [Apache 2.0 License](./LICENSE).
