using Bonsai.Design;
using Bonsai.Vision.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using OpenCV.Net;
using System.Reactive.Linq;

namespace Neurophotometrics.Design
{
    partial class FP3001CalibrationEditorForm : Form
    {
        IObservable<PhotometryDataFrame> source;
        ActivityVisualizer activityVisualizer;
        CalibrationRoiEditor roiEditor;
        ITypeDescriptorContext context;
        IDisposable dataSubscription;
        EditorSite editorSite;

        public FP3001CalibrationEditorForm(object instance, IObservable<PhotometryDataFrame> capture, IServiceProvider provider)
        {
            InitializeComponent();
            source = capture.Publish().RefCount();
            activityVisualizer = new ActivityVisualizer();
            roiEditor = new CalibrationRoiEditor(source);
            context = new RoiTypeDescriptorContext(instance, provider);
            editorSite = new EditorSite(this, provider);
        }

        public void AddCalibrationControl(Control control)
        {
            tableLayoutPanel.RowCount++;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, control.Height));
            tableLayoutPanel.Controls.Add(control);
            Height += control.Height;
        }

        protected override void OnLoad(EventArgs e)
        {
            activityVisualizer.Load(editorSite);
            dataSubscription = source.ObserveOn(this).Subscribe(activityVisualizer.Show, activityVisualizer.SequenceCompleted);
            base.OnLoad(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            dataSubscription.Dispose();
            base.OnHandleDestroyed(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            activityVisualizer.Unload();
            base.OnFormClosed(e);
        }

        private void calibrateRoiButton_Click(object sender, EventArgs e)
        {
            var value = context.PropertyDescriptor.GetValue(context.Instance);
            roiEditor.EditValue(context, editorSite, value);
        }

        class RoiTypeDescriptorContext : ITypeDescriptorContext
        {
            IServiceProvider serviceProvider;

            internal RoiTypeDescriptorContext(object instance, IServiceProvider provider)
            {
                Instance = instance;
                serviceProvider = provider;
                PropertyDescriptor = TypeDescriptor.GetProperties(instance).Find(nameof(FP3001.Regions), false);
            }

            public IContainer Container => null;

            public object Instance { get; private set; }

            public PropertyDescriptor PropertyDescriptor { get; private set; }

            public object GetService(Type serviceType)
            {
                return serviceProvider.GetService(serviceType);
            }

            public void OnComponentChanged()
            {
            }

            public bool OnComponentChanging()
            {
                return true;
            }
        }

        class CalibrationRoiEditor : PhotometryDataFrameRoiEditor
        {
            IObservable<IplImage> imageSource;

            internal CalibrationRoiEditor(IObservable<PhotometryDataFrame> source)
            {
                imageSource = source.Select(input => input.Image);
            }

            protected override IObservable<IplImage> GetImageSource(IObservable<IObservable<object>> dataSource)
            {
                return imageSource;
            }
        }

        class EditorSite : IServiceProvider, IWindowsFormsEditorService, IDialogTypeVisualizerService
        {
            FP3001CalibrationEditorForm form;
            IServiceProvider parentProvider;

            internal EditorSite(FP3001CalibrationEditorForm owner, IServiceProvider provider)
            {
                form = owner;
                parentProvider = provider;
            }

            public void AddControl(Control control)
            {
                form.visualizerPanel.Controls.Add(control);
            }

            public void CloseDropDown()
            {
            }

            public void DropDownControl(Control control)
            {
                throw new NotSupportedException();
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IWindowsFormsEditorService) ||
                    serviceType == typeof(IDialogTypeVisualizerService))
                {
                    return this;
                }

                return parentProvider.GetService(serviceType);
            }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern bool EnableWindow(IntPtr hWnd, bool enable);

            public DialogResult ShowDialog(Form dialog)
            {
                FormClosingEventHandler cancelClosing = (sender, e) => { e.Cancel = true; };
                form.BeginInvoke(new Action(() => EnableWindow(form.Handle, true)));
                form.FormClosing += cancelClosing;
                try { return dialog.ShowDialog(form); }
                finally { form.FormClosing -= cancelClosing; }
            }
        }
    }
}
