using System;
using System.IO;

namespace DriverLibrary
{
    public class Report
    {
        private string path;
        private string reportName;


        public Report()
        {
            //Declare the path where report will be saved
            path = @"C:\Report\";


            //Check if directory exists
            if (!Directory.Exists(path))
            {
                //Create the directory if it does not exist
                Directory.CreateDirectory(path);
            }
            string timeStamp = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss");
            reportName = "report" + timeStamp + ".html";

            CreateNewReport();
        }

        /// <summary>
        /// Starts a new HTML report
        /// </summary>
        public void CreateNewReport()
        {
            AddToReport("<html><head><title>Automation Report</title><style>table, td {border: 1px solid black;}</style></head><body><table><tr><th colspan=\"6\">Automation Results</th></tr><tr></tr><tr><td>Test Name</td><td>Expected Outcome</td><td>Start Time</td><td>End Time</td><td>Actual Outcome</td><td>Test Result</td></tr>");
        }


        /// <summary>
        /// Add new content to the report
        /// </summary>
        /// <param name="reportUpdate">Content to be added to the report</param>
        public void AddToReport(string reportUpdate)
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(path, reportName), true))
            {
                //Write the message, function and exception to the log file
                file.WriteLine(reportUpdate);
            }
        }

        /// <summary>
        /// Closes the HTML report
        /// </summary>
        public void EndReport()
        {
            AddToReport("</table></body></html>");
        }

        /// <summary>
        /// Sets the end time for the test and adds the test to the report
        /// </summary>
        /// <param name="test"></param>
        public void AddTestCaseToReport(TestCase test)
        {
            test.SetEndTime(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            AddToReport(test.ToString());
        }

    }
}
