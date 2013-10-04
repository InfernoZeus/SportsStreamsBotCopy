using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SportsStreamsBot
{
	class Program
	{
		static void Main(string[] args)
		{
			var sports = ConfigurationManager.AppSettings["Sports"];
			var offsetFromEasternTime = int.Parse(ConfigurationManager.AppSettings["OffsetFromEasternTime"]);

			var submitter = RedditDataSubmitter.Create();

			var reader = new GamesReader(offsetFromEasternTime);
			var writer = new GamesWriter(submitter);

			foreach (var sport in sports.Split(','))
			{
				var summaryDownloader = SummaryDownloaderFactory.GetDownloader(sport);
				var url = ConfigurationManager.AppSettings[sport];
				var games = reader.GetGames(url, summaryDownloader);
				writer.WriteGames(sport, games);
			}

		}
	}
}
