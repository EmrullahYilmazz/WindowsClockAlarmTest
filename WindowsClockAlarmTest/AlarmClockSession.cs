using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace AlarmClockTest
{
    public class AlarmClockSession
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string AlarmClockAppId = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";

        protected static WindowsDriver<WindowsElement> session;
        protected static RemoteTouchScreen touchScreen;

        public static void Setup(TestContext context)
        {
            if (session == null || touchScreen == null)
            {
                TearDown();

                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AlarmClockAppId);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

                touchScreen = new RemoteTouchScreen(session);
                Assert.IsNotNull(touchScreen);
            }
        }

        public static void TearDown()
        {
            touchScreen = null;

            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public virtual void TestInit()
        {
            WindowsElement alarmButtonElement = null;

            try
            {
                alarmButtonElement = session.FindElementByAccessibilityId("AlarmButton");
            }
            catch
            {
                session.FindElementByAccessibilityId("CancelButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                alarmButtonElement = session.FindElementByAccessibilityId("AlarmButton");
            }

            Assert.IsNotNull(alarmButtonElement);
            Assert.IsTrue(alarmButtonElement.Displayed);
        }
    }
}