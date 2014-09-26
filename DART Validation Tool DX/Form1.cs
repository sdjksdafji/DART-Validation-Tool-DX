using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Office.Web.Datacenter.Telemetry;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Collections;

namespace DART_Validation_Tool_DX
{
	public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
	{
		private ServerInfo osiServerInfo = null;
		private ServerInfo gfsServerInfo = null;
		private List<DataSeries> osiDataSeries = null;
		private List<DataSeries> gfsDataSeries = null;
		private Object semaphore = new Object();
		public Form1()
		{
			InitializeComponent();
		}

		private void connectButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (this.osiTextInput.EditValue != null)
			{
				osiServerInfo = new ServerInfo();
				osiServerInfo.FullName = (this.osiTextInput.EditValue).ToString();
				osiServerInfo.WebServiceEndpointHint = "http://" + osiServerInfo.FullName;
			}

			if (this.gfsTextInput.EditValue != null)
			{
				gfsServerInfo = new ServerInfo();
				gfsServerInfo.FullName = (this.gfsTextInput.EditValue).ToString();
				gfsServerInfo.WebServiceEndpointHint = "http://" + gfsServerInfo.FullName;

				BeginGetMetrics(gfsServerInfo);
			}
		}

		private void metricsBox_EditValueChanged(object sender, EventArgs e)
		{
			if (metricsBox.EditValue != null && gfsServerInfo != null)
			{
				BeginGetInstancesForMetric(gfsServerInfo, metricsBox.EditValue.ToString());
			}
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			CompareDataSeires compareDataSeires = new CompareDataSeires();
			if (gfsServerInfo != null && osiServerInfo != null && metricsBox.EditValue != null && instancesBox.EditValue != null)
			{
				compareDataSeires.BeginGetDataSeries(gfsServerInfo, metricsBox.EditValue.ToString(), instancesBox.EditValue.ToString(), false, this);
				compareDataSeires.BeginGetDataSeries(osiServerInfo, metricsBox.EditValue.ToString(), instancesBox.EditValue.ToString(), true, this);
			}
		}

		private void gfsTextInput_EditValueChanged(object sender, EventArgs e)
		{
			clearMetricsListAndInstancesList();
		}


		private void osiTextInput_EditValueChanged(object sender, EventArgs e)
		{
			clearMetricsListAndInstancesList();
		}



		// Get metrics list from a server
		private void BeginGetMetrics(ServerInfo server)
		{
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl("/metrics")) as HttpWebRequest;
			request.BeginGetResponse(EndGetMetrics, new GetMetricsRequest { HttpWebRequest = request });
		}

		// update GUI for metrics list
		private void EndGetMetrics(IAsyncResult async)
		{
			GetMetricsRequest request = async.AsyncState as GetMetricsRequest;
			try
			{
				HttpWebResponse response = request.HttpWebRequest.EndGetResponse(async) as HttpWebResponse;
				Stream stream = response.GetResponseStream();

				DataContractSerializer serializer = new DataContractSerializer(typeof(List<Metric>));
				List<Metric> metrics = (List<Metric>)serializer.ReadObject(stream);

				Invoke(new MethodInvoker(() =>
				{
					(metricsBox.Edit as RepositoryItemComboBox).Items.Clear();
					foreach (Metric metric in metrics)
					{
						(metricsBox.Edit as RepositoryItemComboBox).Items.Add(metric.Name);
					}
				}));


			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		// private helper class
		private class GetMetricsRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
		}

		// Get instances list of a metrics
		private void BeginGetInstancesForMetric(ServerInfo server, String metricName)
		{
			String url = "/instances?metric=" + metricName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			request.BeginGetResponse(EndGetInstancesForMetric, new GetInstancesRequest { HttpWebRequest = request });
		}

		// update GUI for instances list
		private void EndGetInstancesForMetric(IAsyncResult async)
		{
			GetInstancesRequest request = async.AsyncState as GetInstancesRequest;
			try
			{
				HttpWebResponse response = request.HttpWebRequest.EndGetResponse(async) as HttpWebResponse;
				Stream stream = response.GetResponseStream();

				DataContractSerializer serializer = new DataContractSerializer(typeof(List<Instance>));
				List<Instance> instances = (List<Instance>)serializer.ReadObject(stream);

				Invoke(new MethodInvoker(() =>
				{
					(instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
					(instancesBox.Edit as RepositoryItemComboBox).Items.Add("All Instances");
					foreach (Instance instance in instances)
					{
						(instancesBox.Edit as RepositoryItemComboBox).Items.Add(instance.Name);
					}
				}));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		// private helper class
		private class GetInstancesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
		}

		private void clearMetricsListAndInstancesList()
		{
			(this.metricsBox.Edit as RepositoryItemComboBox).Items.Clear();
			(this.instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
			this.osiServerInfo = null;
			this.gfsServerInfo = null;
		}

	}

	public static class DataSeriesListExtension
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

	public class CompareDataSeires
	{
		private List<DataSeries> osiDataSeries = null;
		private List<DataSeries> gfsDataSeries = null;
		private Object semaphore = new Object();

		// private helper class
		private class GetInstancesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
		}

		public void BeginGetDataSeries(ServerInfo server, String metricName, String instanceName, Boolean isOsiDart, Form1 control)
		{
			String url = "/dataseries?key=" + metricName + "!" + instanceName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			request.BeginGetResponse(EndGetDataSeries, new GetDataSeriesRequest { HttpWebRequest = request, isOsiDart = isOsiDart, control = control });
		}

		private void EndGetDataSeries(IAsyncResult async)
		{
			GetDataSeriesRequest request = async.AsyncState as GetDataSeriesRequest;
			try
			{
				HttpWebResponse response = request.HttpWebRequest.EndGetResponse(async) as HttpWebResponse;
				Stream stream = response.GetResponseStream();

				DataContractSerializer serializer = new DataContractSerializer(typeof(List<DataSeries>));
				List<DataSeries> dataSeriesList = (List<DataSeries>)serializer.ReadObject(stream);

				request.control.Invoke(new MethodInvoker(() =>
				{
					compareTwoDataSeries(dataSeriesList, request.isOsiDart, request.control);
				}));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		private void compareTwoDataSeries(List<DataSeries> dataSeriesList, Boolean isOsiDart,Form1 control)
		{
			lock (this.semaphore)
			{
				if (isOsiDart)
				{
					this.osiDataSeries = dataSeriesList;
					if (this.gfsDataSeries != null)
					{
						displayResult(control);
					}
				}
				else
				{
					this.gfsDataSeries = dataSeriesList;
					if (this.osiDataSeries != null)
					{
						displayResult(control);
					}
				}
			}
		}

		private void displayResult(Form1 control)
		{
			//this.osiData = this.osiDataSeries.DataSeriesListToString();
			//this.gfsData = this.gfsDataSeries.DataSeriesListToString();
			List<Tuple<DateTime, String, String>> diffList = this.osiDataSeries.diffDataSeriesList(this.gfsDataSeries);
			Boolean match = diffList.Count == 0;
			DevExpress.XtraEditors.XtraMessageBox.Show(match ? "Match" : "Unmach", "Result", MessageBoxButtons.OK, match ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			LogInfo.WriteComparisonToLog(control.osiTextInput.EditValue.ToString(), control.gfsTextInput.EditValue.ToString(), diffList, this.osiDataSeries.First().Values.Length, this.gfsDataSeries.First().Values.Length);
			this.gfsDataSeries = null;
			this.osiDataSeries = null;
		}

		// private helper class
		private class GetDataSeriesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public Boolean isOsiDart { get; set; }

			public Form1 control { get; set; }
		}
	}

	public static class LogInfo
	{
		private static Object logLock = new Object();
		public static void WriteComparisonToLog(String osiServerName, String gfsServerName, List<Tuple<DateTime, String, String>> diffList, int osiCount, int gfsCount)
		{
			String path = @"log";
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path += @"\log.txt";
			// This text is always added, making the file longer over time 
			// if it is not deleted. 
			lock (logLock)
			{
				using (StreamWriter sw = File.AppendText(path))
				{
					if (diffList.Count == 0)
					{
						sw.WriteLine("-- Matched --: " + osiCount + " data in " + gfsServerName + " and " + osiServerName);
					}
					else
					{
						sw.WriteLine("!! Unmatched !!: " + diffList.Count + " data in " + gfsServerName + " and " + osiServerName);
						sw.WriteLine("\tSamples:");
						int i = 0;
						foreach (Tuple<DateTime, String, String> tuple in diffList)
						{
							sw.WriteLine("\tTime: " + tuple.Item1 + "\t" + gfsServerName + ": " + tuple.Item2 + " instances\t" + osiServerName + ": " + tuple.Item3 + " instances");
							if (++i >= 5)
							{
								break;
							}
						}
					}
				}
			}
		}
	}
}
