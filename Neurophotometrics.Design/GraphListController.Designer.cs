using System;

namespace Neurophotometrics.Design
{
    partial class GraphListController
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(Tuple<Tuple<int, bool[], int, bool>, Tuple<bool[], bool[], double[], double[]>> ReactiveGraphInitProps)
        {
            this.SuspendLayout();
            this.reactiveGraph = new ReactiveGraph(ReactiveGraphInitProps);
            this.components = new System.ComponentModel.Container();
            this.reactiveGraph.SuspendLayout();

            //
            // reactiveGraph
            //
            this.reactiveGraph.Name = "reactiveGraph";
            this.reactiveGraph.MinsChanged += ReactiveGraph_MinsChanged;
            this.reactiveGraph.MaxesChanged += ReactiveGraph_MaxesChanged;

            this.ReactiveGraphList = new ReactiveGraph[1];
            this.ReactiveGraphList[0] = this.reactiveGraph;
            this.Controls.Add(this.reactiveGraph);
        }
        #endregion

        private ReactiveGraph reactiveGraph;
    }
}
