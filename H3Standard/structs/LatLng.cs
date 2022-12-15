/*
 * Copyright 2018-2022 Shom, Swail, Arnaud Ménard
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


using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace H3Standard
{

    [StructLayout(LayoutKind.Sequential)]
    public struct LatLng
    {
        public double lat;  ///< latitude in radians
        public double lng;  ///< longitude in radians

        public double LatWGS84
        {
            get { return H3Net.RadToDeg(this.lat); }
            set { this.lat = H3Net.DegToRad(value); }
        }

        public double LngWGS84
        {
            get { return H3Net.RadToDeg(this.lng); }
            set { this.lng = H3Net.DegToRad(value); }
        }

        public LatLng(GeoCoord coord)
        {
            lat = H3Net.DegToRad(coord.lat);
            lng = H3Net.DegToRad(coord.lng);
        }

        public LatLng(double lat, double lng)
        {
            this.lat = H3Net.DegToRad(lat);
            this.lng = H3Net.DegToRad(lng);
        }

        /// <summary>
        /// Builds a LatLng array from an array [lat,lng] of WGS84 coords
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static LatLng[] FromDoubleArray(double[][] coords)
        {
            var latLngs = new List<LatLng>();
            foreach (var coord in coords)
            {
                var latLng = new LatLng(coord[0], coord[1]);
                latLngs.Add(latLng);
            }
            return latLngs.ToArray();
        }
    }
}

