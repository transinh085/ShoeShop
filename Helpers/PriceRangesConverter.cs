namespace ShoeShop.Helpers
{
	public class PriceRangesConverter
	{
		public static List<(decimal Min, decimal Max)> Parse(string prices)
		{
			var priceRanges = prices.Split(',');
			var rangeList = new List<(decimal Min, decimal Max)>();

			foreach (var range in priceRanges)
			{
				var parts = range.Split(':');
				if (parts.Length == 2 && decimal.TryParse(parts[0], out decimal min) && decimal.TryParse(parts[1], out decimal max))
				{
					rangeList.Add((min, max));
				}
			}

			return rangeList;
		}

		public static List<(decimal Min, decimal Max)> Parse(string[] prices)
		{
			var rangeList = new List<(decimal Min, decimal Max)>();

			foreach (var range in prices)
			{
				var parts = range.Split(':');
				if (parts.Length == 2 && decimal.TryParse(parts[0], out decimal min) && decimal.TryParse(parts[1], out decimal max))
				{
					rangeList.Add((min, max));
				}
			}

			return rangeList;
		}
	}
}
