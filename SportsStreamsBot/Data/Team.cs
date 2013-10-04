using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SportsStreamsBot.Data
{
	[XmlRoot("team")]
	public class Team
	{
		[XmlAttribute("city")]
		public string City { get; set; }

		[XmlAttribute("streamName")]
		public string StreamName { get; set; }

		[XmlAttribute("server")]
		public string Server { get; set; }
	}
}
