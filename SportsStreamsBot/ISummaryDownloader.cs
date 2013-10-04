using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStreamsBot
{
	interface ISummaryDownloader
	{

		string GetGameSummary(string month, string gameID);

	}
}
