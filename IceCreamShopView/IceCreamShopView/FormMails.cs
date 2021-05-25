using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.ViewModel;
using IceCreamShopBusinessLogic.BindingModels;
using System.Windows.Forms;

namespace IceCreamShopView
{
    public partial class FormMails : Form
    {
        private readonly MailLogic _mailLogic;

        private int pageNumber = 1;

        public FormMails(MailLogic mailLogic)
        {
            _mailLogic = mailLogic;
            InitializeComponent();
        }

        private void FormMails_Load(object sender, EventArgs e)
        {
            LoadData();
            textBoxPage.Text = pageNumber.ToString();
        }
        private void LoadData()
        {
            var list = _mailLogic.Read(new MessageInfoBindingModel
            {
                PageNumber = pageNumber
            });
            if (list != null)
            {
                dataGridView.DataSource = list;
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textBoxPage.Text = pageNumber.ToString();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            int stringsCountOnPage = _mailLogic.Read(new MessageInfoBindingModel
            {
                PageNumber = pageNumber + 1
            }).Count;

            if (stringsCountOnPage != 0)
            {
                pageNumber++;
                LoadData();
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (pageNumber > 1)
            {
                pageNumber--;
            }

            LoadData();
        }

        private void textBoxPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxPage.Text != "")
                {
                    int pageNumberValue = Convert.ToInt32(textBoxPage.Text);

                    if (pageNumberValue < 1)
                    {
                        throw new Exception();
                    }

                    int stringsCountOnPage = _mailLogic.Read(new MessageInfoBindingModel
                    {
                        PageNumber = pageNumberValue
                    }).Count;

                    if (stringsCountOnPage == 0)
                    {
                        throw new Exception();
                    }

                    pageNumber = pageNumberValue;
                    LoadData();
                }
            }
            catch (Exception)
            {
                textBoxPage.Text = pageNumber.ToString();
            }
        }
    }
}
