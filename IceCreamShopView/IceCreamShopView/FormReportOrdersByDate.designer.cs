
namespace IceCreamShopView
{
    partial class FormReportOrdersByDate
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewerOrders = new Microsoft.Reporting.WinForms.ReportViewer();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.ReportOrderByDateViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ReportOrderByDateViewModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewerOrders
            // 
            reportDataSource1.Name = "DataSet";
            reportDataSource1.Value = this.ReportOrderByDateViewModelBindingSource;
            this.reportViewerOrders.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerOrders.LocalReport.ReportEmbeddedResource = "IceCreamShopView.ReportOrders.rdlc";
            this.reportViewerOrders.Location = new System.Drawing.Point(-3, 64);
            this.reportViewerOrders.Name = "reportViewerOrders";
            this.reportViewerOrders.ServerReport.BearerToken = null;
            this.reportViewerOrders.Size = new System.Drawing.Size(807, 372);
            this.reportViewerOrders.TabIndex = 6;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(698, 16);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(91, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "В Pdf";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(595, 15);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(97, 23);
            this.buttonCreate.TabIndex = 7;
            this.buttonCreate.Text = "Сформировать";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // ReportOrderByDateViewModelBindingSource
            // 
            this.ReportOrderByDateViewModelBindingSource.DataSource = typeof(IceCreamShopBusinessLogic.ViewModel.ReportOrderByDateViewModel);
            // 
            // FormReportOrdersByDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewerOrders);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCreate);
            this.Name = "FormReportOrdersByDate";
            this.Text = "Отчёты по заказам";
            this.Load += new System.EventHandler(this.FormReportOrdersByDate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportOrderByDateViewModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewerOrders;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.BindingSource ReportOrderByDateViewModelBindingSource;
    }
}