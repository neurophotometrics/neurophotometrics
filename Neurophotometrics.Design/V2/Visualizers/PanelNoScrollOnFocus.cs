using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Visualizers
{
    public partial class PanelNoScrollOnFocus : Panel
    {
        public PanelNoScrollOnFocus()
        {
            InitializeComponent();
        }

        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }
    }
}