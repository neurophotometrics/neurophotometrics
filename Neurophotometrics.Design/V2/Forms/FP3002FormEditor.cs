using Bonsai.Design;
using Neurophotometrics.Design.Properties;
using Neurophotometrics.Design.V2.Editors;
using Neurophotometrics.V2;
using Neurophotometrics.V2.Definitions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Forms
{
    public class FP3002FormEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider == null) return false;

            var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
            if (editorState == null) return false;

            if (editorState.WorkflowRunning)
                throw new InvalidOperationException(Resources.MsgBox_Error_WorkflowRunning);

            var fp3002 = (FP3002)component;
            CreateForm(fp3002, owner);

            return false;
        }

        private void CreateForm(FP3002 fp3002, IWin32Window owner)
        {
            SplashScreen.ShowSplash();

            var acquisitionMode = fp3002.AcquisitionMode;

            fp3002.AcquisitionMode = AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera;
            fp3002.SetAutoCrop(false);

            TryShowForm(fp3002, owner);

            fp3002.AcquisitionMode = acquisitionMode;
            fp3002.SetAutoCrop(true);
        }

        private void TryShowForm(FP3002 fp3002, IWin32Window owner)
        {
            try
            {
                using (var fp3002Form = new FP3002Form(fp3002))
                {
                    fp3002Form.ShowDialog(owner);
                }
            }
            catch (TimeoutException ex)
            {
                SplashScreen.CloseSplash();
                MessageBox.Show(Resources.MsgBox_Error_TimeoutFindSystem, $"Error: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(NotSupportedException ex)
            {
                SplashScreen.CloseSplash();
                MessageBox.Show(Resources.MsgBox_Error_HarpVersion, $"Error: Not Supported Bonsai.Harp Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                SplashScreen.CloseSplash();
                ConsoleLogger.LogError(ex);
            }
        }
    }
}