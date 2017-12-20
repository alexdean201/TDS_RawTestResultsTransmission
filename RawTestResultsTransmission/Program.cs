using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawTestResultsTransmission.BAL;
using NLog;

namespace RawTestResultsTransmission
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            // initialize app
            InitializeApp initializer = new InitializeApp();
            if (!initializer.Errors) // pass initialization?                
            {
                ProcessRawResults processor = new ProcessRawResults();

                processor.GetRawResultRecords();
                processor.GenerateOppIdAndExtractSSID();
                processor.SendTRT();
                processor.UpdateLastRun();
                processor.SendDigest();
            }
            else
            {
                _logger.Warn("Application initialization failed.");
            }            
        }
    }
}
