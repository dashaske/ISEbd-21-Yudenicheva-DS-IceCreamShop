using Microsoft.Reporting.WinForms;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.BusinessLogics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace IceCreamShopView
{
    public partial class FormReportOrdersByDate : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportOrdersByDate(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportOrdersByDate_Load(object sender, EventArgs e)
        {

            
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var dataSource = logic.GetOrdersInfo();
               
                ReportDataSource source = new ReportDataSource("DataSetOrders", dataSource);
                reportViewerOrders.LocalReport.DataSources.Add(source);
                reportViewerOrders.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveOrdersInfoToPdfFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
