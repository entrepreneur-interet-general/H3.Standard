using System;
using System.Collections.Generic;
using H3Standard;
using H3Lib;

namespace ValidateH3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello H3!");

            // GeoToH3
            var origin = H3.GeoToH3(47, -3, 8);
            Console.WriteLine(H3.H3ToString(origin));

            // KRing
            int k = 5;
            var rings = H3.GetKRingDistances(origin, k);
            int distance = 0;
            Console.WriteLine($"Neighbors by distance for {origin}");
            foreach (var ring in rings)
            {
                Console.WriteLine($"Distance: {distance}");
                foreach (var h in ring)
                {
                    Console.WriteLine(H3.H3ToString(h));
                }
                distance++;
            }
            var destination = rings[k][0];

            // Line
            var line = H3.GetLine(origin, destination);
            Console.WriteLine($"Line between {origin} and {destination}");
            foreach (var h in line)
            {
                Console.WriteLine(H3.H3ToString(h));
            }

            // GeoToH3
            var coordinates = CreatePoints();
            ( List<ulong> h3Indexes, TimeSpan ts1 ) = NativeGeoToH3(coordinates);
            var tsGeoToH3DotNet = DotNetGeoToH3(coordinates);
            Console.WriteLine($"Ratio 2 / 1 = {tsGeoToH3DotNet / ts1}");

            // KRingDistances
            var tsNativeKRing = NativeKRingDistances(h3Indexes, k);
            var tsDotNetKRing = DotNetKRingDistances(h3Indexes, k);
            Console.WriteLine($"Ratio 2 / 1 = {tsDotNetKRing / tsNativeKRing }");

            // Polyfill
            var resolution = 9;
            var polygons = CreatePolygons(coordinates);
            TimeSpan tsPolyfillH3 = NativePolyfill(resolution, polygons);
            TimeSpan tsPolyfillDotNet = DotNetPolyfill(resolution, polygons);
            Console.WriteLine($"Ratio 2 / 1 = {tsPolyfillDotNet / tsPolyfillH3}");

            var pipi = 0;
        }

        private static List<double[]> CreatePoints()
        {
            var coordinates = new List<double[]>();
            var rand = new Random();
            for (var i = 0; i < 10000; i++)
            {
                var lat = rand.Next(-900000, 900000);
                var lon = rand.Next(-1800000, 1800000);
                coordinates.Add(new double[] { lat / 10000, lon / 10000 });
            }
            return coordinates;
        }

        private static List<List<double[]>> CreatePolygons(List<double[]> coordinates)
        {
            var polygons = new List<List<double[]>>();
            foreach (var coordinate in coordinates)
            {
                var polygon = new List<double[]>();
                polygon.Add(coordinate);
                polygon.Add(new double[] { coordinate[0] + 0.01, coordinate[1] });
                polygon.Add(new double[] { coordinate[0] + 0.01, coordinate[1] + 0.01 });
                polygon.Add(new double[] { coordinate[0], coordinate[1] + 0.01 });
                polygon.Add(coordinate);
                polygons.Add(polygon);
            }
            return polygons;
        }

        private static ( List<ulong> h3Indexes, TimeSpan ts ) NativeGeoToH3(List<double[]> coordinates)
        {
            var nativeH3Indexes = new List<ulong>();
            var start = DateTime.UtcNow;
            foreach (var coordinate in coordinates)
            {
                var h3Index = H3.GeoToH3(coordinate[0], coordinate[1], 9);
                nativeH3Indexes.Add(h3Index);
            }
            var end = DateTime.UtcNow;
            var ts1 = end - start;
            Console.WriteLine($"Computed with Native Lib in {ts1}");
            return ( nativeH3Indexes, ts1 );
        }

        private static TimeSpan DotNetGeoToH3(List<double[]> coordinates)
        {
            var dotNetH3Indexes = new List<ulong>();
            var start = DateTime.UtcNow;
            foreach (var coordinate in coordinates)
            {
                var h3Index = Api.GeoToH3(new H3Lib.GeoCoord(
                    Api.DegreesToRadians(Convert.ToDecimal(coordinate[0])),
                    Api.DegreesToRadians(Convert.ToDecimal(coordinate[1]))),
                    9);
                dotNetH3Indexes.Add(h3Index);
            }
            var end = DateTime.UtcNow;
            var tsGeoToH3DotNet = end - start;
            Console.WriteLine($"Computed with .Net Lib in {tsGeoToH3DotNet}");
            return tsGeoToH3DotNet;
        }

        private static TimeSpan NativeKRingDistances(List<ulong> h3Indexes, int k)
        {
            var start = DateTime.UtcNow;
            foreach (var h3Index in h3Indexes)
            {
                var neighbors = H3.GetKRingDistances(h3Index, k);
            }
            var end = DateTime.UtcNow;
            var ts = end - start;
            Console.WriteLine($"Native KRing computed in {ts}");
            return ts;
        }

        private static TimeSpan DotNetKRingDistances(List<ulong> h3Indexes, int k)
        {
            var start = DateTime.UtcNow;
            foreach (var h3Index in h3Indexes)
            {
                var neighbors = Api.KRingDistances(h3Index, k);
            }
            var end = DateTime.UtcNow;
            var ts = end - start;
            Console.WriteLine($"DotNet KRing computed in {ts}");
            return ts;
        }

        private static TimeSpan NativePolyfill(int resolution, List<List<double[]>> polygons)
        {
            var start = DateTime.UtcNow;
            var polyFills = new List<ulong[]>();
            foreach (var polygon in polygons)
            {
                var geoCoords = new List<GeoCoord>();
                foreach (var coordinate in polygon)
                {
                    geoCoords.Add(new GeoCoord(coordinate[0], coordinate[1]));
                }
                var h3Indexes = H3.Polyfill(geoCoords, resolution);
                polyFills.Add(h3Indexes);
            }
            var end = DateTime.UtcNow;
            var tsPolyfillH3 = end - start;
            Console.WriteLine($"Polyfill computed with H3 Lib in {tsPolyfillH3}");
            return tsPolyfillH3;
        }

        private static TimeSpan DotNetPolyfill(int resolution, List<List<double[]>> polygons)
        {
            var start = DateTime.UtcNow;
            var polyFillsDotNet = new List<List<H3Index>>();
            foreach (var polygon in polygons)
            {
                var geoPolygon = new GeoPolygon();
                geoPolygon.GeoFence = new GeoFence();
                geoPolygon.GeoFence.NumVerts = 5;
                geoPolygon.GeoFence.Verts = new H3Lib.GeoCoord[5];
                int index = 0;
                foreach (var coordinate in polygon)
                {
                    geoPolygon.GeoFence.Verts[index++] = new H3Lib.GeoCoord(
                        Api.DegreesToRadians(Convert.ToDecimal(coordinate[0])),
                        Api.DegreesToRadians(Convert.ToDecimal(coordinate[1])));
                }
                List<H3Index> polyFill;
                Api.PolyFill(geoPolygon, resolution, out polyFill);
                polyFillsDotNet.Add(polyFill);
            }
            var end = DateTime.UtcNow;
            var tsPolyfillDotNet = end - start;
            Console.WriteLine($"Polyfill computed with .Net Lib in {tsPolyfillDotNet}");
            return tsPolyfillDotNet;
        }
    }
}
