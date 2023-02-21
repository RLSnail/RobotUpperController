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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SerialController serialCommandController = new SerialController("COM3", 115200);
            serialCommandController.SerialControllerDataReceived += new SerialControllerDataReceivedHandler(OnSerialDataReceived);
            serialCommandController.OpenSerialPort();
            serialCommandController.StartReceive();
            
        }

        void OnSerialDataReceived(string name, string[] values)
        {
            Console.WriteLine("name=" + name);
            foreach (string value in values) 
            {
                Console.WriteLine("value=" + value);
            }
        }
    }
}
