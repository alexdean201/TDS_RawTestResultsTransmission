using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawTestResultsTransmission.BAL;
using System.Configuration;

namespace RawTestResultsTransmission
{
    public class InitializeApp
    {
        EtsService etsService = new EtsService();
        TdsQueries tdsQueries = new TdsQueries();

        public bool Errors { get; set; }
        
        public InitializeApp()
        {
            // Check ETS service endpoint. If unable to connect, no need to continue with the checks
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];
            etsService.Username = ConfigurationManager.AppSettings["EtsServiceUsername"];
            etsService.Password = ConfigurationManager.AppSettings["EtsServicePassword"];

            if (etsService.CheckIfUrlIsValid() && etsService.CheckIfConnectionIsGood())
            {
                Errors = false;
            }
            else
            {
                Errors = true;
                return;
            }

            // Check if connection to TDS database is good
            tdsQueries.ConnectionString = ConfigurationManager.ConnectionStrings["exam_auditEntities"].ToString();
            if (tdsQueries.CheckIfConnectionIsGood() && tdsQueries.RunSimpleQuery())
            {
                Errors = false;
            }
            else
            {
                Errors = true;
                return;
            }

        }
    }
}
