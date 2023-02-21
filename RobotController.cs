﻿using System;
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
            serialController.SerialControllerDataReceived += new SerialControllerDataReceivedHandler(OnSerialControllerDataReceived);
        }

        void OnSerialControllerDataReceived(string name, string[] values)
        {
            //串口控制器收到了下位机传来的格式化数据，直接转发到上一层级
            RobotDataReceived(name, values);
        }

        void SendStr(string str) 
        {
            if (serialController != null)
            {
                serialController.sendStr(str);
            }
            else
            {
                //串口控制器没初始化，不用管
            }
        }

        /*
         * 下面是运动控制相关代码
         */

        //平移控制
        public void Translation(int x, int y)
        {
            if (x > 0)
            {
                serialController.sendCmd("FORWARD", string.Format("{0}", x));
            }
            else
            {
                serialController.sendCmd("BACKWARD", string.Format("{0}", -x));
            }

            if (y > 0)
            {
                serialController.sendCmd("LEFT", string.Format("{0}", y));
            }
            else
            {
                serialController.sendCmd("RIGHT", string.Format("{0}", -y));
            }
        }

        //旋转控制
        public void Rotate(int angle)
        {
            if (angle > 0)
            {
                serialController.sendCmd("TL", string.Format("{0}", angle));
            }
            else
            {
                serialController.sendCmd("TR", string.Format("{0}", -angle));
            }
        }
    }
}
