/*
 * Copyright 2018 Shom, Arnaud Ménard
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
    public class H3
    {

        private static void UnpackNativeLibrary(string libraryName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"{assembly.GetName().Name}.{libraryName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream(stream.CanSeek ? (int)stream.Length : 0))
            {
                stream.CopyTo(memoryStream);
                File.WriteAllBytes(libraryName, memoryStream.ToArray());
            }
        }

        public static void InstanciateNativeLibrary()
        {
            if (!_libInstanciateDone)
            {
                var libraryName = "h3lib.dll";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    libraryName = "h3lib.dll.so";
                }
                UnpackNativeLibrary(libraryName);
            }
            _libInstanciateDone = true;
        }

        private static bool _libInstanciateDone = false;

        static H3()
        {
            InstanciateNativeLibrary();
        }

        #region H3 static extern declarations

        // INDEXING : https://uber.github.io/h3/#/documentation/api-reference/indexing

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong geoToH3(ref H3GeoCoord g, int res);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl )]
        private static extern void h3ToGeo( ulong h3, ref H3GeoCoord g);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void h3ToGeoBoundary(ulong h3, ref H3GeoBoundary gp);


        // INSPECTION : https://uber.github.io/h3/#/documentation/api-reference/inspection

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3GetResolution(ulong h3);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3GetBaseCell(ulong h);

        // stringToH3

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void h3ToString(ulong h3Index, byte[] s, int size);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3IsValid(ulong h);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3IsResClassIII(ulong h);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3IsPentagon(ulong h);


        // NEIGHBOURS : https://uber.github.io/h3/#/documentation/api-reference/neighbors

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void kRing(ulong origin, int k, ulong[] neighbours);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int maxKringSize(int k);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void kRingDistances(ulong origin, int k, ulong[] neighbours, int[] distances);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int hexRange(ulong origin, int k, ulong[] neighbours);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int hexRangeDistances(ulong origin, int k, ulong[] neighbours, int[] distances);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int hexRanges(ulong[] h3Set, int length, int k, ulong[] neighbours);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int hexRing(ulong origin, int k, ulong[] neighbours);


        // DISTANCE: https://uber.github.io/h3/#/documentation/api-reference/distance

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3Distance(ulong origin, ulong h3);


        // HIERARCHY : https://uber.github.io/h3/#/documentation/api-reference/hierarchy

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong h3ToParent(ulong index, int parentRes);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void h3ToChildren(ulong h, int childRes, ulong[] children);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int maxH3ToChildrenSize(ulong h, int childRes);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int compact(ulong[] h3Set, ulong[] compactedSet, int numHexes);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int uncompact(ulong[] compactedSet, int numHexes, ulong[] h3Set, int maxHexes, int res );

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int maxUncompactSize( ulong[] compactedSet, int numHexes, int res);

        // REGION : https://uber.github.io/h3/#/documentation/api-reference/regions

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void polyfill(ref H3GeoPolygon polygon, int res, ulong[] index);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int maxPolyfillSize(ref H3GeoPolygon polygon, int res);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void h3SetToLinkedGeo(ulong[] h3Set, int numHexes, H3LinkedGeoPolygon[] linkedGeos);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void destroyLinkedPolygon(H3LinkedGeoPolygon[] polygon);


        // UNIDIRECTIONAL EDGES : https://uber.github.io/h3/#/documentation/api-reference/unidirectional-edges

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3IndexesAreNeighbors(ulong origin, ulong destination);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong getH3UnidirectionalEdge(ulong origin, ulong destination);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3UnidirectionalEdgeIsValid(ulong edge);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong getOriginH3IndexFromUnidirectionalEdge(ulong edge);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong getDestinationH3IndexFromUnidirectionalEdge(ulong edge);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void getH3IndexesFromUnidirectionalEdge(ulong edge, ulong[] originDestination);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void getH3UnidirectionalEdgesFromHexagon(ulong origin, ulong[] edges);

        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void getH3UnidirectionalEdgeBoundary(ulong edge, H3GeoBoundary[] gb);


        // MISCELLANEOUS : https://uber.github.io/h3/#/documentation/api-reference/miscellaneous

        // degsToRad
        // radsToDeg
        // hexAreaKm2
        // hexAreaM2
        // edgeLengthKm
        // edgeLengthM
        // numHexagons

        // IJK
        [DllImport("h3lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int h3ToIjk(ulong origin, ulong h3, CoordIJK ijk);

        #endregion

        // INDEXING 

        public static ulong GeoToH3(double latitude, double longitude, int resolution)
        {
            var coord = new H3GeoCoord { lat = DegToRad(latitude), lon = DegToRad(longitude) };
            return geoToH3(ref coord, resolution);
        }

        public static string H3ToString(ulong index)
        {
            return string.Format("{0}", index.ToString("x"));
        }

        public static ulong StringToH3(string index)
        {
            return Convert.ToUInt64(index, 16);
        }


        public static int IsValid(ulong h)
        {
            return h3IsValid(h);
        }

        public static bool IsResClassIII(ulong h)
        {
            return h3IsResClassIII(h) != 0;
        }

        public static bool IsPentagon(ulong h)
        {
            return h3IsPentagon(h) != 0;
        }

        public static int GetResolution(string h3)
        {
            return h3GetResolution(StringToH3(h3));
        }

        public static int GetResolution(ulong h3)
        {
            return h3GetResolution(h3);
        }

        public static int GetBaseCell(ulong h3)
        {
            return h3GetBaseCell(h3);
        }

        public static ulong H3ToParent(ulong index, int parentResolution = 0)
        {
            if (parentResolution == 0)
            {
                parentResolution = GetResolution(index) - 1;
            }
            return h3ToParent(index, parentResolution);
        }

        public static ulong H3ToParent(string index, int parentResolution = 0)
        {
            return H3ToParent(StringToH3(index), parentResolution);
        }

        public static int H3Distance(ulong origin, ulong index)
        {
            return h3Distance(origin, index);
        }

        public static int H3Distance(string origin, string index)
        {
            return H3Distance(StringToH3(origin), StringToH3(index));
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

        public static (double latitude, double longitude) GetCenter(ulong index)
        {
            H3GeoCoord geoCoord = new H3GeoCoord();
            h3ToGeo(index, ref geoCoord);
            if (geoCoord.lon > 180) geoCoord.lon -= 360;
            GeoCoord coords = new GeoCoord(geoCoord);
            return (coords.latitude, coords.longitude);
        }

        public static H3GeoBoundary[] GetEdgeBoundary(ulong index)
        {
            var boundaries = new H3GeoBoundary[2];
            getH3UnidirectionalEdgeBoundary(index, boundaries);
            return boundaries;
        }

        // Needs System.ValueTuple to compile --> Nuget
        public static (ulong origin, ulong destination) GetOriginAndDestination(ulong edgeIndex)
        {
            ulong[] indexes = new ulong[2];
            getH3IndexesFromUnidirectionalEdge(edgeIndex, indexes);
            return (indexes[0], indexes[1]);
        }

        public static ulong[] GetEdges(ulong h3Index)
        {
            ulong[] edges = new ulong[6];
            getH3UnidirectionalEdgesFromHexagon(h3Index, edges);
            return edges;
        }

        public static ulong[] GetKRing(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            int result = hexRange(origin, k, neighbours);
            return neighbours;
        }

        public static ulong[] GetHexRange(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            int result = hexRange(origin, k, neighbours);
            return neighbours;
        }

        public static ulong[] GetHexRing(ulong origin, int k)
        {
            int nbHex = maxKringSize(k);
            ulong[] neighbours = new ulong[nbHex];
            int result = hexRing(origin, k, neighbours);
            return neighbours;
        }

        public static ulong GetDestination(ulong edge )
        {
            return getDestinationH3IndexFromUnidirectionalEdge(edge);
        }

        public static ulong GetOrigin(ulong edge)
        {
            return getOriginH3IndexFromUnidirectionalEdge(edge);
        }

        public static ulong[] GetChildren(string hexIndex, int childResolution)
        {
            return GetChildren(StringToH3(hexIndex), childResolution);
        }

        public static ulong[] GetChildren(ulong index, int childResolution = 0)
        {
            ulong[] children = null;
            if (childResolution == 0)
            {
                childResolution = GetResolution(index) + 1;
            }
            int nbChildren = maxH3ToChildrenSize(index, childResolution);
            children = new ulong[nbChildren + 1];
            h3ToChildren( index, childResolution, children );
            return children;
        }

        public static ulong[] Polyfill(List<GeoCoord> coords, int resolution)
        {
            ulong[] index = null;
            int nbIndex = MaxPolyfillSize(coords, resolution );
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
                index = new ulong[nbIndex+1];
                Console.Write("{0}", nbIndex );
                polyfill(ref polygon, resolution, index);

                for (int i = 0; i < index.Length; i++)
                {
                    if (index[i] != 0)
                    {
                        validIndexes.Add(index[i]);
                    }
                }
                Console.WriteLine(" --> {0}", validIndexes.Count );
            }
            finally
            {
                arrHandle.Free();
            }
            return validIndexes.ToArray();
        }

        private static int MaxPolyfillSize( List<GeoCoord> coords,  int resolution)
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

        public static ulong[] Compact( ulong[] h3Set )
        {
            ulong[] compactedSet = new ulong[h3Set.Length];
            compact(h3Set, compactedSet, h3Set.Length);
            return compactedSet;
        }

        public static ulong[] Uncompact( ulong[] compactedSet, int resolution  )
        {
            int maxHexes = maxUncompactSize(compactedSet, compactedSet.Length, resolution);
            ulong[] h3Set = new ulong[maxHexes];
            uncompact(compactedSet, compactedSet.Length, h3Set, maxHexes, resolution);
            return h3Set;
        }

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

    }

}
