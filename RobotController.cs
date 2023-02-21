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
    public class RobotController
    {
        //定义一个串口控制器
        SerialController serialController;
        //定义一个定时器定时更新数据
        System.Timers.Timer timer_update = null;

        public RobotController(string portName, int baudrate)
        {
            //初始化串口控制器
            serialController = new SerialController(portName, baudrate);
            serialController.DataReceived += new DataReceivedHandler(OnDataReceived);

            //初始化定时器
        }

        void OnDataReceived(string name, string[] values)
        {
            //串口控制器收到了下位机传来的格式化数据
        }
    }
}
