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
	public class UnitTest_06_Edges
	{
		[TestMethod]
		public void TestAreNeighborCells()
		{
			ulong origin = 621923649824456703;
			ulong destination = 621923649824260095;
			int result = 0;
            var error = H3.areNeighborCells(origin, destination, ref result);
			Assert.AreEqual(result, 1);
        }

		[TestMethod]
		public void TestCellsToDirectedEdge()
		{
			ulong origin = 621923649824456703;
			ulong destination = 621923649824260095;
			ulong edge = 0;
            var error = H3.cellsToDirectedEdge(origin, destination, ref edge);
			Assert.AreEqual(edge, (UInt64)1270441996165808127);
        }

		[TestMethod]
		public void TestIsValidDirectedEdge()
		{
			ulong edge = 1270441996165808127;
			int result = H3.isValidDirectedEdge(edge);
			Assert.AreEqual(result, 1);
		}

		[TestMethod]
		public void TestGetDirectedEdgeOrigin()
		{
			ulong edge = 1270441996165808127;
			ulong origin = 0;
            var error = H3.getDirectedEdgeOrigin(edge, ref origin);
			Assert.AreEqual(origin, (UInt64)621923649824456703);
        }

		[TestMethod]
		public void TestGetDirectedEdgeDestination()
        {
			ulong edge = 1270441996165808127;
			ulong destination = 0;
            var error = H3.getDirectedEdgeDestination(edge, ref destination);
			Assert.AreEqual(destination, (UInt64)621923649824260095);
		}

		[TestMethod]
		public void TestDirectedEdgeToCells()
		{
			ulong edge = 1270441996165808127;
			ulong[] originDestination = new ulong[2];
			var error = H3.directedEdgeToCells(edge, originDestination);
			Assert.AreEqual(originDestination[0], (UInt64)621923649824456703);
			Assert.AreEqual(originDestination[1], (UInt64)621923649824260095);
        }

		[TestMethod]
		public void TestOriginToDirectedEdges()
		{
			ulong origin = 621923649824456703;
			ulong[] edges = new ulong[6];
            var error = H3.originToDirectedEdges(origin, edges);
			Assert.AreEqual(edges[0], (UInt64)1270441996165808127);
        }

        [TestMethod]
		public void TestDirectedEdgeBoundary()
		{
			ulong edge = 1270441996165808127;
			CellBoundary cellBoundary = new CellBoundary();
			var error = H3.directedEdgeToBoundary(edge, ref cellBoundary);
            Assert.AreEqual(cellBoundary.numVerts, 2);
            Assert.AreEqual(cellBoundary.verts0.LatWGS84 - 47.699286520432835 < UnitTest.DoubleTolerance, true);
            Assert.AreEqual(cellBoundary.verts0.LngWGS84 + 3.0004833543571734 < UnitTest.DoubleTolerance, true);
        }
    }
}

