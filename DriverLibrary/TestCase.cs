using System;

namespace DriverLibrary
{
    public class TestCase
    {
        private string testName;
        private string expectedOutcome;
        private string actualOutcome;
        private string startTime;
        private string endTime;
        private string testResult;
        private string color;

        /// <summary>
        /// Creates an instance of Test case and sets the start time
        /// </summary>
        /// <param name="name">Name of the current test</param>
        /// <param name="outcome">Expected outcome of the current test</param>
        public TestCase(string name, string outcome)
        {
            SetTestName(name);
            SetExpectedOutcome(outcome);
            SetStartTime();
        }

        /// <summary>
        /// Sets the test name
        /// </summary>
        /// <param name="name">Name of the current test</param>
        public void SetTestName(string name)
        {
            testName = name;
        }

        /// <summary>
        /// Sets the expected outcome
        /// </summary>
        /// <param name="outcome">Expected outcome of the current test</param>
        public void SetExpectedOutcome(string outcome)
        {
            expectedOutcome = outcome;
        }

        /// <summary>
        /// Sets the start time for the current test
        /// </summary>
        public void SetStartTime()
        {
            startTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Sets the actual outcome of the test
        /// </summary>
        /// <param name="outcome">Actual outcome of the test</param>
        public void SetActualOutcome(string outcome)
        {
            actualOutcome = outcome;
        }

        /// <summary>
        /// Sets the end time of the current test
        /// </summary>
        /// <param name="time">End time of the current test</param>
        public void SetEndTime(string time)
        {
            endTime = time;
        }

        /// <summary>
        /// Sets the test result for the current test
        /// </summary>
        /// <param name="result">Result of the current test either true or false</param>
        public void SetTestResult(bool result)
        {
            if (result)
            {
                testResult = "PASS";
                color = "green";
            }
            else
            {
                testResult = "FAIL";
                color = "red";
            }
        }

        /// <summary>
        /// Creates a string in the correct HTML format for the report
        /// </summary>
        /// <returns>A HTML string that contains all the test details</returns>
        public string ToString()
        {
            return "<tr><td>" + testName + "</td><td>" + expectedOutcome + "</td><td>" + startTime + "</td><td>" + endTime + "</td><td>" + actualOutcome + "</td><td bgcolor=" + color + ">" + testResult + "</td></tr>";
        }
    }
}
