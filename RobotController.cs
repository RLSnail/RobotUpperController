using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace RobotUpprtController
{
    public class RobotController
    {
        //定义一个串口控制器
        SerialController serialController;
        //定义一个参数字典，保存下位机发来的数据
        Dictionary<string, string[]> parameters = new Dictionary<string, string[]>();

        public RobotController(string portName, int baudrate)
        {
            //初始化串口控制器
            serialController = new SerialController(portName, baudrate);
            serialController.DataReceived += new DataReceivedHandler(OnDataReceived);
        }

        void OnDataReceived(string name, string[] values)
        {
            //串口控制器收到了下位机传来的格式化数据，每次收到数据都要更新参数字典
            //先判断有没有这个属性
            if (parameters.ContainsKey(name))
            {
                parameters[name] = values;
            }
            else
            {
                //如果属性不存在，那就添加进去
                parameters.Add(name, values);
            }
        }
    }
}
