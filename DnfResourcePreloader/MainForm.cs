using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DnfResourcePreloader
{
    public partial class MainForm : Form
    {
        private FileCacheLogic cacheLogic_ = new FileCacheLogic();
        private DnfResourcePreset preset = DnfResourcePreset.kFull;

        public MainForm()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void SetProgressGague(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                //progressBar1.Invoke(new Action<int>(SetProgressGague), new object[] { value });
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private void PrintLogMessage(String msg)
        {
            if (textBox2.InvokeRequired)
            {
               //textBox2.Invoke(new Action<String>(PrintLogMessage), new object[] { msg });
            }
            else
            {
                textBox2.AppendText(msg + "\n");
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        private void InitializeSettings()
        {
            preset = DnfResourcePreset.kMajor;
            cacheLogic_.SetProgressGaugeCallback(SetProgressGague);
            cacheLogic_.SetPrintfLogCallback(PrintLogMessage);

            String path = cacheLogic_.GetDNFInstallPath();
            if (String.IsNullOrEmpty(path) == false)
            {
                textBox1.Text = path;
            }
            else
            {
            }
        }

        // Select Path
        private void Button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }
        }

        // Run
        private void Button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            SetProgressGague(0);
            cacheLogic_.Run(textBox1.Text, preset);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kFull;
            if (radioButton1.Checked)
            {
                PrintLogMessage("Preload all images, sounds, musics and videos");
            }
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kMajor;
            if (radioButton2.Checked)
            {
                PrintLogMessage("Preload major contents images, sounds, musics and raid cut scene videos.");
                PrintLogMessage("This preset is targeted to all contents except event.");
            }
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kTypical;
            if (radioButton3.Checked)
            {
                PrintLogMessage("Preload typical contents images, sounds, musics and raid cut scene videos.");
                PrintLogMessage("This preset is targeted to only high level content. (hell, raid, tayberrs, etc.)");
            }
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kRaid;
            if (radioButton4.Checked)
            {
                PrintLogMessage("Preload raid contents images, sounds, musics and raid cut scene videos.");
                PrintLogMessage("This preset is targeted to only raid content. (anton, luke, fiendwar.)");
            }
        }

        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kPvp;
            if (radioButton5.Checked)
            {
                PrintLogMessage("Preload pvp contents images and sounds.");
                PrintLogMessage("This preset is targeted to only pvp contents. (All resource of characters.)");
            }
        }

        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            preset = DnfResourcePreset.kNone;
            if (radioButton6.Checked)
            {
                PrintLogMessage("Preload every file.");
                PrintLogMessage("This preset is targeted to any games. If you want, just use it.");
            }
        }
    }
}
