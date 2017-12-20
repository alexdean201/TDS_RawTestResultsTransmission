using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawTestResultsTransmission.BAL
{
    public class TestResult
    {
        public int OppId { get; set; }
        public string SSID { get; set; }
        public string TRT { get; set; }
        public bool TRTSent { get; set; }
        public string Message { get; set; }
        
        public void GenerateNewOppId(string placeValue)
        {
            // The raw TRT stored in the exam_audit.exam_report table has the oppId attribute value set to 0. As reported by Fairway in TDS-42:
            // The oppID was populated with a database auto-incrementing sequence id in prior to the June 2017 release.
            // The auto-incrementing database id did came from the DB queueing table the previous version of the TIS bridge used for submission notifications. 
            // Even though oppID was populated with a value greater than 1 during that time the oppID could have been populated with a random integer 
            // and nothing would have changed as TIS does not use it.Instead TDS now sends TRT's with oppID as 0 and TIS populates it when it does it's scoring.
            // All that being said I agree with Jeff that the open source Student application has never produced valid TRT's based on the XSD. 
            // We ported the same logic that resides in ReportingDLL creating the same TRT except for this one value since we no longer use a database table 
            // for queueing and messaging between applications.
            //
            // The Spanish test is not making use of the TIS, which is why the oppID remains 0. ETS requires that the oppID be greater than 0 and unique for each opportunity.
            // A method of generating oppId can be this: yyyymmdd + {numeric place value of the test result batch returned from the query}
            try {
                OppId = int.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + placeValue);
            }
            catch (Exception ex)
            {
                Message = "Exception in GenerationNewOppId for placeValue = '" + placeValue + "'. " + ex.Message;
                OppId = 0;
            }
        }
    }
}
