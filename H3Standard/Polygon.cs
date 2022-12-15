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
    public class Polygon : IDisposable
    {
        private LatLng[] latLngs;
        private GCHandle arrHandle;
        public GeoPolygon geoPolygon;

        public Polygon(double[][] coords)
        {
            this.latLngs = LatLng.FromDoubleArray(coords);
            this.arrHandle = GCHandle.Alloc(this.latLngs, GCHandleType.Pinned);
            this.geoPolygon = GetGeoPolygon(this.arrHandle, latLngs.Length);
        }

        public GeoPolygon GeoPolygon
        {
            get { return this.geoPolygon; }
        }

        private GCHandle GetHandle(LatLng[] latLngs)
        {
            return GCHandle.Alloc(latLngs, GCHandleType.Pinned);
        }

        private GeoPolygon GetGeoPolygon(GCHandle arrHandle, int nbLatLngs)
        {
            var geoPolygon = new GeoPolygon();
            geoPolygon.geoloop = new GeoLoop();
            geoPolygon.geoloop.verts = arrHandle.AddrOfPinnedObject();
            geoPolygon.geoloop.numVerts = nbLatLngs;
            geoPolygon.numHoles = 0;
            geoPolygon.holes = IntPtr.Zero;
            return geoPolygon;
        }

        public void Dispose()
        {
            arrHandle.Free();
        }
    }

}

