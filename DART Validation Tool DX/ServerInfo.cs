using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART_Validation_Tool_DX
{
	public class ServerInfo
	{
		public string FullName { get; set; }
		public string ShortName { get; set; }
		public string OldShortName { get; set; } // Use when short name changes to support old links
		public string ServiceName { get; set; }

		public string WebServiceEndpointHint { get; set; }
		public string WebServiceEndpoint
		{
			get
			{
				return WebServiceEndpointHint;
			}
		}
		public string EnvironmentXml { get; set; }
		public string DefaultMetric { get; set; }

		public ServerInfo(String FullName)
		{
			this.FullName = FullName;
			this.WebServiceEndpointHint = "http://" + this.FullName;
		}


		public string GetFullRequestUrl(string relativeUrl, bool hasNoParams = true)
		{
			string fullUrl = WebServiceEndpoint + relativeUrl;

			if (!string.IsNullOrWhiteSpace(ServiceName))
			{
				fullUrl = fullUrl + (hasNoParams ? "?" : "&") + "serviceName=" + ServiceName;
			}
			return fullUrl;
		}
	}
}
