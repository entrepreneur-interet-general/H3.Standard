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
namespace H3Standard.Tests.H3Net;


[TestClass]
public class UnitTest_03_Traversal
{
    [TestMethod]
    public void TestGridDisk()
    {
        ulong cell = 621923649824456703;
        int k = 3;
        ulong[] cells = H3Standard.H3Net.GridDisk(cell, k);
        Assert.AreEqual(cells[1], (UInt64)621923649821540351);
    }

    [TestMethod]
    public void TestGridPathCells()
    {
        ulong origin = 621923649824456703;
        ulong destination = 621923649821540351;
        var path = H3Standard.H3Net.GridPathCells(origin, destination);
        Assert.AreEqual(path[1], (UInt64)621923649821540351);
    }
}

