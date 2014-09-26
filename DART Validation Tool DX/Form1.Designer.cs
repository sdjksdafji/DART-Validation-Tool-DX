namespace DART_Validation_Tool_DX
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			DevExpress.XtraCharts.SimpleDiagram3D simpleDiagram3D1 = new DevExpress.XtraCharts.SimpleDiagram3D();
			DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.Pie3DSeriesLabel pie3DSeriesLabel1 = new DevExpress.XtraCharts.Pie3DSeriesLabel();
			DevExpress.XtraCharts.PiePointOptions piePointOptions1 = new DevExpress.XtraCharts.PiePointOptions();
			DevExpress.XtraCharts.Pie3DSeriesView pie3DSeriesView1 = new DevExpress.XtraCharts.Pie3DSeriesView();
			DevExpress.XtraCharts.Pie3DSeriesLabel pie3DSeriesLabel2 = new DevExpress.XtraCharts.Pie3DSeriesLabel();
			DevExpress.XtraCharts.PiePointOptions piePointOptions2 = new DevExpress.XtraCharts.PiePointOptions();
			DevExpress.XtraCharts.Pie3DSeriesView pie3DSeriesView2 = new DevExpress.XtraCharts.Pie3DSeriesView();
			DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
			this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.gfsTextInput = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.osiTextInput = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.connectButton = new DevExpress.XtraBars.BarButtonItem();
			this.metricsBox = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.instancesBox = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemComboBox3 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroupServerInfo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroupConnect = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroupMetricsAndInstances = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroupValidate = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.diffResult = new DevExpress.XtraVerticalGrid.VGridControl();
			this.resultChart = new DevExpress.XtraCharts.ChartControl();
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.diffResult)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.resultChart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(simpleDiagram3D1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesView2)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbonControl1
			// 
			this.ribbonControl1.ExpandCollapseItem.Id = 0;
			this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.gfsTextInput,
            this.osiTextInput,
            this.connectButton,
            this.metricsBox,
            this.instancesBox,
            this.barButtonItem1});
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.MaxItemId = 11;
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
			this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemComboBox1,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemComboBox2,
            this.repositoryItemComboBox3});
			this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
			this.ribbonControl1.Size = new System.Drawing.Size(602, 144);
			// 
			// gfsTextInput
			// 
			this.gfsTextInput.Caption = "DART GFS";
			this.gfsTextInput.Edit = this.repositoryItemTextEdit1;
			this.gfsTextInput.Id = 2;
			this.gfsTextInput.Name = "gfsTextInput";
			this.gfsTextInput.Width = 150;
			this.gfsTextInput.EditValueChanged += new System.EventHandler(this.gfsTextInput_EditValueChanged);
			// 
			// repositoryItemTextEdit1
			// 
			this.repositoryItemTextEdit1.AutoHeight = false;
			this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
			// 
			// osiTextInput
			// 
			this.osiTextInput.Caption = "DART OSI";
			this.osiTextInput.Edit = this.repositoryItemTextEdit2;
			this.osiTextInput.Id = 4;
			this.osiTextInput.Name = "osiTextInput";
			this.osiTextInput.Width = 150;
			this.osiTextInput.EditValueChanged += new System.EventHandler(this.osiTextInput_EditValueChanged);
			// 
			// repositoryItemTextEdit2
			// 
			this.repositoryItemTextEdit2.AutoHeight = false;
			this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
			// 
			// connectButton
			// 
			this.connectButton.Caption = "   Connect   ";
			this.connectButton.Glyph = ((System.Drawing.Image)(resources.GetObject("connectButton.Glyph")));
			this.connectButton.Id = 5;
			this.connectButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("connectButton.LargeGlyph")));
			this.connectButton.Name = "connectButton";
			this.connectButton.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
			this.connectButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.connectButton_ItemClick);
			// 
			// metricsBox
			// 
			this.metricsBox.Caption = "Metrics:    ";
			this.metricsBox.Edit = this.repositoryItemComboBox2;
			this.metricsBox.Id = 8;
			this.metricsBox.Name = "metricsBox";
			this.metricsBox.Width = 150;
			this.metricsBox.EditValueChanged += new System.EventHandler(this.metricsBox_EditValueChanged);
			// 
			// repositoryItemComboBox2
			// 
			this.repositoryItemComboBox2.AutoHeight = false;
			this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
			// 
			// instancesBox
			// 
			this.instancesBox.Caption = "Instances:";
			this.instancesBox.Edit = this.repositoryItemComboBox3;
			this.instancesBox.Id = 9;
			this.instancesBox.Name = "instancesBox";
			this.instancesBox.Width = 150;
			// 
			// repositoryItemComboBox3
			// 
			this.repositoryItemComboBox3.AutoHeight = false;
			this.repositoryItemComboBox3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemComboBox3.Name = "repositoryItemComboBox3";
			// 
			// barButtonItem1
			// 
			this.barButtonItem1.Caption = "   Validate   ";
			this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
			this.barButtonItem1.Id = 10;
			this.barButtonItem1.Name = "barButtonItem1";
			this.barButtonItem1.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
			this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
			// 
			// ribbonPage1
			// 
			this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupServerInfo,
            this.ribbonPageGroupConnect,
            this.ribbonPageGroupMetricsAndInstances,
            this.ribbonPageGroupValidate});
			this.ribbonPage1.Name = "ribbonPage1";
			this.ribbonPage1.Text = "Connect";
			// 
			// ribbonPageGroupServerInfo
			// 
			this.ribbonPageGroupServerInfo.ItemLinks.Add(this.gfsTextInput);
			this.ribbonPageGroupServerInfo.ItemLinks.Add(this.osiTextInput);
			this.ribbonPageGroupServerInfo.Name = "ribbonPageGroupServerInfo";
			this.ribbonPageGroupServerInfo.Text = "Sever Info";
			// 
			// ribbonPageGroupConnect
			// 
			this.ribbonPageGroupConnect.ItemLinks.Add(this.connectButton);
			this.ribbonPageGroupConnect.Name = "ribbonPageGroupConnect";
			this.ribbonPageGroupConnect.Text = "Connect";
			// 
			// ribbonPageGroupMetricsAndInstances
			// 
			this.ribbonPageGroupMetricsAndInstances.ItemLinks.Add(this.metricsBox);
			this.ribbonPageGroupMetricsAndInstances.ItemLinks.Add(this.instancesBox);
			this.ribbonPageGroupMetricsAndInstances.Name = "ribbonPageGroupMetricsAndInstances";
			this.ribbonPageGroupMetricsAndInstances.Text = "Metrics And Instances";
			// 
			// ribbonPageGroupValidate
			// 
			this.ribbonPageGroupValidate.ItemLinks.Add(this.barButtonItem1);
			this.ribbonPageGroupValidate.Name = "ribbonPageGroupValidate";
			this.ribbonPageGroupValidate.Text = "Validate";
			// 
			// repositoryItemCheckEdit1
			// 
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.Caption = "Check";
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			// 
			// repositoryItemComboBox1
			// 
			this.repositoryItemComboBox1.AutoHeight = false;
			this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
			// 
			// repositoryItemLookUpEdit1
			// 
			this.repositoryItemLookUpEdit1.AutoHeight = false;
			this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
			// 
			// diffResult
			// 
			this.diffResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diffResult.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.diffResult.Location = new System.Drawing.Point(0, 144);
			this.diffResult.Name = "diffResult";
			this.diffResult.Size = new System.Drawing.Size(602, 210);
			this.diffResult.TabIndex = 3;
			// 
			// resultChart
			// 
			simpleDiagram3D1.RotationMatrixSerializable = "1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1";
			this.resultChart.Diagram = simpleDiagram3D1;
			this.resultChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultChart.Location = new System.Drawing.Point(0, 144);
			this.resultChart.Name = "resultChart";
			piePointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Percent;
			pie3DSeriesLabel1.PointOptions = piePointOptions1;
			series1.Label = pie3DSeriesLabel1;
			series1.Name = "Series 1";
			pie3DSeriesView1.SizeAsPercentage = 100D;
			series1.View = pie3DSeriesView1;
			this.resultChart.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
			piePointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
			pie3DSeriesLabel2.PointOptions = piePointOptions2;
			this.resultChart.SeriesTemplate.Label = pie3DSeriesLabel2;
			pie3DSeriesView2.SizeAsPercentage = 100D;
			this.resultChart.SeriesTemplate.View = pie3DSeriesView2;
			this.resultChart.Size = new System.Drawing.Size(602, 210);
			this.resultChart.TabIndex = 5;
			chartTitle1.Text = "Validation Result";
			this.resultChart.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
			this.resultChart.Visible = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(602, 354);
			this.Controls.Add(this.resultChart);
			this.Controls.Add(this.diffResult);
			this.Controls.Add(this.ribbonControl1);
			this.Name = "Form1";
			this.Ribbon = this.ribbonControl1;
			this.Text = "DART Validation Tool";
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.diffResult)).EndInit();
			((System.ComponentModel.ISupportInitialize)(simpleDiagram3D1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pie3DSeriesView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.resultChart)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupServerInfo;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupConnect;
		private DevExpress.XtraBars.BarButtonItem connectButton;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupMetricsAndInstances;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
		private DevExpress.XtraBars.BarEditItem metricsBox;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
		private DevExpress.XtraBars.BarEditItem instancesBox;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox3;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupValidate;
		private DevExpress.XtraBars.BarButtonItem barButtonItem1;
		public DevExpress.XtraBars.BarEditItem gfsTextInput;
		public DevExpress.XtraBars.BarEditItem osiTextInput;
		public DevExpress.XtraVerticalGrid.VGridControl diffResult;
		private DevExpress.XtraCharts.ChartControl resultChart;

	}
}

