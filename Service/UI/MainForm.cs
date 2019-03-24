using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Models;
namespace UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
           
        }  

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeProgramList()
        {
            var path = ConfigurationManager.AppSettings["ProgrammListFilePath"];
            List<Item> items = new List<Item>();
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            ProgramList.Columns.Add("Name");
            ProgramList.Items.AddRange(items.Select(x=> new ListViewItem(x.Name)).ToArray());
        }
    }
}
