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
using System.Runtime.InteropServices;
using System.Text;

namespace H3Standard.Tests.H3Net;

[TestClass]
public class UnitTest_02_Inspection
{
    [TestMethod]
    public void TestGetResolution()
    {
        ulong cell = 621923649824456703;
        var resolution =  H3Standard.H3Net.GetResolution(cell);
        Assert.AreEqual(resolution, 10);
    }

    [TestMethod]
    public void TestGetBaseCellNumber()
    {
        ulong cell = 621923649824456703;
        var baseCellNumber = H3Standard.H3Net.GetBaseCellNumber(cell);
        Assert.AreEqual(baseCellNumber, 12);
    }

    [TestMethod]
    public void TestStringToH3()
    {
        var str = "8a18443b1337fff";
        ulong cell = H3Standard.H3Net.StringToH3(str);
        Assert.AreEqual(cell, (UInt64)621923649824456703);
    }

    [TestMethod]
    public void TestH3ToString()
    {
        ulong cell = 621923649824456703;
        string s = H3Standard.H3Net.H3ToString(cell);
        Assert.AreEqual(s, "8a18443b1337fff");
    }

    [TestMethod]
    public void TestIsValidCell()
    {
        ulong cell = 621923649824456703;
        var result = H3Standard.H3Net.IsValidCell(cell);
        Assert.AreEqual(result, true);
    }

    [TestMethod]
    public void TestIsResClassIII()
    {
        ulong cell = 621923649824456703;
        var result = H3Standard.H3Net.IsResClassIII(cell);
        Assert.AreEqual(result, false);
    }

    [TestMethod]
    public void TestIsPentagon()
    {
        ulong cell = 621923649824456703;
        var result = H3Standard.H3Net.IsPentagon(cell);
        Assert.AreEqual(result, false);
    }

    [TestMethod]
    public void TestGetIcosahedronFaces()
    {
        ulong cell = 621923649824456703;
        var faces = H3Standard.H3Net.GetIcosahedronFaces(cell);
        Assert.AreEqual(faces[0], 3);
    }
}

