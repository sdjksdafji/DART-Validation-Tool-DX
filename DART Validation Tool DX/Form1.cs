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

namespace DART_Validation_Tool_DX
{
	public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
	{
		private ServerInfo osiServerInfo = null;
		private ServerInfo gfsServerInfo = null;
		private DataSeries oisDataSeries = null;
		private DataSeries gfsDataSeries = null;
		private Object semaphore;
		public Form1()
		{
			InitializeComponent();
		}

		private void connectButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			osiServerInfo = new ServerInfo();
			osiServerInfo.FullName = (this.osiTextInput.EditValue).ToString();
			osiServerInfo.WebServiceEndpointHint = "http://" + osiServerInfo.FullName;

			gfsServerInfo = new ServerInfo();
			gfsServerInfo.FullName = (this.gfsTextInput.EditValue).ToString();
			gfsServerInfo.WebServiceEndpointHint = "http://" + gfsServerInfo.FullName;

			BeginGetMetrics(gfsServerInfo);
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
			if (gfsServerInfo != null && osiServerInfo != null && metricsBox.EditValue != null && instancesBox.EditValue != null)
			{
				BeginGetDataSeries(gfsServerInfo, metricsBox.EditValue.ToString(), instancesBox.EditValue.ToString(), false);
			}
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

		private void BeginGetDataSeries(ServerInfo server, String metricName, String instanceName, Boolean isOsiDart)
		{
			String url = "/dataseries?key=" + metricName + "!" + instanceName;
			HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl(url)) as HttpWebRequest;
			request.BeginGetResponse(EndGetDataSeries, new GetDataSeriesRequest { HttpWebRequest = request, isOsiDart = isOsiDart });
		}

		private void EndGetDataSeries(IAsyncResult async)
		{
			GetDataSeriesRequest request = async.AsyncState as GetDataSeriesRequest;
			try
			{
				HttpWebResponse response = request.HttpWebRequest.EndGetResponse(async) as HttpWebResponse;
				Stream stream = response.GetResponseStream();

				DataContractSerializer serializer = new DataContractSerializer(typeof(List<DataSeries>));
				List<DataSeries> instances = (List<DataSeries>)serializer.ReadObject(stream);

				Invoke(new MethodInvoker(() =>
				{
					//(instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
					//foreach (Instance instance in instances)
					//{
					//    (instancesBox.Edit as RepositoryItemComboBox).Items.Add(instance.Name);
					//}
				}));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		// private helper class
		private class GetDataSeriesRequest
		{
			public HttpWebRequest HttpWebRequest { get; set; }
			public Boolean isOsiDart { get; set; }
		}
	}



}
