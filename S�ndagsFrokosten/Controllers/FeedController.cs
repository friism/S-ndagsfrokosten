using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SøndagsFrokosten.Controllers
{
	public class FeedController : Controller
	{
		private readonly string _uri = "http://podcast.dr.dk/p1/rssfeed/p1debat.xml";
		private readonly string _description = "Fordi DR ikke fatter at lave det selv";

		[OutputCache(Duration=60 * 60)]
		public ActionResult Index()
		{
			var xml = XElement.Load(_uri);

			XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

			var result = new XElement("rss",
				new XAttribute(XNamespace.Xmlns + "itunes", itunes.NamespaceName),
				new XAttribute("version", "2.0"),
				new XElement("channel",
					new XElement("title", "P1 Debat | Kun Søndagsfrokosten"),
					new XElement("description", _description),
					new XElement("copyright", "DR"),
					new XElement(itunes + "summary", _description),
					new XElement(itunes + "author", "DR"),
					new XElement(itunes + "explicit", "no"),
					xml.Descendants("item")
						.Where(x => x.Element("title").Value.Contains("Søndagsfrokosten"))
				));

			return Content(result.ToString(), "text/xml");
		}
	}
}
