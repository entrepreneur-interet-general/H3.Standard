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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using H3Standard;

namespace H3Standard.Tests;

public static class UnitTest
{
    public static double DoubleTolerance = 0.000001;
}

[TestClass]
public class UnitTest_01_Indexing
{
    [TestMethod]
    public void TestLatLngToCell()
    {
        var latLng = new LatLng(47.7, -3);
        ulong h3Index = 0;
        var error = H3.latLngToCell(ref latLng, 10, ref h3Index);
        Assert.AreEqual(h3Index, (UInt64)621923649824456703);
    }

    [TestMethod]
    public void TestCellToLatLng()
    {
        ulong cell = 621923649824456703;
        LatLng latLng = new LatLng(0, 0);
        var error = H3.cellToLatLng(cell, ref latLng);
        Assert.AreEqual(
            ((latLng.LatWGS84 - 47.69995960804585) < UnitTest.DoubleTolerance) &&
            ((latLng.LngWGS84 + 3.000345901177671) < UnitTest.DoubleTolerance), true);
    }

    [TestMethod]
    public void TestCellToBoundary()
    {
        ulong cell = 621923649824456703;
        CellBoundary boundary = new CellBoundary();
        var error = H3.cellToBoundary(cell, ref boundary);
        Assert.AreEqual(
            boundary.numVerts == 6 &&
            (boundary.verts0.LatWGS84 - 47.70063269164244 < UnitTest.DoubleTolerance) &&
            (boundary.verts0.LngWGS84 + 3.0002084452356717 < UnitTest.DoubleTolerance)
            , true);
    }
}
