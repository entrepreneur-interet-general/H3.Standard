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

namespace H3Standard.Tests;

[TestClass]
public class UnitTest_02_Inspection
{
    [TestMethod]
    public void TestGetResolution()
    {
        ulong cell = 621923649824456703;
        var resolution =  H3.getResolution(cell);
        Assert.AreEqual(resolution, 10);
    }

    [TestMethod]
    public void TestGetBaseCellNumber()
    {
        ulong cell = 621923649824456703;
        var baseCellNumber = H3.getBaseCellNumber(cell);
        Assert.AreEqual(baseCellNumber, 12);
    }

    [TestMethod]
    public void TestStringToH3()
    {
        var str = "8a18443b1337fff";
        ulong cell = 0;
        var error = H3.stringToH3(Encoding.Default.GetBytes(str), ref cell);
        Assert.AreEqual(cell, (UInt64)621923649824456703);
    }

    [TestMethod]
    public void TestH3ToString()
    {
        ulong cell = 621923649824456703;
        byte[] bytes = new byte[17];
        int size = 0;
        var error = H3.h3ToString(cell, bytes, size);
        string s = Encoding.UTF8.GetString(bytes);
    }

    [TestMethod]
    public void TestIsValidCell()
    {
        ulong cell = 621923649824456703;
        var result = H3.isValidCell(cell);
        Assert.AreEqual(result, 1);
    }

    [TestMethod]
    public void TestIsResClassIII()
    {
        ulong cell = 621923649824456703;
        var result = H3.isResClassIII(cell);
        Assert.AreEqual(result, 0);
    }

    [TestMethod]
    public void TestIsPentagon()
    {
        ulong cell = 621923649824456703;
        var result = H3.isPentagon(cell);
        Assert.AreEqual(result, 0);
    }

    [TestMethod]
    public void TestGetIcosahedronFaces()
    {
        ulong cell = 621923649824456703;
        int count = 0;
        var error = H3.maxFaceCount(cell, ref count);
        int[] faces = new int[5];
        error = H3.getIcosahedronFaces(cell, faces);
        Console.WriteLine(faces[0]);
        Assert.AreEqual(faces[0], 3);
    }

    [TestMethod]
    public void TestMaxFaceCount()
    {
        ulong cell = 621923649824456703;
        int count = 0;
        var error = H3.maxFaceCount(cell, ref count);
        Assert.AreEqual(count, 2);
    }
}

