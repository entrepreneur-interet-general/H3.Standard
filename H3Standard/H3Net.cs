/*
 * Copyright 2022, Swail, Arnaud Ménard
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
using System.Linq;
using System.Runtime.InteropServices;

namespace H3Standard
{

    /// <summary>
    /// Dotnet style H3 methods
    /// </summary>
    public class H3Net
    {
        #region INDEXING

        public static ulong LatLngToCell(LatLng latLng, int resolution)
        {
            ulong h3Index = 0;
            uint error = H3.latLngToCell(ref latLng, resolution, ref h3Index);
            if (error != (uint)H3ErrorCodes.E_SUCCESS)
            {
                return 0;
            }
            return h3Index;
        }
        public static ulong LatLngToCell(double lat, double lng, int resolution)
        {
            var latLng = new LatLng { lat = DegToRad(lat), lng = DegToRad(lng) };
            return H3Net.LatLngToCell(latLng, resolution);
        }

        [Obsolete("Use LatLngToCell instead")]
        public static ulong GeoToH3(double lat, double lng, int resolution)
        {
            return LatLngToCell(lat, lng, resolution);
        }

        public static LatLng CellToLatLng(ulong h3Index)
        {
            LatLng latLng = new LatLng();
            H3.cellToLatLng(h3Index, ref latLng);
            return latLng;
        }

        [Obsolete("Use CellToLatLng instead")]
        public static LatLng H3ToGeo(ulong h3Index)
        {
            return CellToLatLng(h3Index);
        }

        public static LatLng[] CellToBoundary(ulong h3Index)
        {
            CellBoundary geoBoundary = new CellBoundary();
            geoBoundary.numVerts = 10;
            H3.cellToBoundary(h3Index, ref geoBoundary);
            return Normalize(geoBoundary);
        }

        [Obsolete("Use CellToBoundary instead")]
        public static LatLng[] H3ToGeoBoundary(ulong h3Index)
        {
            return CellToBoundary(h3Index);
        }
        #endregion

        #region INSPECTION

        public static int GetResolution(ulong h3)
        {
            return H3.getResolution(h3);
        }

        public static int GetBaseCellNumber(ulong h3)
        {
            return H3.getBaseCellNumber(h3);
        }

        [Obsolete("Please use GetBaseCellNumber instead")]
        public static int GetBaseCell(ulong h3)
        {
            return GetBaseCellNumber(h3);
        }

        public static ulong StringToH3(string index)
        {
            return Convert.ToUInt64(index, 16);
        }

        public static string H3ToString(ulong index)
        {
            return string.Format("{0}", index.ToString("x"));
        }

        public static bool IsValidCell(ulong h)
        {
            return H3.isValidCell(h) != 0;
        }

        [Obsolete("Please use IsValidCell instead")]
        public static bool IsValid(ulong h)
        {
            return IsValidCell(h);
        }

        public static bool IsResClassIII(ulong h)
        {
            return H3.isResClassIII(h) != 0;
        }

        public static bool IsPentagon(ulong h)
        {
            return H3.isPentagon(h) != 0;
        }

        public static int[] GetIcosahedronFaces(ulong h)
        {
            int n = 0;
            var error = H3.maxFaceCount(h, ref n);
            if (error != (int)H3ErrorCodes.E_SUCCESS)
            {
                return new int[] { };
            }
            int[] faces = new int[n];
            H3.getIcosahedronFaces(h, faces);
            return faces;
        }

        [Obsolete("Use GridIcosahedronFaces instead")]
        public static int[] GetFaces(ulong h3Index)
        {
            return GetIcosahedronFaces(h3Index);
        }

        #endregion

        #region TRAVERSAL

        public static ulong[] GridDisk(ulong origin, int k)
        {
            long nbHex = 0;
            H3.maxGridDiskSize(k, ref nbHex);
            ulong[] neighbours = new ulong[nbHex];
            H3.gridDisk(origin, k, neighbours);
            return neighbours;
        }

        [Obsolete("Use GridDisk instead")]
        public static ulong[] GetKRing(ulong origin, int k)
        {
            return GridDisk(origin, k);
        }

        public static ulong[][] GridDiskDistances(ulong origin, int k)
        {
            long n = 0;
            var error = H3.maxGridDiskSize(k, ref n);
            var neighbors = new List<ulong>[k + 1];
            for (var i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = new List<ulong>();
            }
            int[] distances = new int[n];
            var h3Neighbors = new ulong[n];
            error = H3.gridDiskDistances(origin, k, h3Neighbors, distances);
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

        public static ulong[] GridDiskUnsafe(ulong origin, int k)
        {
            long nbHex = 0;
            var error = H3.maxGridDiskSize(k, ref nbHex);
            ulong[] neighbours = new ulong[nbHex];
            error = H3.gridDiskUnsafe(origin, k, neighbours);
            return neighbours;
        }

        [Obsolete("Use GridDiskDistances instead")]
        public static ulong[][] GetKRingDistances(ulong origin, int k)
        {
            return GridDiskDistances(origin, k);
        }

        public static ulong[][] GridDiskDistancesUnsafe(ulong origin, int k)
        {
            long n = 0;
            var error = H3.maxGridDiskSize(k, ref n);
            var neighbors = new List<ulong>[k + 1];
            for (var i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = new List<ulong>();
            }
            int[] distances = new int[n];
            var h3Neighbors = new ulong[n];
            error = H3.gridDiskDistancesUnsafe(origin, k, h3Neighbors, distances);
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

        public static ulong[] GridDisksUnsafe(ulong[] h3Set, int k)
        {
            long nbHex = 0;
            var error = H3.maxGridDiskSize(k, ref nbHex);
            ulong[] neighbours = new ulong[nbHex];
            error = H3.gridDisksUnsafe(h3Set, (int)nbHex, k, neighbours);
            return neighbours;
        }

        public static ulong[][] GridDiskDistancesSafe(ulong origin, int k)
        {
            long n = 0;
            var error = H3.maxGridDiskSize(k, ref n);
            var neighbors = new List<ulong>[k + 1];
            for (var i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = new List<ulong>();
            }
            int[] distances = new int[n];
            var h3Neighbors = new ulong[n];
            error = H3.gridDiskDistancesSafe(origin, k, h3Neighbors, distances);
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

        public static ulong[] GridRingUnsafe(ulong origin, int k)
        {
            long nbHex = 0;
            var error = H3.maxGridDiskSize(k, ref nbHex);
            ulong[] neighbours = new ulong[nbHex];
            error = H3.gridRingUnsafe(origin,k, neighbours);
            return neighbours;
        }

        [Obsolete("Use GridDiskUnsafe instead")]
        public static ulong[] GetHexRange(ulong origin, int k)
        {
            return GridRingUnsafe(origin, k);
        }

        public static ulong[] GridPathCells(ulong origin, ulong destination)
        {
            long size = 0;
            var error = H3.gridPathCellsSize(origin, destination, ref size);
            if (size != -1)
            {
                ulong[] line = new ulong[size];
                H3.gridPathCells(origin, destination, line);
                return line;
            }
            return new ulong[] { };
        }

        [Obsolete("Use GridPathCells instead")]
        public static ulong[] GetLine(ulong origin, ulong destination)
        {
            return GridPathCells(origin, destination);
        }

        public static long GridDistance(ulong origin, ulong destination)
        {
            long distance = 0;
            var error = H3.gridDistance(origin, destination, ref distance);
            return distance;
        }

        [Obsolete("Use GridDistance instead")]
        public static long H3Distance(ulong origin, ulong destination)
        {
            return GridDistance(origin, destination);
        }
        #endregion

        #region HIERARCHY

        public static ulong CellToParent(ulong index, int parentResolution = -1)
        {
            if (parentResolution == -1)
            {
                parentResolution = GetResolution(index) - 1;
            }
            ulong parent = 0;
            var error = H3.cellToParent(index, parentResolution, ref parent);
            return parent;
        }

        [Obsolete("Use CellToParent instead")]
        public static ulong H3ToParent(ulong index, int parentResolution = -1)
        {
            return CellToParent(index, parentResolution);
        }

        public static ulong[] CellToChildren(ulong index, int childResolution = -1)
        {
            ulong[] children = null;
            if (childResolution == -1)
            {
                childResolution = GetResolution(index) + 1;
            }
            long nbChildren = 0;
            var error = H3.cellToChildrenSize(index, childResolution, ref nbChildren);
            children = new ulong[nbChildren + 1];
            error = H3.cellToChildren(index, childResolution, children);
            return children;
        }

        [Obsolete("Use CellToChildren instead")]
        public static ulong[] GetChildren(ulong index, int childResolution = -1)
        {
            return CellToChildren(index, childResolution);
        }

        public static ulong CellToCenterChild(ulong index, int childResolution = -1)
        {
            if (childResolution == -1)
            {
                childResolution = GetResolution(index) + 1;
            }
            ulong child = 0;
            H3.cellToCenterChild(index, childResolution, ref child);
            return child;
        }

        [Obsolete("Use CellToCenterChild instead")]
        public static ulong CenterChild(ulong index, int childResolution = -1)
        {
            return CellToCenterChild(index, childResolution);
        }

        public static ulong[] CompactCells(ulong[] h3Set)
        {
            ulong[] compactedSet = new ulong[h3Set.Length];
            var error = H3.compactCells(h3Set, compactedSet, h3Set.Length);
            return compactedSet;
        }
        [Obsolete("Use Compact instead")]
        public static ulong[] Compact(ulong[] h3Set)
        {
            return CompactCells(h3Set);
        }

        public static ulong[] UncompactCells(ulong[] compactedSet, int resolution)
        {
            long maxHexes = 0;
            var error = H3.uncompactCellsSize(compactedSet, compactedSet.Length, resolution, ref maxHexes);
            ulong[] h3Set = new ulong[maxHexes];
            H3.uncompactCells(compactedSet, compactedSet.Length, h3Set, maxHexes, resolution);
            return h3Set;
        }

        [Obsolete("Use UncompactCells instead")]
        public static ulong[] Uncompact(ulong[] compactedSet, int resolution)
        {
            return UncompactCells(compactedSet, resolution);
        }

        #endregion

        #region REGIONS

        public static ulong[] PolygonToCells(List<GeoCoord> coords, int resolution)
        {
            ulong[] indexes = null;
            var latLngs = FromGeoCoords(coords);
            GeoPolygon polygon = new GeoPolygon();
            polygon.geoloop = new GeoLoop();
            GCHandle arrHandle = GCHandle.Alloc(latLngs.ToArray(), GCHandleType.Pinned);
            List<ulong> validIndexes = new List<ulong>();
            try
            {
                polygon.geoloop.verts = arrHandle.AddrOfPinnedObject();
                polygon.geoloop.numVerts = latLngs.Length;
                polygon.numHoles = 0;
                polygon.holes = IntPtr.Zero;
                UInt32 flags = 0;
                long nbIndex = 0;
                var error = H3.maxPolygonToCellsSize(ref polygon, resolution, flags, ref nbIndex);
                indexes = new ulong[nbIndex + 1];
                H3.polygonToCells(ref polygon, resolution, flags, indexes);
                for (int i = 0; i < indexes.Length; i++)
                {
                    if (indexes[i] != 0)
                    {
                        validIndexes.Add(indexes[i]);
                    }
                }
            }
            finally
            {
                arrHandle.Free();
            }
            return validIndexes.ToArray();
        }

        [Obsolete("Use PolygonToCells instead")]
        public static ulong[] Polyfill(List<GeoCoord> coords, Int32 resolution)
        {
            return PolygonToCells(coords, resolution);
        }

        #endregion

        #region UNIDIRECTIONAL EDGES

        public static bool AreNeighborCells(ulong origin, ulong destination)
        {
            int result = 0;
            var error = H3.areNeighborCells(origin, destination, ref result);
            return result == 1;
        }

        [Obsolete("Use AreNeighborCells instead")]
        public static bool AreNeighbors(ulong origin, ulong destination)
        {
            return AreNeighborCells(origin, destination);
        }

        public static ulong CellsToDirectedEdge(ulong origin, ulong destination)
        {
            ulong edge = 0;
            var error = H3.cellsToDirectedEdge(origin, destination, ref edge);
            return edge;
        }

        [Obsolete("Use CellsToDirectedEdge instead")]
        public static long GetEdge(ulong origin, ulong destination)
        {
            return GetEdge(origin, destination);
        }

        public static bool IsValidDirectedEdge(ulong edge)
        {
            return H3.isValidDirectedEdge(edge) == 1;
        }

        [Obsolete("Use IsValidDirectedEdge instead")]
        public static bool IsValidEdge(ulong edge)
        {
            return IsValidDirectedEdge(edge);
        }

        public static ulong GetDirectedEdgeOrigin(ulong edge)
        {
            ulong origin = 0;
            var error = H3.getDirectedEdgeOrigin(edge, ref origin);
            return origin;
        }

        [Obsolete("Use GetDirectedEdgeOrigin instead")]
        public static ulong GetOrigin(ulong edge)
        {
            return GetDirectedEdgeOrigin(edge);
        }

        public static ulong GetDirectedEdgeDestination(ulong edge)
        {
            ulong destination = 0;
            var error = H3.getDirectedEdgeDestination(edge, ref destination);
            return destination;
        }

        [Obsolete("Use GetDirectedEdgeDestination instead")]
        public static ulong GetDestination(ulong edge)
        {
            return GetDirectedEdgeDestination(edge);
        }

        public static (ulong origin, ulong destination) DirectedEdgeToCells(ulong edge)
        {
            ulong[] indexes = new ulong[2];
            var error = H3.directedEdgeToCells(edge, indexes);
            return (indexes[0], indexes[1]);
        }

        [Obsolete("Use DirectedEdgeToCells instead")]
        public static (ulong origin, ulong destination) GetOriginAndDestination(ulong edge)
        {
            return DirectedEdgeToCells(edge);
        }

        public static ulong[] OriginToDirectedEdges(ulong cell)
        {
            ulong[] edges = new ulong[6];
            H3.originToDirectedEdges(cell, edges);
            return edges;
        }

        [Obsolete("Use OriginToDirectedEdges instead")]
        public static ulong[] GetEdges(ulong cell)
        {
            return OriginToDirectedEdges(cell);
        }

        public static CellBoundary DirectedEdgeToBoundary(ulong index)
        {
            CellBoundary boundary = new CellBoundary();
            var error = H3.directedEdgeToBoundary(index, ref boundary);
            return boundary;
        }

        [Obsolete("Use instead")]
        public static CellBoundary GetEdgeBoundary(ulong edge)
        {
            return DirectedEdgeToBoundary(edge);
        }

        #endregion

        #region VERTEX
        public static ulong CellToVertex(ulong origin, int vertexNum)
        {
            ulong vertex = 0;
            var error = H3.cellToVertex(origin, vertexNum, ref vertex);
            return vertex;
        }

        public static ulong[] CellToVertexes(ulong origin)
        {
            ulong[] vertexes = new ulong[6];
            var error = H3.cellToVertexes(origin, vertexes);
            return vertexes;
        }

        public static LatLng VertexToLatLng(ulong origin)
        {
            LatLng latLng = new LatLng();
            var error = H3.vertexToLatLng(origin, ref latLng);
            return latLng;
        }

        public static bool IsValidVertex(ulong origin)
        {
            return H3.isValidVertex(origin) == 1;
        }
        #endregion

        #region MISCELLANEOUS

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

        public static ulong[] GetRes0Cells()
        {
            ulong[] res0Indexes = new ulong[122];
            H3.getRes0Cells(res0Indexes);
            return res0Indexes;
        }
        [Obsolete("Use GetRes0Cells instead")]
        public static ulong[] Res0Indexes
        {
            get{ return GetRes0Cells();  }
        }


        public static ulong[] GetPentagons(int res)
        {
            ulong[] pentagons = new ulong[H3.pentagonCount()];
            H3.getPentagons( res, pentagons);
            return pentagons;
        }

        #endregion

        #region Private utility methods
        public static LatLng[] FromGeoCoords(List<GeoCoord> coords)
        {
            var h3GeoCoords = new List<LatLng>();
            foreach (var g in coords)
            {
                h3GeoCoords.Add(new LatLng(g));
            }
            return h3GeoCoords.ToArray();
        }

        private static LatLng[] Normalize(CellBoundary geoBoundary)
        {
            List<LatLng> geoCoords = new List<LatLng>();
            if (geoBoundary.verts0.lat != 0 && geoBoundary.verts0.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts0);
            }
            if (geoBoundary.verts1.lat != 0 && geoBoundary.verts1.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts1);
            }
            if (geoBoundary.verts2.lat != 0 && geoBoundary.verts2.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts2);
            }
            if (geoBoundary.verts3.lat != 0 && geoBoundary.verts3.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts3);
            }
            if (geoBoundary.verts4.lat != 0 && geoBoundary.verts4.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts4);
            }
            if (geoBoundary.verts5.lat != 0 && geoBoundary.verts5.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts5);
            }
            if (geoBoundary.verts6.lat != 0 && geoBoundary.verts6.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts6);
            }
            if (geoBoundary.verts7.lat != 0 && geoBoundary.verts7.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts7);
            }
            if (geoBoundary.verts8.lat != 0 && geoBoundary.verts8.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts8);
            }
            if (geoBoundary.verts9.lat != 0 && geoBoundary.verts9.lng != 0)
            {
                geoCoords.Add(geoBoundary.verts9);
            }
            return geoCoords.ToArray();
        }
        #endregion

    }
}