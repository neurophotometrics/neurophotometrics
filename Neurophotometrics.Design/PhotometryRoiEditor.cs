using Bonsai.Vision.Design;
using System;
using System.Linq;
using OpenCV.Net;
using System.Reactive.Linq;
using Bonsai.Harp;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Bonsai.Design;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    public class PhotometryRoiEditor : IplImageEllipseRoiEditor
    {
        const byte PhotometryEvent = 84;

        public PhotometryRoiEditor()
            : base(DataSource.Output)
        {
        }

        protected override IObservable<IplImage> GetImageSource(IObservable<IObservable<object>> dataSource)
        {
            return dataSource.Merge().Select(input =>
            {
                var harpMessage = input as HarpMessage;
                if (harpMessage != null)
                {
                    return harpMessage.Address == PhotometryEvent
                        ? ((PhotometryHarpMessage)harpMessage).PhotometryData.Image : null;
                }

                return ((PhotometryDataFrame)input).Image;
            }).Where(image => image != null);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (context != null && editorService != null)
            {
                using (var visualizerDialog = new TypeVisualizerDialog())
                {
                    var roiPickerControl = new PhotometryRoiPicker();
                    var propertyDescriptor = context.PropertyDescriptor;
                    var regions = (RotatedRect[])value;

                    roiPickerControl.Dock = DockStyle.Fill;
                    visualizerDialog.Text = propertyDescriptor.Name;
                    if (regions != null)
                    {
                        foreach (var region in regions) roiPickerControl.Regions.Add(region);
                    }

                    roiPickerControl.RegionsChanged += (sender, e) => propertyDescriptor.SetValue(context.Instance, roiPickerControl.Regions.ToArray());
                    visualizerDialog.AddControl(roiPickerControl);
                    roiPickerControl.Canvas.MouseDoubleClick += (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Left && roiPickerControl.Image != null && !roiPickerControl.SelectedRegion.HasValue)
                        {
                            visualizerDialog.ClientSize = new System.Drawing.Size(
                                roiPickerControl.Image.Width,
                                roiPickerControl.Image.Height);
                        }
                    };

                    IDisposable subscription = null;
                    var source = GetDataSource(context, provider);
                    var imageSource = GetImageSource(source.Output);
                    roiPickerControl.Load += delegate { subscription = imageSource.Subscribe(image => roiPickerControl.Image = image); };
                    roiPickerControl.HandleDestroyed += delegate { subscription.Dispose(); };
                    editorService.ShowDialog(visualizerDialog);
                    return roiPickerControl.Regions.ToArray();
                }
            }

            return base.EditValue(context, provider, value);
        }
    }
}
