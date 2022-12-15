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
using H3Standard;

namespace H3Standard.Tests.H3Net
{
	[TestClass]
	public class UnitTest_05_Region
	{
        [TestMethod]
        public void TestPolygonToCells()
        {
            var coords = new List<GeoCoord>();
            coords.Add(new GeoCoord(47.7, -3));
            coords.Add(new GeoCoord(46.7, -3));
            coords.Add(new GeoCoord(46.7, -4));
            coords.Add(new GeoCoord(47.7, -4));
            coords.Add(new GeoCoord(47.7, -3));
           
            var h3Indexes = H3Standard.H3Net.PolygonToCells(coords, 7);
            Assert.AreEqual(h3Indexes[0], (UInt64)608412563192938495);
        }
    }
}

