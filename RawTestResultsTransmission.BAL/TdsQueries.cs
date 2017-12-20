using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawTestResultsTransmission.DAL;

namespace RawTestResultsTransmission.BAL
{
    public class TdsQueries
    {
        public string ConnectionString { get; set; }
        public string Message { get; set; }

        public bool CheckIfConnectionIsGood()
        {
            exam_auditEntities examAuditDb = new exam_auditEntities();

            try
            {
                examAuditDb.Database.Connection.Open();
                examAuditDb.Database.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Message = "Exception in opening a connection to the exam_audit database. " + ex.Message;
                return false;
            }
        }

        public bool RunSimpleQuery()
        {
            exam_auditEntities examAuditDb = new exam_auditEntities();

            try
            {
                var recCount = (from rec in examAuditDb.exam_report select rec).ToList();
                return true;
            }
            catch (Exception ex)
            {
                Message = "Exception in running a simple record count query. " + ex.Message;
                return false;
            }
        }

        public int NumberOfNewTestResults(DateTime lastRunDateTime)
        {
            exam_auditEntities examAuditDb = new exam_auditEntities();

            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                var records = (from rec in examAuditDb.exam_report where rec.status == "received" && 
                               (rec.created_at >= lastRunDateTime && rec.created_at <= currentDateTime)
                               select rec).ToList();
                return records.Count;
            }
            catch (Exception ex)
            {
                Message = "Exception in finding the number of new test results. " + ex.Message;
                return -1;
            }
        }
    }
}
