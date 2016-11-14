using DB_Works.My_DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DB_Works
{
    public partial class Frmmain : Form
    {
        public Frmmain()
        {
            InitializeComponent();

            PiggyTable.Initialise();

        }

        public void UpdateList()
        {
            var table = PiggyTable.LoadTable();
            listView1.Items.Clear();
            foreach (DataRow item in table.Rows)
            {
                var listitem = listView1.Items.Add(item.ItemArray[0].ToString());
                listitem.SubItems.Add(item.ItemArray[1].ToString());
                listitem.SubItems.Add(item.ItemArray[2].ToString());
            }
        }


        private void Frmmain_Load(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == "edit")
                {
                    PiggyTable.update(int.Parse(button1.Tag.ToString()), textBox1.Text, textBox2.Text);
                    button1.Text = "Insert";
                    button1.Tag = "";
                    MessageBox.Show("Data has been updated");
                }
                else
                {
                    PiggyTable.insert(textBox1.Text, textBox2.Text);
                    MessageBox.Show("Data has been inserted");
                }
                UpdateList();
                textBox1.Text = "";
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                    throw new Exception("select row first");

                button1.Tag = listView1.SelectedItems[0].Text;
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
                button1.Text = "edit";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                    throw new Exception("select row first");

                PiggyTable.delete(int.Parse(listView1.SelectedItems[0].Text));
                UpdateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
