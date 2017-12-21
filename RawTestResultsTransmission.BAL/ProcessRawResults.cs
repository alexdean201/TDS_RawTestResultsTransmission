using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawTestResultsTransmission.DAL;
using System.Net;
using System.IO;
using System.Configuration;

namespace RawTestResultsTransmission.BAL
{
    public class ProcessRawResults
    {
        exam_auditEntities examAuditDb = new exam_auditEntities();
        List<exam_report> records = new List<exam_report>();
        List<TestResult> testResults = new List<TestResult>();
        TransmissionStatus transmissionStatus = new TransmissionStatus();

        public ProcessRawResults()
        {
            //GetRawResults();
        }

        public void GetRawResultRecords()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];
            if (transmissionStatus.CheckIfLastRunDateTimeIsValid())
            {
                // get records
                DateTime currentDateTime = DateTime.UtcNow;
                records = (from rec in examAuditDb.exam_report
                           where rec.status == "received" &&
                          (rec.created_at >= transmissionStatus.LastRunDateTime && rec.created_at <= currentDateTime)
                           select rec).ToList();
            }            
        }

        // Insert an oppId, extract the SSID, change the bank key to 210
        public void AlterData()
        {
            int i = 1;
            foreach(exam_report rec in records)
            {
                TestResult testResult = new TestResult();

                // insert oppId
                testResult.GenerateNewOppId(i.ToString());
                var record = rec.report.ToString();
                record = record.Replace("oppId=\"0\"", "oppId=\"" + testResult.OppId.ToString() + "\"");
                
                // get the SSID of the student
                var ssid = record.Substring(record.IndexOf("StudentIdentifier\" value=\"") + 26,
                                            record.IndexOf("contextDate", record.IndexOf("StudentIdentifier\" value=\"")) - 
                                           (record.IndexOf("StudentIdentifier\" value=\"") + 26) - 2);
                testResult.SSID = ssid;

                // change bank key
                record = record.Replace("bankKey=\"200\"", "bankKey=\"210\"");
                record = record.Replace("formKey=\"200", "formKey=\"210");

                testResult.TRT = record;

                testResults.Add(testResult);
                i++;
            }
        }

        

        public void SendTRT()
        {
            EtsService etsService = new EtsService();
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];
            etsService.Username = ConfigurationManager.AppSettings["EtsServiceUsername"];
            etsService.Password = ConfigurationManager.AppSettings["EtsServicePassword"];

            foreach (TestResult testResult in testResults)
            {
                // send record
                if (etsService.PostResults(testResult.TRT).Equals(HttpStatusCode.OK))
                {
                    testResult.TRTSent = true;
                }
                else
                {
                    testResult.TRTSent = false;
                }                
            }                               
        }

        public void UpdateLastRun()
        {
            transmissionStatus.RecordLastRunDateTime();
        }

        public void SendDigest()
        {
            Digest digest = new Digest();
            digest.PrepareBody(testResults);
            digest.Send();
        }
    }
}
