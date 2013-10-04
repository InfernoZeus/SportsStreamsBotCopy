using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SportsStreamsBot.Data
{
	[XmlRoot("game")]
	[XmlType("game")]
	public class Game
	{
		[XmlAttribute("id")]
		public string GameID { get; set; }

		[XmlAttribute("month")]
		public string Month { get; set; }
		
		[XmlElement("utcStart")]
		public string UtcStartSTring
		{
			get { return UtcStart.ToString("yyyy-MM-dd HH:mm:ss") + "+0000"; }
			set { throw new NotImplementedException(); }
		}

		[XmlIgnore]
		public DateTime UtcStart { get; set; }

		[XmlElement("summary")]
		public string Summary { get; set; }

		[XmlElement("homeTeam")]
		public Team HomeTeam { get; set; }

		[XmlElement("awayTeam")]
		public Team AwayTeam { get; set; }

		public Game()
		{
			HomeTeam = new Team();
			AwayTeam = new Team();
		}
	}
}
