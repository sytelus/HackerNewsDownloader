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
            this.buttonFetch = new System.Windows.Forms.Button();
            this.buttonAnalyzeUniqueUrls = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxStoriesFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAnalysisFilePath = new System.Windows.Forms.TextBox();
            this.labelTimeElapsedCaption = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelrequestCount = new System.Windows.Forms.Label();
            this.labelTimeElapsed = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCommentsFilePath = new System.Windows.Forms.TextBox();
            this.buttonFetchComents = new System.Windows.Forms.Button();
            this.buttongetStats = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFetch
            // 
            this.buttonFetch.Location = new System.Drawing.Point(70, 38);
            this.buttonFetch.Name = "buttonFetch";
            this.buttonFetch.Size = new System.Drawing.Size(138, 23);
            this.buttonFetch.TabIndex = 0;
            this.buttonFetch.Text = "Fetch HN Stories";
            this.buttonFetch.UseVisualStyleBackColor = true;
            this.buttonFetch.Click += new System.EventHandler(this.buttonFetch_Click);
            // 
            // buttonAnalyzeUniqueUrls
            // 
            this.buttonAnalyzeUniqueUrls.Location = new System.Drawing.Point(70, 218);
            this.buttonAnalyzeUniqueUrls.Name = "buttonAnalyzeUniqueUrls";
            this.buttonAnalyzeUniqueUrls.Size = new System.Drawing.Size(138, 27);
            this.buttonAnalyzeUniqueUrls.TabIndex = 1;
            this.buttonAnalyzeUniqueUrls.Text = "Analyze Unique URLs";
            this.buttonAnalyzeUniqueUrls.UseVisualStyleBackColor = true;
            this.buttonAnalyzeUniqueUrls.Click += new System.EventHandler(this.buttonAnalyzeUniqueUrls_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(539, 291);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(175, 30);
            this.button4.TabIndex = 3;
            this.button4.Text = "Test RegEx";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonRegExText_Click);
            // 
            // textBoxStoriesFilePath
            // 
            this.textBoxStoriesFilePath.Location = new System.Drawing.Point(70, 12);
            this.textBoxStoriesFilePath.Name = "textBoxStoriesFilePath";
            this.textBoxStoriesFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxStoriesFilePath.TabIndex = 4;
            this.textBoxStoriesFilePath.Text = "c:\\temp\\HNStoriesAll.json";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Save to file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Save to file:";
            // 
            // textBoxAnalysisFilePath
            // 
            this.textBoxAnalysisFilePath.Location = new System.Drawing.Point(70, 192);
            this.textBoxAnalysisFilePath.Name = "textBoxAnalysisFilePath";
            this.textBoxAnalysisFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxAnalysisFilePath.TabIndex = 6;
            this.textBoxAnalysisFilePath.Text = "c:\\temp\\WPUrlAnalysis.txt";
            // 
            // labelTimeElapsedCaption
            // 
            this.labelTimeElapsedCaption.AutoSize = true;
            this.labelTimeElapsedCaption.Location = new System.Drawing.Point(67, 64);
            this.labelTimeElapsedCaption.Name = "labelTimeElapsedCaption";
            this.labelTimeElapsedCaption.Size = new System.Drawing.Size(102, 13);
            this.labelTimeElapsedCaption.TabIndex = 8;
            this.labelTimeElapsedCaption.Text = "Time Elapsed (Sec):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Request Count:";
            // 
            // labelrequestCount
            // 
            this.labelrequestCount.AutoSize = true;
            this.labelrequestCount.Location = new System.Drawing.Point(175, 81);
            this.labelrequestCount.Name = "labelrequestCount";
            this.labelrequestCount.Size = new System.Drawing.Size(106, 13);
            this.labelrequestCount.TabIndex = 11;
            this.labelrequestCount.Text = "                                 ";
            // 
            // labelTimeElapsed
            // 
            this.labelTimeElapsed.AutoSize = true;
            this.labelTimeElapsed.Location = new System.Drawing.Point(175, 64);
            this.labelTimeElapsed.Name = "labelTimeElapsed";
            this.labelTimeElapsed.Size = new System.Drawing.Size(85, 13);
            this.labelTimeElapsed.TabIndex = 10;
            this.labelTimeElapsed.Text = "                          ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Save to file:";
            // 
            // textBoxCommentsFilePath
            // 
            this.textBoxCommentsFilePath.Location = new System.Drawing.Point(377, 12);
            this.textBoxCommentsFilePath.Name = "textBoxCommentsFilePath";
            this.textBoxCommentsFilePath.Size = new System.Drawing.Size(138, 20);
            this.textBoxCommentsFilePath.TabIndex = 13;
            this.textBoxCommentsFilePath.Text = "c:\\temp\\HNCommentsAll.json";
            // 
            // buttonFetchComents
            // 
            this.buttonFetchComents.Location = new System.Drawing.Point(377, 38);
            this.buttonFetchComents.Name = "buttonFetchComents";
            this.buttonFetchComents.Size = new System.Drawing.Size(138, 23);
            this.buttonFetchComents.TabIndex = 12;
            this.buttonFetchComents.Text = "Fetch HN Comments";
            this.buttonFetchComents.UseVisualStyleBackColor = true;
            this.buttonFetchComents.Click += new System.EventHandler(this.buttonFetchComents_Click);
            // 
            // buttongetStats
            // 
            this.buttongetStats.Location = new System.Drawing.Point(539, 327);
            this.buttongetStats.Name = "buttongetStats";
            this.buttongetStats.Size = new System.Drawing.Size(175, 32);
            this.buttongetStats.TabIndex = 15;
            this.buttongetStats.Text = "Get Stats";
            this.buttongetStats.UseVisualStyleBackColor = true;
            this.buttongetStats.Click += new System.EventHandler(this.buttongetStats_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 427);
            this.Controls.Add(this.buttongetStats);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCommentsFilePath);
            this.Controls.Add(this.buttonFetchComents);
            this.Controls.Add(this.labelrequestCount);
            this.Controls.Add(this.labelTimeElapsed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelTimeElapsedCaption);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAnalysisFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxStoriesFilePath);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.buttonAnalyzeUniqueUrls);
            this.Controls.Add(this.buttonFetch);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFetch;
        private System.Windows.Forms.Button buttonAnalyzeUniqueUrls;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBoxStoriesFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAnalysisFilePath;
        private System.Windows.Forms.Label labelTimeElapsedCaption;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelrequestCount;
        private System.Windows.Forms.Label labelTimeElapsed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCommentsFilePath;
        private System.Windows.Forms.Button buttonFetchComents;
        private System.Windows.Forms.Button buttongetStats;
    }
}

