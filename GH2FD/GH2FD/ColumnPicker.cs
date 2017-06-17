using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GH2FD
{
    public partial class ColumnPicker : Form
    {
        Read_CSV csvreader;
        List<CheckBox> columns = new List<CheckBox>();

        public ColumnPicker(Read_CSV _csvreader)
        {
            InitializeComponent();
            csvreader = _csvreader;
        }

        private void ColumnPicker_Load(object sender, EventArgs e)
        {
            if (!(csvreader.columns.Length == 0))
            {
                foreach(string column in csvreader.columns)
                {
                    checkedListBox1.Items.Add(column);
                }
            }
        }

        private void ColumnPicker_FormClosed(object sender, FormClosedEventArgs e)
        {
            csvreader.cp_set = true;
            csvreader.c_picked.Clear();
            foreach(int ind in checkedListBox1.CheckedIndices)
            {
                csvreader.c_picked.Add(ind + 2);
            }
            csvreader.ExpireSolution(true);
        }
    }
}
