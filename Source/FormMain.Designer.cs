namespace woanware
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSepOne = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSepTwo = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSepOne = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolBtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnExport = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainerBottom = new System.Windows.Forms.SplitContainer();
            this.treeStreams = new System.Windows.Forms.TreeView();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuExportStream = new System.Windows.Forms.ToolStripMenuItem();
            this.listLinkInfo = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listFiles = new System.Windows.Forms.ListView();
            this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAppName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).BeginInit();
            this.splitContainerBottom.Panel1.SuspendLayout();
            this.splitContainerBottom.Panel2.SuspendLayout();
            this.splitContainerBottom.SuspendLayout();
            this.ctxMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.mnuTools,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(623, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileLoad,
            this.menuFileSepOne,
            this.menuFileExport,
            this.menuFileSepTwo,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileLoad
            // 
            this.menuFileLoad.Name = "menuFileLoad";
            this.menuFileLoad.Size = new System.Drawing.Size(107, 22);
            this.menuFileLoad.Text = "Load";
            this.menuFileLoad.Click += new System.EventHandler(this.menuFileLoad_Click);
            // 
            // menuFileSepOne
            // 
            this.menuFileSepOne.Name = "menuFileSepOne";
            this.menuFileSepOne.Size = new System.Drawing.Size(104, 6);
            // 
            // menuFileExport
            // 
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size(107, 22);
            this.menuFileExport.Text = "&Export";
            this.menuFileExport.Click += new System.EventHandler(this.menuFileExport_Click);
            // 
            // menuFileSepTwo
            // 
            this.menuFileSepTwo.Name = "menuFileSepTwo";
            this.menuFileSepTwo.Size = new System.Drawing.Size(104, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(107, 22);
            this.menuFileExit.Text = "&Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsOptions});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(48, 20);
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsOptions
            // 
            this.mnuToolsOptions.Name = "mnuToolsOptions";
            this.mnuToolsOptions.Size = new System.Drawing.Size(116, 22);
            this.mnuToolsOptions.Text = "Options";
            this.mnuToolsOptions.Click += new System.EventHandler(this.mnuToolsOptions_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpHelp,
            this.menuSepOne,
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            // 
            // menuHelpHelp
            // 
            this.menuHelpHelp.Name = "menuHelpHelp";
            this.menuHelpHelp.Size = new System.Drawing.Size(107, 22);
            this.menuHelpHelp.Text = "&Help";
            this.menuHelpHelp.Click += new System.EventHandler(this.menuHelpHelp_Click);
            // 
            // menuSepOne
            // 
            this.menuSepOne.Name = "menuSepOne";
            this.menuSepOne.Size = new System.Drawing.Size(104, 6);
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuHelpAbout.Text = "&About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnOpen,
            this.toolStripSeparator1,
            this.toolBtnExport});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(623, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolBtnOpen
            // 
            this.toolBtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnOpen.Image")));
            this.toolBtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnOpen.Name = "toolBtnOpen";
            this.toolBtnOpen.Size = new System.Drawing.Size(23, 22);
            this.toolBtnOpen.ToolTipText = "Load";
            this.toolBtnOpen.Click += new System.EventHandler(this.toolBtnOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolBtnExport
            // 
            this.toolBtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnExport.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnExport.Image")));
            this.toolBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnExport.Name = "toolBtnExport";
            this.toolBtnExport.Size = new System.Drawing.Size(23, 22);
            this.toolBtnExport.ToolTipText = "Export";
            this.toolBtnExport.Click += new System.EventHandler(this.toolBtnExport_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 456);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(623, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitContainerBottom
            // 
            this.splitContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottom.Name = "splitContainerBottom";
            // 
            // splitContainerBottom.Panel1
            // 
            this.splitContainerBottom.Panel1.Controls.Add(this.treeStreams);
            // 
            // splitContainerBottom.Panel2
            // 
            this.splitContainerBottom.Panel2.Controls.Add(this.listLinkInfo);
            this.splitContainerBottom.Size = new System.Drawing.Size(623, 296);
            this.splitContainerBottom.SplitterDistance = 206;
            this.splitContainerBottom.SplitterWidth = 5;
            this.splitContainerBottom.TabIndex = 4;
            // 
            // treeStreams
            // 
            this.treeStreams.ContextMenuStrip = this.ctxMenu;
            this.treeStreams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeStreams.HideSelection = false;
            this.treeStreams.Location = new System.Drawing.Point(0, 0);
            this.treeStreams.Name = "treeStreams";
            this.treeStreams.Size = new System.Drawing.Size(206, 296);
            this.treeStreams.TabIndex = 0;
            this.treeStreams.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeNodes_AfterSelect);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuExportStream});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ctxMenu.Size = new System.Drawing.Size(148, 26);
            this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
            // 
            // ctxMenuExportStream
            // 
            this.ctxMenuExportStream.Name = "ctxMenuExportStream";
            this.ctxMenuExportStream.Size = new System.Drawing.Size(147, 22);
            this.ctxMenuExportStream.Text = "Export Stream";
            this.ctxMenuExportStream.Click += new System.EventHandler(this.ctxMenuExportStream_Click);
            // 
            // listLinkInfo
            // 
            this.listLinkInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.listLinkInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLinkInfo.FullRowSelect = true;
            this.listLinkInfo.HideSelection = false;
            this.listLinkInfo.Location = new System.Drawing.Point(0, 0);
            this.listLinkInfo.Name = "listLinkInfo";
            this.listLinkInfo.Size = new System.Drawing.Size(412, 296);
            this.listLinkInfo.TabIndex = 1;
            this.listLinkInfo.UseCompatibleStateImageBehavior = false;
            this.listLinkInfo.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "";
            // 
            // colValue
            // 
            this.colValue.Text = "";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Jump Lists|*.automaticDestinations-ms|Custom Jump Lists|*.customDestinations-ms";
            this.openFileDialog.Multiselect = true;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 49);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listFiles);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerBottom);
            this.splitContainerMain.Size = new System.Drawing.Size(623, 407);
            this.splitContainerMain.SplitterDistance = 106;
            this.splitContainerMain.SplitterWidth = 5;
            this.splitContainerMain.TabIndex = 5;
            // 
            // listFiles
            // 
            this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colAppName});
            this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFiles.FullRowSelect = true;
            this.listFiles.HideSelection = false;
            this.listFiles.Location = new System.Drawing.Point(0, 0);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(623, 106);
            this.listFiles.TabIndex = 0;
            this.listFiles.UseCompatibleStateImageBehavior = false;
            this.listFiles.View = System.Windows.Forms.View.Details;
            this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
            // 
            // colFileName
            // 
            this.colFileName.Text = "File Name";
            // 
            // colAppName
            // 
            this.colAppName.Text = "Application";
            this.colAppName.Width = 95;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select output folder";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 478);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JumpLister";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainerBottom.Panel1.ResumeLayout(false);
            this.splitContainerBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).EndInit();
            this.splitContainerBottom.ResumeLayout(false);
            this.ctxMenu.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainerBottom;
        private System.Windows.Forms.TreeView treeStreams;
        private System.Windows.Forms.ListView listLinkInfo;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileLoad;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripSeparator menuFileSepOne;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator menuFileSepTwo;
        private System.Windows.Forms.ToolStripSeparator menuSepOne;
        private System.Windows.Forms.ToolStripButton toolBtnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnExport;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListView listFiles;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ColumnHeader colAppName;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuExportStream;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

