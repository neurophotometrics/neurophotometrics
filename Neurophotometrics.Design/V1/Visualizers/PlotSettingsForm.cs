using Neurophotometrics.Design.V1.Converters;
using Neurophotometrics.V1.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V1.Visualizers
{
    public partial class PlotSettingsForm : Form
    {
        public event PlotSettingsEventHandler PlotSettingsChanged;

        private readonly List<PlotSetting> PlotSettings;
        private DataGridView Top_DataGridView;

        public PlotSettingsForm(List<PlotSetting> plotSettings)
        {
            InitializeComponent();
            PlotSettings = plotSettings;
        }

        private void PlotSettingsForm_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            TryCreateDataGrid();
            ResumeLayout();
        }

        private void TryCreateDataGrid()
        {
            try
            {
                CreateDataGrid();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void CreateDataGrid()
        {
            SuspendLayout();
            ConfigureDataGrid();
            AddColumns();
            AddRows();

            Controls.Add(Top_DataGridView);
            ResumeLayout(false);
        }

        private void ConfigureDataGrid()
        {
            Top_DataGridView = new DataGridView
            {
                AutoSize = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            Top_DataGridView.ColumnHeadersHeight *= 3;
            Top_DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Top_DataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            Top_DataGridView.EnableHeadersVisualStyles = false;
            Top_DataGridView.Font = Font;
            Top_DataGridView.RowHeadersVisible = false;

            Top_DataGridView.Paint += Top_DataGridView_Paint;
            Top_DataGridView.CellBeginEdit += Top_DataGridView_CellBeginEdit;
            Top_DataGridView.CellValueChanged += Top_DataGridView_CellValueChanged;
            Top_DataGridView.CurrentCellDirtyStateChanged += Top_DataGridView_CurrentCellDirtyStateChanged;
            Top_DataGridView.Resize += Top_DataGridView_Resize;
            Top_DataGridView.EditingControlShowing += Top_DataGridView_EditingControlShowing;
        }

        private void AddColumns()
        {
            var columns = new List<DataGridViewColumn>()
            {
                new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Region",
                    Name = "RegionName",
                    ReadOnly = true,
                },
                new DataGridViewCheckBoxColumn()
                {
                    HeaderText = "Plot Visible",
                    Name = "RegionVisible",
                }
            };

            foreach (var plotSetting in PlotSettings)
                plotSetting.SignalSettings = plotSetting.SignalSettings.OrderBy(signalSetting => (ushort)signalSetting.LEDFlag).ToList();

            foreach (var signalSetting in PlotSettings[0].SignalSettings)
            {
                var signalName = GetSignalName(signalSetting.LEDFlag);
                var visibleColumn = new DataGridViewCheckBoxColumn()
                {
                    HeaderText = "Visible",
                    Name = signalName + "Visible",
                };
                var scalingColumn = new DataGridViewComboBoxColumn()
                {
                    HeaderText = "Scaling",
                    Name = signalName + "Scaling",
                };
                var minColumn = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Min",
                    Name = signalName + "Min",
                };
                var maxColumn = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Max",
                    Name = signalName + "Max",
                };
                columns.Add(visibleColumn);
                columns.Add(scalingColumn);
                columns.Add(minColumn);
                columns.Add(maxColumn);
            }

            Top_DataGridView.Columns.AddRange(columns.ToArray());

            for (var i = 0; i < Top_DataGridView.Columns.Count; i++)
            {
                var column = Top_DataGridView.Columns[i];
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (column is DataGridViewTextBoxColumn textBoxColumn)
                {
                    textBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    textBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (column is DataGridViewComboBoxColumn comboBoxColumn)
                {
                    comboBoxColumn.Items.Add("Auto");
                    comboBoxColumn.Items.Add("Manual");
                }

                var color = column.DefaultCellStyle.BackColor;
                if (column.Name.Contains("Region"))
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                else if (column.Name.Contains("L470") && column.Name.Contains("L560"))
                    color = TriggerSequenceConverter.GetLightColor(FrameFlags.L470 | FrameFlags.L560);
                else if (column.Name.Contains("L415"))
                    color = TriggerSequenceConverter.GetLightColor(FrameFlags.L415);
                else if (column.Name.Contains("L470"))
                    color = TriggerSequenceConverter.GetLightColor(FrameFlags.L470);
                else if (column.Name.Contains("L560"))
                    color = TriggerSequenceConverter.GetLightColor(FrameFlags.L560);
                else if (column.Name.Contains("All"))
                    color = TriggerSequenceConverter.GetLightColor(FrameFlags.All);
                column.HeaderCell.Style.BackColor = color;
                column.DefaultCellStyle.BackColor = color;
            }
        }

        private string GetSignalName(FrameFlags ledFlag)
        {
            switch (ledFlag)
            {
                case FrameFlags.None:
                    return FrameFlags.None.ToString();

                case FrameFlags.L415:
                    return FrameFlags.L415.ToString();

                case FrameFlags.L470:
                    return FrameFlags.L470.ToString();

                case FrameFlags.L560:
                    return FrameFlags.L560.ToString();

                case FrameFlags.All:
                    return FrameFlags.All.ToString();

                case FrameFlags.L470 | FrameFlags.L560:
                    return FrameFlags.L470.ToString() + " + " + FrameFlags.L560.ToString();

                default:
                    return "";
            }
        }

        private void AddRows()
        {
            Top_DataGridView.Rows.Add(PlotSettings.Count);
            for (var i = 0; i < Top_DataGridView.Rows.Count; i++)
            {
                var cells = Top_DataGridView.Rows[i].Cells;
                cells[0].Value = PlotSettings[i].PhotometryRegion.Name;
                cells[1].Value = PlotSettings[i].IsVisible;

                PlotSettings[i].SignalSettings = PlotSettings[i].SignalSettings.OrderBy(setting => (ushort)setting.LEDFlag).ToList();
                for (var j = 0; j < PlotSettings[i].SignalSettings.Count; j++)
                {
                    var index = 4 * j + 2;
                    cells[index].Value = PlotSettings[i].SignalSettings[j].IsVisible;
                    cells[index + 1].Value = PlotSettings[i].SignalSettings[j].Scaling.Mode.ToString();

                    cells[index + 2].Value = PlotSettings[i].SignalSettings[j].Scaling.Min.ToString("N4");
                    cells[index + 3].Value = PlotSettings[i].SignalSettings[j].Scaling.Max.ToString("N4");
                }
            }
        }

        private void Top_DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TryApplyKeyFiltering(e);
        }

        private void Top_DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            TryFilterAutoScalingEdits(e);
        }

        private void Top_DataGridView_Resize(object sender, EventArgs e)
        {
            Width = Top_DataGridView.Width;
            Height = Top_DataGridView.Height;
            TryEnsureHeaderRect();
        }

        private void Top_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                CommitEdit();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void Top_DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            TryHandleCellValueChanged(e);
        }

        private void Top_DataGridView_Paint(object sender, PaintEventArgs e)
        {
            TryDrawSignalColumns(e);
        }

        private void TextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!KeyFilteringHelpers.IsDecimalNumeric(e.KeyChar))
                e.Handled = true;
        }

        private void TryApplyKeyFiltering(DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                ApplyKeyFiltering(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryFilterAutoScalingEdits(DataGridViewCellCancelEventArgs e)
        {
            try
            {
                FilterAutoScalingEdits(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryHandleCellValueChanged(DataGridViewCellEventArgs e)
        {
            try
            {
                HandleCellValueChanged(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryDrawSignalColumns(PaintEventArgs e)
        {
            try
            {
                DrawSignalColumns(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryEnsureHeaderRect()
        {
            try
            {
                EnsureHeaderRect();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ApplyKeyFiltering(DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(TextBoxColumn_KeyPress);
            if (e.Control is TextBox textBoxControl)
            {
                textBoxControl.KeyPress += new KeyPressEventHandler(TextBoxColumn_KeyPress);
            }
        }

        private void FilterAutoScalingEdits(DataGridViewCellCancelEventArgs e)
        {
            var cell = Top_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell is DataGridViewTextBoxCell)
            {
                var isAutoScale = GetIsAutoScale(e.RowIndex, e.ColumnIndex);
                if (isAutoScale) e.Cancel = true;
            }
        }

        private void CommitEdit()
        {
            var isDirty = Top_DataGridView.IsCurrentCellDirty;
            var isTextBox = Top_DataGridView.CurrentCell is DataGridViewTextBoxCell;

            if (isDirty && !isTextBox)
            {
                // This fires the cell value changed handler below
                Top_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void HandleCellValueChanged(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) return;

            var isCurrentCellCaller = GetIsCurrentCellCaller(e);
            if (!isCurrentCellCaller) return;

            var isCellScalingDependent = GetIsCellScalingDependent(e);

            if (isCellScalingDependent)
            {
                var isAutoScale = GetIsAutoScale(e.RowIndex, e.ColumnIndex);
                if (isAutoScale) return;

                Top_DataGridView.CellValueChanged -= Top_DataGridView_CellValueChanged;
                var val = Top_DataGridView.CurrentCell.Value;
                var valDouble = 0.0;
                if (val is string valStr)
                    double.TryParse(valStr, out valDouble);

                Top_DataGridView.CurrentCell.Value = valDouble.ToString("N4");
                Top_DataGridView.CellValueChanged += Top_DataGridView_CellValueChanged;
                SendChangedSetting(e);
            }
            else
                SendChangedSetting(e);
        }

        private void SendChangedSetting(DataGridViewCellEventArgs e)
        {
            var settingPathList = new List<string>
            {
                GetRegion(e.RowIndex),
                GetSignal(e.ColumnIndex),
                GetSetting(e.ColumnIndex)
            };
            var settingPath = string.Join(".", settingPathList);
            var settingValue = Top_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            PlotSettingsChanged?.Invoke(this, new PlotSettingsEventArgs(settingPath, settingValue));
        }

        private string GetRegion(int rowIndex)
        {
            return (string)Top_DataGridView.Rows[rowIndex].Cells[0].Value;
        }

        private string GetSignal(int columnIndex)
        {
            if (columnIndex == 1)
                return "IsVisible";

            var signalIndex = (columnIndex - 2) / 4;
            var ledFlag = PlotSettings[0].SignalSettings[signalIndex].LEDFlag;
            return GetSignalName(ledFlag);
        }

        private string GetSetting(int columnIndex)
        {
            if ((columnIndex - 2) % 4 == 0)
                return "IsVisible";
            else if ((columnIndex - 2) % 4 == 1)
                return "Scaling";
            else if ((columnIndex - 2) % 4 == 2)
                return "Min";
            else
                return "Max";
        }

        private bool GetIsAutoScale(int rowIndex, int colIndex)
        {
            var scalingColumnIndex = (colIndex - 2) / 4 * 4 + 3;
            var cellValue = Top_DataGridView.Rows[rowIndex].Cells[scalingColumnIndex].Value;

            if (cellValue is string autoScaleStr)
                return autoScaleStr.Contains("Auto");
            else
                return true;
        }

        private bool GetIsCellScalingDependent(DataGridViewCellEventArgs e)
        {
            return (e.ColumnIndex - 2) % 4 > 1;
        }

        private bool GetIsCurrentCellCaller(DataGridViewCellEventArgs e)
        {
            return Top_DataGridView.CurrentCell.RowIndex == e.RowIndex && Top_DataGridView.CurrentCell.ColumnIndex == e.ColumnIndex;
        }

        private void DrawSignalColumns(PaintEventArgs e)
        {
            var signalSettings = PlotSettings[0].SignalSettings;
            var dispRects = new Rectangle[signalSettings.Count];
            for (var i = 0; i < dispRects.Length; i++)
                dispRects[i].X = int.MaxValue;

            for (var i = 2; i < Top_DataGridView.Columns.Count; i++)
            {
                var parentColIndex = GetDisplayRectIndex(Top_DataGridView.Columns[i]);
                if (parentColIndex == -1) continue;

                var dispRect = dispRects[parentColIndex];
                var cellDispRect = Top_DataGridView.GetCellDisplayRectangle(i, -1, true);
                dispRect.X = Math.Min(cellDispRect.X, dispRect.X);
                dispRect.Y = Math.Max(cellDispRect.Y, dispRect.Y);
                dispRect.Width += cellDispRect.Width;
                dispRect.Height = Math.Max(cellDispRect.Height, dispRect.Height) / 2;
                dispRects[parentColIndex] = dispRect;
            }

            for (var i = 0; i < dispRects.Length; i++)
            {
                dispRects[i].X += 1;
                dispRects[i].Y += 1;
                dispRects[i].Width -= 2;
                dispRects[i].Height -= 2;
                var color = TriggerSequenceConverter.GetLightColor(signalSettings[i].LEDFlag);
                e.Graphics.FillRectangle(new SolidBrush(color), dispRects[i]);
            }

            for (var i = 0; i < dispRects.Length; i++)
            {
                var parentColName = GetSignalName(signalSettings[i].LEDFlag);
                var format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(parentColName, Top_DataGridView.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Top_DataGridView.ColumnHeadersDefaultCellStyle.ForeColor), dispRects[i], format);
            }
        }

        private int GetDisplayRectIndex(DataGridViewColumn dataGridViewColumn)
        {
            var signalSettings = PlotSettings[0].SignalSettings;
            for (var i = 0; i < signalSettings.Count; i++)
                if (dataGridViewColumn.Name.Contains(GetSignalName(signalSettings[i].LEDFlag)))
                    return i;
            return -1;
        }

        private void EnsureHeaderRect()
        {
            var headerRect = Top_DataGridView.DisplayRectangle;
            headerRect.Height = Top_DataGridView.ColumnHeadersHeight / 2;
            Top_DataGridView.Invalidate(headerRect);
        }

        internal void TryUpdateScaling(EventPattern<PlotSettingsEventArgs> obj)
        {
            try
            {
                UpdateScaling(obj);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateScaling(EventPattern<PlotSettingsEventArgs> obj)
        {
            if (obj.Sender is SignalPane signalPane)
            {
                var value = (double)obj.EventArgs.SettingValue;
                var rowIndex = signalPane.PhotometryRegion.Index;
                var colIndex = 0;
                for (var i = 0; i < PlotSettings[0].SignalSettings.Count; i++)
                {
                    if (PlotSettings[0].SignalSettings[i].LEDFlag == signalPane.LEDFlag)
                    {
                        colIndex = 2 + 4 * i;
                        break;
                    }
                }

                if (obj.EventArgs.SettingName.Contains("Min"))
                    colIndex += 2;
                else if (obj.EventArgs.SettingName.Contains("Max"))
                    colIndex += 3;

                Top_DataGridView.CellValueChanged -= Top_DataGridView_CellValueChanged;
                Top_DataGridView.Rows[rowIndex].Cells[colIndex].Value = value.ToString("N4");
                Top_DataGridView.CellValueChanged += Top_DataGridView_CellValueChanged;
            }
        }
    }

    public delegate void PlotSettingsEventHandler(object sender, PlotSettingsEventArgs args);

    public class PlotSettingsEventArgs : EventArgs
    {
        public string SettingName { get; private set; }
        public object SettingValue { get; private set; }

        public PlotSettingsEventArgs(string settingName, object settingValue)
        {
            SettingName = settingName;
            SettingValue = settingValue;
        }
    }
}