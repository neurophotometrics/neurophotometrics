using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    class PropertyGrid : System.Windows.Forms.PropertyGrid
    {
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
                    control.Height = (int)(control.Height * factor.Height * 2);
                }
            }

            base.ScaleControl(factor, specified);
        }
    }
}
