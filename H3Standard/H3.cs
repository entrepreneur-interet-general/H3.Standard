/*
 * Copyright 2018 - 2022, Shom, Arnaud Ménard
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;


namespace H3Standard
{
    public static class H3
    {

#if _WINDOWS
        private const string LIBRARY_NAME = "h3.dll";
#else
        private const string LIBRARY_NAME = "h3";
#endif

        #region H3 static extern declarations

        // INDEXING : https://uber.github.io/h3/#/documentation/api-reference/indexing, https://h3geo.org/docs/api/indexing

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong geoToH3(ref H3GeoCoord g, int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3ToGeo(ulong h3, ref H3GeoCoord g);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3ToGeoBoundary(ulong h3, ref H3GeoBoundary gp);


        // INSPECTION : https://uber.github.io/h3/#/documentation/api-reference/inspection, https://h3geo.org/docs/api/inspection

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3GetResolution(ulong h3);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3GetBaseCell(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong stringToH3(byte[] s);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3ToString(ulong h3Index, byte[] s, int size);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3IsValid(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3IsResClassIII(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3IsPentagon(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3GetFaces(ulong h, int[] faces);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxFaceCount(ulong h);


        // TRAVERSAL : https://uber.github.io/h3/#/documentation/api-reference/neighbors, https://h3geo.org/docs/api/traversal

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void kRing(ulong origin, int k, ulong[] neighbours);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxKringSize(int k);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void kRingDistances(ulong origin, int k, ulong[] neighbours, int[] distances);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hexRange(ulong origin, int k, ulong[] neighbours);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hexRangeDistances(ulong origin, int k, ulong[] neighbours, int[] distances);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hexRanges(ulong[] h3Set, int length, int k, ulong[] neighbours);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hexRing(ulong origin, int k, ulong[] neighbours);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3Line(ulong start, ulong end, ulong[] line);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3LineSize(ulong start, ulong end);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3Distance(ulong origin, ulong h3);


        // HIERARCHY : https://uber.github.io/h3/#/documentation/api-reference/hierarchy, https://h3geo.org/docs/api/hierarchy

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong h3ToParent(ulong index, int parentRes);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3ToChildren(ulong h, int childRes, ulong[] children);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong h3ToCenterChild(ulong h, int childRes);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxH3ToChildrenSize(ulong h, int childRes);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int compact(ulong[] h3Set, ulong[] compactedSet, int numHexes);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int uncompact(ulong[] compactedSet, int numHexes, ulong[] h3Set, int maxHexes, int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxUncompactSize(ulong[] compactedSet, int numHexes, int res);


        // REGION : https://uber.github.io/h3/#/documentation/api-reference/regions, https://h3geo.org/docs/api/regions

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void polyfill(ref H3GeoPolygon polygon, int res, ulong[] index);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int maxPolyfillSize(ref H3GeoPolygon polygon, int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void h3SetToLinkedGeo(ulong[] h3Set, int numHexes, H3LinkedGeoPolygon[] linkedGeos);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroyLinkedPolygon(H3LinkedGeoPolygon[] polygon);


        // UNIDIRECTIONAL EDGES : https://uber.github.io/h3/#/documentation/api-reference/unidirectional-edges, https://h3geo.org/docs/api/uniedge

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3IndexesAreNeighbors(ulong origin, ulong destination);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong getH3UnidirectionalEdge(ulong origin, ulong destination);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int h3UnidirectionalEdgeIsValid(ulong edge);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong getOriginH3IndexFromUnidirectionalEdge(ulong edge);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong getDestinationH3IndexFromUnidirectionalEdge(ulong edge);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getH3IndexesFromUnidirectionalEdge(ulong edge, ulong[] originDestination);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getH3UnidirectionalEdgesFromHexagon(ulong origin, ulong[] edges);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getH3UnidirectionalEdgeBoundary(ulong edge, H3GeoBoundary[] gb);


        // MISCELLANEOUS : https://uber.github.io/h3/#/documentation/api-reference/miscellaneous, https://h3geo.org/docs/api/misc

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double degsToRad(double degrees);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double radsToDeg(double radians);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double hexAreaKm2(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double hexAreaM2(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double cellAreaM2(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double cellAreaRads2(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double edgeLengthKm(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double edgeLengthM(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double exactEdgeLengthKm(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double exactEdgeLengthM(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double exactEdgeLengthRads(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong numHexagons(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getRes0Indexes(ulong[] hexagons);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int res0IndexCount();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getPentagonIndexes(int res, ulong[] hexagons);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pentagonIndexCount();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double pointDistKm(ref GeoCoord a, ref GeoCoord b);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double pointDistM(ref GeoCoord a, ref GeoCoord b);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double pointDistRads(ref GeoCoord a, ref GeoCoord b);

        #endregion

        #region .Net Style H3 Methods

        // INDEXING 

        public static ulong GeoToH3(double latitude, double longitude, int resolution)
        {
            var coord = new H3GeoCoord { lat = DegToRad(latitude), lon = DegToRad(longitude) };
            return geoToH3(ref coord, resolution);
        }

        public static GeoCoord H3ToGeo(ulong h)
        {
            H3GeoCoord geoCoord = new H3GeoCoord();
            h3ToGeo(h, ref geoCoord);
            return new GeoCoord(geoCoord);
        }

        public static GeoCoord[] H3ToGeoBoundary(string key)
        {
            return H3ToGeoBoundary(StringToH3(key));
        }

        public static GeoCoord[] H3ToGeoBoundary(ulong h3Index)
        {
            H3GeoBoundary geoBoundary = new H3GeoBoundary();
            geoBoundary.numVerts = 10;
            h3ToGeoBoundary(h3Index, ref geoBoundary);
            return Normalize(geoBoundary);
        }

        public static Boundaries GetBoundaries(string key)
        {
            return GetBoundaries(StringToH3(key));
        }

        public static Boundaries GetBoundaries(ulong key)
        {
            var b = H3ToGeoBoundary(key);
            Boundaries boundaries = new Boundaries();
            boundaries.key = key;
            for (int i = 0; i < b.Length; i++)
            {
                var coord = new double[2]
                {
                    b[i].latitude,
                    b[i].longitude
                };
                boundaries.coords.Add(coord);
            }
            return boundaries;
        }


        // INSPECTION

        public static int GetResolution(ulong h3)
        {
            return h3GetResolution(h3);
        }

        public static int GetBaseCell(ulong h3)
        {
            return h3GetBaseCell(h3);
        }

        public static ulong StringToH3(string index)
        {
            return Convert.ToUInt64(index, 16);
        }

        public static string H3ToString(ulong index)
        {
            return string.Format("{0}", index.ToString("x"));
        }

        public static bool IsValid(ulong h)
        {
            return h3IsValid(h) == 1;
        }

        public static bool IsResClassIII(ulong h)
        {
            return h3IsResClassIII(h) != 0;
        }

        public static bool IsPentagon(ulong h)
        {
            return h3IsPentagon(h) != 0;
        }

        public static int[] GetFaces(ulong h)
        {
            int n = maxFaceCount(h);
            int[] faces = new int[n];
            h3GetFaces(h, faces);
            return faces;
        }

        // maxFaceCount: utility function for c

        // TRAVERSAL

        public static ulong[] GetKRing(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            int result = hexRange(origin, k, neighbours);
            return neighbours;
        }

        // maxKringSize : utility function for c

        /// <summary>
        /// Returns an array of array of h3Indexes by distance from the origin
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="k"></param>
        /// <returns>An array of array of h3Indexes by distance from the origin</returns>
        public static ulong[][] GetKRingDistances(ulong origin, int k)
        {
            int n = maxKringSize(k);
            var neighbors = new List<ulong>[k + 1];
            for (var i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = new List<ulong>();
            }
            int[] distances = new int[n];
            var h3Neighbors = new ulong[n];
            kRingDistances(origin, k, h3Neighbors, distances);
            int currentDistance = distances[0];
            for (int i = 0; i < h3Neighbors.Length; i++)
            {
                if (distances[i] != currentDistance)
                {
                    currentDistance = distances[i];
                }
                if (h3Neighbors[i] != 0)
                {
                    neighbors[currentDistance].Add(h3Neighbors[i]);
                }
            }
            return neighbors.Select((arr) => { return arr.ToArray(); }).ToArray();
        }

        public static ulong[] GetHexRange(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            hexRange(origin, k, neighbours);
            return neighbours;
        }

        // TODO: hexRangeDistances

        // TODO: hexRanges

        public static ulong[] GetHexRing(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            int result = hexRing(origin, k, neighbours);
            return neighbours;
        }

        public static ulong[] GetLine(ulong start, ulong end)
        {
            int d = h3LineSize(start, end);
            ulong[] line = new ulong[d];
            h3Line(start, end, line);
            return line;
        }

        // h3LineSize : utility function for c

        public static int H3Distance(ulong origin, ulong index)
        {
            return h3Distance(origin, index);
        }

        public static int H3Distance(string origin, string index)
        {
            return H3Distance(StringToH3(origin), StringToH3(index));
        }

        // TODO: experimentalH3ToLocalIj

        // TODO: experimentalLocalIjToH3


        // HIERARCHY

        public static H3Index H3ToParent(ulong index, int parentResolution = -1)
        {
            if (parentResolution == -1)
            {
                parentResolution = GetResolution(index) - 1;
            }
            return new H3Index(h3ToParent(index, parentResolution));
        }

        public static ulong[] GetChildren(ulong index, int childResolution = -1)
        {
            ulong[] children = null;
            if (childResolution == -1)
            {
                childResolution = GetResolution(index) + 1;
            }
            int nbChildren = maxH3ToChildrenSize(index, childResolution);
            children = new ulong[nbChildren + 1];
            h3ToChildren(index, childResolution, children);
            return children;
        }

        public static ulong CenterChild(ulong index, int childResolution = -1)
        {
            if (childResolution == -1)
            {
                childResolution = GetResolution(index) + 1;
            }
            return h3ToCenterChild(index, childResolution);
        }

        // maxH3ToChildrenSize: utility function for c

        public static (double latitude, double longitude) GetCenter(ulong index)
        {
            H3GeoCoord geoCoord = new H3GeoCoord();
            h3ToGeo(index, ref geoCoord);
            if (geoCoord.lon > 180) geoCoord.lon -= 360;
            GeoCoord coords = new GeoCoord(geoCoord);
            return (coords.latitude, coords.longitude);
        }

        public static ulong[] Compact(ulong[] h3Set)
        {
            ulong[] compactedSet = new ulong[h3Set.Length];
            compact(h3Set, compactedSet, h3Set.Length);
            return compactedSet;
        }

        public static ulong[] Uncompact(ulong[] compactedSet, int resolution)
        {
            int maxHexes = maxUncompactSize(compactedSet, compactedSet.Length, resolution);
            ulong[] h3Set = new ulong[maxHexes];
            uncompact(compactedSet, compactedSet.Length, h3Set, maxHexes, resolution);
            return h3Set;
        }

        // maxUncompactSize: utility function for c

        // REGIONS

        public static H3Index[] Polyfill(List<GeoCoord> coords, int resolution)
        {
            ulong[] index = null;
            int nbIndex = MaxPolyfillSize(coords, resolution);
            var h3Coords = FromGeoCoords(coords);
            H3GeoPolygon polygon = new H3GeoPolygon();
            polygon.geofence = new H3GeoFence();
            GCHandle arrHandle = GCHandle.Alloc(h3Coords.ToArray(), GCHandleType.Pinned);
            List<ulong> validIndexes = new List<ulong>();
            try
            {
                polygon.geofence.verts = arrHandle.AddrOfPinnedObject();
                polygon.geofence.numVerts = h3Coords.Length;
                polygon.numHoles = 0;
                polygon.holes = IntPtr.Zero;
                index = new ulong[nbIndex + 1];
                //Console.Write("{0}", nbIndex);
                polyfill(ref polygon, resolution, index);
                for (int i = 0; i < index.Length; i++)
                {
                    if (index[i] != 0)
                    {
                        validIndexes.Add(index[i]);
                    }
                }
                //Console.WriteLine(" --> {0}", validIndexes.Count);
            }
            finally
            {
                arrHandle.Free();
            }
            return validIndexes.Select((ulong h3Index) => { return new H3Index(h3Index); }).ToArray();
        }

        private static int MaxPolyfillSize(List<GeoCoord> coords, int resolution)
        {
            var h3Coords = FromGeoCoords(coords);
            int nbIndex = 0;
            H3GeoPolygon polygon = new H3GeoPolygon();
            polygon.geofence = new H3GeoFence();
            GCHandle arrHandle = GCHandle.Alloc(h3Coords.ToArray(), GCHandleType.Pinned);
            try
            {
                polygon.geofence.verts = arrHandle.AddrOfPinnedObject();
                polygon.geofence.numVerts = h3Coords.Length;
                polygon.numHoles = 0;
                polygon.holes = IntPtr.Zero;
                nbIndex = maxPolyfillSize(ref polygon, resolution);
            }
            finally
            {
                arrHandle.Free();
            }
            return nbIndex;
        }

        // TODO: h3SetToLinkedGeo / h3SetToMultiPolygon

        // TODO: destroyLinkedPolygon

        // UNIDIRECTIONAL EDGES

        public static bool AreNeighbors(ulong origin, ulong destination)
        {
            return h3IndexesAreNeighbors(origin, destination) == 1;
        }

        public static ulong GetEdge(ulong origin, ulong destination)
        {
            return getH3UnidirectionalEdge(origin, destination);
        }

        public static bool IsValidEdge(ulong edge)
        {
            return h3UnidirectionalEdgeIsValid(edge) == 1;
        }

        public static ulong GetOrigin(ulong edge)
        {
            return getOriginH3IndexFromUnidirectionalEdge(edge);
        }

        public static ulong GetDestination(ulong edge)
        {
            return getDestinationH3IndexFromUnidirectionalEdge(edge);
        }

        // Needs System.ValueTuple to compile --> Nuget
        public static (H3Index origin, H3Index destination) GetOriginAndDestination(ulong edgeIndex)
        {
            ulong[] indexes = new ulong[2];
            getH3IndexesFromUnidirectionalEdge(edgeIndex, indexes);
            return (new H3Index(indexes[0]), new H3Index(indexes[1]));
        }

        public static ulong[] GetEdges(ulong h3Index)
        {
            ulong[] edges = new ulong[6];
            getH3UnidirectionalEdgesFromHexagon(h3Index, edges);
            return edges;
        }

        public static H3GeoBoundary[] GetEdgeBoundary(ulong index)
        {
            var boundaries = new H3GeoBoundary[2];
            getH3UnidirectionalEdgeBoundary(index, boundaries);
            return boundaries;
        }

        // MISCELLANEOUS

        public static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public static double RadToDeg(double rad)
        {
            var val = rad * 180 / Math.PI;
            if (val > 180) val = val - 360;
            return val;
        }

        public static ulong[] Res0Indexes
        {
            get
            {
                ulong[] res0Indexes = new ulong[122];
                getRes0Indexes(res0Indexes);
                return res0Indexes;
            }
        }

        #endregion

        #region Private utility methods
        private static H3GeoCoord[] FromGeoCoords(List<GeoCoord> coords)
        {
            var h3GeoCoords = new List<H3GeoCoord>();
            foreach (var g in coords)
            {
                h3GeoCoords.Add(new H3GeoCoord(g));
            }
            return h3GeoCoords.ToArray();
        }

        private static GeoCoord[] FromH3GeoCoords(H3GeoCoord[] h3Coords)
        {
            var geoCoords = new List<GeoCoord>();
            foreach (var g in h3Coords)
            {
                geoCoords.Add(new GeoCoord(g));
            }
            return geoCoords.ToArray();
        }

        private static GeoCoord[] Normalize(H3GeoBoundary geoBoundary)
        {
            geoBoundary.verts0.latitude = RadToDeg(geoBoundary.verts0.latitude);
            geoBoundary.verts0.longitude = RadToDeg(geoBoundary.verts0.longitude);
            geoBoundary.verts1.latitude = RadToDeg(geoBoundary.verts1.latitude);
            geoBoundary.verts1.longitude = RadToDeg(geoBoundary.verts1.longitude);
            geoBoundary.verts2.latitude = RadToDeg(geoBoundary.verts2.latitude);
            geoBoundary.verts2.longitude = RadToDeg(geoBoundary.verts2.longitude);
            geoBoundary.verts3.latitude = RadToDeg(geoBoundary.verts3.latitude);
            geoBoundary.verts3.longitude = RadToDeg(geoBoundary.verts3.longitude);
            geoBoundary.verts4.latitude = RadToDeg(geoBoundary.verts4.latitude);
            geoBoundary.verts4.longitude = RadToDeg(geoBoundary.verts4.longitude);
            geoBoundary.verts5.latitude = RadToDeg(geoBoundary.verts5.latitude);
            geoBoundary.verts5.longitude = RadToDeg(geoBoundary.verts5.longitude);
            geoBoundary.verts6.latitude = RadToDeg(geoBoundary.verts6.latitude);
            geoBoundary.verts6.longitude = RadToDeg(geoBoundary.verts6.longitude);
            geoBoundary.verts7.latitude = RadToDeg(geoBoundary.verts7.latitude);
            geoBoundary.verts7.longitude = RadToDeg(geoBoundary.verts7.longitude);
            geoBoundary.verts8.latitude = RadToDeg(geoBoundary.verts8.latitude);
            geoBoundary.verts8.longitude = RadToDeg(geoBoundary.verts8.longitude);
            geoBoundary.verts9.latitude = RadToDeg(geoBoundary.verts9.latitude);
            geoBoundary.verts9.longitude = RadToDeg(geoBoundary.verts9.longitude);
            List<GeoCoord> geoCoords = new List<GeoCoord>();
            if (geoBoundary.verts0.latitude != 0 && geoBoundary.verts0.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts0.latitude, geoBoundary.verts0.longitude));
            }
            if (geoBoundary.verts1.latitude != 0 && geoBoundary.verts1.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts1.latitude, geoBoundary.verts1.longitude));
            }
            if (geoBoundary.verts2.latitude != 0 && geoBoundary.verts2.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts2.latitude, geoBoundary.verts2.longitude));
            }
            if (geoBoundary.verts3.latitude != 0 && geoBoundary.verts3.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts3.latitude, geoBoundary.verts3.longitude));
            }
            if (geoBoundary.verts4.latitude != 0 && geoBoundary.verts4.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts4.latitude, geoBoundary.verts4.longitude));
            }
            if (geoBoundary.verts5.latitude != 0 && geoBoundary.verts5.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts5.latitude, geoBoundary.verts5.longitude));
            }
            if (geoBoundary.verts6.latitude != 0 && geoBoundary.verts6.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts6.latitude, geoBoundary.verts6.longitude));
            }
            if (geoBoundary.verts7.latitude != 0 && geoBoundary.verts7.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts7.latitude, geoBoundary.verts7.longitude));
            }
            if (geoBoundary.verts8.latitude != 0 && geoBoundary.verts8.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts8.latitude, geoBoundary.verts8.longitude));
            }
            if (geoBoundary.verts9.latitude != 0 && geoBoundary.verts9.longitude != 0)
            {
                geoCoords.Add(new GeoCoord(geoBoundary.verts9.latitude, geoBoundary.verts9.longitude));
            }
            return geoCoords.ToArray();
        }
        #endregion
    }   

}
