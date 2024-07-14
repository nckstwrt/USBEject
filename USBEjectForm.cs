using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Management;
using USBEject.RemoveDriveByLetter;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using Crmc.Core.BuildTasks;

namespace USBEject
{
    public partial class USBEjectForm : Form
    {
        string GetStringBetweenStrings(string input, string startString, string endString)
        {
            var pattern = $"{Regex.Escape(startString)}(.*?){Regex.Escape(endString)}";
            var match = Regex.Match(input, pattern);
            if (match.Success)
                return match.Groups[1].Value;
            else
                return null;
        }

        public USBEjectForm()
        {
            InitializeComponent();

            RefreshDrives();/*

            var log225 = @"The application \Device\HarddiskVolume3\Windows\System32\Taskmgr.exe with process id 30904 stopped the removal or ejection for the device USB\VID_1058&PID_2620\575853324138325259563743.
                           Process command line: ""C:\windows\System32\Taskmgr.exe"" /2
                           List of affected devices:";

            var AppName = GetStringBetweenStrings(log225, "The application ", " with process id");
            var ProcessID = GetStringBetweenStrings(log225, "process id ", " stopped the removal");

            var process = Process.GetProcessById(int.Parse(ProcessID));
            process.Kill();

            int AppNameStart, AppNameEnd, ProcessIdStart, ProcessIdEnd;
            AppNameStart = log225.IndexOf("The application ");
            if (AppNameStart != -1)
            {
                AppNameStart += "The application ".Length;
                AppNameEnd = log225.IndexOf(" with process id");
                if (AppNameEnd  != -1)
                {
                    var AppName2 = log225.Substring(AppNameStart, AppNameEnd - AppNameStart);
                }
            }

            

            EventLog log = new EventLog("System");
            //var entries = log.Entries.Cast<EventLogEntry>().Where(x => x.TimeGenerated > DateTime.Now.AddMinutes(-25) && x.InstanceId == 8019).OrderByDescending(x => x.TimeGenerated).ToList();
            var logs = new List<EventLogEntry>();
            for (int i = log.Entries.Count-1; i >= Math.Max(log.Entries.Count-100, 0); --i)
            {
                if (log.Entries[i].TimeGenerated > DateTime.Now.AddMinutes(-25) && log.Entries[i].InstanceId == 8019)   // 225
                    logs.Add(log.Entries[i]);
            }



            DateTime latest = logs[0].TimeGenerated;*/
        }

        void RefreshDrives()
        {
            var index = comboBoxDrive.SelectedIndex;

            comboBoxDrive.Items.Clear();

            var drives = DriveInfo.GetDrives().ToList();

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            /*var y = searcher.Get().Cast<ManagementObject>().ToList();
            foreach (var y2 in y)
            {
                foreach (var prop in y2.Properties)
                {
                    Console.WriteLine(prop.Name.ToString() + " " + ((prop.Value != null) ? prop.Value.ToString() : ""));
                }
                string y3 = y2["InterfaceType"].ToString();
                Console.WriteLine(y3);
            }*/
            // MediaType External hard disk media
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if ((queryObj["InterfaceType"] != null && queryObj["InterfaceType"].ToString() == "USB") || (queryObj["MediaType"] != null && queryObj["MediaType"].ToString().Contains("External")))
                {
                    foreach (ManagementObject b in queryObj.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject c in b.GetRelated("Win32_LogicalDisk"))
                        {
                            var driveInfo = drives.FirstOrDefault(x => x.Name == c["Name"].ToString() + "\\");
                            if (driveInfo != null)
                                comboBoxDrive.Items.Add(new DriveItem() { driveInfo = driveInfo });
                        }
                    }
                }
            }

            if (comboBoxDrive.Items.Count > 0)
            {
                if (index == -1 || index >= comboBoxDrive.Items.Count)
                    index = 0;
                comboBoxDrive.SelectedIndex = index;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDrives();
        }

        private void buttonEject_Click(object sender, EventArgs e)
        {
            if (comboBoxDrive.SelectedIndex != -1)
            {
                List<ServiceController> stoppedServices = new List<ServiceController>();
                var driveItem = comboBoxDrive.SelectedItem as DriveItem;
                while (true)
                {
                    if (EjectDriveTool.RemoveDrive(driveItem.driveInfo.Name.Replace("\\", "")))
                        break;

                    // Can't eject - check log
                    EventLog log = new EventLog("System");
                    var logs = new List<EventLogEntry>();
                    for (int i = log.Entries.Count - 1; i >= Math.Max(log.Entries.Count - 100, 0); --i)
                    {
                        if (log.Entries[i].TimeGenerated > DateTime.Now.AddMinutes(-2) && log.Entries[i].InstanceId == 225)   // 225
                            logs.Add(log.Entries[i]);
                    }

                    if (logs.Count > 0)
                    { 
                        var lastLog = logs.OrderByDescending(x => x.TimeGenerated).FirstOrDefault();

                        var AppName = GetStringBetweenStrings(lastLog.Message, "The application ", " with process id");
                        var ProcessID = GetStringBetweenStrings(lastLog.Message, "process id ", " stopped the removal");

                        if (AppName == "System")
                        {
                            /*
                            var result = MessageBox.Show(string.Format("Failed to eject due to Process: {0} (PID: {1})\r\nThis could be the Distributed Link Tracking Client (TrkWks)\r\nStop Service and continue?", AppName, ProcessID), "USB Eject", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                ServiceController[] services = ServiceController.GetServices();
                                var TrkWks = services.FirstOrDefault(x => x.ServiceName == "TrkWks");
                                if (TrkWks != null)
                                {
                                    TrkWks.Stop();
                                    stoppedTrkWks = true;
                                }
                            }
                            else
                                break;
                            */

                            var processes = DetectOpenFiles.GetProcessesUsingFile(driveItem.driveInfo.Name);
                            var serviceProcesses = processes.Where(x => x.ProcessName.ToLower().Contains("svchost")).ToList();
                            var foundServices = ServiceController.GetServices().Where(x => serviceProcesses.Select(y => y.Id).Contains(x.GetServiceProcessId())).ToList();
                            if (foundServices.Count > 0)
                            {
                                AppName = foundServices[0].DisplayName;
                                ProcessID = foundServices[0].GetServiceProcessId().ToString();
                                var result = MessageBox.Show(string.Format("Failed to eject due to Service: {0} (PID: {1})\r\n\r\nStop Service and continue?", AppName, ProcessID), "USB Eject", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    stoppedServices.Add(foundServices[0]);
                                    foundServices[0].Stop();
                                }
                                else
                                    break;
                            }
                            else
                            {
                                MessageBox.Show("Could not eject disk and could not find issue while assessing Services!", "USB Eject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        else
                        if (!string.IsNullOrEmpty(AppName) && !string.IsNullOrEmpty(ProcessID))
                        {
                            var result = MessageBox.Show(string.Format("Failed to eject due to Process: {0} (PID: {1})\r\n\r\nKill Process and continue?", AppName, ProcessID), "USB Eject", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                var process = Process.GetProcessById(int.Parse(ProcessID));
                                process.Kill();
                            }
                            else
                                break;
                        }
                        else
                        {
                            MessageBox.Show("Could not eject disk and could not find issue in Event Log!", "USB Eject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }

                // Restart services we stopped
                foreach (var stoppedService in stoppedServices)
                {
                    stoppedService.Start();
                }

                RefreshDrives();
            }
        }
    }

    public class DriveItem
    {
        public DriveInfo driveInfo;

        public override string ToString()
        {

            string volumeLabel = "";
            try { volumeLabel = "  (" + driveInfo.VolumeLabel + ")"; }
            catch { }
            return driveInfo.Name + volumeLabel;
        }
    }
}

