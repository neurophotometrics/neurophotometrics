
namespace Neurophotometrics.Design.V2.UserControls
{
    partial class StimRepsControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PulseTrainType_ComboBox = new System.Windows.Forms.ComboBox();
            this.StimReps_Label = new System.Windows.Forms.Label();
            this.StimRepsValue_Label = new System.Windows.Forms.Label();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 4;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Controls.Add(this.PulseTrainType_ComboBox, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.StimReps_Label, 1, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.StimRepsValue_Label, 2, 0);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 1;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(298, 30);
            this.Top_TableLayoutPanel.TabIndex = 0;
            this.Top_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // PulseTrainType_ComboBox
            // 
            this.PulseTrainType_ComboBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.PulseTrainType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PulseTrainType_ComboBox.FormattingEnabled = true;
            this.PulseTrainType_ComboBox.Items.AddRange(new object[] {
            "Finite",
            "Continuous"});
            this.PulseTrainType_ComboBox.Location = new System.Drawing.Point(3, 3);
            this.PulseTrainType_ComboBox.Name = "PulseTrainType_ComboBox";
            this.PulseTrainType_ComboBox.Size = new System.Drawing.Size(100, 31);
            this.PulseTrainType_ComboBox.TabIndex = 0;
            this.PulseTrainType_ComboBox.SelectedIndexChanged += new System.EventHandler(this.PulseTrainType_ComboBox_SelectedIndexChanged);
            // 
            // StimReps_Label
            // 
            this.StimReps_Label.AutoSize = true;
            this.StimReps_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.StimReps_Label.Enabled = false;
            this.StimReps_Label.Location = new System.Drawing.Point(106, 0);
            this.StimReps_Label.Margin = new System.Windows.Forms.Padding(0);
            this.StimReps_Label.Name = "StimReps_Label";
            this.StimReps_Label.Size = new System.Drawing.Size(67, 30);
            this.StimReps_Label.TabIndex = 1;
            this.StimReps_Label.Text = "Count:";
            this.StimReps_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StimReps_Label.Visible = false;
            this.StimReps_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // StimRepsValue_Label
            // 
            this.StimRepsValue_Label.AutoSize = true;
            this.StimRepsValue_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.StimRepsValue_Label.Location = new System.Drawing.Point(173, 0);
            this.StimRepsValue_Label.Margin = new System.Windows.Forms.Padding(0);
            this.StimRepsValue_Label.Name = "StimRepsValue_Label";
            this.StimRepsValue_Label.Size = new System.Drawing.Size(0, 30);
            this.StimRepsValue_Label.TabIndex = 2;
            this.StimRepsValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StimRepsValue_Label.Click += new System.EventHandler(this.StimRepsValue_Label_Click);
            // 
            // StimRepsControl
            // 
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StimRepsControl";
            this.Size = new System.Drawing.Size(298, 30);
            this.Click += new System.EventHandler(this.StrayClick);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.ComboBox PulseTrainType_ComboBox;
        private System.Windows.Forms.Label StimReps_Label;
        private System.Windows.Forms.Label StimRepsValue_Label;
    }
}
