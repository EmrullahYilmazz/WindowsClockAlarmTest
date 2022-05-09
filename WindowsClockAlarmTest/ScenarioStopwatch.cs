using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Drawing;
using System;

namespace AlarmClockTest
{
    [TestClass]
    public class ScenarioStopwatch : AlarmClockSession
    {
        private static Size originalSize;

        [TestMethod]
        public void StopwatchLap()
        {
            int numberOfEntry = 8;

            var stopwatchPlayPauseButton = session.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            stopwatchPlayPauseButton.Click();

            var stopwatchLapButton = session.FindElementByAccessibilityId("StopWatchLapButton");
            for (uint count = 0; count < numberOfEntry; count++)
            {
                stopwatchLapButton.Click();
            }

            stopwatchPlayPauseButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            var lapListView = session.FindElementByAccessibilityId("LapsAndSplitsListView");
            var lapEntries = lapListView.FindElementsByClassName("ListViewItem");
            Assert.IsNotNull(lapEntries);
            Assert.AreEqual(numberOfEntry, lapEntries.Count);

            var firstLapEntry = lapEntries[numberOfEntry - 1];
            var lastLapEntry = lapEntries[0];
            Assert.IsTrue(lastLapEntry.Displayed);
            Assert.IsFalse(firstLapEntry.Displayed);

            touchScreen.Scroll(lapListView.Coordinates, 0, -100);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(firstLapEntry.Displayed);
            Assert.IsFalse(lastLapEntry.Displayed);

            touchScreen.Scroll(lapListView.Coordinates, 0, 100);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(lastLapEntry.Displayed);
            Assert.IsFalse(firstLapEntry.Displayed);
        }

        [TestMethod]
        public void StopwatchStart()
        {
            var stopwatchResetButton = session.FindElementByAccessibilityId("StopWatchResetButton");

            var stopwatchTimer = session.FindElementByAccessibilityId("StopwatchTimerText");
            string stopwatchTimerText = stopwatchTimer.GetAttribute("Name");

            var stopwatchPlayPauseButton = session.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            Assert.AreEqual("Başlat",stopwatchPlayPauseButton.Text);
            Assert.IsFalse(stopwatchResetButton.Enabled);

            stopwatchPlayPauseButton.Click();
            

            stopwatchPlayPauseButton.Click();
            Assert.AreEqual("Başlat", stopwatchPlayPauseButton.Text);
            Assert.IsTrue(stopwatchResetButton.Displayed);
            Assert.IsTrue(stopwatchResetButton.Enabled);

            Assert.AreNotEqual(stopwatchTimerText, stopwatchTimer.GetAttribute("Name"));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);

            originalSize = session.Manage().Window.Size;
            Assert.IsNotNull(originalSize);
            session.Manage().Window.Size = new Size(550, 500);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            session.Manage().Window.Size = originalSize;

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();

            session.FindElementByAccessibilityId("StopwatchButton").Click();

            TestCleanup();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                session.FindElementByName("Duraklat").Click();
            }
            catch { }

            session.FindElementByAccessibilityId("StopWatchResetButton").Click();
        }
    }
}
