using Bonsai.Design;

using Neurophotometrics.Design.Properties;
using Neurophotometrics.V1;
using Neurophotometrics.V1.Definitions;

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V1.Forms
{
    public class FP3001FormEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider == null) return false;

            var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
            if (editorState == null) return false;

            if (editorState.WorkflowRunning)
                throw new InvalidOperationException(Resources.MsgBox_Error_WorkflowRunning);

            var fp3001 = (FP3001)component;
            CreateForm(fp3001, owner);

            return false;
        }

        private void TryShowForm(FP3001 fp3001, IWin32Window owner)
        {
            try
            {
                using (var fp3001Form = new FP3001Form(fp3001))
                {
                    fp3001Form.ShowDialog(owner);
                }
            }
            catch (SerialNumberNotFoundException ex)
            {
                MessageBox.Show(Resources.MsgBox_Error_SerialNumNotFound, $"Error: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void CreateForm(FP3001 fp3001, IWin32Window owner)
        {
            fp3001.SetAutoCrop(false);
            TryShowForm(fp3001, owner);
            fp3001.SetAutoCrop(true);
        }
    }
}