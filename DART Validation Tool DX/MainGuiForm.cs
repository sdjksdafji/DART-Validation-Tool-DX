using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Office.Web.Datacenter.Telemetry;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraCharts;

namespace DART_Validation_Tool_DX
{
	public partial class MainGuiForm : DevExpress.XtraBars.Ribbon.RibbonForm
	{
		private ServerInfo osiServerInfo = null;
		private ServerInfo gfsServerInfo = null;
		private long numMatched = 0;
		private long numUnmathed = 0;
		private Object statSemaphore = new Object();
		private Object semaphore = new Object();
		public MainGuiForm()
		{
			InitializeComponent();
		}

		public void ClearStat()
		{
			lock (this.statSemaphore)
			{
				this.numMatched = 0;
				this.numUnmathed = 0;
			}
		}

		public void IncreaseMatched()
		{
			lock (this.statSemaphore)
			{
				this.numMatched++;
			}
			this.UpdateChart();
		}

		public void IncreaseUnmatched()
		{
			lock (this.statSemaphore)
			{
				this.numUnmathed++;
			}
			this.UpdateChart();
		}
		private void ConnectButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (this.osiTextInput.EditValue != null)
			{
				osiServerInfo = new ServerInfo((this.osiTextInput.EditValue).ToString());
			}

			if (this.gfsTextInput.EditValue != null)
			{
				gfsServerInfo = new ServerInfo((this.gfsTextInput.EditValue).ToString());

				(new GetMetrics()).BeginGetMetrics(gfsServerInfo, this);
			}
		}

		private void BarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			GetAndCompareDataSeires.OnlyCompareLast15MinData = (bool) Latest15MinCheck.EditValue;
			String logFilename = DateTime.Now.ToString().Replace('/', ' ').Replace(':', ' ');
			LogInfo.FileName = logFilename;
			this.ClearStat();
			var selectedMetrics = (metricsBox.Edit as RepositoryItemCheckedComboBoxEdit).GetCheckedItems().ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			if (gfsServerInfo != null && osiServerInfo != null && selectedMetrics != null)
			{
				if (selectedMetrics.Count() > 1) //multiple metrics
				{
					this.resultChart.Titles.Clear();
					ChartTitle chartTitle1 = new ChartTitle();
					chartTitle1.Text = "Validation Result";
					this.resultChart.Titles.Add(chartTitle1);
					this.resultChart.Visible = true;
					this.MainSplitContainerControl.Visible = false;
					foreach (String metricsName in selectedMetrics)
					{
						(new GetInstances()).GetInstancesForMetric(gfsServerInfo, osiServerInfo, metricsName, this);
						Thread.Sleep(1000);
						ComboBoxItemCollection instances = (instancesBox.Edit as RepositoryItemComboBox).Items;
						foreach (var item in instances)
						{
							if (item.ToString().Equals("All Instances")) continue;
							GetAndCompareDataSeires compareDataSeires = new GetAndCompareDataSeires();
							compareDataSeires.isAllInstances = true;
							compareDataSeires.GetAndCompareDataSeries(gfsServerInfo, metricsName, item.ToString(), false, this);
							compareDataSeires.GetAndCompareDataSeries(osiServerInfo, metricsName, item.ToString(), true, this);
						}
					}
				}
				else if (selectedMetrics.Count() == 1 && instancesBox.EditValue != null) // single metric
				{
					String metricsName = selectedMetrics[0];
					if (instancesBox.EditValue.ToString().Equals("All Instances"))
					{
						this.resultChart.Titles.Clear();
						ChartTitle chartTitle1 = new ChartTitle();
						chartTitle1.Text = metricsName + " Validation Result";
						this.resultChart.Titles.Add(chartTitle1);
						this.resultChart.Visible = true;
						this.MainSplitContainerControl.Visible = false;
						ComboBoxItemCollection instances = (instancesBox.Edit as RepositoryItemComboBox).Items;
						foreach (var item in instances)
						{
							if (item.ToString().Equals("All Instances")) continue;
							GetAndCompareDataSeires compareDataSeires = new GetAndCompareDataSeires();
							compareDataSeires.isAllInstances = true;
							compareDataSeires.GetAndCompareDataSeries(gfsServerInfo, metricsName, item.ToString(), false, this);
							compareDataSeires.GetAndCompareDataSeries(osiServerInfo, metricsName, item.ToString(), true, this);
						}
						Console.WriteLine("all");
					}
					else
					{
						this.resultChart.Visible = false;
						this.MainSplitContainerControl.Visible = true;
						GetAndCompareDataSeires compareDataSeires = new GetAndCompareDataSeires();
						compareDataSeires.isAllInstances = false;
						compareDataSeires.GetAndCompareDataSeries(gfsServerInfo, metricsName, instancesBox.EditValue.ToString(), false,
							this);
						compareDataSeires.GetAndCompareDataSeries(osiServerInfo, metricsName, instancesBox.EditValue.ToString(), true,
							this);
					}
				}
			}
		}

		private void GfsTextInputEditValueChanged(object sender, EventArgs e)
		{
			ClearMetricsListAndInstancesList();
		}


		private void OsiTextInputEditValueChanged(object sender, EventArgs e)
		{
			ClearMetricsListAndInstancesList();
		}





		private void ClearMetricsListAndInstancesList()
		{
			(this.metricsBox.Edit as RepositoryItemCheckedComboBoxEdit).Items.Clear();
			(this.instancesBox.Edit as RepositoryItemComboBox).Items.Clear();
			this.osiServerInfo = null;
			this.gfsServerInfo = null;
		}

		private void UpdateChart()
		{
			this.resultChart.Series.Clear();
			Series series = new Series("Validation Result", ViewType.Pie3D);
			// Populate the series with points.series.Points.Add(new SeriesPoint("Matched", this.numMatched));
			series.Points.Add(new SeriesPoint("Unmatch", this.numUnmathed));
			series.Points.Add(new SeriesPoint("Matched", this.numMatched));

			series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;

			series.Label.PointOptions.Pattern = "{A} - {V}";
			// Add the series to the chart.
			this.resultChart.Series.Add(series);
		}

		private void metricsBox_EditValueChanged(object sender, EventArgs e)
		{
			this.ClearStat();
			var selectedMetrics = (metricsBox.Edit as RepositoryItemCheckedComboBoxEdit).GetCheckedItems().ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			if (selectedMetrics != null && selectedMetrics.Count() == 1 && gfsServerInfo != null)
			{
				(new GetInstances()).GetInstancesForMetric(gfsServerInfo, osiServerInfo, metricsBox.EditValue.ToString(), this);
				this.instancesBox.Enabled = true;
			}
			else
			{
				//this.instancesBox.Enabled = false;
			}
		}

	}



}
