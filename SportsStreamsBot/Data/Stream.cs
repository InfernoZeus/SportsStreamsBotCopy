using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStreamsBot.Data
{
	class Stream
	{
		public enum FeedTypes
		{
			Home,
			Away
		}

		public string SeasonID { get; set; }
		public string Month { get; set; }
		public string GameID { get; set; }
		public DateTime GameTimeEastern { get; set; }
		public string HomeStreamName { get; set; }
		public string AwayStreamName { get; set; }
		public string HomeCity { get; set; }
		public string AwayCity { get; set; }
		public string StreamUrl { get; set; }
		public FeedTypes FeedType { get; set; }

		public string Key
		{
			get { return string.Format("{0}_{1}", HomeCity, AwayCity); }
		}

		public Stream(DateTime gameTimeEastern, string homeStreamName, string awayStreamName, string homeCity, string awayCity, string streamUrl, FeedTypes feedType, string gameID, string seasonID, string month)
		{
			this.GameTimeEastern = gameTimeEastern;
			this.HomeStreamName = homeStreamName;
			this.AwayStreamName = awayStreamName;
			this.HomeCity = homeCity;
			this.AwayCity = awayCity;
			this.StreamUrl = streamUrl;
			this.FeedType = feedType;
			this.GameID = gameID;
			this.SeasonID = seasonID;
			this.Month = month;
		}
	}
}
