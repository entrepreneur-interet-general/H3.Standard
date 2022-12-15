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
public class UnitTest_06_Edges
{
	[TestMethod]
	public void TestAreNeighborCells()
	{
		ulong origin = 621923649824456703;
		ulong destination = 621923649824260095;
		bool result = H3Standard.H3Net.AreNeighborCells(origin, destination);
		Assert.AreEqual(result, true);
        }

	[TestMethod]
	public void TestCellsToDirectedEdge()
	{
		ulong origin = 621923649824456703;
		ulong destination = 621923649824260095;
		ulong edge = H3Standard.H3Net.CellsToDirectedEdge(origin, destination);
		Assert.AreEqual(edge, (UInt64)1270441996165808127);
        }

	[TestMethod]
	public void TestIsValidDirectedEdge()
	{
		ulong edge = 1270441996165808127;
		bool result = H3Standard.H3Net.IsValidDirectedEdge(edge);
		Assert.AreEqual(result, true);
	}

	[TestMethod]
	public void TestGetDirectedEdgeOrigin()
	{
		ulong edge = 1270441996165808127;
		ulong origin = H3Standard.H3Net.GetDirectedEdgeOrigin(edge);
		Assert.AreEqual(origin, (UInt64)621923649824456703);
        }

	[TestMethod]
	public void TestGetDirectedEdgeDestination()
        {
		ulong edge = 1270441996165808127;
		ulong destination = H3Standard.H3Net.GetDirectedEdgeDestination(edge);
		Assert.AreEqual(destination, (UInt64)621923649824260095);
	}

	[TestMethod]
	public void TestDirectedEdgeToCells()
	{
		ulong edge = 1270441996165808127;
		var (origin, destination) = H3Standard.H3Net.DirectedEdgeToCells(edge);
		Assert.AreEqual(origin, (UInt64)621923649824456703);
		Assert.AreEqual(destination, (UInt64)621923649824260095);
        }

	[TestMethod]
	public void TestOriginToDirectedEdges()
	{
		ulong origin = 621923649824456703;
		ulong[] edges = H3Standard.H3Net.OriginToDirectedEdges(origin);
		Assert.AreEqual(edges[0], (UInt64)1270441996165808127);
        }

        [TestMethod]
	public void TestDirectedEdgeBoundary()
	{
		ulong edge = 1270441996165808127;
		CellBoundary cellBoundary = H3Standard.H3Net.DirectedEdgeToBoundary(edge);
        Assert.AreEqual(cellBoundary.numVerts, 2);
        Assert.AreEqual(cellBoundary.verts0.LatWGS84 - 47.699286520432835 < UnitTest.DoubleTolerance, true);
        Assert.AreEqual(cellBoundary.verts0.LngWGS84 + 3.0004833543571734 < UnitTest.DoubleTolerance, true);
    }
}

