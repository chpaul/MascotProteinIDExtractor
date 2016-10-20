using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MascotProteinIDExtractor
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }

        private MascotIDResultExtractor MascotResultExtractor;
        private void btnDATBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Mascot dat|*.dat";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDAT.Text = openFileDialog1.FileName;
            }
        }

        private void btnRawBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Theomo RAW|*.raw|mzXML|*.mzxml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRaw.Text = openFileDialog1.FileName;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtDAT.Text) && File.Exists(txtRaw.Text))
            {
                MascotResultExtractor = new MascotIDResultExtractor(txtDAT.Text, txtRaw.Text);
                MascotResultExtractor.MinMascotScore = Convert.ToSingle(txtMinMascotScore.Text);
                MascotResultExtractor.ReadMascotFile();
                //MascotResultExtractor.ProcessAll();
                //saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtRaw.Text) + ".csv";
                //saveFileDialog1.Filter = "CSV file|*.csv";
                //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    MascotResultExtractor.Export(saveFileDialog1.FileName);
                //    MessageBox.Show("Done");
                //}
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please check files");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int Total = MascotResultExtractor.MascotReader.PeptideQueries.Keys.Count;
            int Completed = 0;
            Parallel.ForEach(MascotResultExtractor.MascotReader.PeptideQueries.Keys, key =>
            {
                MascotResultExtractor.ProcessOneQuery(key);
            });
            //foreach (int key in MascotResultExtractor.MascotReader.PeptideQueries.Keys)
            //{

            //    MascotResultExtractor.ProcessOneQuery(key);
            //    Completed++;
            //    if (Completed%100 == 0)
            //    {
            //        backgroundWorker1.ReportProgress(Convert.ToInt32(Completed/(float) Total*100));
            //    }
            //}
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtRaw.Text) + "_MascotIDExtractor.csv";
            saveFileDialog1.Filter = "CSV file|*.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MascotResultExtractor.Export(saveFileDialog1.FileName);
                progressBar1.Value = 100;
                MessageBox.Show("Done");
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
