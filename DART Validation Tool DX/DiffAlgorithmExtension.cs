using Microsoft.Office.Web.Datacenter.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART_Validation_Tool_DX
{
	public static class DiffAlgorithmExtension
	{
		public static String DataSeriesListToString(this List<DataSeries> dataSeriesList)
		{StringBuilder sb = new StringBuilder();
			foreach (DataSeries element in dataSeriesList)
			{
				sb.Append(element.ToString());
			}
			return sb.ToString();
		}

		public static List<Tuple<DateTime, String, String>> diffDataSeriesList(this List<DataSeries> osiDataSeriesList, List<DataSeries> gfsDataSeriesList)
		{
			if (osiDataSeriesList != null && gfsDataSeriesList != null)
			{
				List<Tuple<DateTime, String>> osiDataSeries = osiDataSeriesList.First().ToTupleList();
				List<Tuple<DateTime, String>> gfsDataSeries = gfsDataSeriesList.First().ToTupleList();
				if (osiDataSeries != null && gfsDataSeries != null)
				{
					List<Tuple<DateTime, String, String>> diffList = new List<Tuple<DateTime, String, String>>();
					var iterOsi = osiDataSeries.GetEnumerator();
					var iterGfs = gfsDataSeries.GetEnumerator();
					iterOsi.MoveNext();
					iterGfs.MoveNext();
					while (true)
					{
						if (iterOsi.Current == null || iterGfs.Current == null)
						{
							while (iterGfs.Current != null)
							{
								diffList.Add(new Tuple<DateTime, string, string>(iterGfs.Current.Item1, iterGfs.Current.Item2, "null"));
								iterGfs.MoveNext();
							}
							while (iterOsi.Current != null)
							{
								diffList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, "null", iterOsi.Current.Item2));
								iterOsi.MoveNext();
							}
							break;
						}
						else
						{
							if (iterOsi.Current.Item1.Equals(iterGfs.Current.Item1))
							{
								if (!iterOsi.Current.Item2.Equals(iterGfs.Current.Item2))
								{
									diffList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, iterGfs.Current.Item2, iterOsi.Current.Item2));
								}
								iterGfs.MoveNext();
								iterOsi.MoveNext();
							}
							else
							{
								if (iterOsi.Current.Item1 < iterGfs.Current.Item1)
								{
									diffList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, "null", iterOsi.Current.Item2));
									iterOsi.MoveNext();
								}
								else
								{
									diffList.Add(new Tuple<DateTime, string, string>(iterGfs.Current.Item1, iterGfs.Current.Item2, "null"));
									iterGfs.MoveNext();
								}
							}
						}
					}
					return diffList;
				}
			}
			return null;
		}
	}
}
