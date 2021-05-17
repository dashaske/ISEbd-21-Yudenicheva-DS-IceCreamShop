using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace IceCreamShopView
{
    public partial class FormReportWareHouseIngredients : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportWareHouseIngredients(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportStoreHouseComponents_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = logic.GetWareHouseIngredient();
                if (dict != null)
                {
                    dataGridViewReportWareHouseIngredients.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridViewReportWareHouseIngredients.Rows.Add(new object[]
                        {
                            elem.WareHouseName, "", ""
                        });
                        foreach (var listElem in elem.Ingredients)
                        {
                            dataGridViewReportWareHouseIngredients.Rows.Add(new object[]
                            {
                                "", listElem.Item1, listElem.Item2
                            });
                        }
                        dataGridViewReportWareHouseIngredients.Rows.Add(new object[]
                        {
                            "Итого", "", elem.TotalCount
                        });
                        dataGridViewReportWareHouseIngredients.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveWareHouseIngredientsToExcel(new ReportBindingModel
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
