﻿using Bonsai.Design;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Neurophotometrics.Design.Properties;
using System.Reflection;

namespace Neurophotometrics.Design
{
    class FP3002CalibrationEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
                if (editorState != null)
                {
                    if (editorState.WorkflowRunning)
                    {
                        throw new InvalidOperationException(Resources.WorkflowRunning_Error);
                    }

                    var capture = (FP3002)component;
                    using (var editorForm = new FP3002CalibrationEditorForm(capture, provider))
                    {
                        var crop = capture.AutoCrop;
                        var acquisitionMode = capture.AcquisitionMode;
                        try
                        {
                            capture.AutoCrop = false;
                            capture.AcquisitionMode = AcquisitionModes.StartPhotometry;
                            editorForm.ShowDialog(owner);
                        }
                        catch (TargetInvocationException ex)
                        {
                            throw ex.InnerException;
                        }
                        finally
                        {
                            capture.AutoCrop = crop;
                            capture.AcquisitionMode = acquisitionMode;
                        }
                    }
                }
            }

            return false;
        }
    }
}
