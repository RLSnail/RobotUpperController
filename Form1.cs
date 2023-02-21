using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotUpprtController
{
    public partial class Form1 : Form
    {
        RobotController rc = null;
        public Form1()
        {
            InitializeComponent();

            //初始化机器人控制器
            rc = new RobotController("COM3", 115200);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
