using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using RfidApiLib;  // ÐÞ¸Ä£¬È¥µôÕâ¸öÒýÓÃ£¬Í¬Ê±ÔÚ½â¾ö·½°¸µÄÒýÓÃÀ¸ÖÐÉ¾³ýÕâ¸ö¿âµÄÒýÓÃ¡£
using System.Threading;

namespace Demo
{
    public partial class Form1 : Form
    {
        //DesktopRfidApi.RfidApi Api = new DesktopRfidApi.RfidApi();
        RfidApi Api = new RfidApi();
        public byte IsoReading = 0;
        public byte EpcReading = 0;
        public int TagCnt = 0;
        public int ScanTimes = 0;
        public List<String> allID = new List<string>();
        private System.Windows.Forms.Timer timer1;
        public Form1()
        {
            InitializeComponent();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 300; // 1 second
            timer1.Start();
            Logger.LogThisLine("12312312");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cCommPort.SelectedIndex = 0;
            //cBaudrate.SelectedIndex = 0;
            bRs232Con.Enabled = true;
            bRs232Discon.Enabled = false;
            
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            Logger.LogThisLine("vao");
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < allID.Count; i++)
            {
                stringBuilder.Append(allID[i]);
                stringBuilder.Append(Environment.NewLine);
            }
            ThreadHelperClass.SetText(this,textBox,stringBuilder.ToString());
            
        }
        private void bRs232Discon_Click(object sender, EventArgs e)
        {

        }

        private void bRs232Con_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        Thread thread;

        private void bRs232Con_Click_1(object sender, EventArgs e)
        {
            int status;
            byte v1 = 0;
            byte v2 = 0;
            status = Api.OpenCommPort(cCommPort.Text);
            if (status != 0)
            {
                MessageBox.Show("Cổng không tồn tại");
                return;
            }
            status = Api.GetFirmwareVersion(ref v1, ref v2);
            if (status != 0)
            {
                MessageBox.Show("Không thể kết nối đến đầu đọc");
                Api.CloseCommPort();
                return;
            }
            startRead();


            thread = new Thread(new ThreadStart(runForever));
            thread.Start();
        }
        private void startRead()
        {
            if (EpcReading == 0)
            {
                Api.ClearIdBuf();
                TagCnt = 0;
                ScanTimes = 9999;
                EpcReading = 1;
            }
            else
            {
                EpcReading = 0;
            }
        }



        private void runForever()
        {
            while (true)
            {
                int status;
                int i, j;
                byte[,] IsoBuf = new byte[100, 12];
                byte tag_cnt = 0;
                string s = "";
                string s1 = "";
                byte tag_flag = 0;
                status = Api.EpcMultiTagIdentify(ref IsoBuf, ref tag_cnt, ref tag_flag);
                if (tag_cnt > 0)
                {
                    for (i = 0; i < tag_cnt; i++)
                    {
                        for (j = 0; j < 12; j++)
                        {
                            s = string.Format("{0:X2}", IsoBuf[i, j]);
                            s1 += s;
                        }
                       
                    }
                    while (s1.Length != 0)
                    {
                        string s2 = s1.Substring(0, 24);
                        allID.Add(s2);
                        s1 = s1.Replace(s2,"");
                    }
                    //if (s1.Length > 24)
                    //{
                    //    s2 = s1.Substring(0, 23);
                    //    s3 = s1.Substring(24, 47);
                    //    allID.Add(s2);
                    //    allID.Add(s3);
                
                    //}
                    //else
                    //{
                    //    allID.Add(s1);
                    //}
                }
               
            }
        }
        private void show()
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //label1.ReadOnly = false;
        }
    }
    public static class ThreadHelperClass
    {
        delegate void SetTextCallback(Form f, Control ctrl, string text);

        public static void SetText(Form form, Control ctrl, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;               
            }
        }
    }
    public static class Logger
    {
        private static StreamWriter swLog;
        private const string sLOG_FILE_PATH = "log.txt";

        static Logger()
        {
            Logger.OpenLogger();
        }

        public static void OpenLogger()
        {
            Logger.swLog = new StreamWriter(sLOG_FILE_PATH, false);
            Logger.swLog.AutoFlush = true;
        }

        public static void LogThisLine(string sLogLine)
        {
            Logger.swLog.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + sLogLine);
            Logger.swLog.Flush();
        }

        public static void CloseLogger()
        {
            Logger.swLog.Flush();
            Logger.swLog.Close();
        }
    }

}
/*
 * 
 */