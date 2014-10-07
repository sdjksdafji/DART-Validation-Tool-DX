using DevExpress.XtraRichEdit.API.Native;
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
		{
			StringBuilder sb = new StringBuilder();
			foreach (DataSeries element in dataSeriesList)
			{
				sb.Append(element.ToString());
			}
			return sb.ToString();
		}


		// Compare the first element in the list (we assume dart always returns a list of DataSeries with length one)
		// Returns three list of matched, unmatched, and missing data point. The format of the list's tuple is <TimeStamp, Value in server 1, Value in server 2>
		public static Tuple<
			List<Tuple<DateTime, String, String>>,
			List<Tuple<DateTime, String, String>>,
			List<Tuple<DateTime, String, String>>> DiffDataSeriesList(this List<DataSeries> osiDataSeriesList, List<DataSeries> gfsDataSeriesList)
		{
			if (osiDataSeriesList != null && gfsDataSeriesList != null)
			{
				List<Tuple<DateTime, String>> osiDataSeries = osiDataSeriesList.First().ToTupleList();
				List<Tuple<DateTime, String>> gfsDataSeries = gfsDataSeriesList.First().ToTupleList();
				if (osiDataSeries != null && gfsDataSeries != null)
				{
					List<Tuple<DateTime, String, String>> matchList = new List<Tuple<DateTime, String, String>>();
					List<Tuple<DateTime, String, String>> diffList = new List<Tuple<DateTime, String, String>>();
					List<Tuple<DateTime, String, String>> missingList = new List<Tuple<DateTime, String, String>>();
					var iterOsi = osiDataSeries.GetEnumerator();
					var iterGfs = gfsDataSeries.GetEnumerator();
					iterOsi.MoveNext();
					iterGfs.MoveNext();
					while (true)
					{
						// if one of the list has been iterated
						if (iterOsi.Current == null || iterGfs.Current == null)
						{
							while (iterGfs.Current != null)
							{
								missingList.Add(new Tuple<DateTime, string, string>(iterGfs.Current.Item1, iterGfs.Current.Item2, "null"));
								iterGfs.MoveNext();
							}
							while (iterOsi.Current != null)
							{
								missingList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, "null", iterOsi.Current.Item2));
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
									diffList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, iterGfs.Current.Item2,
										iterOsi.Current.Item2));
								}
								else
								{
									matchList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, iterGfs.Current.Item2,
										iterOsi.Current.Item2));
								}
								iterGfs.MoveNext();
								iterOsi.MoveNext();
							}
							else
							{
								if (iterOsi.Current.Item1 < iterGfs.Current.Item1)
								{
									missingList.Add(new Tuple<DateTime, string, string>(iterOsi.Current.Item1, "null", iterOsi.Current.Item2));
									iterOsi.MoveNext();
								}
								else
								{
									missingList.Add(new Tuple<DateTime, string, string>(iterGfs.Current.Item1, iterGfs.Current.Item2, "null"));
									iterGfs.MoveNext();
								}
							}
						}
					}
					return new Tuple<
						List<Tuple<DateTime, String, String>>,
						List<Tuple<DateTime, String, String>>,
						List<Tuple<DateTime, String, String>>>
						(matchList, diffList, missingList);
				}
			}
			return null;
		}

		public static List<Tuple<String, String, String>> NormalizeDateTimeToString(
			 this List<Tuple<DateTime, String, String>> tupleList)
		{
			List<Tuple<String, String, String>> returnList = new List<Tuple<String, String, String>>();
			foreach (Tuple<DateTime, String, String> tuple in tupleList)
			{
				returnList.Add(new Tuple<String, String, String>(tuple.Item1.ToLocalTime().ToString(), tuple.Item2, tuple.Item3));
			}
			return returnList;
		}
	}
}
