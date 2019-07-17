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

        public FP3001CalibrationEditorForm(FP3001 capture, IServiceProvider provider)
        {
            InitializeComponent();
            source = capture.Generate().Publish().RefCount();
            activityVisualizer = new ActivityVisualizer();
            roiEditor = new CalibrationRoiEditor(source);
            context = new RoiTypeDescriptorContext(capture, provider);
            editorSite = new EditorSite(this, provider);
        }

        protected override void OnLoad(EventArgs e)
        {
            activityVisualizer.Load(editorSite);
            dataSubscription = source.ObserveOn(this).Subscribe(activityVisualizer.Show, activityVisualizer.SequenceCompleted);
            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dataSubscription.Dispose();
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

            internal RoiTypeDescriptorContext(FP3001 source, IServiceProvider provider)
            {
                Instance = source;
                serviceProvider = provider;
                PropertyDescriptor = TypeDescriptor.GetProperties(source).Find(nameof(source.Regions), false);
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

            public DialogResult ShowDialog(Form dialog)
            {
                return dialog.ShowDialog(form);
            }
        }
    }
}
