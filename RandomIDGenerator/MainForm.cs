using RandomIDSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomIDGenerator
{
    public partial class MainForm : Form
    {
        private RandomID randomID;

        public MainForm()
        {
            InitializeComponent();
            randomID = new RandomID();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string seedText = seedTextBox.Text.Trim();
            if (seedText.Length == 0)
            {
                seedText = "0";
            }

            int.TryParse(seedText, out int seed);
            randomID.MakeIDs(seed);
        }

        private void GetButton_Click(object sender, EventArgs e)
        {
            GetID();
        }

        private void GetID()
        {
            string indexText = indexTextBox.Text.Trim();

            if (indexText.Length == 0)
            {
                indexText = "0";
            }

            int.TryParse(indexText, out int index);

            string id = randomID.GetID(index);
            idTextBox.Text = id;

            string intervalText = intervalTextBox.Text.Trim();
            bool correctFormat = int.TryParse(intervalText, out int interval);
            if (!correctFormat || interval == 0)
            {
                intervalTextBox.Text = "1";
                interval = 1;
            }

            index += interval;
            indexTextBox.Text = index.ToString();

            histroyTextBox.AppendText(id + "\n");
            histroyTextBox.ScrollToCaret();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            histroyTextBox.Text = "";
        }

        private void IndexTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetID();
            }
        }
    }
}
