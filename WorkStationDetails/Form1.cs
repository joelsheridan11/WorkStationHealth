using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkStationDetails
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        string line = Environment.NewLine;


        private void wksBtn_Click(object sender, EventArgs e)
        {


            outputTxt.Clear();
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {


                results += isOnline(workstation);
                results += hardwareStats(workstation);
                results += serialNum(workstation);
                results += WorkstationStats(workstation);
                results += CheckDrives(workstation);
                results += line;
                results += CPUStatus(workstation);
                //results += CPUTemp(workstation);
                results += line;
                results += printerInstalled(workstation);
                results += line;
                results += printerSpooler(workstation);
                results += line;



            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }
            outputTxt.Text = results;


        }


        private bool isOnline(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            Ping pinger = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 5;
            try
            {
                PingReply reply = pinger.Send(host, timeout, buffer, options);
                if (reply.Status != IPStatus.Success)
                {
                    //try again
                    PingReply reply2 = pinger.Send(host, timeout, buffer, options);
                    if (reply2.Status != IPStatus.Success)
                    {
                        return false;
                        Output += (String.Format("{0} is offline or does not exist.", host)) + line;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return false;

            }
            return true;
            Output += (String.Format("{0} is online", host)) + line;
        }


        public static string hardwareStats(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_ComputerSystem"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //string hostName = (queryObj["Name"].ToString());
                    //string domain = (queryObj["Domain"].ToString());
                    //string manufac = (queryObj["Manufacturer"].ToString());
                    //string model = (queryObj["Model"].ToString());
                    //string user = (queryObj["UserName"].ToString());
                    Output += (String.Format("Computer Name: {0}", queryObj["Name"])) + line;
                    Output += (String.Format("Domain: {0}", queryObj["Domain"])) + line;
                    Output += (String.Format("Current User: {0}", queryObj["UserName"])) + line;
                    Output += (String.Format("Manufacturer: {0}", queryObj["Manufacturer"])) + line;
                    Output += (String.Format("Model: {0}", queryObj["Model"])) + line;


                }

            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }

        public static string serialNum(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_BIOS"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string serialNum = (queryObj["SerialNumber"].ToString());
                    Output += (String.Format("Serial Number: {0}", queryObj["SerialNumber"])) + line;
                }


            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }



        public static string WorkstationStats(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            string time = string.Empty;
            string InstallDate = string.Empty;
            string LastBoot = string.Empty;
            string boottime = string.Empty;
            double fspc = 0.0;
            double tspc = 0.0;
            double percent = 0.0;
            string serialNum = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_OperatingSystem"));
                var biosQuery = new ObjectQuery(string.Format("SELECT * FROM Win32_BIOS"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    double free = Double.Parse(queryObj["FreePhysicalMemory"].ToString());
                    double total = Double.Parse(queryObj["TotalVisibleMemorySize"].ToString());
                    long TotalFree = Convert.ToInt32(queryObj["FreePhysicalMemory"].ToString());
                    long TF = TotalFree * 1024;
                    long TotalMem = Convert.ToInt32(queryObj["TotalVisibleMemorySize"].ToString());
                    long TM = TotalMem * 1024;
                    fspc = Convert.ToInt32(queryObj["FreePhysicalMemory"]);
                    tspc = Convert.ToInt32(queryObj["TotalVisibleMemorySize"]);
                    percent = (fspc / tspc) * 100;
                    int left = (int)percent;
                    int num = 100 - left;
                    string OS = queryObj["Name"].ToString().Substring(0, queryObj["Name"].ToString().LastIndexOf(" |") + 1);
                    string FS = SpaceConversion(TF);
                    string TS = SpaceConversion(TM);
                    InstallDate = Date(queryObj["InstallDate"].ToString());
                    LastBoot = Date(queryObj["LastBootUpTime"].ToString());
                    DateTime convertBoot = ManagementDateTimeConverter.ToDateTime(queryObj.Properties["LastBootUpTime"].Value.ToString());
                    TimeSpan UpTime = DateTime.Now - convertBoot;



                    Output += (String.Format("Operating System: {0}", queryObj["Caption"])) + line;
                    Output += (String.Format("Install Date: {0}", InstallDate)) + line;
                    Output += (String.Format("Last Boot Up: {0}", LastBoot)) + line;
                    Output += (String.Format("Physical Memory: {0}% in use ({1} in use / {2} total)", num, FS, TS)) + line;
                }

            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }


        public static string CPUStatus(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Processor"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //string clockSpeed = (queryObj["CurrentCLockSpeed"].ToString());
                    //string maxSpeed = (queryObj["MaxClockSpeed"].ToString());
                    //string loadPerc = (queryObj["LoadPercentage"].ToString());
                    //string CPU = (queryObj["Name"].ToString());
                    //string processorNum = (queryObj["NumberofCores"].ToString());
                    //string CPUStatus = (queryObj["Status"].ToString());
                    Output += (String.Format("CPU: {0}", queryObj["Name"])) + line;
                    Output += (String.Format("Number of Cores: {0}", queryObj["NumberofCores"])) + line;
                    Output += (String.Format("CPU Usage: {0}%", queryObj["LoadPercentage"])) + line;
                    Output += (String.Format("CPU Clock Speed: {0}", queryObj["CurrentCLockSpeed"])) + line;
                    Output += (String.Format("CPU Peak Clock Speed: {0}", queryObj["MaxClockSpeed"])) + line;
                    Output += (String.Format("CPU Status: {0}", queryObj["Status"])) + line;
                }


            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }



        public static string CPUTemp(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\WMI", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM MSAcpi_ThermalZoneTemperature"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    double currentTemp = Double.Parse(queryObj["CurrentTemperature"].ToString());
                    double Threshold = Double.Parse(queryObj["CriticalTripPoint"].ToString());
                    double tempCel = (currentTemp / 10 - 273.15);
                    double thresholdCel = (Threshold / 10 - 273.15);
                    Output += (String.Format("CPU Current Temperature: {0}", tempCel + " C")) + line;
                    Output += (String.Format("CPU Max Temperature: {0}", thresholdCel + " C")) + line;

                }


            }
            catch (ManagementException e)
            {
                Output += ("CPU Temp: An error Occurred while querying WMI data") + line;
            }
            return Output;
        }


        public static string CheckDrives(string host)  //version check
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();

                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_LogicalDisk"));
                //var scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", host));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    int targetNumber;
                    string amount = string.Empty;
                    double fspc = 0.0;
                    double tspc = 0.0;
                    double percent = 0.0;
                    fspc = Convert.ToInt64(queryObj["FreeSpace"]);
                    tspc = Convert.ToInt64(queryObj["Size"]);
                    if (tspc != 0)
                    {
                        percent = (fspc / tspc) * 100;
                        double Rounding = Math.Round(percent);
                        int num = (int)Rounding;
                        string FS = SpaceConversion(Convert.ToInt64(queryObj["FreeSpace"]));
                        string TS = SpaceConversion(Convert.ToInt64(queryObj["Size"]));
                        Match Name = Regex.Match(queryObj["Name"].ToString(), @"[a-z,A-Z]");
                        if (Regex.IsMatch(FS, @"MB"))
                        {
                            targetNumber = 0;
                        }
                        else
                        {
                            Match test = Regex.Match(FS, @"\d*");
                            targetNumber = Convert.ToInt32(test.ToString());
                        }
                        if (targetNumber <= 2)
                        {
                            Output += (String.Format(@"{0} Drive : {1}% in use ({2} free / {3} total).", Name, num, FS, TS)) + line;
                            //Output += (String.Format(@"<span class = ""error"">Drive {0} has {1} ({2}%) free of {3}{4}.</span>", d.Name, FS, num, TS, amount)) + line;
                        }
                        else
                        {
                            Output += (String.Format("{0} Drive: {1}% used ({2} used/ {3} total).", Name, num, FS, TS)) + line;
                            //Output += (String.Format("Drive {0} has {1} ({2}%) free of {3}{4}.", d.Name, FS, num, TS, amount)) + line;
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                Output = ("An error occurred while querying for the drive information: " + e.Message) + line;
            }
            return Output;
        }


        public static string printerInstalled(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Printer"));
                var searcher = new ManagementObjectSearcher(scope, query);
                if (searcher.Get().Count == 0)
                {
                    Output += (String.Format("No Printer Installed")) + line;
                }
                Output += ("Installed Printers") + line;
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string deviceID = (queryObj["Name"].ToString());
                    string driverName = (queryObj["DriverName"].ToString());
                    Output += (String.Format(deviceID)) + line;
                    Output += (String.Format(driverName)) + line;

                }

            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }


        public static string printerSpooler(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Service WHERE Name = 'Spooler'"));
                var searcher = new ManagementObjectSearcher(scope, query);
                Output += ("Printer SpoolerStatus") + line;
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string spooler = (queryObj["Name"].ToString());
                    string spoolerStatus = (queryObj["State"].ToString());

                    Output += (String.Format("Spooler State: {0}", queryObj["State"])) + line;
                }

            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }


        public static string processesRunning(string host)
        {

            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {

                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Process WHERE Name <> 'svchost.exe' "));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    UInt64 memory = (UInt64)queryObj["WorkingSetSize"];
                    string mm = SpaceConversion(Convert.ToInt64(queryObj["WorkingSetSize"]));
                    int mem = Convert.ToInt32(memory / 1024);
                    string processName = (queryObj["Name"].ToString());
                    Output += (String.Format("{0}: {1}", processName, mm)) + line;
                }


                //Process[] processes = Process.GetProcessesByName(host);
                //foreach (Process p in processes)
                //{
                //    string processName = p.ProcessName;
                //    double processBytes = p.WorkingSet64 / 1024;
                //    Output += String.Format("{0}:", processName) + line;
                //    //PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set",p.ProcessName);
                //    //PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Process Time",p.ProcessName);
                //    //while(true)
                //}
            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }


        public static string services(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {

                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Service"));
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string serviceName = (queryObj["Name"].ToString());
                    string serviceState = (queryObj["State"].ToString());

                    Output += (String.Format("{0}: {1}", serviceName, serviceState)) + line;

                }
            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;

        }



        public static string eventLog(string host)
        {
            string line = Environment.NewLine;
            string Search1 = "disk";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string Output = string.Empty;
            string Output1 = string.Empty;
            string Output2 = string.Empty;
            string Output3 = string.Empty;
            string output;
            string amount1 = string.Empty;
            string amount2 = string.Empty;
            string amount3 = string.Empty;
            int count = 0;
            int count1 = 0;
            int total1 = 0;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                //ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE LogFile = 'System' AND (SourceName = 'TermDD' OR SourceName = 'EventLog') AND TimeGenerated > '{0}'", date));
                ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE LogFile = 'System' AND (SourceName = 'Disk') AND TimeGenerated > '{0}'", DateTime.Now.AddDays(-14)));
                ManagementObjectSearcher mos = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in mos.Get())
                {
                    output = queryObj["SourceName"].ToString();
                    if (output == Search1)
                    {
                        if (count1 == 0)
                        {
                            Output1 += (String.Format("Type: {0}", queryObj["Type"])) + line;
                            Output1 += (String.Format("Source Name: {0}", queryObj["SourceName"])) + line;
                            Output1 += (String.Format("Time Generated: {0}", ManagementDateTimeConverter.ToDateTime(queryObj["TimeGenerated"].ToString()))) + line;
                            Output1 += (String.Format("Message: {0}", queryObj["Message"])) + line;
                            Output1 += line;
                            count1++;
                            total1++;
                        }
                        else
                        {
                            count1++;
                            total1++;
                        }
                    }
                    count++;
                    if (count > 31)
                    {
                        break;
                    }
                }
                if (total1 > 0)
                {
                    amount1 += "There are a total of " + total1 + " errors like the following: " + line;
                    amount1 += Output1;
                    Output += amount1;
                }
                else if (total1 == 0)
                {
                    Output = "No 'disk' errors were found.";
                }
            }
            catch (ManagementException e)
            {
                Output = ("An error occurred while querying the Event Viewer: " + e.Message) + line;
            }
            return Output;
        }

        public static string logInEvents(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;


            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                //ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE LogFile = 'System' AND (SourceName = 'TermDD' OR SourceName = 'EventLog') AND TimeGenerated > '{0}'", date));
                ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE EventCode = '4624' or EventCode = '4634' or EventCode = '4800' or EVentCode = '4801' AND TimeGenerated > '{0}'", DateTime.Now.AddDays(-14)));
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                if (searcher.Get().Count == 0)
                {
                    Output += (String.Format("No Events Found")) + line;
                }
                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Output += (String.Format("Type: {0}", queryObj["Type"])) + line;
                    Output += (String.Format("Source Name: {0}", queryObj["SourceName"])) + line;
                    Output += (String.Format("Time Generated: {0}", ManagementDateTimeConverter.ToDateTime(queryObj["TimeGenerated"].ToString()))) + line;
                    Output += (String.Format("Message: {0}", queryObj["Message"])) + line;
                    Output += line;

                }
            }

            catch (ManagementException e)
            {
                Output = ("An error occurred while querying the Event Viewer: " + e.Message) + line;
            }
            return Output;
        }

        public static string eventLogErrors(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;

            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                //ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE LogFile = 'System' AND (SourceName = 'TermDD' OR SourceName = 'EventLog') AND TimeGenerated > '{0}'", date));
                ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_NTLogEvent WHERE LogFile = 'System' AND (EventCode > '3001' and EventCode < '5000') AND TimeGenerated > '{0}'", DateTime.Now.AddDays(-14)));
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                if (searcher.Get().Count == 0)
                {
                    Output += (String.Format("No Critical Errors Found")) + line;
                }
                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Output += (String.Format("Type: {0}", queryObj["Type"])) + line;
                    Output += (String.Format("Source Name: {0}", queryObj["SourceName"])) + line;
                    Output += (String.Format("Time Generated: {0}", ManagementDateTimeConverter.ToDateTime(queryObj["TimeGenerated"].ToString()))) + line;
                    Output += (String.Format("Message: {0}", queryObj["Message"])) + line;
                    Output += line;

                }
            }

            catch (ManagementException e)
            {
                Output = ("An error occurred while querying the Event Viewer: " + e.Message) + line;
            }
            return Output;
        }


        public static string patchesInstalled(string host)
        {
            string line = Environment.NewLine;
            string Output = string.Empty;

            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();
                ObjectQuery query = new ObjectQuery(String.Format("SELECT * FROM Win32_QuickFixEngineering WHERE InstalledOn > '{0}'", DateTime.Now.AddDays(-30)));
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                Output += (String.Format("Patches installed in last 30 days:")) + line;
                if (searcher.Get().Count == 0)
                {
                    Output += (String.Format("No patches installed in last 30 days")) + line;
                }
                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Output += (String.Format("Description: {0}", queryObj["Description"])) + line;
                    Output += (String.Format("HotFix ID {0}", queryObj["HotFixID"])) + line;
                    Output += (String.Format("InstalledOn: {0}", queryObj["InstalledOn"].ToString())) + line;
                    Output += line;

                }
            }

            catch (ManagementException e)
            {
                Output = ("An error occurred while querying the Event Viewer: " + e.Message) + line;
            }
            return Output;
        }

        public static string softwareInstalled(string host)
        {

            string line = Environment.NewLine;
            string Output = string.Empty;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", host));
                scope.Connect();

                var query = new ObjectQuery(string.Format("SELECT * FROM Win32_Product"));
                var searcher = new ManagementObjectSearcher(scope, query);
                if (searcher.Get().Count == 0)
                {
                    Output += (String.Format("No Software Installed")) + line;
                }
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //string desc = (queryObj["Description"].ToString());
                    //string version = (queryObj["Version"].ToString());
                    //string installDate = (queryObj["InstallDate"].ToString());
                    //string vendor = (queryObj["Vendor"].ToString());
                    Output += (String.Format("Name: {0}", queryObj["Name"])) + line;
                    Output += (String.Format("Vendor: {0}", queryObj["Vendor"])) + line;
                    Output += (String.Format("Version: {0}", queryObj["Version"])) + line;
                    Output += (String.Format("InstallDate: {0}", queryObj["InstallDate"])) + line;
                    Output += line;


                }

            }
            catch (ManagementException e)
            {
                Output += ("An error Occurred while querying WMI data") + line;
            }
            return Output;
        }

        void AppendText(RichTextBox outputTxt, Color color, string text)
        {
            int start = outputTxt.TextLength;
            outputTxt.AppendText(text);
            int end = outputTxt.TextLength;
            // Textbox may transform chars, so (end-start) != text.Length
            outputTxt.Select(start, end - start);
            {
                outputTxt.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            outputTxt.SelectionLength = 0; // clear
            outputTxt.SelectionColor = Color.Black;
        }

        public static string SpaceConversion(long byteCount)
        {
            string Output;
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
            {
                int place = (int)byteCount;
                Output = "0" + suf[0];
            }
            else
            {
                long bytes = Math.Abs(byteCount);
                int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                double num = Math.Round(bytes / Math.Pow(1024, place), 1);
                Output = (Math.Sign(byteCount) * num).ToString() + suf[place];
            }
            return Output;
        }

        public static string Date(string date)  //version check
        {
            string Output = string.Empty;
            string time = string.Empty;
            string InstallDate = string.Empty;
            string LastBoot = string.Empty;
            string boottime = string.Empty;
            Match install = Regex.Match(date, @"\d{8}");
            time = Regex.Replace(date, @"\d{8}", "");
            Match HMS = Regex.Match(time.ToString(), @"\d{6}");
            string InstallTime = HMS.ToString().Insert(4, ":");
            InstallTime = InstallTime.Insert(2, ":");
            DateTime install2 = DateTime.ParseExact(install.ToString(), "yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            Match install3 = Regex.Match(install2.ToString(), @"\d*/\d*/\d*");
            InstallDate = install3.ToString();
            Output = InstallTime + " on " + InstallDate;
            return Output;
        }


        private void copyBtn_Click(object sender, EventArgs e)
        {
            string Output = "";
            string[] outputArray = Regex.Split(outputTxt.Text, @"\n");
            foreach (string data in outputArray)
            {
                Output += data + line;
            }
            Clipboard.SetText(Output);
            //Clipboard.SetText(outputTxt.Text);
        }


        private void EventBtn_Click(object sender, EventArgs e)
        {
            outputTxt.Clear();
            outputTxt.Text = "Checking Eventlog for Errors . . . ." + line;
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {

                results += eventLog(workstation);
                results += line;
                results += eventLogErrors(workstation);

            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }

            outputTxt.Text = results;

        }


        private void svcButton_Click(object sender, EventArgs e)
        {
            outputTxt.Clear();
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {
                results += services(workstation);

            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }
            outputTxt.Text = results;

        }


        private void patchBtn_Click(object sender, EventArgs e)
        {
            outputTxt.Clear();
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {
                results += patchesInstalled(workstation);

            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }
            outputTxt.Text = results;


        }

        private void softwareBtn_Click(object sender, EventArgs e)
        {

            outputTxt.Clear();
            outputTxt.Text = "Checking Software installed . . . ." + line;
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {
                results += softwareInstalled(workstation);

            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }
            outputTxt.Text = results;

        }


        private void sccmBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\Program Files (x86)\ConfigMgrConsole\bin\i386\CmRcViewer.exe"))
            {
                string workstation = wksTxt.Text.ToUpper().Trim();

                if (isOnline(workstation))
                {
                    ProcessStartInfo pi = new ProcessStartInfo();
                    pi.FileName = @"C:\Program Files (x86)\ConfigMgrConsole\bin\i386\CmRcViewer.exe";
                    pi.Arguments = workstation;
                    Process.Start(pi);
                }
                else MessageBox.Show("Workstation offline");

            }
            else MessageBox.Show("You do not have SCCM installed. Please download and install from Software Center.");
        }

        private void wksTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void logonBtn_Click(object sender, EventArgs e)
        {
            outputTxt.Clear();
            outputTxt.Text = "Checking Eventlog for Errors . . . ." + line;
            string workstation = wksTxt.Text.ToUpper().Trim();
            string results = "";

            if (isOnline(workstation))
            {

                results += logInEvents(workstation);
                
            }
            else
            {
                AppendText(this.outputTxt, Color.Red, "Status: Offline!");
            }

            outputTxt.Text = results;

        }
    }
}
