using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SportsStreamsBot.Data;
using System.Net;
using System.Diagnostics;

namespace SportsStreamsBot
{
	class GamesReader
	{
		private int offsetFromEasternTime;

		public GamesReader(int offsetFromEasternTime)
		{
			this.offsetFromEasternTime = offsetFromEasternTime;
		}

		public Games GetGames(string url, ISummaryDownloader summaryDownloader)
		{
			var games = new Games();

			var contents = GetContents(url);

			if (string.IsNullOrEmpty(contents))
			{
				Debug.WriteLine("No data found for " + url); // not necessarily an error, could be no games
				return games;
			}

			// streams aren't grouped by game, they are listed as seperate entries with common data
			// streams are seperated by a semi colon
			// we're createing a list of streams grouped by key (homecity_awaycity)
			// so we can easily match them later
			var streams = new Dictionary<string, List<Stream>>();

			foreach (var streamString in contents.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				var data = streamString.Split('|');

				// I don't know what all the parts are, but I know enough...
				/*
					0 seasonid			1213|
					1 gameTime(E)		2013-05-18 20:00:00|
					2 month of season	3|
					3 gameid			41200216|
					4 feedName			IND	41200216 (Home Feed)|
					5 unknown			NYK|
					6 homeStreamName	ind|
					7 awayStreamName	nyk|
					8 homeCity			IND|
					9 awayCity			NYK|
					10 unknown			172.16.72.77|
					11 streamUrl		adaptive://nlds127.neulion.com:443/nlds/nba/ind/as/live/s_ind_live_game_hd|
					12 feedType			home|
					13 feedId?			3_41200216_nyk_ind_2012_h;
				 */
				// this is 
				var gameTimeEastern = DateTime.Parse(data[1]);
				// this is ugly, but the feed time is eastern and contains no timezone info
				// at least a config setting makes this a bit less ugly should someone else run this some day...
				gameTimeEastern = gameTimeEastern.AddHours(offsetFromEasternTime);

				var seasonId = data[0].Substring(0, 2);
				var month = data[2];
				var gameId = data[3];
				var homeStreamName = data[6];
				var awayStreamName = data[7];
				var homeCity = data[8];
				var awayCity = data[9];
				var streamUrl = data[11];
				var feedType = (Stream.FeedTypes)Enum.Parse(typeof(Stream.FeedTypes), data[12], true);

				var stream = new Stream(gameTimeEastern, homeStreamName, awayStreamName, homeCity, awayCity, streamUrl, feedType, gameId, seasonId, month);

				if (!streams.ContainsKey(stream.Key))
				{
					streams.Add(stream.Key, new List<Stream>());
				}

				streams[stream.Key].Add(stream);
			}

			// now sort through the streams and combine them into games
			foreach (var streamList in streams.Values)
			{
				var homeStream = streamList.FirstOrDefault(s => s.FeedType == Stream.FeedTypes.Home);
				var awayStream = streamList.FirstOrDefault(s => s.FeedType == Stream.FeedTypes.Away);

				// sanity check
				if (homeStream == null && awayStream == null)
				{
					throw new NullReferenceException("both streams are null");
				}

				// one of them has to be active, use that one to setup the data
				Stream gameData = homeStream != null ? homeStream : awayStream;

				var game = new Game();
				game.GameID = gameData.GameID;
				game.Month = gameData.Month;
				game.UtcStart = gameData.GameTimeEastern.ToUniversalTime();
				game.HomeTeam.City = gameData.HomeCity;
				game.HomeTeam.StreamName = gameData.HomeStreamName;
				game.AwayTeam.City = gameData.AwayCity;
				game.AwayTeam.StreamName = gameData.AwayStreamName;
				game.Summary = summaryDownloader.GetGameSummary(gameData.SeasonID, gameData.Month, gameData.GameID);

				if (homeStream != null)
					game.HomeTeam.Server = GetServerNumberFromUrl(homeStream.StreamUrl);
				if (awayStream != null)
					game.AwayTeam.Server = GetServerNumberFromUrl(awayStream.StreamUrl);

				games.Add(game);
			}

			return games;

		}

		private string GetServerNumberFromUrl(string streamUrl)
		{
			// just in case:
			streamUrl = streamUrl.ToLower();
			//adaptive://nlds127.neulion.com:443/nlds/nba/ind/as/live/s_ind_live_game_hd|
			// comes after the first instance of "nlds", goes until first instance of "."
			var startIndex = streamUrl.IndexOf("nlds") + 4; // + 4 for the substring length
			var endIndex = streamUrl.IndexOf(".");

			var serverNumber = streamUrl.Substring(startIndex, endIndex - startIndex);
			return serverNumber;
		}

		private string GetContents(string url)
		{
			try
			{
				using (var client = new WebClient())
				{
					return client.DownloadString(url);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to download from " + url);
				Debug.WriteLine(ex);
				throw;
			}
		}

	}
}
