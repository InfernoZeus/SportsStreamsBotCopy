using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStreamsBot
{
	class HockeySummaryDownloader : ISummaryDownloader
	{
		public string GetGameSummary(string month, string gameID)
		{
			return "Test summary";
		}
	}
}
