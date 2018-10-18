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


public struct GeoCoord
{
    public double latitude;
    public double longitude;

    public GeoCoord(double lat, double lon)
    {
        latitude = lat;
        longitude = lon;
    }

    public GeoCoord(H3GeoCoord coord)
    {
        latitude = H3Standard.H3.RadToDeg(coord.lat);
        longitude = H3Standard.H3.RadToDeg(coord.lon);
    }
}

