namespace HackerNewsDownloader
{
    partial class MainForm
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
            this.buttonAnalyzeUniqueUrls = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUrlAnalysisFilePath = new System.Windows.Forms.TextBox();
            this.buttongetStats = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxStoriesFilePath = new System.Windows.Forms.TextBox();
            this.buttonFetch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCommentsFilePath = new System.Windows.Forms.TextBox();
            this.labelTimeElapsedCaption = new System.Windows.Forms.Label();
            this.buttonFetchComents = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelrequestCount = new System.Windows.Forms.Label();
            this.labelTimeElapsed = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxPostTimeStatsFilePath = new System.Windows.Forms.TextBox();
            this.buttonAnalyzePostTimes = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAnalyzeUniqueUrls
            // 
            this.buttonAnalyzeUniqueUrls.Location = new System.Drawing.Point(84, 51);
            this.buttonAnalyzeUniqueUrls.Name = "buttonAnalyzeUniqueUrls";
            this.buttonAnalyzeUniqueUrls.Size = new System.Drawing.Size(138, 27);
            this.buttonAnalyzeUniqueUrls.TabIndex = 1;
            this.buttonAnalyzeUniqueUrls.Text = "Analyze Unique URLs";
            this.buttonAnalyzeUniqueUrls.UseVisualStyleBackColor = true;
            this.buttonAnalyzeUniqueUrls.Click += new System.EventHandler(this.buttonAnalyzeUniqueUrls_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Save to file:";
            // 
            // textBoxUrlAnalysisFilePath
            // 
            this.textBoxUrlAnalysisFilePath.Location = new System.Drawing.Point(84, 25);
            this.textBoxUrlAnalysisFilePath.Name = "textBoxUrlAnalysisFilePath";
            this.textBoxUrlAnalysisFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxUrlAnalysisFilePath.TabIndex = 6;
            this.textBoxUrlAnalysisFilePath.Text = "c:\\temp\\WPUrlAnalysis.txt";
            // 
            // buttongetStats
            // 
            this.buttongetStats.Location = new System.Drawing.Point(344, 156);
            this.buttongetStats.Name = "buttongetStats";
            this.buttongetStats.Size = new System.Drawing.Size(175, 32);
            this.buttongetStats.TabIndex = 15;
            this.buttongetStats.Text = "Get Stats";
            this.buttongetStats.UseVisualStyleBackColor = true;
            this.buttongetStats.Click += new System.EventHandler(this.buttongetStats_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(746, 427);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxStoriesFilePath);
            this.tabPage1.Controls.Add(this.buttongetStats);
            this.tabPage1.Controls.Add(this.buttonFetch);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxCommentsFilePath);
            this.tabPage1.Controls.Add(this.labelTimeElapsedCaption);
            this.tabPage1.Controls.Add(this.buttonFetchComents);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.labelrequestCount);
            this.tabPage1.Controls.Add(this.labelTimeElapsed);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 401);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Download";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxStoriesFilePath
            // 
            this.textBoxStoriesFilePath.Location = new System.Drawing.Point(74, 9);
            this.textBoxStoriesFilePath.Name = "textBoxStoriesFilePath";
            this.textBoxStoriesFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxStoriesFilePath.TabIndex = 4;
            this.textBoxStoriesFilePath.Text = "c:\\temp\\HNStoriesAll.json";
            // 
            // buttonFetch
            // 
            this.buttonFetch.Location = new System.Drawing.Point(74, 35);
            this.buttonFetch.Name = "buttonFetch";
            this.buttonFetch.Size = new System.Drawing.Size(138, 23);
            this.buttonFetch.TabIndex = 0;
            this.buttonFetch.Text = "Fetch HN Stories";
            this.buttonFetch.UseVisualStyleBackColor = true;
            this.buttonFetch.Click += new System.EventHandler(this.buttonFetch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(320, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Save to file:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Save to file:";
            // 
            // textBoxCommentsFilePath
            // 
            this.textBoxCommentsFilePath.Location = new System.Drawing.Point(381, 9);
            this.textBoxCommentsFilePath.Name = "textBoxCommentsFilePath";
            this.textBoxCommentsFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxCommentsFilePath.TabIndex = 13;
            this.textBoxCommentsFilePath.Text = "c:\\temp\\HNCommentsAll.json";
            // 
            // labelTimeElapsedCaption
            // 
            this.labelTimeElapsedCaption.AutoSize = true;
            this.labelTimeElapsedCaption.Location = new System.Drawing.Point(71, 61);
            this.labelTimeElapsedCaption.Name = "labelTimeElapsedCaption";
            this.labelTimeElapsedCaption.Size = new System.Drawing.Size(118, 15);
            this.labelTimeElapsedCaption.TabIndex = 8;
            this.labelTimeElapsedCaption.Text = "Time Elapsed (Sec):";
            // 
            // buttonFetchComents
            // 
            this.buttonFetchComents.Location = new System.Drawing.Point(381, 35);
            this.buttonFetchComents.Name = "buttonFetchComents";
            this.buttonFetchComents.Size = new System.Drawing.Size(138, 23);
            this.buttonFetchComents.TabIndex = 12;
            this.buttonFetchComents.Text = "Fetch HN Comments";
            this.buttonFetchComents.UseVisualStyleBackColor = true;
            this.buttonFetchComents.Click += new System.EventHandler(this.buttonFetchComents_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Request Count:";
            // 
            // labelrequestCount
            // 
            this.labelrequestCount.AutoSize = true;
            this.labelrequestCount.Location = new System.Drawing.Point(179, 78);
            this.labelrequestCount.Name = "labelrequestCount";
            this.labelrequestCount.Size = new System.Drawing.Size(106, 15);
            this.labelrequestCount.TabIndex = 11;
            this.labelrequestCount.Text = "                                 ";
            // 
            // labelTimeElapsed
            // 
            this.labelTimeElapsed.AutoSize = true;
            this.labelTimeElapsed.Location = new System.Drawing.Point(179, 61);
            this.labelTimeElapsed.Name = "labelTimeElapsed";
            this.labelTimeElapsed.Size = new System.Drawing.Size(85, 15);
            this.labelTimeElapsed.TabIndex = 10;
            this.labelTimeElapsed.Text = "                          ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.buttonAnalyzeUniqueUrls);
            this.tabPage2.Controls.Add(this.textBoxUrlAnalysisFilePath);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(738, 401);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "WordPress Analysis";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(26, 137);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(175, 30);
            this.button4.TabIndex = 8;
            this.button4.Text = "Test RegEx";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxPostTimeStatsFilePath);
            this.tabPage3.Controls.Add(this.buttonAnalyzePostTimes);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(738, 401);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Optimal Posting Time";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxPostTimeStatsFilePath
            // 
            this.textBoxPostTimeStatsFilePath.Location = new System.Drawing.Point(74, 17);
            this.textBoxPostTimeStatsFilePath.Name = "textBoxPostTimeStatsFilePath";
            this.textBoxPostTimeStatsFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxPostTimeStatsFilePath.TabIndex = 7;
            this.textBoxPostTimeStatsFilePath.Text = "c:\\temp\\HNPostTimeStats.tsv";
            // 
            // buttonAnalyzePostTimes
            // 
            this.buttonAnalyzePostTimes.Location = new System.Drawing.Point(74, 43);
            this.buttonAnalyzePostTimes.Name = "buttonAnalyzePostTimes";
            this.buttonAnalyzePostTimes.Size = new System.Drawing.Size(138, 23);
            this.buttonAnalyzePostTimes.TabIndex = 6;
            this.buttonAnalyzePostTimes.Text = "Analyze Posting Times";
            this.buttonAnalyzePostTimes.UseVisualStyleBackColor = true;
            this.buttonAnalyzePostTimes.Click += new System.EventHandler(this.buttonAnalyzePostTimes_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Save to file:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 427);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Hacker News Downloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAnalyzeUniqueUrls;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUrlAnalysisFilePath;
        private System.Windows.Forms.Button buttongetStats;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBoxStoriesFilePath;
        private System.Windows.Forms.Button buttonFetch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCommentsFilePath;
        private System.Windows.Forms.Label labelTimeElapsedCaption;
        private System.Windows.Forms.Button buttonFetchComents;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelrequestCount;
        private System.Windows.Forms.Label labelTimeElapsed;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxPostTimeStatsFilePath;
        private System.Windows.Forms.Button buttonAnalyzePostTimes;
        private System.Windows.Forms.Label label5;
    }
}

