using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Services.Scheduling;
using System.Data.SqlClient;

namespace ATR.CleanDBlogSchedull
{
    public class CleanDBlog : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        public int getSQLresponseDBsizeInt = 0;
public string getSQLresponseDBsizeStr = "";
public StringBuilder logEntryStrB = new StringBuilder("");
public string thisDBname;

        public CleanDBlog(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
            : base()
        {
            ScheduleHistoryItem = objScheduleHistoryItem;
        }
        public override void DoWork()
        {
            try
            {
                //--start the pocessing
                this.Progressing();
                logEntryStrB.Append("ATR Clean DB log. Success. <br />Size before clean = ");
                //--call the function/process that you wish to be executed
                ExecuteCleanDBlog();

                //--intimate the schedule mechanism to write log note in schedule history
               // StringBuilder logEntryStrB = new StringBuilder("");

                
                //logEntryStrB.Append(getSQLresponseDBsizeStr);
                String LogEntry = logEntryStrB.ToString() ;
                this.ScheduleHistoryItem.AddLogNote(LogEntry);

                this.ScheduleHistoryItem.Succeeded = true;
            }
            catch (Exception Ex)
            {
                //--intimate the schedule mechanism to write log note in schedule history
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("ATR Clean DB log. Failed. " + Ex.ToString());
                this.Errored(ref Ex);
            }
        }

        //--function which needs tobe executed at specified interval
        public void ExecuteCleanDBlog()
        {
            try
            {
                //System.IO.File.WriteAllText(@"C:\\HostingSpaces\\dec-128\\galilmaaravi.co.il\\wwwroot\\Portals\\0\\imports\\GMService" + Guid.NewGuid().ToString() + ".txt",
                //                            "Galil Maaravi Import Containers details Service. Ran at - " + DateTime.Now.ToString() + " BULK INSERT cont FROM \'C:\\HostingSpaces\\dec-128\\galilmaaravi.co.il\\wwwroot\\Portals\\0\\imports\\cont.txt\' WITH (FIELDTERMINATOR = \'\\t\',ROWTERMINATOR = \'\\n\')");

                // ************************
                // drop the cont, contact and partbal tables


                string connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //sp_spaceused
                    //ALTER DATABASE [connection.DataSource] SET RECOVERY simple
                    //dbcc shrinkdatabase ([connection.DataSource], TRUNCATEONLY)
                    //TRUNCATE TABLE eventlog
                    //TRUNCATE TABLE sitelog
                    //DBCC SHRINKDATABASE ([connection.DataSource]);
                    //sp_spaceused
                    //string GetDBsize = "sp_spaceused";


                    //string addImportedDataToPartBalTbl = "BULK INSERT partbal FROM 'c:\\Local Sites\\GalilMaaraviDev\\Portals\\0\\partbal.txt' WITH (FIELDTERMINATOR = '\\t',ROWTERMINATOR = '\\n')";
                    connection.Open();
                    /// *** excute command with no data return
                    //SqlCommand GetDBsizeCmd = new SqlCommand(GetDBsize, connection);
                    //getSQLresponseDBsizeInt = GetDBsizeCmd.ExecuteNonQuery();
                    // 1. Instantiate a new command with a query and connection
                    SqlCommand sp_spaceusedBeforeCmd = new SqlCommand("sp_spaceused", connection);
                    // 2. Call Execute reader to get query results
                    SqlDataReader rdr = sp_spaceusedBeforeCmd.ExecuteReader();
                    // print the CategoryName of each record
                    while (rdr.Read())
                    {
                        logEntryStrB.Append("<br />database_name: ");
                        logEntryStrB.Append (rdr.GetSqlString(0).Value);
                        thisDBname = rdr.GetSqlString(0).Value;
                        logEntryStrB.Append("<br />database_size: ");
                        logEntryStrB.Append(rdr.GetSqlString(1).Value);
                        logEntryStrB.Append(" <br />unallocated space: ");
                        logEntryStrB.Append(rdr.GetSqlString(2).Value);
                    }
                    connection.Close();
                    connection.Open();
                    // *** Do the cleaning *****
                    //ALTER DATABASE [connection.DataSource] SET RECOVERY simple
                    //dbcc shrinkdatabase ([connection.DataSource], TRUNCATEONLY)
                    //TRUNCATE TABLE eventlog
                    //TRUNCATE TABLE sitelog
                    //DBCC SHRINKDATABASE ([connection.DataSource]);
                    //StringBuilder cleanDBstring = null;
                    //cleanDBstring.Append("ALTER DATABASE [");
                    //cleanDBstring.Append(thisDBname);
                    //cleanDBstring.Append("] SET RECOVERY simple;");
                    //cleanDBstring.Append("dbcc shrinkdatabase ([");
                    //cleanDBstring.Append(thisDBname);
                    //cleanDBstring.Append("], TRUNCATEONLY);");
                    //cleanDBstring.Append("TRUNCATE TABLE eventlog;");
                    //cleanDBstring.Append("TRUNCATE TABLE sitelog;");
                    //cleanDBstring.Append("DBCC SHRINKDATABASE ([");
                    //cleanDBstring.Append(thisDBname);
                    //cleanDBstring.Append("]);");



                    string cleanDBstring1 = "ALTER DATABASE [" + thisDBname + "] SET RECOVERY simple; dbcc shrinkdatabase ([" + thisDBname + "], TRUNCATEONLY); TRUNCATE TABLE eventlog; TRUNCATE TABLE sitelog; DBCC SHRINKDATABASE ([" + thisDBname + "]);";
                    SqlCommand CleanDBCmd = new SqlCommand(cleanDBstring1.ToString(), connection);
                    int getSQLresponseForCleanCmd = CleanDBCmd.ExecuteNonQuery();
                    // *** End the cleaning *****

                    // 1. Instantiate a new command with a query and connection
                    SqlCommand sp_spaceusedAfterCmd = new SqlCommand("sp_spaceused", connection);
                    // 2. Call Execute reader to get query results
                    SqlDataReader rdrAfter = sp_spaceusedAfterCmd.ExecuteReader();
                    // print the CategoryName of each record
                    while (rdrAfter.Read())
                    {
                        logEntryStrB.Append("<br />--- After ---   <br />database_name: ");
                        logEntryStrB.Append(rdrAfter.GetSqlString(0).Value);
                        logEntryStrB.Append("<br />database_size: ");
                        logEntryStrB.Append(rdrAfter.GetSqlString(1).Value);
                        logEntryStrB.Append(" <br />unallocated space: ");
                        logEntryStrB.Append(rdrAfter.GetSqlString(2).Value);
                    }

                    logEntryStrB.Append(thisDBname);

                    connection.Close();

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

    }
}
