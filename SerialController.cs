using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Timers;

namespace RobotUpprtController
{
    //定义收到数据时的委托
    public delegate void SerialControllerDataReceivedHandler(string name, string[] values);

    internal class SerialController
    {
        //定义一个收到数据的事件
        public event SerialControllerDataReceivedHandler SerialControllerDataReceived;
        //定义收发数据用的串口
        SerialPort serial = null;
        //定义当前接收到的数据
        string received_data;
        //定义数据队列
        Queue<string> data_queue = null;
        //定义一个定时器用于定时解析数据队列中的数据
        Timer timer_parse = null;

        public SerialController(string portName, int badurate) 
        {
            //初始化数据队列
            data_queue= new Queue<string>();

            //初始化数据解析定时器
            timer_parse = new Timer();
            timer_parse.Interval = 200; //每隔200ms解析一次
            timer_parse.Elapsed += new ElapsedEventHandler(OnTimerParseElapsed);

            //初始化串口
            this.serial = new SerialPort
            {
                PortName = portName,        //端口名
                BaudRate = badurate,        //波特率
                DataBits = 8,               //数据位
                Parity = Parity.None,       //校验位
                StopBits= StopBits.One      //停止位
            };

            //添加收到串口数据时的处理函数
            this.serial.ReceivedBytesThreshold = 1; //设置每收到1字节就调用一次处理函数
            this.serial.DataReceived += new SerialDataReceivedEventHandler(this.OnSerialDataReceived);
        }

        //打开串口
        public void OpenSerialPort() 
        {
            if (!this.serial.IsOpen)
            {
                //打开串口
                this.serial.Open();
            }
            else
            {
                //串口已经打开了，不处理
            }
        }

        //启动接收
        public void StartReceive()
        {
            //启动定时器
            this.timer_parse.Start();
        }

        //串口收到数据时的处理函数
        void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //接收到的数据都是以\n结尾
            //每次触发这个函数都把缓冲区里的数据全读出来
            while(this.serial.BytesToRead > 0) 
            {
                char ch = (char)this.serial.ReadByte();
                if (ch == '\n')
                {
                    //读完了一条数据，把数据压入队列
                    data_queue.Enqueue(received_data);
                    //解析完成后清空当前接收的数据
                    received_data = "";
                }
                else
                {
                    //没遇到\n，把数据加入
                    received_data += ch;
                }
            }
        }

        //定时解析数据队列里的数据
        void OnTimerParseElapsed(object sender, ElapsedEventArgs e) 
        {
            /*
             * 数据格式：
             * name=value1,value2, ...
             * name、=、value之间没有空格，不同value之间用逗号分隔
             */
            //解析完成后调用DataReceived事件

            //先判断队列里有没有数据，没有数据则直接返回
            if (data_queue.Count == 0) return;

            //从队列里获取一个数据
            string data = this.data_queue.Dequeue();

            (string name, string[] values) = this.parseData(data);
            if(name != null && values != null) 
            {
                //解析成功才调用对应事件
                this.SerialControllerDataReceived(name, values);
            }
            else
            {
                //解析失败，暂不处理
            }
        }

        //解析数据的函数
        (string, string[]) parseData(string data)
        {
            //先用冒号分割
            string[] strings = data.Split(':');
            
            if(strings.Length == 2 )
            {
                //再用逗号分割
                string[] values = strings[1].Split(',');
                //解析完成，返回解析结果
                return (strings[0], values);
            }
            else
            {
                //解析出错
                return (null, null);
            }
        }

        //发送指令
        public bool sendCmd(string name, string value)
        {
            //判断串口是否已经打开
            if (this.serial.IsOpen)
            {
                string cmd = name + ":" + value + "\n";
                this.serial.Write(cmd);
                return true;
            }
            else
            {
                //串口没打开
                return false;
            }
        }
    }
}
