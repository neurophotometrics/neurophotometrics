using System;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace Neurophotometrics.Design
{
    partial class GlobalStatusController : UserControl
    {
        /****** DEBUG VARIABLES ******/
        private int layoutCount = 0;

        /***** GLOBAL VARIABLES *****/
        private int roiCount;
        private bool[] foundLEDs;
        private int capacity;
        private bool[] deinterleaves;
        private bool[,] roisVisible;
        private bool[,] autoScales;
        private double[,] mins;
        private double[,] maxes;

        /***** PRIVATE CONSTANTS FOR FORMATTING *****/
        private const int PopOutControllerHeight = 30;
        private const int VerticalMargin = 5;
        private const int HorizontalMargin = 5;
        private const int MinLocalConfigControllerWidth = 200;

        /****** PRIVATE VARIABLES FOR FORMATTING *****/
        private bool inConfigMode = false;
        private bool inColorBlindMode = false;
        private LocalStatusController[] localStatusControllerList;
        private int[] matrix;
        private bool[] localConfigModes;
        //private float dpiX;
        //private float dpiY;


        /***** DEFAULT CONSTRUCTOR ******/
        public GlobalStatusController()
        {
        }

        /***** CONSTRUCTOR WITH INITIALIZATION ******/
        public GlobalStatusController(Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> InitProps)
        {
            //Graphics graphics = this.CreateGraphics();
            //dpiX = graphics.DpiX;
            //dpiY = graphics.DpiY;

            //Console.WriteLine("DPI X = {0}, DPI Y = {1}", dpiX, dpiY);
            // Initialize GlobalStatusController variables
            roiCount = InitProps.Item1.Item1;
            foundLEDs = InitProps.Item1.Item2;
            capacity = InitProps.Item1.Item3;
            deinterleaves = InitProps.Item2.Item1;
            roisVisible = InitProps.Item3.Item1;
            autoScales = InitProps.Item3.Item2;
            mins = InitProps.Item3.Item3;
            maxes = InitProps.Item3.Item4;

            matrix = new int[2] { 1, 1 };
            //WriteCurrentState(1);
            int roiNum = 0;
            bool[] roiFoundLEDs = FoundLEDs;
            bool roiDeinterleave = Deinterleaves[0];
            bool[] roiVisible = new bool[FoundLEDs.Length + 1];
            bool[] roiAutoScales = new bool[FoundLEDs.Length + 1];
            double[] roiMins = new double[FoundLEDs.Length + 1];
            double[] roiMaxes = new double[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                roiVisible[i] = ROIsVisible[0, i];
                roiAutoScales[i] = AutoScales[0, i];
                roiMins[i] = Mins[0, i];
                roiMaxes[i] = Maxes[0, i];
            }
            Tuple<int, bool[], bool> ROIGlobals = Tuple.Create(roiNum, roiFoundLEDs, roiDeinterleave);
            Tuple<bool[], bool[], double[], double[]> ROILocals = Tuple.Create(roiVisible, roiAutoScales, roiMins, roiMaxes);

            Tuple<Tuple<int, bool[], bool>, Tuple<bool[], bool[], double[], double[]>> ROIData = Tuple.Create(ROIGlobals, ROILocals);

            InitializeComponent(ROIData);
        }

        /****** PUBLIC GLOBAL VARIABLES TO BE PUSHED TO MASTER CONTROLLER *****/
        public virtual int ROICount
        {
            get { return roiCount; }
            set
            {
                roiCount = value;

                // Update Global Status Controller Variables
                bool[] tempLocalConfigModes = new bool[ROICount];
                bool[] tempDeinterleaves = new bool[ROICount];
                bool[,] tempROIsVisible = new bool[ROICount, FoundLEDs.Length + 1];
                bool[,] tempAutoScales = new bool[ROICount, FoundLEDs.Length + 1];
                double[,] tempMins = new double[ROICount, FoundLEDs.Length + 1];
                double[,] tempMaxes = new double[ROICount, FoundLEDs.Length + 1];
                for (int i = 0; i < ROICount; i++)
                {
                    tempDeinterleaves[i] = false;
                    tempLocalConfigModes[i] = false;
                    for (int j = 0; j < FoundLEDs.Length + 1; j++)
                    {

                        tempROIsVisible[i, j] = true;
                        tempAutoScales[i, j] = true;
                        tempMins[i, j] = 0.0;
                        tempMaxes[i, j] = 1.0;
                    }
                }
                deinterleaves = tempDeinterleaves;
                roisVisible = tempROIsVisible;
                autoScales = tempAutoScales;
                mins = tempMins;
                maxes = tempMaxes;
                localConfigModes = tempLocalConfigModes;


                //WriteCurrentState(2);

                AddROIControllers();
            }
        }
        public virtual bool[] FoundLEDs
        {
            get { return foundLEDs; }
            set
            {
                foundLEDs = value;

                bool[,] tempROIsVisible = new bool[ROICount, FoundLEDs.Length + 1];
                bool[,] tempAutoScales = new bool[ROICount, FoundLEDs.Length + 1];
                double[,] tempMins = new double[ROICount, FoundLEDs.Length + 1];
                double[,] tempMaxes = new double[ROICount, FoundLEDs.Length + 1];

                for (int i = 0; i < ROICount; i++)
                {
                    for (int j = 0; j < FoundLEDs.Length + 1; j++)
                    {
                        tempROIsVisible[i, j] = true;
                        tempAutoScales[i, j] = true;
                        tempMins[i, j] = 0.0;
                        tempMaxes[i, j] = 1.0;
                    }
                }
                roisVisible = tempROIsVisible;
                autoScales = tempAutoScales;
                mins = tempMins;
                maxes = tempMaxes;

                //WriteCurrentState(2);

                for (int i = 0; i < ROICount; i++)
                {
                    LocalStatusControllerList[i].FoundLEDs = FoundLEDs;
                }
            }
        }
        public virtual int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                Capacity_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool[] Deinterleaves
        {
            get { return deinterleaves; }
            set
            {
                deinterleaves = value;
                Deinterleaves_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool[,] ROIsVisible
        {
            get { return roisVisible; }
            set
            {
                roisVisible = value;
                ROIsVisible_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool[,] AutoScales
        {
            get { return autoScales; }
            set
            {
                autoScales = value;
                AutoScales_Changed(this, EventArgs.Empty);
            }
        }

        public virtual double[,] Mins
        {
            get { return mins; }
            set
            {
                mins = value;
                Mins_Changed(this, EventArgs.Empty);
            }
        }

        public virtual double[,] Maxes
        {
            get { return maxes; }
            set
            {
                maxes = value;
                Maxes_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool InConfigMode
        {
            get { return inConfigMode; }
            set
            {
                inConfigMode = value;
                ConfigMode_Changed(this, EventArgs.Empty);
            }
        }

        public virtual bool InColorBlindMode
        {
            get { return inColorBlindMode; }
            set
            {
                inColorBlindMode = value;
                ColorBlindMode_Changed(this, EventArgs.Empty);
            }
        }

        public virtual int[] Matrix
        {
            get { return matrix; }
            set
            {
                matrix = value;
            }
        }

        public virtual bool[] LocalConfigModes
        {
            get { return localConfigModes; }
            set
            {
                localConfigModes = value;
                SuspendLayout();
                EnsureLayout();
                ResumeLayout();
            }
        }

        /***** REACTIVELY ADD ROI CONTROLLERS WHEN ROICOUNT CHANGED *****/
        private LocalStatusController[] LocalStatusControllerList
        {
            get { return localStatusControllerList; }
            set
            {
                localStatusControllerList = value;
            }
        }

        /***** REACTIVELY ADD ROI CONTROLLERS WHEN ROICOUNT CHANGED *****/
        private void AddROIControllers()
        {
            // Create ROI Controllers
            globalConfigController.Controls.Clear();
            globalConfigController.Controls.Add(globalConfigControllerVScroll);
            globalConfigControllerVScroll.Visible = false;

            LocalStatusController[] tempLocalStatusControllerList = new LocalStatusController[ROICount];
            for (int i = 0; i < ROICount; i++)
            {
                int roiNum = i;
                bool[] roiFoundLEDs = FoundLEDs;
                bool roiDeinterleave = Deinterleaves[i];
                bool[] roiVisible = new bool[FoundLEDs.Length + 1];
                bool[] roiAutoScales = new bool[FoundLEDs.Length + 1];
                double[] roiMins = new double[FoundLEDs.Length + 1];
                double[] roiMaxes = new double[FoundLEDs.Length + 1];
                for (int j = 0; j <= FoundLEDs.Length; j++)
                {
                    roiVisible[j] = ROIsVisible[i, j];
                    roiAutoScales[j] = AutoScales[i, j];
                    roiMins[j] = Mins[i, j];
                    roiMaxes[j] = Maxes[i, j];
                }

                Tuple<int, bool[], bool> ROIGlobals = Tuple.Create(roiNum, roiFoundLEDs, roiDeinterleave);
                Tuple<bool[], bool[], double[], double[]> ROILocals = Tuple.Create(roiVisible, roiAutoScales, roiMins, roiMaxes);

                Tuple<Tuple<int, bool[], bool>, Tuple<bool[], bool[], double[], double[]>> ROIData = Tuple.Create(ROIGlobals, ROILocals);

                LocalStatusController localStatusController = new LocalStatusController(ROIData);
                localStatusController.LocalConfigModeChanged += LocalStatusController_LocalConfigModeChanged;
                localStatusController.ROIVisibleChanged += LocalStatusController_ROIVisibleChanged;
                localStatusController.DeinterleaveChanged += LocalStatusController_DeinterleaveChanged;
                localStatusController.AutoScalesChanged += LocalStatusController_AutoScalesChanged;
                localStatusController.MinsChanged += LocalStatusController_MinsChanged;
                localStatusController.MaxesChanged += LocalStatusController_MaxesChanged;
                localStatusController.StateIndexChanged += LocalStatusController_StateIndexChanged;
                localStatusController.Size = new Size(localStatusController.State.Width, localStatusController.State.Height + PopOutControllerHeight);
                tempLocalStatusControllerList[i] = localStatusController;
                globalConfigController.Controls.Add(localStatusController);
            }
            LocalStatusControllerList = tempLocalStatusControllerList;

            SuspendLayout();
            EnsureLayout();
            ResumeLayout();
        }


        /******************** EVENT HANDLERS TO PUBLISH GLOBAL VARIABLES TO MASTER CONTROLLER ********************/

        /***** PUBLIC EVENT HANDLERS ******/

        public event EventHandler CapacityChanged;

        public event EventHandler ROIsVisibleChanged;

        public event EventHandler DeinterleavesChanged;

        public event EventHandler AutoScalesChanged;

        public event EventHandler MinsChanged;

        public event EventHandler MaxesChanged;

        /****** INVOKE EVENT HANDLERS *****/

        protected virtual void OnCapacityChanged(EventArgs e)
        {
            var handler = CapacityChanged;
            if (handler != null)
            {
                CapacityChanged.Invoke(this, e);
            }

        }

        protected virtual void OnROIsVisibleChanged(EventArgs e)
        {
            var handler = ROIsVisibleChanged;
            if (handler != null)
            {
                ROIsVisibleChanged.Invoke(this, e);
            }
        }

        protected virtual void OnDeinterleavesChanged(EventArgs e)
        {
            var handler = DeinterleavesChanged;
            if (handler != null)
            {
                DeinterleavesChanged.Invoke(this, e);
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

        /****** EVENT HANDLER FUNCTIONS ******/

        private void Capacity_Changed(object sender, EventArgs e)
        {
            OnCapacityChanged(e);
        }

        private void ROIsVisible_Changed(object sender, EventArgs e)
        {
            OnROIsVisibleChanged(e);
        }

        private void Deinterleaves_Changed(object sender, EventArgs e)
        {
            OnDeinterleavesChanged(e);
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

        /******************** EVENT HANDLERS FOR GLOBAL POP OUT CONTROLLER ********************/
        public event EventHandler ConfigModeChanged;

        public event EventHandler ColorBlindModeChanged;

        protected virtual void OnConfigModeChanged(EventArgs e)
        {
            var handler = ConfigModeChanged;
            if (handler != null)
            {
                ConfigModeChanged.Invoke(this, e);
            }

        }

        protected virtual void OnColorBlindModeChanged(EventArgs e)
        {
            var handler = ColorBlindModeChanged;
            if (handler != null)
            {
                ColorBlindModeChanged.Invoke(this, e);
            }

        }

        private void ConfigMode_Changed(object sender, EventArgs e)
        {
            OnConfigModeChanged(e);
        }

        private void ColorBlindMode_Changed(object sender, EventArgs e)
        {
            // Change all LED colors of Local Status Controllers
            foreach (LocalStatusController localStatusController in LocalStatusControllerList)
            {
                localStatusController.SuspendLayout();
                localStatusController.ColorBlindMode(InColorBlindMode);
            }
            // Then invoke the event to send it through to the Reactive graphs to change their axis colors
            OnColorBlindModeChanged(e);
        }

        private void popOutController_Click(object sender, EventArgs e)
        {
            InConfigMode = !InConfigMode;
            capacityController.Label.Focus();
        }

        private void colorBlindModeButton_Click(object sender, EventArgs e)
        {
            bool newInColorBlindMode = !InColorBlindMode;
            var button = (Button)sender;
            if (newInColorBlindMode)
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

            colorBlindModeController.Label.Focus();
            InColorBlindMode = !InColorBlindMode;
        }

        private void capacityTextBox_LostFocus(object sender, EventArgs e)
        {
            int capacity;
            if (int.TryParse(capacityTextBox.Text, out capacity))
            {
                Capacity = capacity;
                capacityController.Value.Text = Capacity.ToString(CultureInfo.InvariantCulture);

                var textBox = (TextBox)sender;
                var capLabel = capacityController.Value;
                capacityController.SuspendLayout();
                capacityController.Controls.Remove(textBox);
                capacityController.Controls.Add(capLabel);
                capacityController.ResumeLayout();
            }

        }

        private void capacityTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int capacity;
                if (int.TryParse(capacityTextBox.Text, out capacity))
                {
                    Capacity = capacity;
                    capacityController.Value.Text = Capacity.ToString(CultureInfo.InvariantCulture);

                    var textBox = (TextBox)sender;
                    var capLabel = capacityController.Value;
                    capacityController.SuspendLayout();
                    capacityController.Controls.Remove(textBox);
                    capacityController.Controls.Add(capLabel);
                    capacityController.ResumeLayout();
                }
            }
        }

        private void editableCapacityLabel_Click(object sender, EventArgs e)
        {
            var capLabel = (Label)sender;
            var textBox = capacityTextBox;
            capacityController.SuspendLayout();
            capacityController.Controls.Remove(capLabel);
            capacityController.Controls.Add(textBox);
            textBox.Bounds = capLabel.Bounds;
            textBox.Text = capLabel.Text;
            textBox.Font = capLabel.Font;
            capacityController.ResumeLayout();
            textBox.Focus();
        }

        int val = 0;
        private void globalConfigControllerVScroll_ValueChanged(object sender, EventArgs e)
        {

            var vScroll = (VScrollBar)sender;
            int dy = vScroll.Value - val;
            val = vScroll.Value;
            foreach (Control control in globalConfigController.Controls)
            {
                if (!control.Name.Equals("vScroll"))
                {
                    control.SuspendLayout();
                    control.Location = new Point(control.Location.X, control.Location.Y - dy);
                    control.ResumeLayout();
                }

            }

        }

        private void globalConfigController_Click(object sender, EventArgs e)
        {
            globalConfigController.Focus();
        }

        /******************** EVENT HANDLERS FOR LOCAL STATUS CONTROLLERS ********************/
        private void LocalStatusController_LocalConfigModeChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                bool[] tempLocalConfigModes = LocalConfigModes;
                tempLocalConfigModes[roiIndex] = LocalStatusControllerList[roiIndex].LocalConfigMode;
                LocalConfigModes = tempLocalConfigModes;

            }
        }

        private void LocalStatusController_ROIVisibleChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                bool[,] tempROIsVisible = ROIsVisible;
                bool[] localROIVisible = LocalStatusControllerList[roiIndex].ROIVisible;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempROIsVisible[roiIndex, i] = localROIVisible[i];
                }
                ROIsVisible = tempROIsVisible;

            }
        }

        private void LocalStatusController_DeinterleaveChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                bool[] tempDeinterleaves = Deinterleaves;
                tempDeinterleaves[roiIndex] = LocalStatusControllerList[roiIndex].Deinterleave;
                Deinterleaves = tempDeinterleaves;

            }
        }

        private void LocalStatusController_AutoScalesChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                bool[,] tempAutoScales = AutoScales;
                bool[] localAutoScales = LocalStatusControllerList[roiIndex].AutoScales;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempAutoScales[roiIndex, i] = localAutoScales[i];
                }
                AutoScales = tempAutoScales;

            }
        }

        private void LocalStatusController_MinsChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                double[,] tempMins = Mins;
                double[] localMins = LocalStatusControllerList[roiIndex].Mins;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempMins[roiIndex, i] = localMins[i];

                }
                Mins = tempMins;

            }
        }

        private void LocalStatusController_MaxesChanged(object sender, EventArgs e)
        {

            var roiController = (LocalStatusController)sender;
            int roiIndex;
            if (int.TryParse(roiController.Name.Split('_')[1], out roiIndex))
            {

                double[,] tempMaxes = Maxes;
                double[] localMaxes = LocalStatusControllerList[roiIndex].Maxes;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempMaxes[roiIndex, i] = localMaxes[i];
                }
                Maxes = tempMaxes;

            }
        }

        private void LocalStatusController_StateIndexChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            EnsureLayout();
            ResumeLayout();
        }

        /***** ACCEPT DATA FROM MASTER *****/
        public void UpdateMins(double[,] masterMins)
        {
            for (int i = 0; i < ROICount; i++)
            {
                double[] localMins = new double[FoundLEDs.Length + 1];
                for (int j = 0; j < FoundLEDs.Length + 1; j++)
                {
                    localMins[j] = masterMins[i, j];
                }
                LocalStatusControllerList[i].UpdateMins(localMins);
            }
        }

        public void UpdateMaxes(double[,] masterMaxes)
        {
            for (int i = 0; i < ROICount; i++)
            {
                double[] localMaxes = new double[FoundLEDs.Length + 1];
                for (int j = 0; j < FoundLEDs.Length + 1; j++)
                {
                    localMaxes[j] = masterMaxes[i, j];
                }
                LocalStatusControllerList[i].UpdateMaxes(localMaxes);
            }
        }

        /***** HANDLING RESIZING *****/
        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            EnsureLayout();
            ResumeLayout();
        }

        private void EnsureLayout()
        {
            // Resize global pop out controller and its label
            globalPopOutController.SuspendLayout();
            globalPopOutControllerLabel.SuspendLayout();
            globalPopOutControllerLabel1.SuspendLayout();
            globalPopOutControllerLabel2.SuspendLayout();
            globalConfigController.SuspendLayout();

            globalPopOutController.Width = Width;
            globalPopOutControllerLabel3.Location = new Point(Width / 2 - 75 - PopOutControllerHeight, 0);
            globalPopOutControllerLabel4.Location = new Point(Width / 2 + 75, 0);
            globalPopOutControllerLabel.Location = new Point(Width / 2 - 75, 0);
            globalPopOutControllerLabel2.Location = new Point(Width - PopOutControllerHeight, 0);


            if (InConfigMode)
            {
                // Change the arrows of the buttons on the global pop out controller to facing down
                globalPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2193);
                globalPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2193);
                globalPopOutControllerLabel3.Text = char.ConvertFromUtf32(0x2193);
                globalPopOutControllerLabel4.Text = char.ConvertFromUtf32(0x2193);
                // Resize the global config controller and make it visible
                globalConfigController.Bounds = new Rectangle(0, PopOutControllerHeight, Width, Height - PopOutControllerHeight);
                globalConfigController.Visible = true;


                Tuple<int, int> matrix = FindMatrix();

                int roiControllerWidths = (int)((float)(Width - 20 - (matrix.Item2 + 1) * HorizontalMargin) / (float)matrix.Item2);
                int maxYPoint = 0;
                for (int i = 0; i < ROICount; i++)
                {
                    int n = i % matrix.Item2;
                    int m = i / matrix.Item2;

                    int xPoint = n * roiControllerWidths + (n + 1) * HorizontalMargin;
                    int yPoint = VerticalMargin;
                    if (m >= 1)
                    {
                        yPoint = VerticalMargin;
                        for (int j = i - matrix.Item2; j >= 0; j -= matrix.Item2)
                        {
                            yPoint += LocalStatusControllerList[j].Height + VerticalMargin;
                        }
                    }

                    LocalStatusControllerList[i].SuspendLayout();
                    if (LocalConfigModes[i])
                    {
                        LocalStatusControllerList[i].Bounds = new Rectangle(xPoint, yPoint, roiControllerWidths, LocalStatusControllerList[i].State.Height + PopOutControllerHeight);

                    }
                    else
                    {
                        LocalStatusControllerList[i].Bounds = new Rectangle(xPoint, yPoint, roiControllerWidths, PopOutControllerHeight);
                    }
                    if (LocalStatusControllerList[i].Location.Y + LocalStatusControllerList[i].Height > maxYPoint)
                    {
                        maxYPoint = LocalStatusControllerList[i].Location.Y + LocalStatusControllerList[i].Height;
                    }
                }
                if (maxYPoint > globalConfigController.Height)
                {
                    globalConfigControllerVScroll.Visible = true;
                    globalConfigControllerVScroll.Bounds = new Rectangle(Width - 20, 0, 20, globalConfigController.Height);
                    globalConfigControllerVScroll.Maximum = maxYPoint;

                    foreach (LocalStatusController localStatusController in LocalStatusControllerList)
                    {
                        localStatusController.Location = new Point(localStatusController.Location.X, localStatusController.Location.Y - val);
                    }
                }
                else
                {
                    val = 0;

                    globalConfigControllerVScroll.Visible = false;
                }

            }
            else
            {
                // Change the arrows of the buttons on the global pop out controller to facing down
                globalPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2191);
                globalPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2191);
                globalPopOutControllerLabel3.Text = char.ConvertFromUtf32(0x2191);
                globalPopOutControllerLabel4.Text = char.ConvertFromUtf32(0x2191);
                // Make the global config controller invisible
                globalConfigController.Visible = false;
            }
        }

        private Tuple<int, int> FindMatrix()
        {
            // Find how many LocalStatusControllers fit in current Width
            int Cols = (int)((float)(Width - HorizontalMargin - 20) / (float)(MinLocalConfigControllerWidth + HorizontalMargin));
            int Rows;
            if (Cols >= ROICount)
            {
                Cols = ROICount;
                Rows = 1;
            }
            else if (Cols <= 0)
            {
                Cols = 1;
                Rows = ROICount;
            }
            else
            {
                Rows = (int)Math.Ceiling((float)ROICount / (float)Cols);
            }

            return Tuple.Create(Rows, Cols);
        }
    }
}
