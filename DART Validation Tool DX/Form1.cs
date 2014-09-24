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
        ServerInfo osiServerInfo = null;
        ServerInfo gfsServerInfo = null;
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

        public void BeginGetMetrics(ServerInfo server)
        {
            HttpWebRequest request = WebRequest.Create(server.GetFullRequestUrl("/metrics")) as HttpWebRequest;
            request.BeginGetResponse(EndGetMetrics, new GetMetricsRequest { HttpWebRequest = request });
        }

        private void EndGetMetrics(IAsyncResult async)
        {
            GetMetricsRequest request = async.AsyncState as GetMetricsRequest;
            try
            {
                HttpWebResponse response = request.HttpWebRequest.EndGetResponse(async) as HttpWebResponse;
                Stream stream = response.GetResponseStream();

                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Metric>));
                List<Metric> metrics = (List<Metric>)serializer.ReadObject(stream);

                Invoke(new MethodInvoker(() => {
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
    }

    public class GetMetricsRequest
    {
        public HttpWebRequest HttpWebRequest { get; set; }
    }

}
