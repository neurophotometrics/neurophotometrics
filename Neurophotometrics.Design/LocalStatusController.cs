using System;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace Neurophotometrics.Design
{
    partial class LocalStatusController : UserControl
    {
        /****** DEBUG VARIABLES ******/
        private int layoutCount = 0;

        /****** PRIVATE GLOBAL VARIABLES TO BE PUSHED TO REACTIVE GRAPHS *****/
        private int roiNumber;
        private bool[] foundLEDs;
        private bool deinterleave;
        private bool[] roiVisible;
        private bool[] autoScales;
        private double[] mins;
        private double[] maxes;
        private bool localConfigMode;
        private int stateIndex = 0;

        /***** PRIVATE CONSTANTS FOR FORMATTING *****/
        private const int LocalPopOutControllerHeight = 30;
        private const int LocalStatusControllerWidth = 200;
        private const int VerticalMargin = 5;
        private const int HorizontalMargin = 5;

        /***** DEFAULT CONSTRUCTOR ******/
        public LocalStatusController()
        {
        }

        /***** CONSTRUCTOR WITH INITIALIZATION ******/
        public LocalStatusController(Tuple<Tuple<int, bool[], bool>, Tuple<bool[], bool[], double[], double[]>> InitProps)
        {
            // Initialize ReactiveGraph variables
            roiNumber = InitProps.Item1.Item1;
            foundLEDs = InitProps.Item1.Item2;
            deinterleave = InitProps.Item1.Item3;
            roiVisible = InitProps.Item2.Item1;
            autoScales = InitProps.Item2.Item2;
            mins = InitProps.Item2.Item3;
            maxes = InitProps.Item2.Item4;
            localConfigMode = false;

            InitializeComponent();


        }

        /****** PUBLIC GLOBAL VARIABLES *****/
        public virtual int ROINumber
        {
            get { return roiNumber; }
        }

        public virtual bool[] FoundLEDs
        {
            get { return foundLEDs; }
            set
            {
                foundLEDs = value;

                bool[] tempROIsVisible = new bool[FoundLEDs.Length + 1];
                bool[] tempAutoScales = new bool[FoundLEDs.Length + 1];
                double[] tempMins = new double[FoundLEDs.Length + 1];
                double[] tempMaxes = new double[FoundLEDs.Length + 1];

                for (int j = 0; j < FoundLEDs.Length + 1; j++)
                {
                    tempROIsVisible[j] = true;
                    tempAutoScales[j] = true;
                    tempMins[j] = 0.0;
                    tempMaxes[j] = 1.0;
                }
                roiVisible = tempROIsVisible;
                autoScales = tempAutoScales;
                mins = tempMins;
                maxes = tempMaxes;

                UpdatePositions();
            }
        }

        public virtual bool Deinterleave
        {
            get { return deinterleave; }
            set
            {
                deinterleave = value;
                Deinterleave_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool[] ROIVisible
        {
            get { return roiVisible; }
            set
            {
                roiVisible = value;
                ROIVisible_Changed(this, EventArgs.Empty);
            }

        }

        public virtual bool[] AutoScales
        {
            get { return autoScales; }
            set
            {
                autoScales = value;
                AutoScales_Changed(this, EventArgs.Empty);
            }
        }

        public virtual double[] Mins
        {
            get { return mins; }
            set
            {
                mins = value;
                Mins_Changed(this, EventArgs.Empty);
            }
        }

        public virtual double[] Maxes
        {
            get { return maxes; }
            set
            {
                maxes = value;
                Maxes_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool LocalConfigMode
        {
            get { return localConfigMode; }
            set
            {
                localConfigMode = value;
                LocalConfigMode_Changed(this, EventArgs.Empty);
            }
        }

        public virtual int StateIndex
        {
            get { return stateIndex; }
            set
            {
                stateIndex = value;
                StateIndex_Changed(this, EventArgs.Empty);
            }
        }

        public virtual UserControl State
        {
            get { return state; }
        }

        /******************** EVENT HANDLERS LOCAL POP OUT CONTROLLER ********************/
        public event EventHandler LocalConfigModeChanged;

        protected virtual void OnLocalConfigModeChanged(EventArgs e)
        {
            var handler = LocalConfigModeChanged;
            if (handler != null)
            {
                LocalConfigModeChanged.Invoke(this, e);
            }
        }

        private void LocalConfigMode_Changed(object sender, EventArgs e)
        {
            OnLocalConfigModeChanged(e);
        }

        private void localPopOutController_Click(object sender, EventArgs e)
        {
            localPopOutControllerLabel.Focus();
            LocalConfigMode = !LocalConfigMode;
        }

        /******************** EVENT HANDLERS LOCAL CONFIG CONTROLLER ********************/
        private void plotVisible_Click(object sender, EventArgs e)
        {
            bool newPlotVisible = !ROIVisible[0];
            bool[] tempROIVisible = new bool[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                tempROIVisible[i] = ROIVisible[i];
            }
            tempROIVisible[0] = newPlotVisible;


            var button = (Button)sender;
            if (newPlotVisible)
            {
                button.Text = "True";
                button.ForeColor = Color.FromArgb(0, 90, 181);
            }
            else
            {
                button.Text = "False";
                button.ForeColor = Color.FromArgb(220, 50, 32);
            }
            button.Focus();
            if (newPlotVisible)
            {
                if (Deinterleave)
                {
                    NewState(2);
                }
                else
                {
                    NewState(1);
                }
            }
            else
            {
                NewState(0);
            }

            ROIVisible = tempROIVisible;
        }

        private void deinterleave_Click(object sender, EventArgs e)
        {
            bool newDeinterleave = !Deinterleave;
            var button = (Button)sender;
            if (newDeinterleave)
            {
                button.Text = "True";
                button.ForeColor = Color.FromArgb(0, 90, 181);
            }
            else
            {
                button.Text = "False";
                button.ForeColor = Color.FromArgb(220, 50, 32);
            }
            button.Focus();

            if (newDeinterleave)
            {
                NewState(2);
            }
            else
            {
                NewState(1);
            }

            Deinterleave = newDeinterleave;
        }

        private void autoScale_Click(object sender, EventArgs e)
        {
            bool newAutoScale = !AutoScales[0];
            bool[] tempAutoScales = new bool[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                tempAutoScales[i] = AutoScales[i];
            }
            tempAutoScales[0] = newAutoScale;

            var button = (Button)sender;
            if (newAutoScale)
            {
                button.Text = "True";
                button.ForeColor = Color.FromArgb(0, 90, 181);
            }
            else
            {
                button.Text = "False";
                button.ForeColor = Color.FromArgb(220, 50, 32);
            }
            button.Focus();

            AutoScales = tempAutoScales;
        }

        private void min_Click(object sender, EventArgs e)
        {
            var valLabel = (Label)sender;
            valLabel.Focus();
            if (!AutoScales[0])
            {

                var tempTextBox = textBox;
                tempTextBox.Name = valLabel.Name;
                tempTextBox.Bounds = valLabel.Bounds;
                tempTextBox.Text = valLabel.Text;
                tempTextBox.Font = valLabel.Font;
                tempTextBox.TextAlign = HorizontalAlignment.Center;
                minController.SuspendLayout();
                minController.Controls.Remove(valLabel);
                minController.Controls.Add(tempTextBox);
                tempTextBox.Focus();
            }
        }

        private void max_Click(object sender, EventArgs e)
        {
            var valLabel = (Label)sender;
            valLabel.Focus();
            if (!AutoScales[0])
            {
                var tempTextBox = textBox;
                tempTextBox.Name = valLabel.Name;
                tempTextBox.Bounds = valLabel.Bounds;
                tempTextBox.Text = valLabel.Text;
                tempTextBox.Font = valLabel.Font;
                tempTextBox.TextAlign = HorizontalAlignment.Center;
                maxController.SuspendLayout();
                maxController.Controls.Remove(valLabel);
                maxController.Controls.Add(tempTextBox);
                tempTextBox.Focus();
            }
        }

        private void minAxis_Click(object sender, EventArgs e)
        {
            var valLabel = (Label)sender;
            valLabel.Focus();

            int index = 0;
            string LED = valLabel.Name.Split('_')[1];
            if (LED.Equals("415"))
            {
                index = 1;
            }
            else if (LED.Equals("470"))
            {
                index = 2;
            }
            else if (LED.Equals("560"))
            {
                index = 3;
            }
            if (!AutoScales[index])
            {
                var tempTextBox = textBox;
                tempTextBox.Name = valLabel.Name;
                tempTextBox.Bounds = valLabel.Bounds;
                tempTextBox.Text = valLabel.Text;
                tempTextBox.Font = valLabel.Font;
                tempTextBox.TextAlign = HorizontalAlignment.Center;
                minValuesController.SuspendLayout();
                minValuesController.Controls.Remove(valLabel);
                minValuesController.Controls.Add(tempTextBox);
                tempTextBox.Focus();
            }

        }

        private void maxAxis_Click(object sender, EventArgs e)
        {
            var valLabel = (Label)sender;
            valLabel.Focus();

            int index = 0;
            string LED = valLabel.Name.Split('_')[1];
            if (LED.Equals("415"))
            {
                index = 1;
            }
            else if (LED.Equals("470"))
            {
                index = 2;
            }
            else if (LED.Equals("560"))
            {
                index = 3;
            }

            if (!AutoScales[index])
            {
                var tempTextBox = textBox;
                tempTextBox.Name = valLabel.Name;
                tempTextBox.Bounds = valLabel.Bounds;
                tempTextBox.Text = valLabel.Text;
                tempTextBox.Font = valLabel.Font;
                tempTextBox.TextAlign = HorizontalAlignment.Center;
                maxValuesController.SuspendLayout();
                maxValuesController.Controls.Remove(valLabel);
                maxValuesController.Controls.Add(tempTextBox);
                tempTextBox.Focus();
            }
        }

        private void axisVisible_Click(object sender, EventArgs e)
        {
            bool[] tempROIVisible = new bool[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                tempROIVisible[i] = ROIVisible[i];
            }

            var button = (Button)sender;
            string LED = button.Name.Split('_')[1];
            if (LED.Equals("415"))
            {
                tempROIVisible[1] = !ROIVisible[1];
                if (tempROIVisible[1])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }
            else if (LED.Equals("470"))
            {
                tempROIVisible[2] = !ROIVisible[2];
                if (tempROIVisible[2])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }
            else
            {
                tempROIVisible[3] = !ROIVisible[3];
                if (tempROIVisible[3])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }


            ROIVisible = tempROIVisible;
        }

        private void autoScaleLED_Click(object sender, EventArgs e)
        {
            bool[] tempAutoScales = new bool[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                tempAutoScales[i] = AutoScales[i];
            }

            var button = (Button)sender;
            string LED = button.Name.Split('_')[1];
            if (LED.Equals("415"))
            {
                tempAutoScales[1] = !AutoScales[1];
                if (tempAutoScales[1])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }
            else if (LED.Equals("470"))
            {
                tempAutoScales[2] = !AutoScales[2];
                if (tempAutoScales[2])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }
            else
            {
                tempAutoScales[3] = !AutoScales[3];
                if (tempAutoScales[3])
                {
                    button.Text = "True";
                    button.ForeColor = Color.FromArgb(0, 90, 181);
                }
                else
                {
                    button.Text = "False";
                    button.ForeColor = Color.FromArgb(220, 50, 32);
                }
            }


            AutoScales = tempAutoScales;
        }

        private void textBox_LostFocus(object sender, EventArgs e)
        {
            var tempTextBox = (TextBox)sender;
            string valType = tempTextBox.Name.Split('_')[0];
            double val;
            if (double.TryParse(tempTextBox.Text, out val))
            {
                if (valType.Equals("minControllerValue"))
                {
                    double[] tempMins = new double[FoundLEDs.Length + 1];
                    for (int i = 0; i < FoundLEDs.Length + 1; i++)
                    {
                        tempMins[i] = Mins[i];
                    }
                    tempMins[0] = val;
                    Mins = tempMins;

                    minController.Value.Text = Mins[0].ToString("0.###");

                    var minLabel = minController.Value;
                    minController.SuspendLayout();
                    minController.Controls.Remove(tempTextBox);
                    minController.Controls.Add(minLabel);
                    minController.ResumeLayout();
                }
                else if (valType.Equals("maxControllerValue"))
                {
                    double[] tempMaxes = new double[FoundLEDs.Length + 1];
                    for (int i = 0; i < FoundLEDs.Length + 1; i++)
                    {
                        tempMaxes[i] = Maxes[i];
                    }
                    tempMaxes[0] = val;
                    Maxes = tempMaxes;

                    maxController.Value.Text = Maxes[0].ToString("0.###");

                    var maxLabel = maxController.Value;
                    maxController.SuspendLayout();
                    maxController.Controls.Remove(tempTextBox);
                    maxController.Controls.Add(maxLabel);
                    maxController.ResumeLayout();
                }
                else if (valType.Equals("minLabel"))
                {
                    double[] tempMins = new double[FoundLEDs.Length + 1];
                    for (int i = 0; i < FoundLEDs.Length + 1; i++)
                    {
                        tempMins[i] = Mins[i];
                    }

                    string LED = tempTextBox.Name.Split('_')[1];
                    Label minLabel = new Label();
                    if (LED.Equals("415"))
                    {
                        tempMins[1] = val;
                        min415Label.Text = tempMins[1].ToString("0.###");
                        minLabel = min415Label;
                    }
                    else if (LED.Equals("470"))
                    {
                        tempMins[2] = val;
                        min470Label.Text = tempMins[2].ToString("0.###");
                        minLabel = min470Label;
                    }
                    else if (LED.Equals("560"))
                    {
                        tempMins[3] = val;
                        min560Label.Text = tempMins[3].ToString("0.###");
                        minLabel = min560Label;
                    }
                    minValuesController.SuspendLayout();
                    minValuesController.Controls.Remove(tempTextBox);
                    minValuesController.Controls.Add(minLabel);
                    minValuesController.ResumeLayout();
                    Mins = tempMins;
                }
                else if (valType.Equals("maxLabel"))
                {
                    double[] tempMaxes = new double[FoundLEDs.Length + 1];
                    for (int i = 0; i < FoundLEDs.Length + 1; i++)
                    {
                        tempMaxes[i] = Maxes[i];
                    }

                    string LED = tempTextBox.Name.Split('_')[1];
                    Label maxLabel = new Label();
                    if (LED.Equals("415"))
                    {
                        tempMaxes[1] = val;
                        max415Label.Text = tempMaxes[1].ToString("0.###");
                        maxLabel = max415Label;
                    }
                    else if (LED.Equals("470"))
                    {
                        tempMaxes[2] = val;
                        max470Label.Text = tempMaxes[2].ToString("0.###");
                        maxLabel = max470Label;
                    }
                    else if (LED.Equals("560"))
                    {
                        tempMaxes[3] = val;
                        max560Label.Text = tempMaxes[3].ToString("0.###");
                        maxLabel = max560Label;
                    }
                    maxValuesController.SuspendLayout();
                    maxValuesController.Controls.Remove(tempTextBox);
                    maxValuesController.Controls.Add(maxLabel);
                    maxValuesController.ResumeLayout();
                    Maxes = tempMaxes;
                }
            }

        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                var tempTextBox = (TextBox)sender;
                string valType = tempTextBox.Name.Split('_')[0];
                double val;
                if (double.TryParse(tempTextBox.Text, out val))
                {
                    if (valType.Equals("minControllerValue"))
                    {
                        double[] tempMins = new double[FoundLEDs.Length + 1];
                        for (int i = 0; i < FoundLEDs.Length + 1; i++)
                        {
                            tempMins[i] = Mins[i];
                        }
                        tempMins[0] = val;
                        Mins = tempMins;

                        minController.Value.Text = Mins[0].ToString(CultureInfo.InvariantCulture);

                        var minLabel = minController.Value;
                        minController.SuspendLayout();
                        minController.Controls.Remove(tempTextBox);
                        minController.Controls.Add(minLabel);
                        minController.ResumeLayout();
                    }
                    else if (valType.Equals("maxControllerValue"))
                    {
                        double[] tempMaxes = new double[FoundLEDs.Length + 1];
                        for (int i = 0; i < FoundLEDs.Length + 1; i++)
                        {
                            tempMaxes[i] = Maxes[i];
                        }
                        tempMaxes[0] = val;
                        Maxes = tempMaxes;

                        maxController.Value.Text = Maxes[0].ToString(CultureInfo.InvariantCulture);

                        var maxLabel = maxController.Value;
                        maxController.SuspendLayout();
                        maxController.Controls.Remove(tempTextBox);
                        maxController.Controls.Add(maxLabel);
                        maxController.ResumeLayout();
                    }
                    else if (valType.Equals("minLabel"))
                    {
                        double[] tempMins = new double[FoundLEDs.Length + 1];
                        for (int i = 0; i < FoundLEDs.Length + 1; i++)
                        {
                            tempMins[i] = Mins[i];
                        }

                        string LED = tempTextBox.Name.Split('_')[1];
                        Label minLabel = new Label();
                        if (LED.Equals("415"))
                        {
                            tempMins[1] = val;
                            min415Label.Text = tempMins[1].ToString(CultureInfo.InvariantCulture);
                            minLabel = min415Label;
                        }
                        else if (LED.Equals("470"))
                        {
                            tempMins[2] = val;
                            min470Label.Text = tempMins[2].ToString(CultureInfo.InvariantCulture);
                            minLabel = min470Label;
                        }
                        else if (LED.Equals("560"))
                        {
                            tempMins[3] = val;
                            min560Label.Text = tempMins[3].ToString(CultureInfo.InvariantCulture);
                            minLabel = min560Label;
                        }
                        minValuesController.SuspendLayout();
                        minValuesController.Controls.Remove(tempTextBox);
                        minValuesController.Controls.Add(minLabel);
                        minValuesController.ResumeLayout();
                        Mins = tempMins;
                    }
                    else if (valType.Equals("maxLabel"))
                    {
                        double[] tempMaxes = new double[FoundLEDs.Length + 1];
                        for (int i = 0; i < FoundLEDs.Length + 1; i++)
                        {
                            tempMaxes[i] = Maxes[i];
                        }

                        string LED = tempTextBox.Name.Split('_')[1];
                        Label maxLabel = new Label();
                        if (LED.Equals("415"))
                        {
                            tempMaxes[1] = val;
                            max415Label.Text = tempMaxes[1].ToString(CultureInfo.InvariantCulture);
                            maxLabel = max415Label;
                        }
                        else if (LED.Equals("470"))
                        {
                            tempMaxes[2] = val;
                            max470Label.Text = tempMaxes[2].ToString(CultureInfo.InvariantCulture);
                            maxLabel = max470Label;
                        }
                        else if (LED.Equals("560"))
                        {
                            tempMaxes[3] = val;
                            max560Label.Text = tempMaxes[3].ToString(CultureInfo.InvariantCulture);
                            maxLabel = max560Label;
                        }
                        maxValuesController.SuspendLayout();
                        maxValuesController.Controls.Remove(tempTextBox);
                        maxValuesController.Controls.Add(maxLabel);
                        maxValuesController.ResumeLayout();
                        Maxes = tempMaxes;
                    }
                }
            }
        }

        private void Null_Click(object sender, EventArgs e)
        {

            if (sender is Label label)
            {
                label.Focus();
            }
            else if (sender is UserControl controller)
            {
                controller.Focus();
            }
        }

        /******************** EVENT HANDLERS FOR PUSHING VARIABLES FROM LOCAL STATUS CONTROLLER TO GLOBAL STATUS CONTROLLER ********************/
        /***** PUBLIC EVENT HANDLERS ******/
        public event EventHandler ROIVisibleChanged;

        public event EventHandler DeinterleaveChanged;

        public event EventHandler AutoScalesChanged;

        public event EventHandler MinsChanged;

        public event EventHandler MaxesChanged;

        public event EventHandler StateIndexChanged;
        /****** INVOKE EVENT HANDLERS *****/
        protected virtual void OnROIVisibleChanged(EventArgs e)
        {
            var handler = ROIVisibleChanged;
            if (handler != null)
            {
                ROIVisibleChanged.Invoke(this, e);
            }
        }

        protected virtual void OnDeinterleaveChanged(EventArgs e)
        {
            var handler = DeinterleaveChanged;
            if (handler != null)
            {
                DeinterleaveChanged.Invoke(this, e);
            }
        }

        protected virtual void OnAutoScalesChanged(EventArgs e)
        {
            var handler = AutoScalesChanged;
            if (handler != null)
            {
                AutoScalesChanged.Invoke(this, e);
            }
        }

        protected virtual void OnMinsChanged(EventArgs e)
        {
            var handler = MinsChanged;
            if (handler != null)
            {
                MinsChanged.Invoke(this, e);
            }
        }

        protected virtual void OnMaxesChanged(EventArgs e)
        {
            var handler = MaxesChanged;
            if (handler != null)
            {
                MaxesChanged.Invoke(this, e);
            }
        }

        protected virtual void OnStateIndexChanged(EventArgs e)
        {
            var handler = StateIndexChanged;
            if (handler != null)
            {
                StateIndexChanged.Invoke(this, e);
            }
        }
        /****** EVENT HANDLER FUNCTIONS ******/
        private void ROIVisible_Changed(object sender, EventArgs e)
        {
            OnROIVisibleChanged(e);
        }

        private void Deinterleave_Changed(object sender, EventArgs e)
        {
            OnDeinterleaveChanged(e);
        }

        private void AutoScales_Changed(object sender, EventArgs e)
        {
            OnAutoScalesChanged(e);
        }

        private void Mins_Changed(object sender, EventArgs e)
        {
            OnMinsChanged(e);
        }

        private void Maxes_Changed(object sender, EventArgs e)
        {
            OnMaxesChanged(e);
        }
        private void StateIndex_Changed(object sender, EventArgs e)
        {
            OnStateIndexChanged(e);
        }
        /******************** EVENT HANDLERS RESIZING/FORMATTING ********************/
        public void UpdateMins(double[] newMins)
        {
            SuspendLayout();
            bool changed = false;
            double[] tempMins = Mins;
            if ((mins[0] != newMins[0]) && autoScales[0])
            {
                minController.Value.Text = newMins[0].ToString("0.###");
                tempMins[0] = newMins[0];
                changed = true;
            }
            else if ((mins[1] != newMins[1]) && autoScales[1])
            {
                min415Label.Text = newMins[1].ToString("0.###");
                tempMins[1] = newMins[1];
                changed = true;
            }
            else if ((mins[2] != newMins[2]) && autoScales[2])
            {
                min470Label.Text = newMins[2].ToString("0.###");
                tempMins[2] = newMins[2];
                changed = true;
            }
            else if ((mins[3] != newMins[3]) && autoScales[3])
            {
                min560Label.Text = newMins[3].ToString("0.###");
                tempMins[3] = newMins[3];
                changed = true;
            }
            if (changed)
            {
                mins = newMins;
            }
            EnsureLayout();
            ResumeLayout();
        }

        public void UpdateMaxes(double[] newMaxes)
        {
            SuspendLayout();
            bool changed = false;
            double[] tempMaxes = Maxes;
            if ((maxes[0] != newMaxes[0]) && autoScales[0])
            {
                maxController.Value.Text = newMaxes[0].ToString("0.###");
                tempMaxes[0] = newMaxes[0];
                changed = true;
            }
            else if ((maxes[1] != newMaxes[1]) && autoScales[1])
            {
                max415Label.Text = newMaxes[1].ToString("0.###");
                tempMaxes[1] = newMaxes[1];
                changed = true;
            }
            else if ((maxes[2] != newMaxes[2]) && autoScales[2])
            {
                max470Label.Text = newMaxes[2].ToString("0.###");
                tempMaxes[2] = newMaxes[2];
                changed = true;
            }
            else if ((maxes[3] != newMaxes[3]) && autoScales[3])
            {
                max560Label.Text = newMaxes[3].ToString("0.###");
                tempMaxes[3] = newMaxes[3];
                changed = true;
            }
            if (changed)
            {

                // Fix here, currently it is updating ALL if any are changed!!!
                maxes = tempMaxes;
            }
            EnsureLayout();
            ResumeLayout();
        }

        public void ColorBlindMode(bool inColorBlindMode)
        {
            if (inColorBlindMode)
            {
                axis415Controller.Label.ForeColor = Color.FromArgb(30, 136, 229);
                axis470Controller.Label.ForeColor = Color.FromArgb(255, 193, 7);
                axis560Controller.Label.ForeColor = Color.FromArgb(216, 27, 96);
                autoScale415Controller.Label.ForeColor = Color.FromArgb(30, 136, 229);
                autoScale470Controller.Label.ForeColor = Color.FromArgb(255, 193, 7);
                autoScale560Controller.Label.ForeColor = Color.FromArgb(216, 27, 96);
                Label0.ForeColor = Color.FromArgb(30, 136, 229);
                Label1.ForeColor = Color.FromArgb(255, 193, 7);
                Label2.ForeColor = Color.FromArgb(216, 27, 96);
                min415Label.ForeColor = Color.FromArgb(30, 136, 229);
                min470Label.ForeColor = Color.FromArgb(255, 193, 7);
                min560Label.ForeColor = Color.FromArgb(216, 27, 96);
                max415Label.ForeColor = Color.FromArgb(30, 136, 229);
                max470Label.ForeColor = Color.FromArgb(255, 193, 7);
                max560Label.ForeColor = Color.FromArgb(216, 27, 96);
            }
            else
            {
                axis415Controller.Label.ForeColor = Color.Purple;
                axis470Controller.Label.ForeColor = Color.Green;
                axis560Controller.Label.ForeColor = Color.Red;
                autoScale415Controller.Label.ForeColor = Color.Purple;
                autoScale470Controller.Label.ForeColor = Color.Green;
                autoScale560Controller.Label.ForeColor = Color.Red;
                Label0.ForeColor = Color.Purple;
                Label1.ForeColor = Color.Green;
                Label2.ForeColor = Color.Red;
                min415Label.ForeColor = Color.Purple;
                min470Label.ForeColor = Color.Green;
                min560Label.ForeColor = Color.Red;
                max415Label.ForeColor = Color.Purple;
                max470Label.ForeColor = Color.Green;
                max560Label.ForeColor = Color.Red;
            }
        }

        private void UpdatePositions()
        {
            state.SuspendLayout();

        }

        private void NewState(int i)
        {
            state.SuspendLayout();
            state.Controls.Clear();
            if (i == 0)
            {
                state.Height = LocalPopOutControllerHeight;
                //state.BackColor = Color.Red;
                state.Controls.Add(plotVisibleController);
                StateIndex = 0;
            }
            else if (i == 1)
            {
                state.Height = 5 * LocalPopOutControllerHeight + 4 * VerticalMargin;
                //state.BackColor = Color.Green;
                state.Controls.Add(plotVisibleController);
                state.Controls.Add(deinterleaveController);
                state.Controls.Add(autoScaleController);
                state.Controls.Add(minController);
                state.Controls.Add(maxController);
                StateIndex = 1;
            }
            else
            {
                //state.BackColor = Color.Green;
                state.Controls.Add(plotVisibleController);
                state.Controls.Add(deinterleaveController);
                axisLabelMultiButtonController.SuspendLayout();
                autoScaleLabelMultiButtonController.SuspendLayout();
                minValuesController.SuspendLayout();
                maxValuesController.SuspendLayout();
                axisLabelMultiButtonController.Controls.Clear();
                autoScaleLabelMultiButtonController.Controls.Clear();
                minValuesController.Controls.Clear();
                maxValuesController.Controls.Clear();
                axisLabelMultiButtonController.Controls.Add(axisLabel);
                autoScaleLabelMultiButtonController.Controls.Add(autoScaleLabel);
                minValuesController.Controls.Add(minLabel);
                maxValuesController.Controls.Add(maxLabel);
                int tempROIVisibleHeight = LocalPopOutControllerHeight;
                int tempAutoScaleHeight = LocalPopOutControllerHeight;
                int totLEDs = 0;
                if (FoundLEDs[0])
                {
                    tempROIVisibleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    tempAutoScaleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    axisLabelMultiButtonController.Controls.Add(axis415Controller);
                    autoScaleLabelMultiButtonController.Controls.Add(autoScale415Controller);
                    minMaxLabels.Controls.Add(Label0);
                    minValuesController.Controls.Add(min415Label);
                    maxValuesController.Controls.Add(max415Label);
                    totLEDs += 1;
                }
                if (FoundLEDs[1])
                {
                    tempROIVisibleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    tempAutoScaleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    axis470Controller.SuspendLayout();
                    autoScale470Controller.SuspendLayout();
                    axis470Controller.Location = new Point(0, (totLEDs + 1) * (LocalPopOutControllerHeight + VerticalMargin));
                    autoScale470Controller.Location = new Point(0, (totLEDs + 1) * (LocalPopOutControllerHeight + VerticalMargin));
                    axisLabelMultiButtonController.Controls.Add(axis470Controller);
                    autoScaleLabelMultiButtonController.Controls.Add(autoScale470Controller);
                    minMaxLabels.Controls.Add(Label1);
                    minValuesController.Controls.Add(min470Label);
                    maxValuesController.Controls.Add(max470Label);
                    totLEDs += 1;
                }
                if (FoundLEDs[2])
                {
                    tempROIVisibleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    tempAutoScaleHeight += LocalPopOutControllerHeight + VerticalMargin;
                    axis560Controller.SuspendLayout();
                    autoScale560Controller.SuspendLayout();
                    axis560Controller.Location = new Point(0, (totLEDs + 1) * (LocalPopOutControllerHeight + VerticalMargin));
                    autoScale560Controller.Location = new Point(0, (totLEDs + 1) * (LocalPopOutControllerHeight + VerticalMargin));
                    axisLabelMultiButtonController.Controls.Add(axis560Controller);
                    autoScaleLabelMultiButtonController.Controls.Add(autoScale560Controller);
                    minMaxLabels.Controls.Add(Label2);
                    minValuesController.Controls.Add(min560Label);
                    maxValuesController.Controls.Add(max560Label);
                }

                axisLabelMultiButtonController.Size = new Size(LocalStatusControllerWidth, tempROIVisibleHeight);
                autoScaleLabelMultiButtonController.Bounds = new Rectangle(0, 2 * LocalPopOutControllerHeight + 2 * VerticalMargin + tempROIVisibleHeight, LocalStatusControllerWidth, tempAutoScaleHeight);
                minMaxLabelMultiValue2DController.Location = new Point(0, 2 * LocalPopOutControllerHeight + 2 * VerticalMargin + tempROIVisibleHeight + tempAutoScaleHeight);
                minMaxLabelMultiValue2DController.Controls.Add(minMaxLabels);
                minMaxLabelMultiValue2DController.Controls.Add(minValuesController);
                minMaxLabelMultiValue2DController.Controls.Add(maxValuesController);
                state.Controls.Add(axisLabelMultiButtonController);
                state.Controls.Add(autoScaleLabelMultiButtonController);
                state.Controls.Add(minMaxLabelMultiValue2DController);
                state.Height = 2 * LocalPopOutControllerHeight + 2 * VerticalMargin + tempROIVisibleHeight + tempAutoScaleHeight + minMaxLabelMultiValue2DController.Height;
                StateIndex = 2;
            }

            state.ResumeLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            EnsureLayout();
            ResumeLayout();
        }

        private void EnsureLayout()
        {
            localPopOutController.Width = Width;
            localPopOutControllerLabel.Width = Width - 2 * (LocalPopOutControllerHeight + HorizontalMargin);
            localPopOutControllerLabel2.Location = new Point(LocalPopOutControllerHeight + 2 * HorizontalMargin + localPopOutControllerLabel.Width);
            if (LocalConfigMode)
            {
                localPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2191);
                localPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2191);
                localConfigController.Bounds = new Rectangle(0, LocalPopOutControllerHeight, Width, state.Height);
                localConfigController.Visible = true;
            }
            else
            {
                localPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2193);
                localPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2193);
                localConfigController.Visible = false;
            }
        }
    }
}
