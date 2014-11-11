using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART_Validation_Tool_DX
{

	public static class LogInfo
	{
		private static Object logLock = new Object();

		public static String FileName { get; set; }

		private static String GetLogPath()
		{
			String path = @"log";
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path += "\\"+ FileName + ".txt";
			return path;
		}

		public static void WriteInstancesNotMatchToLog(String metric)
		{
			String path = GetLogPath();
			lock (logLock)
			{
				using (StreamWriter sw = File.AppendText(path))
				{
					sw.WriteLine(@"!! Unmatched !!: The instances for " + metric + " are NOT same");
				}
			}
		}

		public static void WriteComparisonToLog(String key, String osiServerName, String gfsServerName, List<Tuple<DateTime, String, String>> matchList,
			List<Tuple<DateTime, String, String>> diffList, List<Tuple<DateTime, String, String>> missList, int osiCount, int gfsCount)
		{
			String path = GetLogPath();
			// This text is always added, making the file longer over time 
			// if it is not deleted. 
			lock (logLock)
			{
				using (StreamWriter sw = File.AppendText(path))
				{
					if (diffList.Count == 0)
					{
						sw.WriteLine("-- Matched --: { key = " + key + "}\t" + "With " + missList.Count + " missing.\t Compared " + matchList.Count + " data in " + gfsServerName + " and " +
									 osiServerName);
					}
					else
					{
						sw.WriteLine("!! Unmatched !!: { key = " + key + "}\t" + "With " + missList.Count + " missing.\t Compared " + (diffList.Count + matchList.Count) + " data in " + gfsServerName + " and " +
									 osiServerName + ". There are " + diffList.Count + " unmatched datapoints and " + matchList.Count + " matched datapoints.");
						sw.WriteLine("\tUnmatched datapoint Samples (up to 15):");
						int i = 0;
						foreach (Tuple<DateTime, String, String> tuple in diffList)
						{
							sw.WriteLine("\tTime: " + tuple.Item1 + "\t" + gfsServerName + ": " + tuple.Item2 + " instances\t" +
										 osiServerName + ": " + tuple.Item3 + " instances");
							if (++i >= 15)
							{
								break;
							}
						}
					}
					if (missList.Count != 0)
					{
						sw.WriteLine("\tMissing datapoint Samples (up to 15):");
						int i = 0;
						foreach (Tuple<DateTime, String, String> tuple in missList)
						{
							sw.WriteLine("\tTime: " + tuple.Item1 + "\t" + gfsServerName + ": " + tuple.Item2 + " instances\t" +
										 osiServerName + ": " + tuple.Item3 + " instances");
							if (++i >= 15)
							{
								break;
							}
						}
					}
				}
			}
		}

		public static void WriteExceptionToLog(Exception ex)
		{
			String path = GetLogPath();
			// This text is always added, making the file longer over time 
			// if it is not deleted. 
			lock (logLock)
			{
				using (StreamWriter sw = File.AppendText(path))
				{
					sw.WriteLine(ex.ToString());
				}
			}
		}
	}
}
