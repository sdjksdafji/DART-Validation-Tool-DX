using DevExpress.XtraEditors.Repository;
using Microsoft.Office.Web.Datacenter.Telemetry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DART_Validation_Tool_DX
{

	public class GetAndCompareDataSeires
	{
		public Boolean isAllInstances { get; set; }

		public static Boolean OnlyCompareLast15MinData { get; set; }

		private List<DataSeries> osiDataSeries = null;
		private List<DataSeries> gfsDataSeries = null;
		private Object semaphore = new Object();

		public void GetAndCompareDataSeries(ServerInfo server, String metricName, String instanceName, Boolean isOsiDart,
			MainGuiForm control)
		{
			String url = "/dataseries?key=" + metricName + "!" + instanceName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;

			Stream stream = request.GetResponse().GetResponseStream();

			DataContractSerializer serializer = new DataContractSerializer(typeof(List<DataSeries>));
			List<DataSeries> dataSeriesList = (List<DataSeries>)serializer.ReadObject(stream);
			compareTwoDataSeries(dataSeriesList, isOsiDart, control, "Metric: " + metricName + " Instance: " + instanceName);

		}

		private void compareTwoDataSeries(List<DataSeries> dataSeriesList, Boolean isOsiDart, MainGuiForm control, String key)
		{
			lock (this.semaphore)
			{
				if (isOsiDart)
				{
					this.osiDataSeries = dataSeriesList;
					if (this.gfsDataSeries != null)
					{
						displayResult(control, key);
					}
				}
				else
				{
					this.gfsDataSeries = dataSeriesList;
					if (this.osiDataSeries != null)
					{
						displayResult(control, key);
					}
				}
			}
		}

		private void displayResult(MainGuiForm control, String key)
		{
			try
			{
				var diffResult = this.osiDataSeries.DiffDataSeriesList(this.gfsDataSeries);
				if (diffResult == null) return;
				List<Tuple<DateTime, String, String>> matchList = diffResult.Item1;
				List<Tuple<DateTime, String, String>> diffList = diffResult.Item2;
				List<Tuple<DateTime, String, String>> missingList = diffResult.Item3;
				Boolean match = diffList.Count == 0;
				if (match) control.IncreaseMatched();
				else
					control.IncreaseUnmatched();
				if (!this.isAllInstances)
				{
					DevExpress.XtraEditors.XtraMessageBox.Show(
						(match ? "Match" : "Unmach") + "!\nWith " + missingList.Count + " missing values.", "Result", MessageBoxButtons.OK,
						match ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					control.diffResult.DataSource = diffList.NormalizeDateTimeToString();
					control.MatchResult.DataSource = matchList.NormalizeDateTimeToString();
				}
				LogInfo.WriteComparisonToLog(key, control.osiTextInput.EditValue.ToString(),
					control.gfsTextInput.EditValue.ToString(), matchList, diffList, missingList,
					this.osiDataSeries.First().Values.Length,
					this.gfsDataSeries.First().Values.Length);
				this.gfsDataSeries = null;
				this.osiDataSeries = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				LogInfo.WriteExceptionToLog(ex);
			}
		}

		// private helper class
		private class GetDataSeriesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public Boolean isOsiDart { get; set; }
			public MainGuiForm control { get; set; }
			public String key { get; set; }
		}
	}

	public class GetMetrics
	{
		// Get metrics list from a server
		public void BeginGetMetrics(ServerInfo server, MainGuiForm control)
		{
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl("/metrics")) as HttpWebRequest;
			request.BeginGetResponse(EndGetMetrics, new GetMetricsRequest { HttpWebRequest = request, Control = control });
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

				var t = from m in metrics orderby m.Name select m;

				request.Control.Invoke(new MethodInvoker(() =>
				{
					(request.Control.metricsBox.Edit as RepositoryItemCheckedComboBoxEdit).Items.Clear();
					foreach (Metric metric in t)
					{
						(request.Control.metricsBox.Edit as RepositoryItemCheckedComboBoxEdit).Items.Add(metric.Name);
					}
				}));


			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				LogInfo.WriteExceptionToLog(e);
			}
		}

		// private helper class
		private class GetMetricsRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public MainGuiForm Control { get; set; }
		}

	}

	public class GetInstances
	{
		private Object locker =new Object();
		private List<String> instancesStored = null;
		private String metricName = "";
		// Get instances list of a metrics
		public void GetInstancesForMetric(ServerInfo server, ServerInfo server2, String metricName, MainGuiForm control)
		{
			String url = "/instances?metric=" + metricName;
			this.metricName = metricName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			HttpWebRequest request2 = WebRequest.Create(server2.GetFullRequestUrl(url)) as HttpWebRequest;
			ResolveRequest(request, control);
			ResolveRequest(request2, control);
		}

		// update GUI for instances list
		private void ResolveRequest(HttpWebRequest request, MainGuiForm control)
		{
			try
			{
				Stream stream = request.GetResponse().GetResponseStream();

				DataContractSerializer serializer = new DataContractSerializer(typeof(List<Instance>));
				List<Instance> instances = (List<Instance>)serializer.ReadObject(stream);

				var temp = new List<String>();
				foreach (Instance instance in instances)
				{
					temp.Add(instance.Name);
				}

				lock (locker)
				{
					if (instancesStored == null)
					{
						instancesStored = temp;
					}
					else
					{
						var diff1 = instancesStored.Except(temp);
						var diff2 = temp.Except(instancesStored);
						if (diff1.Count() != 0 || diff2.Count() != 0)
						{
							LogInfo.WriteInstancesNotMatchToLog(this.metricName);
							DevExpress.XtraEditors.XtraMessageBox.Show("Instances Lists of " + metricName+" Not Same", "Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						var common = from x in instancesStored
							join y in temp on x equals y
							select x;
						control.Invoke(new MethodInvoker(() =>
						{
							(control.instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
							(control.instancesBox.Edit as RepositoryItemComboBox).Items.Add("All Instances");
							foreach (String instance in common)
							{
								(control.instancesBox.Edit as RepositoryItemComboBox).Items.Add(instance);
							}
						}));
					}
				}
			}
			catch (Exception e)
			{Console.WriteLine(e.ToString());
				LogInfo.WriteExceptionToLog(e);
			}
		}

		// private helper class
		private class GetInstancesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public MainGuiForm Control { get; set; }
		}
	}
}