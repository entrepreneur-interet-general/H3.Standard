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

namespace H3Standard.Tests
{
	[TestClass]
	public class UnitTest_05_Region
	{
		[TestMethod]
		public void TestPolygonToCells()
		{
			Polygon polygon = new Polygon(new double[][] {
				new double[] { 47.7, -3},
				new double[] { 46.7, -3},
				new double[] { 46.7, -4},
				new double[] { 47.7, -4},
				new double[] { 47.7, -3}
			});

			Int32 res = 7;
			UInt32 flags = 0;
			long size = 0;
			var error = H3.maxPolygonToCellsSize(ref polygon.geoPolygon, res: res, flags: flags, size: ref size);
			ulong[] h3Indices = new ulong[size];
            error = H3.polygonToCells(ref polygon.geoPolygon, res, flags, h3Indices);
			Assert.AreEqual(h3Indices[2], (UInt64)608412563192938495);
        }

		[TestMethod]
		public void TestCellsToLinkedMultiPolygon()
		{
			ulong[] h3Set = new ulong[] { 621923649824456703, 621923649824260095 };
			int numHexes = h3Set.Length;
			LinkedGeoPolygon linkedGeos = new LinkedGeoPolygon();
            var error = H3.cellsToLinkedMultiPolygon(h3Set, numHexes, ref linkedGeos);
			//H3.destroyLinkedMultiPolygon(linkedGeos);
			Assert.AreEqual(linkedGeos.first.first.vertex.lat,0);
        }
    }
}

