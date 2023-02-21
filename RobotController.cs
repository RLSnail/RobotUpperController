using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Timers;

namespace RobotUpprtController
{
    public delegate void RobotDataReceivedHandler(string name, string[] values);
    public class RobotController
    {
        public event RobotDataReceivedHandler RobotDataReceived;
        //定义一个串口控制器
        SerialController serialController;

        public RobotController(string portName, int baudrate)
        {
            //初始化串口控制器
            serialController = new SerialController(portName, baudrate);
            serialController.SerialControllerDataReceived += new SerialControllerDataReceivedHandler(OnDataReceived);
        }

        void OnDataReceived(string name, string[] values)
        {
            //串口控制器收到了下位机传来的格式化数据，直接转发到上一层级
            RobotDataReceived(name, values);
        }
    }
}
