using System;
namespace H3Standard.Tests
{
	[TestClass]
	public class UnitTest_H3Net_05_Region
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
			var h3Indexes = H3Net.PolygonToCells(coords, 7);
			Assert.AreEqual(h3Indexes[0], (UInt64)608412563192938495);
		}
	}
}

