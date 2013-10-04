using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using RedditApi;
using System.Web;

namespace SportsStreamsBot
{
	class RedditDataSubmitter : IDataSubmitter
	{
		public const string USER_AGENT = "sports streams bot by /u/SportsStreams";
		public const int REQUEST_DELAY_MILLISECONDS = 2 * 1000;
		public const string SUBREDDIT_NAME = "Sports_Streams";

		RedditAPI reddit;

		private RedditDataSubmitter(RedditAPI reddit)
		{
			this.reddit = reddit;
		}

		public static IDataSubmitter Create()
		{
			var redditUser = ConfigurationManager.AppSettings["RedditUser"];
			if (string.IsNullOrEmpty(redditUser))
				throw new ConfigurationErrorsException("RedditUser not set");

			var redditPassword = ConfigurationManager.AppSettings["RedditPassword"];
			if (string.IsNullOrEmpty(redditPassword))
				throw new ConfigurationErrorsException("RedditPassword not set");


			var reddit = new RedditAPI(USER_AGENT);
			reddit.Login(redditUser, redditPassword);

			return new RedditDataSubmitter(reddit);
		}

		public void SubmitGames(string title, string gamesXml)
		{
			// Lets be a good bot and not flood reddit.  Wait between requests.
			Thread.Sleep(REQUEST_DELAY_MILLISECONDS);

			var encoded = HttpUtility.UrlEncode(gamesXml);

			reddit.PostSelf(encoded, title, SUBREDDIT_NAME);
		}
	}
}
