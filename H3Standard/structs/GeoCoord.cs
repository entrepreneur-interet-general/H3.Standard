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

namespace H3Standard {
    public struct GeoCoord
    {
        public double lat;
        public double lng;

        public double LatWGS84
        {
            get { return H3Net.RadToDeg(lat); }
        }

        public double LngWGS84
        {
            get { return H3Net.RadToDeg(lng); }
        }

        public GeoCoord(double lat, double lon)
        {
            this.lat = lat;
            this.lng = lon;
        }

        public GeoCoord(LatLng coord)
        {
            lat = H3Net.RadToDeg(coord.lat);
            lng = H3Net.RadToDeg(coord.lng);
        }
    }
}



