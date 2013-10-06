using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStreamsBot
{
	class SummaryDownloaderFactory
	{

		private class DoNothingDownloader : ISummaryDownloader
		{

			public string GetGameSummary(string season, string month, string gameID)
			{
				return string.Empty;
			}
		}

		public static ISummaryDownloader GetDownloader(string sport)
		{
			switch (sport.ToLower())
			{
				case "hockey":
					return new HockeySummaryDownloader();

				default:
					return new DoNothingDownloader();
			}
		}

	}
}
