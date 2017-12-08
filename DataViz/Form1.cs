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

namespace DataViz
{
    public partial class Form1 : Form
    {
        private List<DataSet> DataSets { get; set; }
        private int SetIndex { get; set; }

        public Form1()
        {
            InitializeComponent();
            DataSets = new List<DataSet>();
            SetIndex = 0;
            buttonLeft.Enabled = false;
            buttonRight.Enabled = false;
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var reader = new StreamReader(ofd.OpenFile()))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var elements = line.Split(' ');
                            if (elements.Contains("OPEN")) continue;
                            if (elements.Contains("CLOSE")) continue;
                            if (elements.Contains("STOP")) continue;
                            if (elements.Contains("START"))
                            {
                                DataSets.Add(new DataSet(long.Parse(elements[0])));
                                continue;
                            }
                            if (elements.Length < 7) continue;
                            DataSets.Last().Add(new DataPoint(long.Parse(elements[0]), double.Parse(elements[1]), double.Parse(elements[2]), double.Parse(elements[3]), double.Parse(elements[4]), double.Parse(elements[5]), double.Parse(elements[6])));
                        }
                    }
                    buttonRight.Enabled = DataSets.Count > 1;
                    
                    var dataPoints = File.ReadAllLines(ofd.FileName)
                        .Where(line => line.Split(' ').Length == 7)
                        .Select(line =>
                            {
                                var split = line.Split(' ');
                                if (split.Length < 6) return null;
                                return new DataPoint(long.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]), double.Parse(split[4]), double.Parse(split[5]), double.Parse(split[6]));
                            }).ToList();
                    bindData();
                }
            }
        }

        private void bindData()
        {
            var date = DataSets[SetIndex].Date;
            dataChart.Titles[0].Text = date.ToShortDateString() + " " + date.ToLongTimeString();
            dataChart.DataSource = DataSets[SetIndex].DataPoints;
            foreach (var series in dataChart.Series)
            {
                series.XValueMember = "TimeString";
            }
            dataChart.Series[0].YValueMembers = "AX";
            dataChart.Series[1].YValueMembers = "AY";
            dataChart.Series[2].YValueMembers = "AZ";
            dataChart.Series[3].YValueMembers = "RX";
            dataChart.Series[4].YValueMembers = "RY";
            dataChart.Series[5].YValueMembers = "RZ";
            dataChart.DataBind();
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            SetIndex++;
            buttonRight.Enabled = SetIndex < DataSets.Count - 1;
            buttonLeft.Enabled = true;
            bindData();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            SetIndex--;
            buttonLeft.Enabled = SetIndex > 0;
            buttonRight.Enabled = true;
            bindData();
        }
    }
}
