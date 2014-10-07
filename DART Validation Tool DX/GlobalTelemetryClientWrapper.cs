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

		private List<DataSeries> osiDataSeries = null;
		private List<DataSeries> gfsDataSeries = null;
		private Object semaphore = new Object();


		// private helper class
		private class GetInstancesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
		}

		public void BeginGetDataSeries(ServerInfo server, String metricName, String instanceName, Boolean isOsiDart,
			MainGuiForm control)
		{
			String url = "/dataseries?key=" + metricName + "!" + instanceName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			request.BeginGetResponse(EndGetDataSeries,
				new GetDataSeriesRequest
				{
					HttpWebRequest = request,
					isOsiDart = isOsiDart,
					control = control,
					key = "Metric: " + metricName + " Instance: " + instanceName
				});
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
					compareTwoDataSeries(dataSeriesList, request.isOsiDart, request.control, request.key);
				}));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				LogInfo.WriteExceptionToLog(e);
			}
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
			//this.osiData = this.osiDataSeries.DataSeriesListToString();
			//this.gfsData = this.gfsDataSeries.DataSeriesListToString();
			var diffResult = this.osiDataSeries.DiffDataSeriesList(this.gfsDataSeries);
			if (diffResult == null) return;
			List<Tuple<DateTime, String, String>> matchList = diffResult.Item1;
			List<Tuple<DateTime, String, String>> diffList = diffResult.Item2;
			List<Tuple<DateTime, String, String>> missingList = diffResult.Item2;
			Boolean match = diffList.Count == 0;
			if (match)
				control.IncreaseMatched();
			else
				control.IncreaseUnmatched();
			if (!this.isAllInstances)
			{
				DevExpress.XtraEditors.XtraMessageBox.Show((match ? "Match" : "Unmach") + "!\nWith " + missingList.Count + " missing values.", "Result", MessageBoxButtons.OK,
					match ? MessageBoxIcon.Information : MessageBoxIcon.Error);
				control.diffResult.DataSource = diffList.NormalizeDateTimeToString();
				control.MatchResult.DataSource = matchList.NormalizeDateTimeToString();
			}
			LogInfo.WriteComparisonToLog(key, control.osiTextInput.EditValue.ToString(),
				control.gfsTextInput.EditValue.ToString(), matchList, diffList, missingList, this.osiDataSeries.First().Values.Length,
				this.gfsDataSeries.First().Values.Length);
			this.gfsDataSeries = null;
			this.osiDataSeries = null;
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

				request.Control.Invoke(new MethodInvoker(() =>
				{
					(request.Control.metricsBox.Edit as RepositoryItemComboBox).Items.Clear();
					foreach (Metric metric in metrics)
					{
						(request.Control.metricsBox.Edit as RepositoryItemComboBox).Items.Add(metric.Name);
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
		// Get instances list of a metrics
		public void BeginGetInstancesForMetric(ServerInfo server, String metricName, MainGuiForm control)
		{
			String url = "/instances?metric=" + metricName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			request.BeginGetResponse(EndGetInstancesForMetric, new GetInstancesRequest { HttpWebRequest = request, Control = control });
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

				request.Control.Invoke(new MethodInvoker(() =>
				{
					(request.Control.instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
					(request.Control.instancesBox.Edit as RepositoryItemComboBox).Items.Add("All Instances");
					foreach (Instance instance in instances)
					{
						(request.Control.instancesBox.Edit as RepositoryItemComboBox).Items.Add(instance.Name);
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
		private class GetInstancesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public MainGuiForm Control { get; set; }
		}
	}
}