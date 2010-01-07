/*
    Purpose: Entry point for test application.
 
    Original file name:  Program.cs

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

namespace Chaliy.Windows.Forms.Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ToolStripManager.Renderer = new RebarRenderer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            Application.Run(new MainForm());
        }
    }
}