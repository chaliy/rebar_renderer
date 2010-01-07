/*
    Purpose: Renderers ToolsStrip and MenuStrip with Microsoft Internet Explorer Rebar style.
        - Full support of themes (including third-party themes and software like StyleXP).
        - Clear rendering with disabled themes.
        - Support of both Rtl and Ltr directions.

    Original file name:  RebarRenderer.cs

    Copyright: Mike Chaliy. All rights reserved.

    License: This work is licensed under a Creative Commons Attribution 2.5 License (http://creativecommons.org/licenses/by/2.5/).
    <rdf:RDF xmlns="http://web.resource.org/cc/" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
      <Work rdf:about="">
        <license rdf:resource="http://creativecommons.org/licenses/by/2.5/" />
        <dc:title>.Net ToolStrip Rebar Renderer</dc:title>
        <dc:date>2006</dc:date>
        <dc:description>Renders ToolsStrip and MenuStrip with Microsoft Internet Explorer Rebar style. Full support of themes (including third-party themes and software like StyleXP). Clear rendering with disabled themes. Support of both Rtl and Ltr directions. Sources included.</dc:description>
        <dc:creator>
          <Agent>
            <dc:title>Mike Chaliy</dc:title>
          </Agent>
        </dc:creator>
        <dc:rights>
          <Agent>
            <dc:title>Mike Chaliy</dc:title>
          </Agent>
        </dc:rights>
        <dc:source rdf:resource="http://www.chaliy.com/Sources/RebarRenderer/" />
      </Work>
      <License rdf:about="http://creativecommons.org/licenses/by/2.5/">
        <permits rdf:resource="http://web.resource.org/cc/Reproduction"/>
        <permits rdf:resource="http://web.resource.org/cc/Distribution"/>
        <requires rdf:resource="http://web.resource.org/cc/Notice"/>
        <requires rdf:resource="http://web.resource.org/cc/Attribution"/>
        <permits rdf:resource="http://web.resource.org/cc/DerivativeWorks"/>
      </License>
    </rdf:RDF>
*/

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace Chaliy.Windows.Forms
{
    /// <summary>
    ///     Explorer like <see cref="ToolStrip"/> renderer.
    /// </summary>
    /// <seealso cref="ToolStrip"/>
    /// <seealso cref="ToolStripManager"/>
    /// <seealso cref="ToolStripRenderer"/>    
    public class RebarRenderer : ToolStripSystemRenderer
    {
        #region Fields

        private static VisualStyleRenderer RebarStyleRenderer;
        private static VisualStyleRenderer RebarBandStyleRenderer;

        private EventHandler _locationChangedEnventHandler;

        #endregion

        #region Constructors

        /// <summary>
        /// 	Initializes the <see cref="RebarRenderer"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static RebarRenderer()
        {
            if (VisualStyleRenderer.IsSupported)
            {
                // For some reasons Rebar zero part (Rebar by itself) was not added to the VisualStyleElements.
                RebarRenderer.RebarStyleRenderer = new VisualStyleRenderer("Rebar", 0, 0);
                RebarRenderer.RebarBandStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Rebar.Band.Normal);
            }
        }

        /// <summary>
        /// 	Initializes a new instance of the <see cref="RebarRenderer"/> class.
        /// </summary>
        public RebarRenderer()
        {
            this._locationChangedEnventHandler = new EventHandler(this.ChildControlLocationChanged);
        }

        #endregion

        #region Implementation

        /// <summary>
        ///     Initializes the specified <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.
        /// </summary>
        /// <param name="toolStripPanel">The <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.</param>
        protected override void InitializePanel(ToolStripPanel toolStripPanel)
        {
            toolStripPanel.SizeChanged += new EventHandler(this.PanelSizeChanged);

            // Because many of the themes use gradients and so on, we need to refresh all toolbars event they simply moved.
            foreach (ToolStripPanelRow row in toolStripPanel.Rows)
            {
                foreach (ToolStrip toolStrip in row.Controls)
                {
                    if (toolStrip != null)
                    {
                        this.ConnectToolStrip(toolStrip);
                    }
                }
            }

            // This need to workaroud issue the Initalize does not invoked when using ToolStripManager.Renderer.
            toolStripPanel.ControlAdded += new ControlEventHandler(PanelControlAdded);
            toolStripPanel.ControlRemoved += new ControlEventHandler(PanelControlRemoved);

            base.InitializePanel(toolStripPanel);
        }

        private void DisconnectToolStrip(ToolStrip toolStrip)
        {
            if (toolStrip != null)
            {
                toolStrip.LocationChanged -= this._locationChangedEnventHandler;
                
                if ((ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsSupported) == false)
                {
                    toolStrip.BackColor = Color.Empty;
                }
            }
        }

        private void ConnectToolStrip(ToolStrip toolStrip)
        {
            if (toolStrip != null)
            {
                toolStrip.LocationChanged += this._locationChangedEnventHandler;

                // This is needed for proper rendering without visual styles enbaled.
                if ((ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsSupported) == false)
                {
                    toolStrip.BackColor = Color.Transparent;
                }
            }
        }

        private void PanelControlRemoved(object sender, ControlEventArgs e)
        {
            this.DisconnectToolStrip(e.Control as ToolStrip);
        }

        private void PanelControlAdded(object sender, ControlEventArgs e)
        {
            this.ConnectToolStrip(e.Control as ToolStrip);
        }

        private void ChildControlLocationChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                control.Parent.Invalidate(true);
            }
        }

        private void PanelSizeChanged(object sender, EventArgs e)
        {
            ToolStripPanel toolStripPanel = sender as ToolStripPanel;            
            if (toolStripPanel != null)
            {
                toolStripPanel.Invalidate(true);
            }
        }      

        /// <summary>
        ///     Raises the render tool strip background event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripRenderEventArgs"/> instance containing the event data.</param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsSupported)
            {
                if (e.ToolStrip is ToolStripDropDownMenu)
                {
                    base.OnRenderToolStripBackground(e);
                }
                else if (e.ToolStrip is ToolStrip || e.ToolStrip is MenuStrip)
                {
                    // Rebar style have interesting bottom border.
                    // Because it does not same like other borders of the tool strip
                    // we can not use standard mechanizm to draw borders.
                    // If tool strip will be parented to the ToolStripPanel
                    // we will paint bands, in other way we will paint full rebar.
                    if (e.ToolStrip.Parent is ToolStripPanel)
                    {
                        if (RebarRenderer.RebarBandStyleRenderer.IsBackgroundPartiallyTransparent())
                        {
                            RebarRenderer.RebarBandStyleRenderer.DrawParentBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.ToolStrip);
                        }
                        RebarRenderer.RebarBandStyleRenderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);
                    }
                    else
                    {
                        RebarRenderer.RebarStyleRenderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle);
                    }
                }
            }           
        }       

        /// <summary>
        ///     Raises the render tool strip border event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripRenderEventArgs"/> instance containing the event data.</param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip.Parent is ToolStripPanel))
            {
                base.OnRenderToolStripBorder(e);
            }
        }       

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripPanelBackground"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripPanelRenderEventArgs"></see> that contains the event data.</param>
        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsSupported)
            {
                e.Handled = true;
                RebarRenderer.RebarStyleRenderer.DrawBackground(e.Graphics, e.ToolStripPanel.ClientRectangle);

                Edges edge = Edges.Top;
                switch (e.ToolStripPanel.Dock)
                {
                    case DockStyle.Left:
                        edge = Edges.Right;
                        break;

                    case DockStyle.Right:
                        edge = Edges.Left;
                        break;
                }
                foreach (ToolStripPanelRow row in e.ToolStripPanel.Rows)
                {
                    Rectangle edgeBounds = row.Bounds;
                    if (edge == Edges.Top)
                    {
                        edgeBounds.Offset(0, -1);
                    }                
                    RebarRenderer.RebarBandStyleRenderer.DrawEdge(e.Graphics, edgeBounds, edge, EdgeStyle.Etched, EdgeEffects.None);                                   
                }
            }
            else
            {
                e.Handled = true;

                Size paddingSize = SystemInformation.Border3DSize;
               
                Rectangle bounds = e.ToolStripPanel.ClientRectangle;
                RebarRenderer.FillRectangle(e.Graphics, bounds, SystemColors.Menu);               
                ControlPaint.DrawBorder3D(e.Graphics, bounds, Border3DStyle.Etched, Border3DSide.All);                

                foreach (ToolStripPanelRow row in e.ToolStripPanel.Rows)
                {                 
                    Rectangle edgeBounds = row.Bounds;
                    edgeBounds.Width -= paddingSize.Width;
                    ControlPaint.DrawBorder3D(e.Graphics, edgeBounds, Border3DStyle.Etched, Border3DSide.Bottom);
                }
            }                                   
        }

        private static void FillRectangle(Graphics g, Rectangle rectangle, Color color)
        {
            if (color.IsSystemColor)
            {
                g.FillRectangle(SystemBrushes.FromSystemColor(color), rectangle);
            }
            else
            {
                using (Brush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, rectangle);
                }
            }

        }
       
        #endregion
    }
}
