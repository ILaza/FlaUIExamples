using System;
using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;

using FlaUI.UIA3;
using NUnit.Framework;


namespace FlaUIExamples
{
    static class AutomationElementsExtantion
    {
        public static void PerformLeftMouseClickByClickablePoint(this AutomationElement element)
        {
            Mouse.Click(element.GetClickablePoint(), MouseButton.Left);
        }
    }

    public class Example1
    {
        private Window activityCenterWindow;
        private Application ndApp;
        private AutomationElement settingImage;
        private AutomationElement settingButton;

        [OneTimeSetUp]
        public void SetUp()
        {
            using (var automation = new UIA3Automation())
            {
                //var profiler = MiniProfiler.StartNew("My Profiler Name");
                //using (profiler.Step("Main Work"))
                //{

                //}
                var desktop = automation.GetDesktop();
                activityCenterWindow = desktop.FindFirstDescendant(c => c.ByText("NetDocuments ndOffice"))
                                                                         .AsWindow();
                activityCenterWindow.Click();

                var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\NetDocuments\ndOffice\ndOffice.exe");
                ndApp = Application.AttachOrLaunch(processStartInfo);

                activityCenterWindow = ndApp.GetMainWindow(automation);
                settingImage = activityCenterWindow.FindFirstDescendant(c => c.ByAutomationId("SettingsMenuIcon"))
                    ?? throw new NullReferenceException("Unable to find 'settings gear' button");
            }
        }

        [Test]
        public void FirstTest()
        {

            var processEventHandler = new ProcessEventHandler();
            processEventHandler.RegisterForProcessEvent();

            settingImage.PerformLeftMouseClickByClickablePoint();

            settingButton = WaitForElement(() => activityCenterWindow?.FindFirstDescendant(cf => cf.ByAutomationId("ExitMenuItem")).AsButton());

            settingButton?.WaitUntilClickable();

            settingButton?.Click();

            var startTime = DateTime.Now;

        }
        private T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull<T>(() => getter(), TimeSpan.FromMilliseconds(10000));

            return retry.Result;
        }
        public void ProcessExitedEventHandler(object sender, System.EventArgs e)
        {
            Console.WriteLine(DateTime.Now);
        }
    }
}