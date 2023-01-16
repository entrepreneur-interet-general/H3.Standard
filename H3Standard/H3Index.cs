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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace H3Standard
{
    public class H3Index : IEquatable<H3Index>, IEquatable<ulong>, IComparable<H3Index>
    {
        private ulong _h3Index;

        internal static readonly int MAX_H3_RES = 15;
        internal static readonly int H3_DIGIT_OFFSET = 3;
        internal static readonly ulong H3_DIGIT_MASK = 7;

        public H3Index(double lat, double lon, int resolution)
        {
            this._h3Index = H3Net.LatLngToCell(lat, lon, resolution);
        }

        public H3Index(ulong h3Index)
        {
            this._h3Index = h3Index;
        }

        public H3Index(LatLng latLng, int resolution)
        {
            this._h3Index = H3Net.LatLngToCell(latLng, resolution);
        }

        public H3Index(string h3Index)
        {
            this._h3Index = H3Net.StringToH3(h3Index);
        }

        public ulong Value
        {
            get { return _h3Index; }
        }

        public int Resolution
        {
            get
            {
                return H3Net.GetResolution(this._h3Index);
            }
        }

        public bool IsValidCell
        {
            get
            {
                return H3Net.IsValidCell(this._h3Index);
            }
        }

        public bool IsValidDirectedEdge
        {
            get
            {
                return H3Net.IsValidDirectedEdge(this._h3Index);
            }
        }


        public bool IsValidVertex
        {
            get
            {
                return H3Net.IsValidVertex(this._h3Index);
            }
        }

        public bool IsResClassIII
        {
            get
            {
                return H3Net.IsResClassIII(this._h3Index);
            }
        }

        public bool IsPentagon
        {
            get
            {
                return H3Net.IsPentagon(this._h3Index);
            }
        }

        public int[] Faces
        {
            get
            {
                var faces = H3Net.GetIcosahedronFaces(this._h3Index);
                var filteredFaces = faces.Where(f => f != -1).ToArray();
                return filteredFaces;
            }
        }

        public H3Index[] Neighbors
        {
            get
            {
                var neighbors = H3Net.GridDisk(this._h3Index, 1).ToArray();
                var filteredNeighbors = new List<H3Index>();
                foreach (H3Index neighbor in neighbors)
                {
                    if (neighbor != 0 && neighbor != this._h3Index)
                    {
                        filteredNeighbors.Add(neighbor);
                    }
                }
                return filteredNeighbors.ToArray();
            }
        }

        public ulong BaseCell
        {
            get { return H3Net.CellToParent(this._h3Index, 0); }
        }

        public int GetIndexDigit(int resolution)
        {
            return (int)((this._h3Index >> ((MAX_H3_RES - resolution) * H3_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        public List<H3Index> GridDisk(int k)
        {
            return H3Net.GridDisk(this._h3Index, k).Select(h => { return new H3Index(h); }).ToList<H3Index>();
        }

        public List<List<H3Index>> GridDiskDistances(int k)
        {
            var gridDiskDistances = H3Net.GridDiskDistances(this._h3Index, k);
            var list = new List<List<H3Index>>();
            foreach (var gridDiskDistance in gridDiskDistances)
            {
                var distanceList = gridDiskDistance.Select(h => { return new H3Index(h); }).ToList<H3Index>();
                list.Add(distanceList);
            }
            return list;
        }

        public List<H3Index> GridDiskUnsafe(int k)
        {
            return H3Net.GridDiskUnsafe(this._h3Index, k).Select(h => { return new H3Index(h); }).ToList<H3Index>();
        }

        public List<List<H3Index>> GridDiskDistancesUnsafe(int k)
        {
            var gridDiskDistances = H3Net.GridDiskDistancesUnsafe(this._h3Index, k);
            var list = new List<List<H3Index>>();
            foreach (var gridDiskDistance in gridDiskDistances)
            {
                var distanceList = gridDiskDistance.Select(h => { return new H3Index(h); }).ToList<H3Index>();
                list.Add(distanceList);
            }
            return list;
        }

        public List<H3Index> PathTo(H3Index destination)
        {
            return H3Net.GridPathCells(this._h3Index, destination).Select(h => { return (H3Index)h; }).ToList<H3Index>();
        }

        public long DistanceTo(H3Index destination)
        {
            return H3Net.GridDistance(this._h3Index, destination);
        }

        public H3Index Parent(int resolution = -1)
        {
            return (H3Index)H3Net.CellToParent(this._h3Index, resolution);
        }

        public H3Index CenterChild(int resolution = -1)
        {
            if (resolution != -1 && resolution <= this.Resolution)
            {
                return (H3Index)0;
            }
            return (H3Index)H3Net.CellToCenterChild(this._h3Index, resolution);
        }

        public List<H3Index> Children(int resolution)
        {
            if (resolution <= this.Resolution || !this.IsValidCell)
            {
                return null;
            }
            return H3Net.CellToChildren(this._h3Index, resolution).Select(h => { return (H3Index)h; }).ToList<H3Index>();
        }

        public List<H3Index> Edges
        {
            get
            {
                return H3Net.OriginToDirectedEdges(this._h3Index)
                    .Where(h => h != 0)
                    .Select(h => { return (H3Index)h; })
                    .ToList<H3Index>();
            }
        }

        public LatLng Center
        {
            get
            {
                return H3Net.CellToLatLng(this._h3Index);
            }
        }

        public LatLng[] Boundaries
        {
            get
            {
                return H3Net.CellToBoundary(this._h3Index);
            }
        }

        public H3Index? Destination
        {
            get
            {
                if (IsValidDirectedEdge)
                {
                    return new H3Index(H3Net.GetDirectedEdgeDestination(this._h3Index));
                }
                return null;
            }
        }

        public H3Index? Origin
        {
            get
            {
                if (IsValidDirectedEdge)
                {
                    return new H3Index(H3Net.GetDirectedEdgeOrigin(this._h3Index));
                }
                return null;
            }
        }

        public List<H3Index> Vertexes
        {
            get
            {
                return H3Net.CellToVertexes(this._h3Index)
                    .Where(h => h != 0)
                    .Select(h => { return (H3Index)h; })
                    .ToList<H3Index>();
            }
        }

        public H3Index Vertex(int vertexNum )
        {
            return (H3Index)H3Net.CellToVertex(this._h3Index, vertexNum);
        }

        public bool OverAntiMeridian
        {
            get
            {
                var latLngs = Boundaries;
                bool throughAntiMeridian = false;
                bool hasNegativeLonLimits = false;
                bool hasPositiveLonLimits = false;
                foreach (var latLng in latLngs)
                {
                    if (latLng.LngWGS84 > 120)
                    {
                        hasPositiveLonLimits = true;
                    }
                    if (latLng.LngWGS84 < -120)
                    {
                        hasNegativeLonLimits = true;
                    }
                }
                if (hasNegativeLonLimits && hasPositiveLonLimits)
                {
                    throughAntiMeridian = true;
                }
                return throughAntiMeridian;
            }
        }

        public string BoundariesWKT
        {
            get
            {
                var boundaries = Boundaries;
                var sb = new StringBuilder();
                sb.Append("POLYGON((");
                bool first = true;
                bool throughAntiMeridian = false;
                bool hasNegativeLonLimits = false;
                bool hasPositiveLonLimits = false;
                foreach (var boundary in boundaries)
                {
                    if (boundary.LngWGS84 > 120)
                    {
                        hasPositiveLonLimits = true;
                    }
                    if (boundary.LngWGS84 < -120)
                    {
                        hasNegativeLonLimits = true;
                    }
                }
                if (hasNegativeLonLimits && hasPositiveLonLimits)
                {
                    throughAntiMeridian = true;
                }
                foreach (var boundary in boundaries)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    var lon = throughAntiMeridian && boundary.LngWGS84 < -120 ? boundary.LngWGS84 + 360 : boundary.LngWGS84;
                    sb.Append(lon + " " + boundary.LatWGS84);
                }
                sb.Append("))");
                return sb.ToString();
            }
        }

        public string EdgeWKT
        {
            get
            {
                if (IsValidDirectedEdge)
                {
                    var origin = this.Origin;
                    var destination = this.Destination;
                    var originLatLng = origin.Center;
                    var destinationLatLng = destination.Center;
                    if (originLatLng.LngWGS84 < -120 && destinationLatLng.LngWGS84 > 120)
                    {
                        originLatLng.LngWGS84 += 360;
                    }
                    else if (originLatLng.LngWGS84 > 120 && destinationLatLng.LngWGS84 < -120)
                    {
                        destinationLatLng.LngWGS84 += 360;
                    }
                    var sb = new StringBuilder();
                    sb.Append("LINESTRING( ");
                    sb.Append(originLatLng.LngWGS84);
                    sb.Append(" ");
                    sb.Append(originLatLng.LatWGS84);
                    sb.Append(",");
                    sb.Append(destinationLatLng.LngWGS84);
                    sb.Append(" ");
                    sb.Append(destinationLatLng.LatWGS84);
                    sb.Append(" )");
                    return sb.ToString();
                }
                return null;
            }
        }

        public bool IsNeighborWith(H3Index h3Index)
        {
            return H3Net.AreNeighborCells(this._h3Index, h3Index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is H3Index)
            {
                return this._h3Index == ((H3Index)obj)._h3Index;
            }
            if (obj is ulong)
            {
                return this._h3Index == (ulong)obj;
            }
            return base.Equals(obj);
        }

        public bool Equals(H3Index other)
        {
            return this._h3Index == other._h3Index;
        }

        public bool Equals(ulong other)
        {
            return this._h3Index == other;
        }

        public int CompareTo(H3Index other)
        {
            return this._h3Index.CompareTo(other._h3Index);
        }

        public static bool operator ==(H3Index h1, H3Index h2)
        {
            return h1.Value == h2.Value;
        }

        public static bool operator !=(H3Index h1, H3Index h2)
        {
            return h1.Value != h2.Value;
        }

        public override string ToString()
        {
            return H3Net.H3ToString(this._h3Index);
        }

        public static implicit operator ulong(H3Index h) => h._h3Index;
        public static implicit operator string(H3Index h) => H3Net.H3ToString(h._h3Index);
        public static implicit operator H3Index(ulong h) => new H3Index(h);

        public override int GetHashCode()
        {
            return _h3Index.GetHashCode();
        }
    }

}
