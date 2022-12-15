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
namespace H3Standard.Tests
{
	[TestClass]
	public class UnitTest_04_Hierarchy
	{
		[TestMethod]
		public void TestCellToParent()
		{
			ulong cell = 621923649824456703;
			int parentRes = 5;
			ulong parent = 0;
			var error = H3.cellToParent(cell, parentRes, ref parent);
			Assert.AreEqual(parent, (UInt64)599405651935887359);
		}

		[TestMethod]
		public void TestCellToChildren()
		{
			ulong cell = 621923649824456703;
			int childRes = 13;
			long size = 0;
			var error = H3.cellToChildrenSize(cell, childRes, ref size);
			ulong[] children = new ulong[size];
			error = H3.cellToChildren(cell, childRes, children);
			Assert.AreEqual(children[0], (UInt64)635434448706535487);
		}

		[TestMethod]
		public void TestCellToCenterChild()
		{
			ulong cell = 621923649824456703;
			int childRes = 12;
			ulong child = 0;
			var error = H3.cellToCenterChild(cell, childRes, ref child);
			Assert.AreEqual(child, (UInt64)630930849079165439);
		}

		//[TestMethod]
		//public void TestCellToChildPos()
		//{
		//	ulong cell = 621923649824456703;
		//	int parentRes = 5;
		//	long pos = 0;
		//	ulong h3Index = H3.cellToChildPos(cell, parentRes, ref pos);
		//	Assert.AreEqual(pos, 0);
		//}

		//[TestMethod]
		//public void TestChildPosToCell()
		//{
		//	long childPos = 0;
		//	ulong parent = 621923649824456703;
		//	int childRes = 13;
		//  ulong child = 0;
		//  ulong cell = H3.childPosToCell(childPos, parent, childRes, ref child);
		//	Assert.AreEqual(child, 0);
		//}

		[TestMethod]
		public void TestCompactCells()
		{
			ulong[] cellSet = new ulong[7] {
				626427249451798527,
				626427249451802623,
				626427249451806719,
				626427249451810815,
				626427249451814911,
				626427249451819007,
				626427249451823103
			};
            int numCells = cellSet.Length;
            ulong[] compactedSet = new ulong[numCells];
            var error = H3.compactCells( cellSet, compactedSet, numCells);
			Assert.AreEqual(compactedSet[0], (UInt64)621923649824456703);
        }

		[TestMethod]
		public void TestUncompactCells()
		{
			ulong[] compactedSet = new ulong[1] { 621923649824456703 };
			int numCells = compactedSet.Length;
			int res = 11;
			long maxCells = 0;
            var error = H3.uncompactCellsSize( compactedSet, numCells, res, ref maxCells);
            ulong[] cellSet = new ulong[maxCells];
            error = H3.uncompactCells(compactedSet, numCells, cellSet, maxCells, res);
			Assert.AreEqual(cellSet[0], (UInt64)626427249451798527);
        }
	}
}

