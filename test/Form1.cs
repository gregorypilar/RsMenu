using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rsMenu1.WebServiceUrl = "";// url del servicio web de rs
            rsMenu1.Reportpath = "";// el path al folder que queremos mostrar
            rsMenu1.UseDefaultCredentials = true;
            rsMenu1.Reportes();
        }

        
    }
}
