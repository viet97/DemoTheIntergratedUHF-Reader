using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RfidApiLib;  // 修改，去掉这个引用，同时在解决方案的引用栏中删除这个库的引用。

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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cCommPort.SelectedIndex = 0;
            //cBaudrate.SelectedIndex = 0;
            bRs232Con.Enabled = true;
            bRs232Discon.Enabled = false;
            bTcpCon.Enabled = true;
            bTcpDiscon.Enabled = false;
            bReset.Enabled = false;

            bRfSet.Enabled = false;
            bRfQuery.Enabled = false;

            bIsoId.Enabled = false;
            bIsoRead.Enabled = false;
            bIsoWrite.Enabled = false;
            bIsoLock.Enabled = false;
            bIsoQueryLock.Enabled = false;

            bEpcId.Enabled = false;
            bEpcRead.Enabled = false;
            bEpcWrite.Enabled = false;
            bEpcKill.Enabled = false;
            bEpcInit.Enabled = false;

            cIsoTimes.SelectedIndex = 0;
            cEpcTimes.SelectedIndex = 0;

            cEpcMembank.SelectedIndex = 1;
            int nLoop = 0;
            for (nLoop = 0; nLoop < 256; nLoop++)
                cEpcWordptr.Items.Add(Convert.ToString(nLoop));
            cEpcWordptr.SelectedIndex = 2;
            for (nLoop = 0; nLoop < 256; nLoop++)
                cEpcWordcnt.Items.Add(Convert.ToString(nLoop));
            cEpcWordcnt.SelectedIndex = 6;
        }

        private void bRs232Con_Click(object sender, EventArgs e)
        {
            int status;
            byte v1 = 0;
            byte v2 = 0;
            string s = "";
            status = Api.OpenCommPort(cCommPort.Text);
            if (status != 0)
            {
                lInfo.Items.Add("Open Comm Port Failed!");
                return;
            }
            status = Api.GetFirmwareVersion(ref v1,ref v2);
            if (status != 0)
            {
                lInfo.Items.Add("Can not connect with the reader!");
                Api.CloseCommPort();
                return;
            }
            lInfo.Items.Add("Connect the reader success!");
            s = string.Format("The reader's firmware version is:V{0:d2}.{1:d2}", v1, v2);
            lInfo.Items.Add(s);
            bTcpCon.Enabled = false;

            bReset.Enabled = true;

            bRs232Con.Enabled = false;
            bRs232Discon.Enabled = true;

            bRfSet.Enabled = true;
            bRfQuery.Enabled = true;

            bIsoId.Enabled = true;
            bIsoRead.Enabled = true;
            bIsoWrite.Enabled = true;
            bIsoLock.Enabled = true;
            bIsoQueryLock.Enabled = true;

            bEpcId.Enabled = true;
            bEpcRead.Enabled = true;
            bEpcWrite.Enabled = true;
            bEpcInit.Enabled = true;
            bEpcKill.Enabled = false;

            bRfQuery_Click(null,null);
        }

        private void bRs232Discon_Click(object sender, EventArgs e)
        {
            Api.CloseCommPort();
            lInfo.Items.Add("The connection has been closed !");
            bRs232Con.Enabled = true;
            bRs232Discon.Enabled = false;
            bTcpCon.Enabled = true;
            bTcpDiscon.Enabled = false;
            bReset.Enabled = false;

            bRfSet.Enabled = false;
            bRfQuery.Enabled = false;

            bIsoId.Enabled = false;
            bIsoRead.Enabled = false;
            bIsoWrite.Enabled = false;
            bIsoLock.Enabled = false;
            bIsoQueryLock.Enabled = false;

            bEpcId.Enabled = false;
            bEpcRead.Enabled = false;
            bEpcWrite.Enabled = false;
            bEpcKill.Enabled = false;
            bEpcInit.Enabled = false;
        }

        private void bTcpCon_Click(object sender, EventArgs e)
        {
            int status;
            int port;
            byte v1 = 0;
            byte v2 = 0;
            string s = "";
            try
            {
                port = int.Parse(tPort.Text);
                s = tIp.Text;
                
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input the ip address and port !");
                return;
            }
            status = Api.TcpConnectReader(tIp.Text, port);
            if (status != 0)
            {
                lInfo.Items.Add("Connect Reader Failed!");
                return;
            }
            status = Api.GetFirmwareVersion(ref v1, ref v2);
            if (status != 0)
            {
                lInfo.Items.Add("Can not connect with the reader!");
                Api.TcpCloseConnect();
                return;
            }
            lInfo.Items.Add("Connect the reader success!");
            s = string.Format("The reader's firmware version is:V{0:d2}.{1:d2}", v1, v2);
            lInfo.Items.Add(s);
            bTcpCon.Enabled = false;
            bTcpDiscon.Enabled = true;

            bReset.Enabled = true;

            bRs232Con.Enabled = false;
            bRs232Discon.Enabled = false;

            bRfSet.Enabled = true;
            bRfQuery.Enabled = true;

            bIsoId.Enabled = true;
            bIsoRead.Enabled = true;
            bIsoWrite.Enabled = true;
            bIsoLock.Enabled = true;
            bIsoQueryLock.Enabled = true;

            bEpcId.Enabled = true;
            bEpcRead.Enabled = true;
            bEpcWrite.Enabled = true;
            bEpcInit.Enabled = true;
            bEpcKill.Enabled = false;
            bRfQuery_Click(null, null);
        }

        private void bTcpDiscon_Click(object sender, EventArgs e)
        {
            Api.TcpCloseConnect();
            bRs232Con.Enabled = true;
            bRs232Discon.Enabled = false;
            bTcpCon.Enabled = true;
            bTcpDiscon.Enabled = false;
            bReset.Enabled = false;

            bRfSet.Enabled = false;
            bRfQuery.Enabled = false;

            bIsoId.Enabled = false;
            bIsoRead.Enabled = false;
            bIsoWrite.Enabled = false;
            bIsoLock.Enabled = false;
            bIsoQueryLock.Enabled = false;

            bEpcId.Enabled = false;
            bEpcRead.Enabled = false;
            bEpcWrite.Enabled = false;
            bEpcInit.Enabled = false;
        }

        private void bRfQuery_Click(object sender, EventArgs e)
        {
            byte pwr = 0;
            byte freq = 0;

            int status;

            status = Api.GetRf(ref pwr, ref freq);
            if (status != 0)
            {
                lInfo.Items.Add("Get Rf settings failed!");
                return;
            }
            tRfPwr.Value = pwr;
            cRfFreq.SelectedIndex = freq;
            lInfo.Items.Add("Get Rf settings success!");
        }

        private void bRfSet_Click(object sender, EventArgs e)
        {
            byte pwr = 0;
            byte freq = 0;

            int status;
            pwr = (byte)(tRfPwr.Value);
            freq = (byte)(cRfFreq.SelectedIndex);
            status = Api.SetRf(pwr, freq);
            if (status != 0)
            {
                lInfo.Items.Add("Set Rf settings failed!");
                return;
            }
            lInfo.Items.Add("Set Rf settings success!");
        }

        private void bIsoId_Click(object sender, EventArgs e)
        {
            if (IsoReading == 0)
            {
                Api.ClearIdBuf();
                lInfo.Items.Clear();
                lInfo.Items.Add("Start multiply tags identify!");
                TagCnt = 0;
                if (cIsoTimes.SelectedIndex > 0)
                    ScanTimes = Convert.ToInt16(cIsoTimes.Text);
                else
                    ScanTimes = 9999;
                timer1.Interval = (tIsoSpeed.Value + 1) * 20;
                timer1.Enabled = true;
                bIsoId.Text = "Stop";
                IsoReading = 1;
            }
            else
            {
                timer1.Enabled = false;
                IsoReading = 0;
                bIsoId.Text = "Identify";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int status;
            int i,j;
            byte[,] IsoBuf = new byte[100,12];
            byte tag_cnt = 0;
            string s = "";
            string s1 = "";

            if (cIsoTimes.SelectedIndex > 0)
            {
                Api.ClearIdBuf();
                ScanTimes--;
            }
            status = Api.IsoMultiTagIdentify(ref IsoBuf, ref tag_cnt);
            if (tag_cnt > 0)
            {
                for (i = 0; i < tag_cnt; i++)
                {
                    s1 = string.Format("NO.{0:D}: ", TagCnt);
                    for (j = 0; j < 8; j++)
                    {
                        s = string.Format("{0:X2} ", IsoBuf[i, j]);
                        s1 += s;
                    }
                    lInfo.Items.Add(s1);
                    TagCnt++;
                   
                }
            }
            if (ScanTimes <= 0)
            {
                bIsoId_Click(sender, e);
            }
        }

        private void bEpcId_Click(object sender, EventArgs e)
        {
            if (EpcReading == 0)
            {
                Api.ClearIdBuf();
                lInfo.Items.Clear();
                lInfo.Items.Add("Start multiply tags identify!");
                TagCnt = 0;
                if (cEpcTimes.SelectedIndex > 0)
                    ScanTimes = Convert.ToInt16(cEpcTimes.Text);
                else
                    ScanTimes = 9999;
                timer2.Interval = (tEpcSpeed.Value + 1) * 20;
                timer2.Enabled = true;
                bEpcId.Text = "Stop";
                EpcReading = 1;
            }
            else
            {
                timer2.Enabled = false;
                EpcReading = 0;
                bEpcId.Text = "Identify";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int status;
            int i, j;
            byte[,] IsoBuf = new byte[100, 12];
            byte tag_cnt = 0;
            string s = "";
            string s1 = "";
            byte tag_flag = 0;
            if (cEpcTimes.SelectedIndex > 0)
            {
                Api.ClearIdBuf();
                ScanTimes--;
            }
            status = Api.EpcMultiTagIdentify(ref IsoBuf, ref tag_cnt,ref tag_flag);
            if (tag_cnt > 0)
            {
                for (i = 0; i < tag_cnt; i++)
                {
                    s1 = string.Format("NO.{0:D}: ", TagCnt);
                    for (j = 0; j < 12; j++)
                    {
                        s = string.Format("{0:X2} ", IsoBuf[i, j]);
                        s1 += s;
                    }
                    lInfo.Items.Add(s1);
                    TagCnt++;
                }
            }
            if (ScanTimes <= 0)
            {
                bEpcId_Click(sender, e);
            }
        }

        private void bIsoRead_Click(object sender, EventArgs e)
        {
            int addr;
            int len;
            int i = 0;
            int status = 0;
            byte[] value = new byte[32];
            string s = "The data is:";
            string s1 = "";
            try
            {
                addr = int.Parse(tIsoAddr.Text);
                len = int.Parse(tIsoCnt.Text);
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input ByteAddr and ByteCnt");
                return;
            }
            
            for (i = 0; i < len; )
            {
                status = Api.IsoRead((byte)(addr+i), ref value);
                if (status != 0)
                {
                    lInfo.Items.Add("Read failed!");
                    return;
                }
                
                for (int j = 0; j < 8; j++)
                {
                    s1 = string.Format("{0:X2}", value[j]);
                    s += s1;
                    if (i + j >= len-1)
                        break;
                }
                i += 8;
            }
            if (status == 0)
            {
                lInfo.Items.Add("Read success!");
                lInfo.Items.Add(s);
            }
            
        }

        private void bIsoWrite_Click(object sender, EventArgs e)
        {
            byte[] value = new byte[16];
            int i = 0;
            int len;
            int addr;
            int status;
            string hexValues;
            try
            {
                addr = int.Parse(tIsoAddr.Text);
                len = int.Parse(tIsoCnt.Text);
                hexValues = tIsoData.Text;
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input the ByteAddr,ByteCnt and Data");
                return;
            }
            string[] hexValuesSplit = hexValues.Split(' ');
            try
            {
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    if (hex.Length >= 2)
                    {
                        value[i++] = Convert.ToByte(hex, 16);
                    }
                }
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input data needed");
                return;

            }
            if(i != len)
            {
                lInfo.Items.Add("Please input data needed");
                return;
            }
            for (i = 0; i < len; i++)
            {
                status = Api.IsoWrite((byte)(addr+i), value[i]);
                if (status != 0)
                {
                    lInfo.Items.Add("Write failed!");
                    return;
                }
            }
            lInfo.Items.Add("Write success!");
        }

        private void bEpcRead_Click(object sender, EventArgs e)
        {
            int membank;
            int wordptr;
            int wordcnt;
            int status = 0;
            byte[] value = new byte[16];

            string s = "The data is:";
            string s1 = "";

            membank = cEpcMembank.SelectedIndex;
            wordptr = cEpcWordptr.SelectedIndex;
            wordcnt = cEpcWordcnt.SelectedIndex;

            status = Api.EpcRead((byte)membank, (byte)wordptr, (byte)wordcnt, ref value);

            if (status != 0)
            {
                lInfo.Items.Add("Read failed!");
                return;
            }
            else
            {
                for (int i = 0; i < wordcnt * 2; i++)
                {
                    s1 = string.Format("{0:X2}", value[i]);
                    s += s1;
                }
                lInfo.Items.Add("Read success!");
                lInfo.Items.Add(s);
            }
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            lInfo.Items.Clear();
        }

        private void bEpcInit_Click(object sender, EventArgs e)
        {
            int status;

            status = Api.EpcInitEpc(96);

            if (status == 0)
            {
                lInfo.Items.Add("Init tag success!");
            }
            else
            {
                lInfo.Items.Add("Init tag failed!");
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void bEpcWrite_Click(object sender, EventArgs e)
        {
            ushort[] value = new ushort[16];
            int i = 0;
            byte membank;
            byte wordptr;
            byte wordcnt;
            int status;
            string hexValues;

            membank = (byte)(cEpcMembank.SelectedIndex);
            wordptr = (byte)(cEpcWordptr.SelectedIndex);
            wordcnt = (byte)(cEpcWordcnt.SelectedIndex);

            hexValues = tEpcData.Text;
            string[] hexValuesSplit = hexValues.Split(' ');
           try
            {
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    if (hex.Length >= 2)
                    {
                        int x = Convert.ToInt32(hex, 16);
                        value[i++] = (ushort)x;
                    }
                }
            }
           catch (Exception)
           {
               lInfo.Items.Add("Please input data needed");
               return;
           }
            if (i != wordcnt)
            {
                lInfo.Items.Add("Please input data needed");
                return;
            }
            for(byte j = 0; j < wordcnt; j++)
            {
                status = Api.EpcWrite(membank,(byte)(wordptr+j),value[j]);
                if (status != 0)
                {
                    lInfo.Items.Add("Write failed!");
                    return;
                }
            }
            lInfo.Items.Add("Write success!");
        }

        private void tIsoData_TextChanged(object sender, EventArgs e)
        {
            if (((tIsoData.Text.Length-2) % 3) == 0)
            {
                tIsoData.Text += " ";
                tIsoData.Select(tIsoData.Text.Length,0);
            }
        }

        private void tEpcData_TextChanged(object sender, EventArgs e)
        {
            if (((tEpcData.Text.Length - 4) % 5) == 0)
            {
                tEpcData.Text += " ";
                tEpcData.Select(tEpcData.Text.Length, 0);
            }
        }

        private void bIsoLock_Click(object sender, EventArgs e)
        {
            int addr = 0;
            int status;

            try
            {
                addr = int.Parse(tIsoAddr.Text);
            }
            catch(Exception)
            {
                lInfo.Items.Add("Please Input ByteAddr!");
                return;
            }
            status = Api.IsoLock((byte)addr);
            if (status == 0)
            {
                lInfo.Items.Add("Lock success!");
            }
            else
            {
                lInfo.Items.Add("Lock failed!");
            }
        }


        private void bIsoQueryLock_Click(object sender, EventArgs e)
        {
            int addr = 0;
            byte byQryLockStatus = 0;
            int status;
            //int qryStatus;
            try
            {
                addr = int.Parse(tIsoAddr.Text);
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please Input ByteAddr!");
                return;
            }
            status = Api.IsoQryLock((byte)addr, ref byQryLockStatus);
            if (status == 0)
            {
                if (1 == byQryLockStatus)
                {
                    lInfo.Items.Add("Is Lock! ");
                }
                else
                {
                    lInfo.Items.Add("Is Not Lock! ");
                }
            }
        }

        private void btnSecRead_Click(object sender, EventArgs e)
        {
            int membank;
            int wordptr;
            int wordcnt;
            int status = 0;
            byte[] value = new byte[16];

            string s = "The data is:";
            string s1 = "";
            if(tEpcAccess.TextLength!=8)
            {
                lInfo.Items.Add("Access Password length not enough");
                return;
            }
            uint unAccPwd;
            unAccPwd=Convert.ToUInt32(tEpcAccess.Text,16);
            membank = cEpcMembank.SelectedIndex;
            wordptr = cEpcWordptr.SelectedIndex;
            wordcnt = cEpcWordcnt.SelectedIndex;

            status = Api.Gen2SecRead(unAccPwd,(byte)membank, (byte)wordptr, (byte)wordcnt, ref value);
            if (status != 0)
            {
                lInfo.Items.Add("Read failed!");
                return;
            }
            else
            {
                for (int i = 0; i < wordcnt * 2; i++)
                {
                    s1 = string.Format("{0:X2}", value[i]);
                    s += s1;
                }
                lInfo.Items.Add("Read success!");
                lInfo.Items.Add(s);
            }
        }

        private void btnSecWrite_Click(object sender, EventArgs e)
        {
            ushort[] value = new ushort[16];
            int i = 0;
            byte membank;
            byte wordptr;
            byte wordcnt;
            int status;
            string hexValues;

            membank = (byte)(cEpcMembank.SelectedIndex);
            wordptr = (byte)(cEpcWordptr.SelectedIndex);
            wordcnt = (byte)(cEpcWordcnt.SelectedIndex);
            uint unAccPwd;
            unAccPwd = Convert.ToUInt32(tEpcAccess.Text, 16);

            hexValues = tEpcData.Text;
            string[] hexValuesSplit = hexValues.Split(' ');
            foreach (String hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                if (hex != "")
                {
                    int x = Convert.ToInt32(hex, 16);
                    value[i++] = (ushort)x;
                }
            }
            if (i != wordcnt)
            {
                lInfo.Items.Add("Please input data needed");
                return;
            }
            for (byte j = 0; j < wordcnt; j++)
            {
                status = Api.Gen2SecWrite(unAccPwd,membank, (byte)(wordptr + j), value[j]);
                if (status != 0)
                {
                    lInfo.Items.Add("Write failed!");
                    return;
                }
            }
            lInfo.Items.Add("Write success!");
        }

        private void bEpcKill_Click(object sender, EventArgs e)
        {
            int status = 0;
            byte[] value = new byte[16];

            string s = "";
            if (tEpcAccess.TextLength != 8)
            {
                lInfo.Items.Add("Access Password length not enough");
                return;
            }
            uint unAccPwd;
            unAccPwd = Convert.ToUInt32(tEpcAccess.Text, 16);
            status = Api.Gen2KillTag(unAccPwd);
            if (status != 0)
            {
                lInfo.Items.Add("Set Password failed!");
                return;
            }
            else
            {
                lInfo.Items.Add("Set Password success!");
                lInfo.Items.Add(s);
            }
        }

        private void btnSecLock_Click(object sender, EventArgs e)
        {
            byte membank;
            byte pwdLevel;

            int status = 0;
            byte[] value = new byte[16];

            string s = "";
            if (tEpcAccess.TextLength != 8)
            {
                lInfo.Items.Add("Access Password length not enough");
                return;
            }
            uint unAccPwd;
            switch(cEpcMembank.SelectedIndex)
            {
                case 0:
                    membank=3;
                    break;
                case 1:
                    membank=2;
                    break;
                case 2:
                    membank=1;
                    break;
                case 3:
                    membank=0;
                    break;
                default:
                    membank=2;
                    break;
            }
            pwdLevel = (byte)(cmbLevel.SelectedIndex);

            unAccPwd = Convert.ToUInt32(tEpcAccess.Text, 16);
            status = Api.Gen2SecLock(unAccPwd, membank,pwdLevel);
            if (status != 0)
            {
                lInfo.Items.Add("Lock EPC tag failed!");
                return;
            }
            else
            {
                lInfo.Items.Add("Lock EPC tag success!");
                lInfo.Items.Add(s);
            }
        }

        private void btnSetAccessPWD_Click(object sender, EventArgs e)
        {
            int status = 0;
            byte[] value = new byte[16];

            string s = "";
            if (tEpcAccess.TextLength != 8)
            {
                lInfo.Items.Add("Access Password length not enough");
                return;
            }
            uint unAccPwd;
            unAccPwd = Convert.ToUInt32(tEpcAccess.Text, 16);
            status = Api.Gen2SetAccPwd(unAccPwd);
            if (status != 0)
            {
                lInfo.Items.Add("Set Password failed!");
                return;
            }
            else
            {
                lInfo.Items.Add("Set Password success!");
                lInfo.Items.Add(s);
            }
        }

        private void btnISOReadWithID_Click(object sender, EventArgs e)
        {
            int addr;
            int len;
            int i = 0;
            int status = 0;
            byte byAntenna = 0;
            byte[] TagID = new byte[16];
            byte[] value = new byte[32];
            string s = "The data is:";
            string s1 = "";
            try
            {
                addr = int.Parse(tIsoAddr.Text);
                len = int.Parse(tIsoCnt.Text);
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input ByteAddr and ByteCnt");
                return;
            }
            string hexValues = txtTagID.Text;
            string[] hexValuesSplit = hexValues.Split(' ');
            try
            {
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    int x = Convert.ToInt32(hex, 16);
                    TagID[i++] = (byte)x;
                }
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input Tag ID needed");
                return;

            }
            if (i != 8)
            {
                lInfo.Items.Add("Please input Tag ID needed");
                return;
            }
            
            for (i = 0; i < len; )
            {
                status = Api.IsoReadWithID(TagID,(byte)(addr+i), ref value,ref byAntenna);
                if (status != 0)
                {
                    lInfo.Items.Add("Read failed!");
                    return;
                }

                for (int j = 0; j < 8; j++)
                {
                    s1 = string.Format("{0:X2}", value[j]);
                    s += s1;
                    if (i + j >= len - 1)
                        break;
                }
                i += 8;
            }
            if (status == 0)
            {
                lInfo.Items.Add("Read success!");
                lInfo.Items.Add(s);
            }
        }

        private void btnISOWriteWithID_Click(object sender, EventArgs e)
        {
            int addr;
            int len;
            int i = 0;
            int status = 0;
           // byte byAntenna = 0;
            byte[] TagID = new byte[16];
            byte[] value = new byte[32];
            //string s = "The data is:";
            //string s1 = "";
            try
            {
                addr = int.Parse(tIsoAddr.Text);
                len = int.Parse(tIsoCnt.Text);
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input ByteAddr and ByteCnt");
                return;
            }
            string hexID = txtTagID.Text;
            string[] hexIDSplit = hexID.Split(' ');
            try
            {
                foreach (String hex in hexIDSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                        int x = Convert.ToInt32(hex, 16);
                        TagID[i++] = (byte)x;
                }
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input Tag ID needed");
                return;

            }
            string hexValues = tIsoData.Text;
            string[] hexValuesSplit = hexValues.Split(' ');
            try
            {
                i = 0;
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    if (hex != "")
                    {
                        int x = Convert.ToInt32(hex, 16);
                        value[i++] = (byte)x;
                    }
                }
            }
            catch (Exception)
            {
                lInfo.Items.Add("Please input data needed");
                return;

            }
            if (i != len)
            {
                lInfo.Items.Add("Please input data needed");
                return;
            }
            for (i = 0; i < len; i++)
            {
                status = Api.IsoWriteWithID(TagID,(byte)(addr + i), value[i]);
                if (status != 0)
                {
                    lInfo.Items.Add("Write failed!");
                    return;
                }
            }
            lInfo.Items.Add("Write success!");
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            Api.ResetReader();
            bRs232Discon_Click(sender, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    }
}