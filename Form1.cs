using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using PS3Lib;
using System.IO;

namespace MCPS3_NAME_TOOL
{
    public partial class Form1 : MetroForm
    {

        public static Dictionary<int, Packet1> packets = new Dictionary<int, Packet1>();
        public static PS3API PS3 = new PS3API();
        public Form1()
        {
            InitializeComponent();
            PS3.ChangeAPI(SelectAPI.ControlConsole);
            

        }

            string[] names = new string[16];
        List<string> name = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            string currentversion = "1.1";
            System.IO.File.Delete("Update.zip");
            try
            {
                var WebClient = new System.Net.WebClient();
                var client = new System.Net.WebClient();
                string updateversion1 = (WebClient.DownloadString("https://pastebin.com/raw/1ZLTG95u"));
                string updateversion = updateversion1.Split(';')[0];
                string changes = (updateversion1.Split(';')[1]);
                if (currentversion != updateversion)
                {
                    MessageBox.Show("Update Avaliable! \n Downloading now! \n" + changes);
                    client.DownloadFile(updateversion1.Split(';')[2], "Update.zip");
                    System.Diagnostics.Process.Start("Updater.exe");
                }
                else
                {
                    MessageBox.Show("You are on the latest Avaliable Version!");
                }
            }
            catch
            {

            }
            TabPage t = metroTabControl1.TabPages[0];
            metroTabControl1.SelectTab(t);

        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
            try

            {

                PS3.ConnectTarget(0);

                MessageBox.Show("Successfully Connected to Target!", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PS3.CCAPI.Notify(CCAPI.NotifyIcon.WRONGWAY, "Connected to PC");


            }

            catch

            {

                MessageBox.Show("Failed to Connect", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void MetroButton3_Click(object sender, EventArgs e)
        {

            try

            {

                PS3.AttachProcess();

                MessageBox.Show("Successfully Attached to Proccess!", "Attached", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PS3.CCAPI.Notify(CCAPI.NotifyIcon.WRONGWAY, "Attached");
                metroLabel2.Text = PS3.Extension.ReadString(0x3000ABE4);

            }

            catch

            {

                MessageBox.Show("Failed to Attach", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PS3.CCAPI.Notify(CCAPI.NotifyIcon.WRONGWAY, "Failed To Attach");
                PS3.CCAPI.RingBuzzer(CCAPI.BuzzerMode.Double);

            }

        }

        private void MetroButton2_Click(object sender, EventArgs e)
        {
            string newname = metroTextBox1.Text;
            string currentname = PS3.Extension.ReadString(0x3000ABE4);
            PS3.CCAPI.SetMemory(0x3000ABE4, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, });
            PS3.CCAPI.SetMemory(0x3000ABF4, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, });
            PS3.Extension.WriteString(0x3000ABE4, newname);
            PS3.Extension.WriteString(0x3000ABFC, "ps3");
            PS3.Extension.ReadString(0x3000ABE4);
            if (listBox1.Items.Contains(newname))
            {

            }
            else
            {
            name.Add(newname);
            listBox1.Items.Add(newname);
            }
            metroLabel2.Text = PS3.Extension.ReadString(0x3000ABE4);
        }

        private void MetroButton4_Click(object sender, EventArgs e)
        {

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "Minecraft Namelist|*.Namelist";
            OpenFileDialog1.FilterIndex = 1;
            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string line = System.IO.File.ReadAllText(OpenFileDialog1.FileName);
                string[] seperated = line.Split(
    new[] { "\r\n", "\r", "\n" },
    StringSplitOptions.None);
                name.Clear();
                foreach (string item in seperated)
                {
                    name.Add(item);
                    listBox1.Items.Add(item);
                }
            }
        }

        private void MetroButton5_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Filter = "Minecraft Namelist|*.Namelist";
            SaveFileDialog1.FilterIndex = 1;
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dir = SaveFileDialog1.FileName;
                TextWriter tw = new StreamWriter(dir);

                foreach (String s in listBox1.Items)
                    tw.WriteLine(s);

                tw.Close();
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            metroTextBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void MetroButton6_Click(object sender, EventArgs e)
        {

            string currentname = PS3.Extension.ReadString(0x3000ABE4);
            string newname = currentname + " ";
            PS3.CCAPI.SetMemory(0x3000ABE4, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, });
            PS3.CCAPI.SetMemory(0x3000ABF4, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, });
            PS3.Extension.WriteString(0x3000ABE4, newname);
            PS3.Extension.WriteString(0x3000ABFC, "ps3");
            PS3.Extension.ReadString(0x3000ABE4);
            metroLabel2.Text = PS3.Extension.ReadString(0x3000ABE4);
        }






        private void metroButton8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox1.Checked == true)
            {
                this.WriteUInt(10006308U, 945815553U);
            }
            else
            {
                this.WriteUInt(10006308U, 945815552U);
            }
        }

        private void WriteUInt(uint addr, uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            PS3.Extension.WriteBytes(addr, bytes);
        }

        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox2.Checked == true)
            {
                this.WriteUInt(3715900U, 2139095040U);
            }
            else
            {
                this.WriteUInt(3715900U, 1065353216U);
            }
        }

        private void metroCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox3.Checked == true)
            {
                this.WriteUInt(2259216U, 1099038760U);
            }
            else
            {
                this.WriteUInt(2259216U, 1099038744U);
            }
        }

        private void metroCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox4.Checked == true)
            {
                this.WriteFloat(3844512U, 0.51000005f);
            }
            else
            {
                this.WriteFloat(3844512U, 0.91f);
            }
        }

        private void WriteFloat(uint addr, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            PS3.Extension.WriteBytes(addr, bytes);
        }

        private void metroCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox5.Checked == true)
            {
                this.WriteFloat(3843972U, 0.82f);
            }
            else
            {
                this.WriteFloat(3843972U, 0.42f);
            }
        }

        private void metroCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox6.Checked == true)
            {
                this.WriteUInt(2314148U, 947912704U);
            }
            else
            {
                this.WriteUInt(2314148U, 947912705U);
            }
        }

        private void metroCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox7.Checked == true)
            {
                PS3.Extension.WriteBytes(11549744U, new byte[]
                {
                    56,
                    96,
                    0,
                    1,
                    78,
                    128,
                    0,
                    32
                });
            }
            else
            {
                PS3.Extension.WriteBytes(11549744U, new byte[]
            {
                248,
                33,
                byte.MaxValue,
                129,
                124,
                8,
                2,
                166
            });
            }
        }

        private void metroCheckBox8_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox8.Checked == true)
            {
                this.WriteFloat(20059128U, 5.75f);
            }
            else
            {
                this.WriteFloat(20059128U, 1.62f);
            }
        }

        private void metroCheckBox9_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox9.Checked == true)
            {
                this.WriteUInt(11059944U, 1610612736U);
            }
            else
            {
                this.WriteUInt(11059944U, 1099038736U);
            }
        }


        private void metroCheckBox13_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox13.Checked == true)
            {
                this.WriteUInt(3850128U, 1610612736U);
            }
            else
            {
                this.WriteUInt(3850128U, 1099040032U);
            }
        }

        

        private void metroCheckBox15_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox15.Checked == true)
            {
                this.WriteUInt(3831032U, 1002438657U);
            }
            else
            {
                this.WriteUInt(3831032U, 1002438656U);
            }
        }

        private void metroCheckBox17_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox17.Checked == true)
            {
                this.WriteUInt(2309896U, 947912705U);
            }
            else
            {
                this.WriteUInt(2309896U, 947912704U);
            }
        }

        private void metroCheckBox16_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox16.Checked == true)
            {
                this.WriteUInt(2272920U, 1317011488U);
            }
            else
            {
                this.WriteUInt(2272920U, 1317011488U);
            }
        }
    }
    }
