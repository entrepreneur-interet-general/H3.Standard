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

using System;
using System.Runtime.InteropServices;
using H3Standard;

namespace H3Standard
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GeoPolygon
    {
        public GeoLoop geoloop;     ///< exterior boundary of the polygon
        public int numHoles;        ///< number of elements in the array pointed to by holes
        public IntPtr holes;        // GeoCoord[] ///< interior boundaries (holes) in the polygon
    }

}


