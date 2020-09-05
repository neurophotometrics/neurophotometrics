using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    class PropertyGrid : System.Windows.Forms.PropertyGrid
    {
        [DefaultValue(1.0)]
        public double SplitterDistance { get; set; } = 1.0;

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            foreach (Control control in Controls)
            {
                var controlType = control.GetType();
                if (controlType.Name == "DocComment")
                {
                    var userSizedField = controlType.BaseType.GetField(
                        "userSized",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    userSizedField.SetValue(control, true);
                    control.Height = (int)(control.Height * factor.Height * SplitterDistance);
                }
            }

            base.ScaleControl(factor, specified);
        }
    }
}
