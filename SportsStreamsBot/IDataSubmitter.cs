using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStreamsBot
{
	public interface IDataSubmitter
	{
		void SubmitGames(string title, string gamesXml);
	}
}
