using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RawTestResultsTransmission.BAL
{
    public class TransmissionStatus
    {
        public enum StatusCommunicationCode
        {
            OK,
            FAIL
        };

        public string Message { get; set; }
        public string LastRunStatusFile { get; set; }
        public DateTime LastRunDateTime { get; set; }
        public DateTime LastRunDefaultDateTime { get; set; }
        public StatusCommunicationCode LastRunStatusCommunication { get; set; }

        public bool LastRunStatusFileExists()
        {
            try
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile))
                {
                    Message = "Last run status file does not exist at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'.";
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                Message = "Last run status file does not exist at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'. " + ex.Message;
                return false;
            }
        }

        public int CreateLastRunStatusFile()
        {
            if (!LastRunStatusFileExists())
            {
                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile.Split('\\').First());
                    File.Create(LastRunStatusFile);
                    return 1;
                }
                catch (Exception ex)
                {
                    Message = "Failed to create the directory and/or file at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'. " + ex.Message;
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }

        public bool CheckIfNeedToUseDefaultLastRunDateTime()
        {
            if (LastRunStatusFileExists())
            {
                try
                {
                    using (StreamReader sr = File.OpenText(LastRunStatusFile))
                    {
                        string s = sr.ReadLine();
                        if (s.Equals(""))
                        {
                            LastRunDateTime = LastRunDefaultDateTime;
                            return true;
                        }
                        else
                        {
                            LastRunDateTime = DateTime.Parse(s);                            
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message = "Exception while reading the file at '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + 
                              "'. The file is empty or has an incorrect datetime value. Using the default first last run datetime '" + LastRunDefaultDateTime + "'. "+ ex.Message;
                    LastRunDateTime = LastRunDefaultDateTime;
                    return true;
                }
            }
            else
            {
                Message = "Last run status file does not exist at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'.";
                return false;
            }
        }

        public bool CheckIfLastRunDateTimeIsValid()
        {
            if (LastRunStatusFileExists())
            {
                try
                { 
                    using (StreamReader sr = File.OpenText(LastRunStatusFile))
                    {
                        string s = sr.ReadLine();
                        try
                        {
                            LastRunDateTime = DateTime.Parse(s);
                            sr.Close();
                            return true;
                        }
                        catch (Exception parseEx)
                        {
                            sr.Close();
                            Message = "Exception parsing '" + s + "' into DateTime. " + parseEx.Message;
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message = "Exception while reading the file at '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'. " + ex.Message;
                    return false;
                }
            }
            else
            {
                Message = "Last run status file does not exist at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'.";
                return false;
            }
        }

        public int RecordLastRunDateTime()
        {
            if (LastRunStatusFileExists())
            {
                try
                {
                    File.WriteAllText(LastRunStatusFile, DateTime.UtcNow.ToString());
                    return 1;
                }
                catch (Exception ex)
                {
                    Message = "Exception when writing the last run datetime to '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'. " + ex.Message;
                    return 0;
                }
            }
            else
            {
                Message = "Last run status file does not exist at: '" + Directory.GetCurrentDirectory() + "\\" + LastRunStatusFile + "'.";
                return 0;
            }
        }        
    }
}
