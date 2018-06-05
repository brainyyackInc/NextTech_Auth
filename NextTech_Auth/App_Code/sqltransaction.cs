using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Class1
/// </summary>
public class sqltransaction
{
    // values to share among the methods
    private string svConn = System.Configuration.ConfigurationManager.ConnectionStrings["ET_AUTHConnectionString"].ToString();
    public string errorText = ""; // trap the errors, on fail these can be read by the page code.

    public sqltransaction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // use this to run SELECT queries where data return is needed.
    // NOTE: class common queries in another object, and use this to reduce 
    //       code volume in those classes.  ErrorText should pass through to the constructor.
    public DataSet doQueryWithResults(string svQueryString, string svDataTableName)
    {
        // construct this for the return.
        DataSet dsResult = new DataSet();
        // create a new SqlConnection object with the appropriate connection string server[localhost\SQLEXPRESS]
        SqlConnection sqlConn = new SqlConnection();
        sqlConn.ConnectionString = svConn;
        // open the connection
        try
        {
            sqlConn.Open();

            // start a command
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            // build a statement
            sqlComm.CommandText = svQueryString;
            sqlComm.CommandType = CommandType.Text;
            // do SQL and open a reader
            // create an SqlDataAdapter to pass data to the dataset
            SqlDataAdapter tmpSqlDataAdapter = new SqlDataAdapter();
            tmpSqlDataAdapter.SelectCommand = sqlComm;
            // fill the data table with the result.
            tmpSqlDataAdapter.Fill(dsResult, svDataTableName);
            sqlComm.Dispose();
            tmpSqlDataAdapter.Dispose();
        }
        finally
        {
            sqlConn.Close();
        }

        return dsResult;
    }

    // use this to execute DELETE, INSERT, UPDATE statements where no data return is needed.
    public Boolean doQueryNoResults(string svQueryString)
    {
        // construct this for the return.
        Boolean success = new Boolean();
        // create a new SqlConnection object with the appropriate connection string server[localhost\SQLEXPRESS]
        SqlConnection sqlConn = new SqlConnection();
        sqlConn.ConnectionString = svConn;
        // open the connection
        try
        {
            sqlConn.Open();
            // start a command
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            // build a statement
            sqlComm.CommandText = svQueryString;
            sqlComm.CommandType = CommandType.Text;
            // do SQL and open a reader
            int rows = sqlComm.ExecuteNonQuery();

            if (rows != 1)
            {
                // PANIC! it didn't happen, the constructor can read the errorText value.
                errorText = "PANIC! There were no rows affected by the query.";
                success = false;
            }
            else
            {
                errorText = sqlComm.CommandText.ToString();
                success = true;
            }
            // kill the command
            sqlComm.Dispose();

        }
        finally
        {
            // close the connection
            sqlConn.Close();
            // close the connection
        }
        return success;
    }

}
