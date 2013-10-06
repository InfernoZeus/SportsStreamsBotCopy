using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using HtmlAgilityPack;

namespace SportsStreamsBot
{
	class HockeySummaryDownloader : ISummaryDownloader
	{

		public const string PREVIEW_URL = "http://www.nhl.com/gamecenter/en/preview?id=20{0}{1}{2}";

		public string GetGameSummary(string season, string month, string gameID)
		{
			try
			{
				var previewUrl = ConstructPreviewURL(season, month, gameID);
				var previewDocument = GetPreviewDocument(previewUrl);
				var bigStoryNode = previewDocument.DocumentNode.SelectSingleNode("//div[@id='wideCol']//p[contains(., 'Big story:')]");
				if (bigStoryNode != null)
				{
					var bigStoryText = bigStoryNode.InnerText;
					var summary = bigStoryText.Substring(11);
					return summary;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			return "";
		}

		private string ConstructPreviewURL(string season, string month, string gameID)
		{
			return string.Format(PREVIEW_URL, season, month.PadLeft(2, '0'), gameID.PadLeft(4, '0'));
		}

		private HtmlDocument GetPreviewDocument(string url)
		{
			try
			{
				using (var client = new WebClient())
				{
					string contents = client.DownloadString(url);
					var document = new HtmlDocument();
					document.LoadHtml(contents);
					return document;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to download from " + url);
				throw;
			}
		}
	}
}
