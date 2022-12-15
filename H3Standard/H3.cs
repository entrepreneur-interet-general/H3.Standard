/*
 * Copyright 2018 - 2022, Shom, Swail, Arnaud Ménard
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
using System.Runtime.InteropServices;

namespace H3Standard
{
    /// <summary>
    /// H3 static extern declarations
    /// </summary>
    public static class H3
    {

#if _WINDOWS
    private const string LIBRARY_NAME = "h3.dll";
#else
        private const string LIBRARY_NAME = "h3";
#endif

        #region INDEXING

        // INDEXING : https://uber.github.io/h3/#/documentation/api-reference/indexing, https://h3geo.org/docs/api/indexing

        // H3Error latLngToCell(const LatLng* g, int res, H3Index *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint latLngToCell(ref LatLng g, int res, ref ulong h3Index);

        // H3Error cellToLatLng(H3Index cell, LatLng* g);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToLatLng(ulong cell, ref LatLng g);

        // H3Error cellToBoundary(H3Index cell, CellBoundary *bndry);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToBoundary(ulong cell, ref CellBoundary bndry);

        #endregion

        #region INSPECTION

        // INSPECTION : https://uber.github.io/h3/#/documentation/api-reference/inspection, https://h3geo.org/docs/api/inspection

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getResolution(ulong h3);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getBaseCellNumber(ulong h);

        // H3Error stringToH3(const char *str, H3Index *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint stringToH3(byte[] str, ref ulong h3Index);

        // H3Error h3ToString(H3Index h, char* str, size_t sz);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint h3ToString(ulong h, byte[] str, int size);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isValidCell(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isResClassIII(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isPentagon(ulong h);

        // H3Error getIcosahedronFaces(H3Index h, int* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint getIcosahedronFaces(ulong h, int[] faces);

        // H3Error maxFaceCount(H3Index h3, int *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint maxFaceCount(ulong h3, ref int count);

        #endregion

        #region TRAVERSAL

        // TRAVERSAL : https://uber.github.io/h3/#/documentation/api-reference/neighbors, https://h3geo.org/docs/api/traversal

        // H3Error gridDisk(H3Index origin, int k, H3Index* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDisk(ulong origin, int k, ulong[] cells);

        // H3Error maxGridDiskSize(int k, int64_t *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint maxGridDiskSize(int k, ref long size);

        // H3Error gridDiskDistances(H3Index origin, int k, H3Index* out, int* distances);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDiskDistances(ulong origin, int k, ulong[] h3Indices, int[] distances);

        // H3Error gridDiskUnsafe(H3Index origin, int k, H3Index* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDiskUnsafe(ulong origin, int k, ulong[] h3Indices);

        // H3Error gridDiskDistancesUnsafe(H3Index origin, int k, H3Index* out, int* distances);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDiskDistancesUnsafe(ulong origin, int k, ulong[] h3Indices, int[] distances);

        // H3Error gridDiskDistancesSafe(H3Index origin, int k, H3Index* out, int* distances);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDiskDistancesSafe(ulong origin, int k, ulong[] h3Indices, int[] distances);

        // H3Error gridDisksUnsafe(H3Index* h3Set, int length, int k, H3Index* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDisksUnsafe(ulong[] h3Set, int length, int k, ulong[] h3Indices);

        // H3Error gridRingUnsafe(H3Index origin, int k, H3Index* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridRingUnsafe(ulong[] h3Set, int length, int k, ulong[] h3Indices);

        // H3Error gridPathCells(H3Index start, H3Index end, H3Index* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridPathCells(ulong start, ulong end, ulong[] h3Indices);

        // H3Error gridDistance(H3Index origin, H3Index h3, int64_t *distance);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridPathCellsSize(ulong start, ulong end, ref long size);

        // H3Error gridDistance(H3Index origin, H3Index h3, int64_t* distance);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint gridDistance(ulong origin, ulong destination, ref long distance);

        // cellToLocalIj : /* H3Error */ uint cellToLocalIj(ulong origin, ulong h3, uint32_t mode, CoordIJ* out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToLocalIj(ulong origin, ulong destination, int mode, ref CoordIJ coordIJ);

        // localIjToCell : /* H3Error */ uint localIjToCell(ulong origin, const CoordIJ *ij, uint32_t mode, ulong* out);

        #endregion

        #region HIERARCHY

        // HIERARCHY : https://uber.github.io/h3/#/documentation/api-reference/hierarchy, https://h3geo.org/docs/api/hierarchy

        // H3Error cellToParent(H3Index cell, int parentRes, H3Index *parent);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToParent(ulong cell, int parentRes, ref ulong parent);

        // H3Error cellToChildren(H3Index cell, int childRes, H3Index* children);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToChildren(ulong cell, int childRes, ulong[] children);

        // H3Error cellToChildrenSize(H3Index cell, int childRes, int64_t *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToChildrenSize(ulong cell, int childRes, ref long size);

        // H3Error cellToCenterChild(H3Index cell, int childRes, H3Index *child);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToCenterChild(ulong h, int childRes, ref ulong child);

        // cellToChildPos : H3Index cellToChildPos(H3Index child, int parentRes, int64_t *out);
        //[DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong cellToChildPos(ulong h, int childRes, ref long pos);

        // childPosToCell : H3Index childPosToCell(int64_t childPos, H3Index parent, int childRes, H3Index *child);
        //[DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong childPosToCell(long childPos, ulong parent, int childRes, ref ulong child);

        // H3Error compactCells(const H3Index* cellSet, H3Index *compactedSet, const int64_t numCells);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint compactCells(ulong[] cellSet, ulong[] compactedSet, long numCells);

        // H3Error uncompactCells(const H3Index *compactedSet, const int64_t numCells, H3Index *cellSet, const int64_t maxCells, const int res);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint uncompactCells(ulong[] compactedSet, int numCells, ulong[] cellSet, long maxCells, int res);

        // H3Error uncompactCellsSize(const H3Index *compactedSet, const int64_t numCompacted, const int res, int64_t *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint uncompactCellsSize(ulong[] compactedSet, int numCompacted, int res, ref long size);

        #endregion

        #region REGION

        // REGION : https://uber.github.io/h3/#/documentation/api-reference/regions, https://h3geo.org/docs/api/regions

        // H3Error polygonToCells(const GeoPolygon* geoPolygon, int res, uint32_t flags, H3Index*out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint polygonToCells(ref GeoPolygon geoPolygon, int res, uint flags, ulong[] h3Indices);

        // H3Error maxPolygonToCellsSize(const GeoPolygon *geoPolygon, int res, uint32_t flags, int64_t *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint maxPolygonToCellsSize(ref GeoPolygon geoPolygon, int res, uint flags, ref long size);

        // H3Error cellsToLinkedMultiPolygon(const H3Index *h3Set, const int numHexes, LinkedGeoPolygon *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellsToLinkedMultiPolygon(ulong[] h3Set, int numHexes, ref LinkedGeoPolygon linkedGeos);

        // void destroyLinkedMultiPolygon(LinkedGeoPolygon *polygon);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroyLinkedMultiPolygon(ref LinkedGeoPolygon polygon);

        #endregion

        #region UNIDIRECTIONAL EDGES

        // UNIDIRECTIONAL EDGES : https://uber.github.io/h3/#/documentation/api-reference/unidirectional-edges, https://h3geo.org/docs/api/uniedge

        // H3Error areNeighborCells(H3Index origin, H3Index destination, int*out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint areNeighborCells(ulong origin, ulong destination, ref int result);

        // H3Error cellsToDirectedEdge(H3Index origin, H3Index destination, H3Index *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellsToDirectedEdge(ulong origin, ulong destination, ref ulong h3Index);

        // int isValidDirectedEdge(H3Index edge);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isValidDirectedEdge(ulong edge);

        // H3Error getDirectedEdgeOrigin(H3Index edge, H3Index *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint getDirectedEdgeOrigin(ulong edge, ref ulong origin);

        // H3Error getDirectedEdgeDestination(H3Index edge, H3Index *out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint getDirectedEdgeDestination(ulong edge, ref ulong destination);

        // H3Error directedEdgeToCells(H3Index edge, H3Index* originDestination);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint directedEdgeToCells(ulong edge, ulong[] originDestination);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint originToDirectedEdges(ulong origin, ulong[] edges);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint directedEdgeToBoundary(ulong edge, ref CellBoundary gb);

        #endregion

        #region VERTEX

        // VERTEX
        // /* H3Error */ uint cellToVertex(H3Index origin, int vertexNum, H3Index*out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToVertex(ulong origin, int vertexNum, ref ulong vertex);

        // /* H3Error */ uint cellToVertexes(H3Index origin, H3Index*out);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint cellToVertexes(ulong origin, ulong[] vertexes);

        // /* H3Error */ uint vertexToLatLng(H3Index vertex, LatLng *point);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* H3Error */ uint vertexToLatLng(ulong origin, ref LatLng latLng);

        // int isValidVertex(H3Index vertex);
        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isValidVertex(ulong origin);

        #endregion

        #region MISCELLANEOUS

        // MISCELLANEOUS : https://uber.github.io/h3/#/documentation/api-reference/miscellaneous, https://h3geo.org/docs/api/misc

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double degsToRad(double degrees);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double radsToDeg(double radians);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getHexagonAreaAvgKm2(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getHexagonAreaAvgM2(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double cellAreaRads2(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double cellAreaKm2(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double cellAreaM2(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getHexagonEdgeLengthAvgKm(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getHexagonEdgeLengthAvgM(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double edgeLengthKm(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double edgeLengthM(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double edgeLengthRads(ulong h);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong getNumCells(int res);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getRes0Cells(ulong[] hexagons);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int res0CellCount();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void getPentagons(int res, ulong[] hexagons);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pentagonCount();

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double greatCircleDistanceKm(ref LatLng a, ref LatLng b);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double greatCircleDistancM(ref LatLng a, ref LatLng b);

        [DllImport(LIBRARY_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double greatCircleDistanceRads(ref LatLng a, ref LatLng b);

        #endregion
    }
}
