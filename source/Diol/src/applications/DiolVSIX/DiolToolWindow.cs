using DiolVSIX.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace DiolVSIX
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid(WindowGuidString)]
    public class DiolToolWindow : ToolWindowPane
    {
        public const string WindowGuidString = "446919b1-4c51-4119-a6ea-6884faa68f06";
        public const string Title = "Diol";

        /// <summary>
        /// Initializes a new instance of the <see cref="DiolToolWindow"/> class.
        /// </summary>
        public DiolToolWindow() : base(null)
        {
            //this.dte = dte;

            this.Caption = Title;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var toolWindowControl = new DiolToolWindowControl();
            var diolBootstrapper = new DiolBootstrapper(toolWindowControl);

            this.Content = toolWindowControl;

            diolBootstrapper.Run();
        }
    }
}
