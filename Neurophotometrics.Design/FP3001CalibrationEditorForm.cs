using Bonsai.Design;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using OpenCV.Net;
using System.Reactive.Linq;

namespace Neurophotometrics.Design
{
    partial class FP3001CalibrationEditorForm : Form
    {
        readonly IObservable<PhotometryDataFrame> source;
        readonly ActivityVisualizer activityVisualizer;
        readonly CalibrationRoiEditor roiEditor;
        readonly ITypeDescriptorContext context;
        readonly EditorSite editorSite;
        IDisposable dataSubscription;
        Control calibration;

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
            control.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount++;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, control.Height));
            tableLayoutPanel.Controls.Add(control);
            Height += control.Height;
            calibration = control;
        }

        protected override void OnLoad(EventArgs e)
        {
            activityVisualizer.Load(editorSite);
            dataSubscription = source.ObserveOn(this).Subscribe(activityVisualizer.Show, activityVisualizer.SequenceCompleted);
            base.OnLoad(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (calibration != null) calibration.Dispose();
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
            readonly IServiceProvider serviceProvider;

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

        class CalibrationRoiEditor : PhotometryRoiEditor
        {
            readonly IObservable<IplImage> imageSource;

            internal CalibrationRoiEditor(IObservable<PhotometryDataFrame> source)
            {
                imageSource = source.Select(input => input.Image);
            }

            protected override IObservable<IplImage> GetImageSource(IObservable<IObservable<object>> dataSource)
            {
                return imageSource.Select(image =>
                {
                    if (image.Depth != IplDepth.U8)
                    {
                        var normalizedImage = new IplImage(image.Size, IplDepth.U8, image.Channels);
                        var scale = byte.MaxValue / (double)ushort.MaxValue;
                        CV.ConvertScale(image, normalizedImage, scale);
                        image = normalizedImage;
                    }
                    return image;
                });
            }
        }

        class EditorSite : IServiceProvider, IWindowsFormsEditorService, IDialogTypeVisualizerService
        {
            readonly FP3001CalibrationEditorForm form;
            readonly IServiceProvider parentProvider;

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
