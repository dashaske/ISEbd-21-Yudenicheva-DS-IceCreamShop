using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.ViewModels;
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
    public partial class FormReplenishmentWareHouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ComponentId
        {
            get { return Convert.ToInt32(comboBoxIngredient.SelectedValue); }
            set { comboBoxIngredient.SelectedValue = value; }
        }

        public int WareHouse
        {
            get { return Convert.ToInt32(comboBoxIngredient.SelectedValue); }
            set { comboBoxIngredient.SelectedValue = value; }
        }

        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }

        private readonly WareHouseLogic wareHouseLogic;

        public FormReplenishmentWareHouse(IngredientLogic logicIngredient, WareHouseLogic logicWareHouse)
        {
            InitializeComponent();
            wareHouseLogic = logicWareHouse;

            List<IngredientViewModel> listIngredients = logicIngredient.Read(null);
            if (listIngredients != null)
            {
                comboBoxIngredient.DisplayMember = "IngredientName";
                comboBoxIngredient.ValueMember = "Id";
                comboBoxIngredient.DataSource = listIngredients;
                comboBoxIngredient.SelectedItem = null;
            }

            List<WareHouseViewModel> listWareHouses = logicWareHouse.Read(null);
            if (listWareHouses != null)
            {
                comboBoxWareHouse.DisplayMember = "WareHouseName";
                comboBoxWareHouse.ValueMember = "Id";
                comboBoxWareHouse.DataSource = listWareHouses;
                comboBoxWareHouse.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxIngredient.SelectedValue == null)
            {
                MessageBox.Show("Выберите продукт", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (comboBoxWareHouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }

            wareHouseLogic.Replenishment(new WareHouseReplenishmentBindingModel
            {
                IngredientId = Convert.ToInt32(comboBoxIngredient.SelectedValue),
                WareHouseId = Convert.ToInt32(comboBoxWareHouse.SelectedValue),
                Count = Convert.ToInt32(textBoxCount.Text)
            });

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
