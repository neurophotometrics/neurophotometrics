using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class TrigSeqControl : UserControl
    {
        private const int MaxRows = 31;
        public FrameFlags[] TriggerSequence { get; set; }

        public event RegSettingChangedEventHandler TrigSeqChanged;

        public TrigSeqControl()
        {
            InitializeComponent();

            LEDName.DataSource = TriggerSequenceConverter.TextToFrameFlag.ToList();
            LEDName.DisplayMember = "Key";
            LEDName.ValueMember = "Value";

            TrigSeq_DataGridView.CurrentCellDirtyStateChanged += TrigSeq_DataGridView_CurrentCellDirtyStateChanged;
            TrigSeq_DataGridView.CellValueChanged += TrigSeq_DataGridView_CellValueChanged;
            TrigSeq_DataGridView.CurrentCellChanged += TrigSeq_DataGridView_CurrentCellChanged;
        }

        internal void TryUpdateTrigSeq(RegSettingChangedEventArgs arg)
        {
            try
            {
                UpdateTrigSeq(arg);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateTrigSeq(RegSettingChangedEventArgs arg)
        {
            if (arg.Name == nameof(TriggerSequence))
            {
                TrigSeq_DataGridView.CurrentCellDirtyStateChanged -= TrigSeq_DataGridView_CurrentCellDirtyStateChanged;
                TrigSeq_DataGridView.CellValueChanged -= TrigSeq_DataGridView_CellValueChanged;
                TrigSeq_DataGridView.CurrentCellChanged -= TrigSeq_DataGridView_CurrentCellChanged;

                TriggerSequence = (FrameFlags[])arg.Value;

                if (TriggerSequence != null)
                {
                    TrigSeq_DataGridView.RowCount = TriggerSequence.Length + 1;
                    for (var i = 0; i < TriggerSequence.Length; i++)
                    {
                        var color = TriggerSequenceConverter.GetLightColor(TriggerSequence[i]);
                        TrigSeq_DataGridView.Rows[i].Cells[0].Value = i;
                        TrigSeq_DataGridView.Rows[i].Cells[0].Style.BackColor = color;
                        TrigSeq_DataGridView.Rows[i].Cells[1].Value = TriggerSequence[i];
                        TrigSeq_DataGridView.Rows[i].Cells[1].Style.BackColor = color;
                        TrigSeq_DataGridView.Rows[i].Cells[2].Value = ((ushort)TriggerSequence[i]).ToString();
                        TrigSeq_DataGridView.Rows[i].Cells[2].Style.BackColor = color;
                    }

                    if (TriggerSequence.Length == MaxRows)
                    {
                        TrigSeq_DataGridView.CurrentCell = null;
                        TrigSeq_DataGridView.AllowUserToAddRows = false;
                    }
                    else
                    {
                        TrigSeq_DataGridView.CurrentCell = TrigSeq_DataGridView.Rows[TrigSeq_DataGridView.RowCount - 1].Cells[1];
                    }
                }

                TrigSeq_DataGridView.CurrentCellDirtyStateChanged += TrigSeq_DataGridView_CurrentCellDirtyStateChanged;
                TrigSeq_DataGridView.CellValueChanged += TrigSeq_DataGridView_CellValueChanged;
                TrigSeq_DataGridView.CurrentCellChanged += TrigSeq_DataGridView_CurrentCellChanged;
            }
        }

        private void TrigSeq_DataGridView_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateAllowUserToAddRows();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateAllowUserToAddRows()
        {
            if (TrigSeq_DataGridView.Rows.Count >= MaxRows)
            {
                TrigSeq_DataGridView.AllowUserToAddRows = false;
            }
            else if (!TrigSeq_DataGridView.AllowUserToAddRows)
            {
                TrigSeq_DataGridView.AllowUserToAddRows = true;
            }
        }

        private void TrigSeq_DataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateBackColors();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateBackColors()
        {
            for (var i = 0; i < TriggerSequence.Length; i++)
            {
                var color = TriggerSequenceConverter.GetLightColor(TriggerSequence[i]);
                TrigSeq_DataGridView.Rows[i].Cells[0].Style.BackColor = color;
                TrigSeq_DataGridView.Rows[i].Cells[1].Style.BackColor = color;
                TrigSeq_DataGridView.Rows[i].Cells[2].Style.BackColor = color;
            }

            if (TrigSeq_DataGridView.CurrentRow != null)
            {
                TrigSeq_DataGridView.CurrentRow.Cells[0].Style.BackColor = Color.White;
                TrigSeq_DataGridView.CurrentRow.Cells[1].Style.BackColor = Color.White;
                TrigSeq_DataGridView.CurrentRow.Cells[2].Style.BackColor = Color.White;
            }
        }

        private void TrigSeq_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
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

        private void CommitEdit()
        {
            if (TrigSeq_DataGridView.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                TrigSeq_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void TrigSeq_DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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

        private void HandleCellValueChanged(DataGridViewCellEventArgs e)
        {
            var numLEDs = TrigSeq_DataGridView.AllowUserToAddRows ? TrigSeq_DataGridView.RowCount - 1 : MaxRows;
            TriggerSequence = new FrameFlags[numLEDs];
            for (var i = 0; i < numLEDs; i++)
            {
                TriggerSequence[i] = (FrameFlags)TrigSeq_DataGridView.Rows[i].Cells[1].Value;
                TrigSeq_DataGridView.Rows[i].Cells[0].Value = i;
            }

            TrigSeq_DataGridView.Rows[e.RowIndex].Cells[2].Value = ((ushort)TriggerSequence[e.RowIndex]).ToString();
            if (TriggerSequence.Length >= MaxRows) TrigSeq_DataGridView.AllowUserToAddRows = false;

            var nextIndex = e.RowIndex + 1;

            if (nextIndex == MaxRows) TrigSeq_DataGridView.CurrentCell = null;
            else TrigSeq_DataGridView.CurrentCell = TrigSeq_DataGridView.Rows[nextIndex].Cells[1];

            TrigSeqChanged?.Invoke(this, new RegSettingChangedEventArgs(nameof(TriggerSequence), TriggerSequence));
        }

        private void Remove_Button_Click(object sender, EventArgs e)
        {
            try
            {
                HandleRemoveCell();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void HandleRemoveCell()
        {
            if (TrigSeq_DataGridView.CurrentRow != null)
            {
                var index = TrigSeq_DataGridView.CurrentRow.Index;
                var isMoreThanOneLED = TriggerSequence.Length > 1;
                var isLEDRow = index < TriggerSequence.Length;

                if (isMoreThanOneLED && isLEDRow)
                {
                    TrigSeq_DataGridView.CurrentCellChanged -= TrigSeq_DataGridView_CurrentCellChanged;
                    TrigSeq_DataGridView.AllowUserToAddRows = true;
                    TrigSeq_DataGridView.Rows.Remove(TrigSeq_DataGridView.CurrentRow);
                    TriggerSequence = new FrameFlags[TrigSeq_DataGridView.RowCount - 1];
                    for (var i = 0; i < TrigSeq_DataGridView.RowCount - 1; i++)
                    {
                        TriggerSequence[i] = (FrameFlags)TrigSeq_DataGridView.Rows[i].Cells[1].Value;
                        TrigSeq_DataGridView.Rows[i].Cells[0].Value = i;
                    }

                    TrigSeq_DataGridView.CurrentCellChanged += TrigSeq_DataGridView_CurrentCellChanged;
                    TrigSeq_DataGridView.CurrentCell = TrigSeq_DataGridView.Rows[index].Cells[1];
                    TrigSeq_DataGridView.CurrentRow.Cells[0].Style.BackColor = Color.White;
                    TrigSeq_DataGridView.CurrentRow.Cells[1].Style.BackColor = Color.White;
                    TrigSeq_DataGridView.CurrentRow.Cells[2].Style.BackColor = Color.White;

                    TrigSeqChanged?.Invoke(this, new RegSettingChangedEventArgs(nameof(TriggerSequence), TriggerSequence));
                }
            }
        }
    }
}