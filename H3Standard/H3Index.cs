using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H3Standard
{
    public struct H3Index : IEquatable<H3Index>, IEquatable<ulong>, IComparable<H3Index>
    {
        private ulong _h3Index;

        internal static readonly int MAX_H3_RES = 15;
        internal static readonly int H3_DIGIT_OFFSET = 3;
        internal static readonly ulong H3_DIGIT_MASK = 7;

        public H3Index(ulong h3Index)
        {
            this._h3Index = h3Index;
        }

        public ulong Value
        {
            get { return _h3Index; }
        }

        public int Resolution
        {
            get
            {
                return H3.h3GetResolution(this._h3Index);
            }
        }

        public bool IsValid
        {
            get
            {
                return H3.IsValid(this._h3Index);
            }
        }

        public bool IsEdge
        {
            get
            {
                return H3.IsValidEdge(this._h3Index);
            }
        }

        public bool IsResClassIII
        {
            get
            {
                return H3.IsResClassIII(this._h3Index);
            }
        }

        public bool IsPentagon
        {
            get {
                return H3.IsPentagon(this._h3Index);
            }
        }

        public int[] Faces
        {
            get
            {
                return H3.GetFaces(this._h3Index);
            }
        }

        public ulong[] Neighbors
        {
            get { return H3.GetKRing(this._h3Index, 1); }
        }

        public ulong BaseCell
        {
            get { return H3.H3ToParent(this._h3Index, 0);  }
        }

        public int GetIndexDigit(int resolution)
        {
            return (int)((this._h3Index >> ((MAX_H3_RES - resolution) * H3_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        public List<H3Index> KRing(int k)
        {
            return H3.GetKRing(this._h3Index, k).Select(h => { return (H3Index)h; }).ToList<H3Index>();
        }

        public List<H3Index> LineTo(H3Index destination)
        {
            return H3.GetLine(this._h3Index, destination).Select(h => { return (H3Index)h; }).ToList<H3Index>();
        }

        public int DistanceTo(H3Index destination)
        {
            return H3.H3Distance(this._h3Index, destination);
        }

        public H3Index ToParent(int resolution)
        {
            return (H3Index)H3.H3ToParent(this._h3Index, resolution);
        }

        public List<H3Index> Children(int resolution)
        {
            return H3.GetChildren(this._h3Index, resolution).Select(h => { return (H3Index)h; }).ToList<H3Index>();
        }

        public List<H3Index> Edges
        {
            get {
                return H3.GetEdges(this._h3Index).Select(h => { return (H3Index)h; }).ToList<H3Index>();
            }
        }

        public (double latitude, double longitude) Center
        {
            get {
                return H3.GetCenter(this._h3Index);
            }
        }

        public double[][] Boundaries
        {
            get {
                var boundaries = H3.GetBoundaries(this._h3Index);
                return boundaries.coords.ToArray();
            }
        }

        public H3Index? Destination
        {
            get
            {
                if (H3.IsValidEdge(this._h3Index))
                {
                    return new H3Index( H3.GetDestination(this._h3Index) );
                }
                return null;
            }
        }

        public H3Index? Origin
        {
            get
            {
                if (H3.IsValidEdge(this._h3Index))
                {
                    return new H3Index(H3.GetOrigin(this._h3Index));
                }
                return null;
            }
        }

        public bool OverAntiMeridian
        {
            get {
                var boundaries = Boundaries;
                bool throughAntiMeridian = false;
                bool hasNegativeLonLimits = false;
                bool hasPositiveLonLimits = false;
                foreach (var boundary in boundaries)
                {
                    if (boundary[1] > 120)
                    {
                        hasPositiveLonLimits = true;
                    }
                    if (boundary[1] < -120)
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
            get {
                var boundaries = Boundaries;
                var sb = new StringBuilder();
                sb.Append("POLYGON((");
                bool first = true;
                bool throughAntiMeridian = false;
                bool hasNegativeLonLimits = false;
                bool hasPositiveLonLimits = false;
                foreach (var boundary in boundaries)
                {
                    if (boundary[1] > 120)
                    {
                        hasPositiveLonLimits = true;
                    }
                    if (boundary[1] < -120)
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
                    var lon = throughAntiMeridian ?
                        boundary[1] < -120 ? boundary[1] + 360 : boundary[1] :
                        boundary[1];
                    sb.Append(lon + " " + boundary[0]);
                }
                sb.Append("))");
                return sb.ToString();
            }
        }

        public bool IsNeighborWith(H3Index h3Index)
        {
            return H3.AreNeighbors(this._h3Index, h3Index);
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

        public static implicit operator ulong(H3Index h) => h._h3Index;
        public static implicit operator string(H3Index h) => H3.H3ToString(h._h3Index);
        public static explicit operator H3Index(ulong h) => new H3Index(h);
    }

}
