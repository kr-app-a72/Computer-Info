using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;




namespace Get_Computer_state
{
    public partial class Main : Form
    {
        public static bool Disconnect = false;
        public static string OperatingSystem = string.Empty;
        public static string MyPath = string.Empty;
        public static string InstallPath = string.Empty;
        public static string AccountType = string.Empty;
        public static string WANIP = string.Empty;
        public static string Country = string.Empty;
        public static string CountryCode = string.Empty;
        public static string City = string.Empty;
        public static string lastStatus = "Active";
        public static int ImageIndex = 0;
        public static String owner;
        public Main(String s)
        {
            owner = s;
            InitializeComponent();
        }
        public static string GetCpu() //CPU
        {
            try
            {
                string CPUName = string.Empty;
                string Query = "SELECT * FROM Win32_Processor";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", Query);
                foreach (ManagementObject Mobject in searcher.Get())
                    CPUName = Mobject["Name"].ToString();

                return CPUName;
            }
            catch
            {
                return "Unknown";
            }
        }

        public static int GetRam() //RAM
        {
            try
            {
                int installedRAM = 0;
                string Query = "Select * From Win32_ComputerSystem";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
                foreach (ManagementObject Mobject in searcher.Get())
                {
                    double bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                    installedRAM = (int)(bytes / 1048576);
                }

                return installedRAM;
            }
            catch
            {
                return -1;
            }
        }

        public static string GetUsername() //유저이름
        {
            return Environment.UserName;
        }

        public static string GetPcName() //컴퓨터이름
        {
            return Environment.MachineName;
        }

        public static string GetGpu() //그래픽카드
        {
            try
            {
                string GPUName = string.Empty;
                string Query = "SELECT * FROM Win32_DisplayConfiguration";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
                foreach (ManagementObject Mobject in searcher.Get())
                    GPUName = Mobject["Description"].ToString();

                return (!string.IsNullOrEmpty(GPUName)) ? GPUName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetLanIp() //내부 아이피
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] hosts = host.AddressList;
            string localIP = (host.AddressList.Length > 0) ? "" : "-";

            for (int i = host.AddressList.Length - 1; i >= 0; i--)
            {
                if (hosts[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP += hosts[i].ToString() + ", ";
                }
            }

            return (localIP == "-") ? localIP : localIP.Remove(localIP.Length - 2); ;
        }

        public static string GetIPAddress() //외부아이피
        {
            string ip = new System.Net.WebClient().DownloadString(("http://bot.whatismyipaddress.com"));
            return ip;
        }

        public static string GetAccountType() //권한
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                return "Admin";
            else if (principal.IsInRole(WindowsBuiltInRole.User))
                return "User";
            else if (principal.IsInRole(WindowsBuiltInRole.Guest))
                return "Guest";
            else
                return "Unknown";
        }

        public string GetMACAddress()
        {
            /*ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)  // only return MAC Address from first card
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }
            MACAddress = MACAddress.Replace(":", "");*/
            String MACAddress = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
            int temp = 0;
            for (int i = 1; i <= 5; i++) {
                temp = i-1;
                MACAddress = MACAddress.Insert(i * 2 + temp, "-");
            }
            return MACAddress;
        }

        public string GETProcessorCount()
        {
            return Environment.ProcessorCount.ToString();
        }

        public Boolean is64()
        {
            Boolean is64 = Environment.Is64BitOperatingSystem;
            return is64;
        }

        public static void save() {
            int i = GetRam();
            string s = i.ToString();
            if (File.Exists(owner + ".ci")) {
                File.Delete(owner+".ci");
            }
            StreamWriter sw = new StreamWriter(owner+".ci");
            sw.Write("c.-"+GetCpu()+"\n");
            sw.Write("r.-"+s + "\n");
            sw.Write("cn.-" + GetPcName() + "\n");
            sw.Write("un.-" + GetUsername() + "\n");
            sw.Write("g.-" + GetGpu() + "\n");
            sw.Write("p.-" + Environment.ProcessorCount.ToString() + "\n");
            sw.Write("m.-" + GetIPAddress() + "\n");
            sw.Write("lp.-" + GetLanIp() + "\n");
            sw.Write("op.-" + GetIPAddress() + "\n");
            sw.Close();

            MessageBox.Show("저장 완료");

        }
      

        private void button1_Click(object sender, EventArgs e)
        {

            
            int i = GetRam();
            string s = i.ToString();
            label1.Text = " CPU :  "+GetCpu(); //CPU
            label2.Text = " RAM :  " + s + "MB"; //램
            label3.Text = " 컴퓨터이름 :  " + GetPcName(); //컴퓨터이름
            label4.Text = " 유저이름 :  " + GetUsername(); //유저이름
            label5.Text = " GPU :  " + GetGpu(); //그래픽카드
            label7.Text = " MAC Address:  " + GetMACAddress(); //MAC
            label6.Text = " Processor :  " + GETProcessorCount();
            if (is64())
            {
                label9.Text = " Bit :   64BIT";
            }
            else {
                label9.Text = " Bit :   32BIT";
            }
            label10.Text = " 내부IP :  " + GetLanIp(); //내부 IP
            label11.Text = " 외부IP :  " + GetIPAddress(); //외부 IP
            save();
            
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }


        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
