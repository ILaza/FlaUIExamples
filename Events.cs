using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Identifiers;
using FlaUI.UIA3;

namespace FlaUIExamples
{
    public class ProcessEventHandler 
    {
        public Process process = Process.GetProcessesByName("ndOffice").SingleOrDefault();
        
        public ProcessEventHandler()
        { 
        }
        public void RegisterForProcessEvent()
        {
            process.Exited += new EventHandler(ProcessExitedEventHandler);
        }

        public void ProcessExitedEventHandler(object sender, System.EventArgs e)
        {
            Console.WriteLine(DateTime.Now);
        }
    }
}
