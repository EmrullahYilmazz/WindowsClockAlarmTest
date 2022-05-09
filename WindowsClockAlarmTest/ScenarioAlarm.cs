using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System.Threading;
using System;

namespace AlarmClockTest
{
    [TestClass]
    public class ScenarioAlarm : AlarmClockSession
    {
        private const string NewAlarmName = "Sample Test Alarm";

        [TestMethod]
        public void AlarmAdd()
        {
         
            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByAccessibilityId("AddAlarmButton").Click();

            session.FindElementByName("Alarm adı").Clear();
            session.FindElementByName("Alarm adı").SendKeys(NewAlarmName);

          
            session.FindElementByAccessibilityId("HourPicker").Click();
            WindowsElement hourSelector = session.FindElementByAccessibilityId("HourPicker");
            Thread.Sleep(TimeSpan.FromSeconds(1));

            hourSelector.SendKeys("3");
            Assert.AreEqual("03", hourSelector.Text);

            session.FindElementByAccessibilityId("MinutePicker").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            WindowsElement minuteSelector = session.FindElementByAccessibilityId("MinutePicker");
            minuteSelector.SendKeys("55");
            Assert.AreEqual("55", minuteSelector.Text);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            
            session.FindElementByAccessibilityId("SaveButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

          
            WindowsElement alarmEntry = session.FindElementByName("Alarmı düzenleyin,Sample Test Alarm,3:55,Sadece bir kere,");

            Assert.IsNotNull(alarmEntry);
            Assert.IsTrue(alarmEntry.Text.Contains("3"));
            Assert.IsTrue(alarmEntry.Text.Contains("55"));
            Assert.IsTrue(alarmEntry.Text.Contains(NewAlarmName));

          
            
        }

        [TestMethod]
        public void AlarmDelete()
        {
            WindowsElement alarmEntry = null;
            
            try
            {
                alarmEntry = session.FindElementByName("Alarmı düzenleyin,Sample Test Alarm,3:55,Sadece bir kere,");
            }
            catch
            {
                AlarmAdd();
                alarmEntry = session.FindElementByName("Alarmı düzenleyin,Sample Test Alarm,3:55,Sadece bir kere,");
            }

            Assert.IsNotNull(alarmEntry);
            touchScreen.LongPress(alarmEntry.Coordinates);
            session.FindElementByName("Sil").Click();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
      
            while (true)
            {
                try
                {
                    var alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewAlarmName}\")]");
                    session.Mouse.ContextClick(alarmEntry.Coordinates);
                    session.FindElementByName("Sil").Click();
                }
                catch
                {
                    break;
                }
            }

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            
            base.TestInit();

          
            session.FindElementByAccessibilityId("AlarmButton").Click();
        }
    }
}
