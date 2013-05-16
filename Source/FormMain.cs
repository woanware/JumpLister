using System;
using System.IO;
using System.Windows.Forms;
using OpenMcdf;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using CsvHelper;
using Shellify;
using Shellify.ExtraData;
using CsvHelper.Configuration;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormMain : Form
    {
        #region Member Variables
        private Settings _settings = null;
        private Dictionary<string, string> _appIds = null;
        private List<JumpListFile> _jumpListFiles = null;
        private Errors _errors = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        private void LoadFiles()
        {
            try
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }

                _errors = new Errors();

                listFiles.Items.Clear();

                _jumpListFiles = new List<JumpListFile>();

                foreach (string file in openFileDialog.FileNames)
                {
                    if (File.Exists(file) == false)
                    {
                        continue;
                    }

                    string extension = System.IO.Path.GetExtension(file);
                    if (extension == ".customDestinations-ms")
                    {
                        List<ShellLinkFile> shellLinkFiles = ShellLinkFile.LoadJumpList(file);
                        if (shellLinkFiles.Count == 0)
                        {
                            _errors.AddError(file, "The custom jump list contained no valid LNK structures");
                            continue;
                        }

                        // Perform a lookup on the AppId
                        string appId = System.IO.Path.GetFileNameWithoutExtension(file);
                        string appName = string.Empty;
                        if (_appIds.ContainsKey(appId) == true)
                        {
                            appName = _appIds[appId];
                        }

                        ListViewItem listItem = new ListViewItem();
                        listItem.Text = System.IO.Path.GetFileName(file);
                        listItem.SubItems.Add(appName);
                        listItem.Tag = file;

                        listFiles.Items.Add(listItem);

                        ParseJumpList(file, appName, shellLinkFiles);
                    }
                    else if (extension == ".automaticDestinations-ms")
                    {
                        // Perform a lookup on the AppId
                        string appId = System.IO.Path.GetFileNameWithoutExtension(file);
                        string appName = string.Empty;
                        if (_appIds.ContainsKey(appId) == true)
                        {
                            appName = _appIds[appId];
                        }

                        ListViewItem listItem = new ListViewItem();
                        listItem.Text = System.IO.Path.GetFileName(file);
                        listItem.SubItems.Add(appName);
                        listItem.Tag = file;

                        listFiles.Items.Add(listItem);

                        ParseJumpList(file, appName);
                    }
                }

                if (listFiles.Items.Count > 0)
                {
                    listFiles.Items[0].Selected = true;
                }

                UserInterface.AutoSizeListViewColumns(listFiles);

                if (_errors.HasErrors == true)
                {
                    DialogResult dialogResult = MessageBox.Show(this, 
                                                                "Errors occurred whilst parsing. Do you want to save the error log?", 
                                                                Application.ProductName, 
                                                                MessageBoxButtons.YesNo, 
                                                                MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "CSV Files|*.csv";
                        saveFileDialog.Title = "Save Errors CSV";
                        saveFileDialog.FileName = "JumpLister.Errors.csv";

                        if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                        {
                            return;
                        }

                        string ret = IO.WriteUnicodeTextToFile(_errors.Csv, saveFileDialog.FileName, false);
                        if (ret.Length > 0)
                        {
                            UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst saving the file: " + ret);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst loading the jump list file(s): " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="appName"></param>
        private void ParseJumpList(string file, string appName)
        {
            JumpListFile jumpListFile = new JumpListFile(false);
            jumpListFile.FilePath = file;
            jumpListFile.FileName = System.IO.Path.GetFileName(file);
            jumpListFile.AppName = appName;

            CompoundFile compoundFile = new CompoundFile(file);
            CFStream cfStream = compoundFile.RootStorage.GetStream("DestList");
            jumpListFile.DestListSize = cfStream.Size;
            List<DestListEntry> destListEntries = ParseDestList(cfStream.GetData());

            jumpListFile.DestListEntries = destListEntries;

            foreach (DestListEntry destListEntry in destListEntries)
            {
                CFStream cfStreamJf = null;
                try
                {
                    cfStreamJf = compoundFile.RootStorage.GetStream(destListEntry.StreamNo);

                    ShellLinkFile linkFile = ShellLinkFile.Load(cfStreamJf.GetData());
                    if (linkFile.Header.CreationTime == DateTime.MinValue |
                        linkFile.Header.AccessTime == DateTime.MinValue |
                        linkFile.Header.WriteTime == DateTime.MinValue |
                        linkFile.Header.CreationTime == ShellLinkFile.WindowsEpoch |
                        linkFile.Header.AccessTime == ShellLinkFile.WindowsEpoch |
                        linkFile.Header.WriteTime == ShellLinkFile.WindowsEpoch)
                    {
                        continue;
                    }
                    
                    if (linkFile != null)
                    {
                        JumpList jumpList = new JumpList();
                        jumpList.Name = destListEntry.StreamNo;
                        jumpList.Size = cfStreamJf.GetData().Length;
                        jumpList.DestListEntry = destListEntry;

                        AddJumpListData(jumpList, linkFile);

                        jumpListFile.JumpLists.Add(jumpList);
                    }
                }
                catch (Exception ex)
                {
                    _errors.AddError(jumpListFile.FileName, destListEntry);
                }                
            }

            _jumpListFiles.Add(jumpListFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jumpList"></param>
        /// <param name="linkFile"></param>
        private void AddJumpListData(JumpList jumpList, ShellLinkFile linkFile)
        {
            if (linkFile.LinkInfo.CommonNetworkRelativeLink == null)
            {
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Flags)", string.Empty));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Device Name)", string.Empty));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Net Name)", string.Empty));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Network Provider Type)", string.Empty));
            }
            else
            {
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Flags)", linkFile.LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkFlags.ToString()));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Device Name)", linkFile.LinkInfo.CommonNetworkRelativeLink.DeviceName));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Net Name)", linkFile.LinkInfo.CommonNetworkRelativeLink.NetName));
                jumpList.Data.Add(new NameValue("Common Network Relative Link (Network Provider Type)", linkFile.LinkInfo.CommonNetworkRelativeLink.NetworkProviderType.ToString()));
            }

            jumpList.Data.Add(new NameValue("Common Path Suffix", linkFile.LinkInfo.CommonPathSuffix));
            jumpList.Data.Add(new NameValue("Local Base Path", linkFile.LinkInfo.LocalBasePath));
            jumpList.Data.Add(new NameValue("Working Path", linkFile.WorkingDirectory));
            jumpList.Data.Add(new NameValue("Arguments", linkFile.Arguments));
            jumpList.Data.Add(new NameValue("Flags", linkFile.LinkInfo.LinkInfoFlags.ToString()));
            jumpList.Data.Add(new NameValue("Attributes", linkFile.Header.FileAttributes.ToString()));
            jumpList.Data.Add(new NameValue("Show Command", linkFile.Header.ShowCommand.ToString()));
            jumpList.Data.Add(new NameValue("Created Timestamp", linkFile.Header.CreationTime.ToLongDateString() + " " + linkFile.Header.CreationTime.ToLongTimeString()));
            jumpList.Data.Add(new NameValue("Accessed Timestamp", linkFile.Header.AccessTime.ToLongDateString() + " " + linkFile.Header.AccessTime.ToLongTimeString()));
            jumpList.Data.Add(new NameValue("Modified Timestamp", linkFile.Header.WriteTime.ToLongDateString() + " " + linkFile.Header.WriteTime.ToLongTimeString()));
            jumpList.Data.Add(new NameValue("Drive Type", linkFile.LinkInfo.VolumeID.DriveType.ToString()));
            jumpList.Data.Add(new NameValue("Serial No.", linkFile.LinkInfo.VolumeID.DriveSerialNumber.ToString()));
            jumpList.Data.Add(new NameValue("Volume Name", linkFile.LinkInfo.VolumeID.VolumeLabel));

            var temp = linkFile.ExtraDataBlocks.OfType<TrackerDataBlock>();
            foreach (TrackerDataBlock edb in temp)
            {
                jumpList.Data.Add(new NameValue("Machine ID", edb.MachineID));
                jumpList.Data.Add(new NameValue("New Volume ID", edb.Droid[0].ToString()));
                jumpList.Data.Add(new NameValue("New Object ID", edb.Droid[1].ToString()));
                jumpList.Data.Add(new NameValue("New Object ID (Timestamp)", edb.Uuid.Timestamp.ToLongDateString() + " " + edb.Uuid.Timestamp.ToLongTimeString()));
                jumpList.Data.Add(new NameValue("New Object ID (MAC)", edb.Uuid.MacAddress));
                jumpList.Data.Add(new NameValue("New Object ID (Seq No.)", edb.Uuid.ClockId.ToString()));
                jumpList.Data.Add(new NameValue("Birth Volume ID", edb.DroidBirth[0].ToString()));
                jumpList.Data.Add(new NameValue("Birth Object ID", edb.DroidBirth[1].ToString()));
                jumpList.Data.Add(new NameValue("Birth Object ID (Timestamp)", edb.UuidBirth.Timestamp.ToLongDateString() + " " + edb.UuidBirth.Timestamp.ToLongTimeString()));
                jumpList.Data.Add(new NameValue("Birth Object ID (MAC)", edb.UuidBirth.MacAddress));
                jumpList.Data.Add(new NameValue("Birth Object ID (Seq No.)", edb.UuidBirth.ClockId.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="appName"></param>
        /// <param name="lnkFiles"></param>
        private void ParseJumpList(string file, string appName, List<ShellLinkFile> lnkFiles)
        {
            JumpListFile jumpListFile = new JumpListFile(true);
            jumpListFile.FilePath = file;
            jumpListFile.FileName = System.IO.Path.GetFileName(file);
            jumpListFile.AppName = appName;

            for (int index = 0; index < lnkFiles.Count; index++)
            {
                ShellLinkFile linkFile = lnkFiles[index];

                JumpList jumpList = new JumpList();
                jumpList.Name = (index + 1).ToString();
                jumpList.Size = 0;

                AddJumpListData(jumpList, linkFile);

                jumpListFile.JumpLists.Add(jumpList);
            }

            _jumpListFiles.Add(jumpListFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destListEntries"></param>
        private void DisplayDestListInfo(List<DestListEntry> destListEntries)
        {
            try
            {
                listLinkInfo.BeginUpdate();
                listLinkInfo.Items.Clear();

                listLinkInfo.Columns.Clear();

                ColumnHeader col = new ColumnHeader();
                col.Text = "No.";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "NetBIOS Name";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "Date/Time";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "MAC (New)";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "Timestamp (New)";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "MAC (Birth)";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "Timestamp (Birth)";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "Data";
                listLinkInfo.Columns.Add(col);

                var sorted = from d in destListEntries orderby d.StreamNo select d;

                foreach (DestListEntry destListEntry in sorted)
                {
                    ListViewItem listItem = new ListViewItem();

                    if (_settings.UseDecimal == true)
                    {
                        listItem.Text = destListEntry.StreamNo + " (" + Int32.Parse(destListEntry.StreamNo, System.Globalization.NumberStyles.HexNumber).ToString() + ")";
                    }
                    else
                    {
                        listItem.Text = destListEntry.StreamNo;
                    }
                    
                    listItem.SubItems.Add(destListEntry.NetBiosName);
                    listItem.SubItems.Add(destListEntry.FileTime.ToLongDateString() + " " + destListEntry.FileTime.ToLongTimeString());
                    listItem.SubItems.Add(destListEntry.Uuid.MacAddress);
                    listItem.SubItems.Add(destListEntry.Uuid.Timestamp.ToLongDateString() + " " + destListEntry.Uuid.Timestamp.ToLongTimeString());
                    listItem.SubItems.Add(destListEntry.UuidBirth.MacAddress);
                    listItem.SubItems.Add(destListEntry.UuidBirth.Timestamp.ToLongDateString() + " " + destListEntry.UuidBirth.Timestamp.ToLongTimeString());
                    listItem.SubItems.Add(destListEntry.Data);
                    listLinkInfo.Items.Add(listItem);
                }
            }
            finally
            {
                listLinkInfo.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                listLinkInfo.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[3].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[5].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[6].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[7].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

                listLinkInfo.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkFile"></param>
        private void DisplayLinkInfo(JumpList jumpList)
        {
            try
            {
                listLinkInfo.BeginUpdate();
                listLinkInfo.Items.Clear();

                listLinkInfo.Columns.Clear();

                ColumnHeader col = new ColumnHeader();
                col.Text = "Name";
                listLinkInfo.Columns.Add(col);
                col = new ColumnHeader();
                col.Text = "Value";
                listLinkInfo.Columns.Add(col);

                foreach (NameValue nameValue in jumpList.Data)
                {
                    AddSimpleListViewItem(listLinkInfo, nameValue.Name, nameValue.Value);
                }
            }
            finally
            {
                listLinkInfo.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listLinkInfo.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

                listLinkInfo.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void AddSimpleListViewItem(ListView listView, 
                                           string name, 
                                           string value)
        {
            ListViewItem listItem = new ListViewItem();
            listItem.Text = name;
            listItem.SubItems.Add(value);
            listView.Items.Add(listItem);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExportFiles()
        {
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            ExportFiles(folderBrowserDialog.SelectedPath);
            ExportLinkInfo(folderBrowserDialog.SelectedPath);
            ExportDestList(folderBrowserDialog.SelectedPath);

            UserInterface.DisplayMessageBox(this, "Export complete", MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        private void ExportLinkInfo(string folder)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.Delimiter = "\t";
            csvConfiguration.HasHeaderRecord = true;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration))
            {
                csvWriter.WriteField("File Name");
                csvWriter.WriteField("File Path");
                //csvWriter.WriteField("File Size");
                csvWriter.WriteField("DestList Stream No.");
                csvWriter.WriteField("DestList NetBIOS Name");
                csvWriter.WriteField("DestList Date/Time");
                csvWriter.WriteField("DestList Data");
                csvWriter.WriteField("Common Network Relative Link (Flags)");
                csvWriter.WriteField("Common Network Relative Link (Device Name)");
                csvWriter.WriteField("Common Network Relative Link (Net Name)");
                csvWriter.WriteField("Common Network Relative Link (Network Provider Type)");
                csvWriter.WriteField("Common Path Suffix");
                csvWriter.WriteField("Local Base Path");
                csvWriter.WriteField("Working Path");
                csvWriter.WriteField("Arguments");
                csvWriter.WriteField("Flags");
                csvWriter.WriteField("Attributes");
                csvWriter.WriteField("Show Command");
                csvWriter.WriteField("Created Timestamp");
                csvWriter.WriteField("Accessed Timestamp");
                csvWriter.WriteField("Modified Timestamp");
                csvWriter.WriteField("Drive Type");
                csvWriter.WriteField("Serial No.");
                csvWriter.WriteField("Volume Name");
                csvWriter.WriteField("Machine ID");
                csvWriter.WriteField("New Volume ID");
                csvWriter.WriteField("New Object ID");
                csvWriter.WriteField("New Object ID (Timestamp)");
                csvWriter.WriteField("New Object ID (MAC)");
                csvWriter.WriteField("New Object ID (Seq No.)");
                csvWriter.WriteField("Birth Volume ID");
                csvWriter.WriteField("Birth Object ID");
                csvWriter.WriteField("Birth Object ID (Timestamp)");
                csvWriter.WriteField("Birth Object ID (MAC)");
                csvWriter.WriteField("Birth Object ID (Seq No.)");
                csvWriter.NextRecord();

                foreach (ListViewItem listItem in listFiles.Items)
                {
                    if (listItem.Selected == false)
                    {
                        continue;
                    }

                    var jlf = (from j in _jumpListFiles where j.FileName == listItem.Text.ToString() select j).SingleOrDefault();
                    if (jlf == null)
                    {
                        return;
                    }

                    var sorted = from j in jlf.JumpLists orderby j.Name select j;
                    
                    StringBuilder output = new StringBuilder();
                    foreach (JumpList jl in sorted)
                    {
                        
                        csvWriter.WriteField(jlf.FileName);
                        csvWriter.WriteField(jlf.FilePath);

                        if (_settings.UseDecimal == true)
                        {
                            csvWriter.WriteField(jl.DestListEntry.StreamNo + " (" + Int32.Parse(jl.DestListEntry.StreamNo, System.Globalization.NumberStyles.HexNumber).ToString() + ")");
                        }
                        else
                        {
                            csvWriter.WriteField(jl.DestListEntry.StreamNo);
                        }

                        csvWriter.WriteField(jl.DestListEntry.NetBiosName);
                        csvWriter.WriteField(jl.DestListEntry.FileTime);
                        csvWriter.WriteField(jl.DestListEntry.Data);

                        AppendSimpleExport(output, "File Name", jlf.FileName);
                        AppendSimpleExport(output, "File Path", jlf.FilePath); 

                        if (_settings.UseDecimal == true)
                        {
                            AppendSimpleExport(output, "DestList Stream No.", jl.DestListEntry.StreamNo + " (" + Int32.Parse(jl.DestListEntry.StreamNo, System.Globalization.NumberStyles.HexNumber).ToString() + ")");
                        }
                        else
                        {
                            AppendSimpleExport(output, "DestList Stream No.", jl.DestListEntry.StreamNo);
                        }

                        AppendSimpleExport(output, "DestList NetBIOS Name", jl.DestListEntry.NetBiosName);
                        AppendSimpleExport(output, "DestList Date/Time", jl.DestListEntry.FileTime.ToLongDateString() + " " + jl.DestListEntry.FileTime.ToLongTimeString());
                        AppendSimpleExport(output, "DestList Data", jl.DestListEntry.Data);

                        foreach (NameValue nameValue in jl.Data)
                        {
                             csvWriter.WriteField(nameValue.Value);
                             AppendSimpleExport(output, nameValue.Name, nameValue.Value);
                        }

                        string path = System.IO.Path.Combine(folder, jlf.FileName + "." + jl.Name + ".txt");

                        IO.WriteUnicodeTextToFile(output.ToString(), path, false);
                        output = new StringBuilder();

                        csvWriter.NextRecord();
                    }
                }

                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    string csv = streamReader.ReadToEnd();

                    string ret = IO.WriteUnicodeTextToFile(csv, System.IO.Path.Combine(folder, "LinkInfo.csv"), false);
                    if (ret.Length > 0)
                    {
                        UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst saving the file: " + ret);
                    }
                }
            } 
        }

        /// <summary>
        /// Output the file related files first
        /// </summary>
        /// <param name="folder"></param>
        private void ExportFiles(string folder)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.Delimiter = "\t";
            csvConfiguration.HasHeaderRecord = true;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration))
            {
                csvWriter.WriteField("File");
                csvWriter.WriteField("Application");
                csvWriter.NextRecord();

                StringBuilder output = new StringBuilder();

                foreach (ListViewItem listItem in listFiles.Items)
                {
                    if (listItem.Selected == false)
                    {
                        continue;
                    }

                    csvWriter.WriteField(listItem.Text);
                    csvWriter.WriteField(listItem.SubItems[1].Text);
                    csvWriter.NextRecord();

                    AppendSimpleExport(output, listItem.Text, listItem.SubItems[1].Text);
                }

                IO.WriteUnicodeTextToFile(output.ToString(), System.IO.Path.Combine(folder, "AppIds.txt"), false);

                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    string csv = streamReader.ReadToEnd();

                    string ret = IO.WriteUnicodeTextToFile(csv, System.IO.Path.Combine(folder, "AppIds.csv"), false);
                    if (ret.Length > 0)
                    {
                        UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst saving the file: " + ret);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExportDestList(string folder)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.Delimiter = "\t";
            csvConfiguration.HasHeaderRecord = true;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration))
            {
                csvWriter.WriteField("File Name");
                csvWriter.WriteField("Application");
                csvWriter.WriteField("Stream No.");
                csvWriter.WriteField("NetBIOS Name");
                csvWriter.WriteField("Date/Time");
                csvWriter.WriteField("MAC (New)");
                csvWriter.WriteField("Timestamp (New)");
                csvWriter.WriteField("MAC (Birth)");
                csvWriter.WriteField("Timestamp (Birth)");
                csvWriter.WriteField("Data");
                csvWriter.NextRecord();

                foreach (ListViewItem listItem in listFiles.Items)
                {
                    if (listItem.Selected == false)
                    {
                        continue;
                    }

                    var jlf = (from j in _jumpListFiles where j.FileName == listItem.Text.ToString() select j).SingleOrDefault();
                    if (jlf == null)
                    {
                        return;
                    }

                    var sorted = from d in jlf.DestListEntries orderby d.StreamNo select d;
                    foreach (DestListEntry destListEntry in sorted)
                    {
                        csvWriter.WriteField(jlf.FileName);
                        csvWriter.WriteField(jlf.AppName);

                        if (_settings.UseDecimal == true)
                        {
                            csvWriter.WriteField(destListEntry.StreamNo + " (" + Int32.Parse(destListEntry.StreamNo, System.Globalization.NumberStyles.HexNumber)  + ")");
                        }
                        else
                        {
                            csvWriter.WriteField(destListEntry.StreamNo);
                        }

                        csvWriter.WriteField(destListEntry.NetBiosName);
                        csvWriter.WriteField(destListEntry.FileTime);
                        csvWriter.WriteField(destListEntry.Uuid.MacAddress);
                        csvWriter.WriteField(destListEntry.Uuid.Timestamp);
                        csvWriter.WriteField(destListEntry.UuidBirth.MacAddress);
                        csvWriter.WriteField(destListEntry.UuidBirth.Timestamp);
                        csvWriter.WriteField(destListEntry.Data);
                        csvWriter.NextRecord();
                    }
                }

                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    string csv = streamReader.ReadToEnd();

                    string ret = IO.WriteUnicodeTextToFile(csv, System.IO.Path.Combine(folder, "DestList.csv"), false);
                    if (ret.Length > 0)
                    {
                        UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst saving the file: " + ret);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void AppendSimpleExport(StringBuilder output,
                                        string name,
                                        string value)
        {
            output.AppendFormat("{0}: {1}" + Environment.NewLine, name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        private void DisplayFile()
        {
            try
            {
                listLinkInfo.Items.Clear();
                treeStreams.Nodes.Clear();

                treeStreams.Nodes.Add("Root", "Root");

                var jlf = (from j in _jumpListFiles where j.FileName == listFiles.SelectedItems[0].Text select j).SingleOrDefault();
                if (jlf == null)
                {
                    return;
                }

                foreach (JumpList jl in jlf.JumpLists)
                {
                    if (jlf.IsCustom == true)
                    {
                        treeStreams.Nodes[0].Nodes.Add(jl.Name, jl.Name);
                    }
                    else
                    {
                        if (_settings.UseDecimal == true)
                        {
                            treeStreams.Nodes[0].Nodes.Add(jl.Name, jl.Name + " (" + Int32.Parse(jl.Name, System.Globalization.NumberStyles.HexNumber) + ") : " + jl.Size + " bytes");
                        }
                        else
                        {
                            treeStreams.Nodes[0].Nodes.Add(jl.Name, jl.Name + ": " + jl.Size + " bytes");
                        }
                    }
                }

                if (jlf.IsCustom == false)
                {
                    treeStreams.Nodes[0].Nodes.Add("DestList", "DestList" + " (" + jlf.DestListSize + " bytes)");
                }
                
                treeStreams.ExpandAll();

                if (treeStreams.Nodes[0].Nodes.Count > 0)
                {
                    treeStreams.SelectedNode = treeStreams.Nodes[0].Nodes[0];
                }
            }
            catch (Exception ex)
            {
                UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst loading the jump file details: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private List<DestListEntry> ParseDestList(byte[] data)
        {
            List<DestListEntry> entries = new List<DestListEntry>();
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    memoryStream.Seek(32, SeekOrigin.Begin);

                    do
                    {
                        DestListEntry destListEntry = new DestListEntry();
                        //memoryStream.Seek(0x48, SeekOrigin.Current);

                        // (8) 0 – 7	A checksum or hash of the entry.  Not known what type.
                        memoryStream.Seek(8, SeekOrigin.Current);
                        // (16) 8 – 23	New Volume ID
                        // (16) 24 – 39	Object ID
                        destListEntry.Droid = new Guid[] { new Guid(StreamReaderHelper.ReadByteArray(memoryStream, 16)), new Guid(StreamReaderHelper.ReadByteArray(memoryStream, 16))};
                        destListEntry.Uuid = new Uuid(destListEntry.Droid[1].ToString());
                        // (16) 40 – 55	Birth Volume ID
                        // (16) 56 – 71	Object ID
                        destListEntry.DroidBirth = new Guid[] { new Guid(StreamReaderHelper.ReadByteArray(memoryStream, 16)), new Guid(StreamReaderHelper.ReadByteArray(memoryStream, 16)) };
                        destListEntry.UuidBirth = new Uuid(destListEntry.DroidBirth[1].ToString());
                        // (16) 72 – 87	NetBIOS name of volume where the target file is stored – May record names of network shares
                        destListEntry.NetBiosName = woanware.Text.ReplaceNulls(StreamReaderHelper.ReadString(memoryStream, 16));
                        // (8) 88 – 95	Entry ID number
                        destListEntry.StreamNo = StreamReaderHelper.ReadInt64(memoryStream).ToString("X");
                        // (4)  96 – 99	Floating point counter to record each time the file is accessed (not necessarily opened) – Can produce unusual results (partial numbers)
                        memoryStream.Seek(4, SeekOrigin.Current); // Floating point counter to record each time the file is accessed (not necessarily opened) – Can produce unusual results (partial numbers)
                        // (8) 100 – 107	MSFILETIME of last recorded access
                        destListEntry.FileTime = StreamReaderHelper.ReadDateTime(memoryStream);
                        // (4) 108 – 111	Entry ‘pin’ status. ‘0xFF 0xFF 0xFF 0xFF’ = Unpinned.  Otherwise a counter starting at ‘0×00 0×00 0×00 0×00’.
                        memoryStream.Seek(4, SeekOrigin.Current);
                        // (2) 112 – 113	Length of Unicode entry string data
                        int stringLength = StreamReaderHelper.ReadInt16(memoryStream);
                        if (stringLength != -1)
                        {
                            //(n) 114 –	Entry string data
                            destListEntry.Data = StreamReaderHelper.ReadStringUnicode(memoryStream, stringLength * 2);
                        }
                        else
                        {
                            memoryStream.Seek(4, SeekOrigin.Current);
                        }

                        destListEntry.Data = woanware.Text.ReplaceNulls(destListEntry.Data);

                        entries.Add(destListEntry);

                    }
                    while (memoryStream.Position < memoryStream.Length);
                }
            }
            catch (Exception ex)
            {

            }

            return entries;
        }
        #endregion        

        #region Treeview Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeStreams.SelectedNode == null)
            {
                listLinkInfo.Items.Clear();
                return;
            }

            if (treeStreams.SelectedNode.Parent == null)
            {
                listLinkInfo.Items.Clear();
                return;
            }

            var jlf = (from j in _jumpListFiles where j.FileName == listFiles.SelectedItems[0].Text select j).SingleOrDefault();
            if (jlf == null)
            {
                return;
            }

            if (treeStreams.SelectedNode.Name.ToLower() == "destlist")
            {
                DisplayDestListInfo(jlf.DestListEntries);
            }
            else
            {
                var jl = (from j in jlf.JumpLists where j.Name == treeStreams.SelectedNode.Name select j).SingleOrDefault();
                if (jl == null)
                {
                    return;
                }

                DisplayLinkInfo(jl);
            }
        }
        #endregion

        #region Toolbar Button Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnOpen_Click(object sender, EventArgs e)
        {
            LoadFiles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnExport_Click(object sender, EventArgs e)
        {
            ExportFiles();
        }
        #endregion

        #region Menu Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileLoad_Click(object sender, EventArgs e)
        {
            LoadFiles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExport_Click(object sender, EventArgs e)
        {
            ExportFiles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpHelp_Click(object sender, EventArgs e)
        {
            Misc.ShellExecuteFile(System.IO.Path.Combine(Misc.GetApplicationDirectory(), "Help.pdf"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            using (FormAbout formAbout = new FormAbout())
            {
                formAbout.ShowDialog(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuToolsOptions_Click(object sender, EventArgs e)
        {
            using (FormOptions formOptions = new FormOptions(_settings))
            {
                if (formOptions.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                _settings = formOptions.Settings;
            }
        }
        #endregion

        #region Listview Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeStreams.Nodes.Clear();
            listLinkInfo.Items.Clear();

            if (listFiles.SelectedItems.Count == 0)
            {
                return;
            }

            if (listFiles.SelectedItems.Count > 1)
            {
                return;
            }

            DisplayFile();
        }
        #endregion

        #region Form Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            UserInterface.AutoSizeListViewColumns(listFiles);

            _appIds = new Dictionary<string,string>();
            if (File.Exists(Misc.GetApplicationDirectory() + @"\AppIds.txt") == true)
            {
                string[] lines = File.ReadAllLines(Misc.GetApplicationDirectory() + @"\AppIds.txt");
                for (int index = 1; index < lines.Length; index++)
                {
                    try
                    {
                        string line = lines[index].Trim();
                        if (line.Length == 0)
                        {
                            continue;
                        }

                        if (line.StartsWith("#") == true)
                        {
                            continue; 
                        }

                        string[] parts = line.Split('#');
                        if (parts.Length != 2)
                        {
                            continue;
                        }

                        _appIds.Add(parts[0], parts[1]);
                    }
                    catch (Exception){ }
                }
            }

            _settings = new Settings();
            if (_settings.FileExists == true)
            {
                string ret = _settings.Load();
                if (ret.Length > 0)
                {
                    UserInterface.DisplayErrorMessageBox(this, ret);
                }
                else
                {
                    this.WindowState = _settings.FormState;

                    if (_settings.FormState != FormWindowState.Maximized)
                    {
                        this.Location = _settings.FormLocation;
                        this.Size = _settings.FormSize;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settings.FormLocation = base.Location;
            this._settings.FormSize = base.Size;
            this._settings.FormState = base.WindowState;
            string ret = this._settings.Save();
            if (ret.Length > 0)
            {
                UserInterface.DisplayErrorMessageBox(this, ret);
            }
        }
        #endregion

        #region Context Menu Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxMenuExportStream_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Select the export file...";
            saveFileDialog.Filter = "Stream|*.stream";
            if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            CFStream cfStream = null;
            try
            {
                CompoundFile compoundFile = new CompoundFile(listFiles.SelectedItems[0].Tag.ToString());
                if (treeStreams.SelectedNode.Name.ToLower() == "destlist")
                {
                    cfStream = compoundFile.RootStorage.GetStream("DestList");
                }
                else
                {
                    cfStream = compoundFile.RootStorage.GetStream(treeStreams.SelectedNode.Name);
                }

                if (cfStream == null)
                {
                    UserInterface.DisplayMessageBox(this, "The stream contains no data", MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch (Exception ex)
            {
                UserInterface.DisplayErrorMessageBox(this, "An error occurred whilst retrieving the stream: " + ex.Message);
                Misc.WriteToEventLog(Application.ProductName, "An error occurred whilst retrieving the stream: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                return;
            }

            File.WriteAllBytes(saveFileDialog.FileName, cfStream.GetData());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (treeStreams.SelectedNode == null)
            {
                ctxMenuExportStream.Enabled = false;
                return;
            }

            if (treeStreams.SelectedNode.Parent == null)
            {
                ctxMenuExportStream.Enabled = false;
                return;
            }
        }
        #endregion
    }
}
