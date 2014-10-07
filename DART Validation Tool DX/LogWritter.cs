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

		private static String GetLogPath()
		{
			String path = @"log";
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path += @"\log.txt";
			return path;
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
						sw.WriteLine("-- Matched --: { key = " + key + "}\t" + "With " + missList.Count + "missing\t" + osiCount + " data in " + gfsServerName + " and " +
									 osiServerName);
					}
					else
					{
						sw.WriteLine("!! Unmatched !!: { key = " + key + "}\t" + "With " + missList.Count + "missing\t" + diffList.Count + " data in " + gfsServerName + " and " +
									 osiServerName);
						sw.WriteLine("\tSamples (up to 5):");
						int i = 0;
						foreach (Tuple<DateTime, String, String> tuple in diffList)
						{
							sw.WriteLine("\tTime: " + tuple.Item1 + "\t" + gfsServerName + ": " + tuple.Item2 + " instances\t" +
										 osiServerName + ": " + tuple.Item3 + " instances");
							if (++i >= 5)
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
