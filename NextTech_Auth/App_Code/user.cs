using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for user
/// </summary>
public class user
{
    public String uid = Guid.NewGuid().ToString();
    public String first_name;
    public String last_name;
    public String altname;
    public String username;
    public String password;
    public String company;
    public String company_url;
    public String email;
    public String phone;
    public String street_address1;
    public String street_address2;
    public String city;
    public String prov_state;
    public String country;
    public String postal_zip;
    public String role;
    public String status;
    public String statusText;
    public String group;
    public String d_created;
    public String d_last_accessed;
    public String d_updated;
    public String err;

	public user()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public bool validateAccessHash(String accessHash)
    {
        if (accessHash == System.Configuration.ConfigurationManager.AppSettings["authAccessHash"].ToString())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void loadUserData(String svUID)
    {
        // loads up the variables for the user at svUID. // updated to load only active users.
        sqltransaction s = new sqltransaction();
        String svQString = "SELECT * FROM users WHERE id='" + svUID + "' AND status='100'";
        DataSet ds = s.doQueryWithResults(svQString, "userdata");
        if (ds.Tables["userdata"].Rows.Count == 1)
        {
            foreach (DataRow r in ds.Tables["userdata"].Rows)
            {
                uid = r["id"].ToString();
                first_name = r["first_name"].ToString();
                last_name = r["last_name"].ToString();
                altname = r["altname"].ToString();
                username = r["username"].ToString();
                password = saltedhash.ComputeHash(r["password"].ToString(), "MD5", Encoding.UTF8.GetBytes(r["id"].ToString()));
                company = r["company"].ToString();
                company_url = r["company_url"].ToString();
                email = r["email"].ToString();
                phone = r["phone_number"].ToString();
                street_address1 = r["street_address1"].ToString();
                street_address2 = r["street_address2"].ToString();
                city = r["city"].ToString();
                prov_state = r["prov_state"].ToString();
                country = r["country"].ToString();
                postal_zip = r["postal_zip"].ToString();
                role = r["role"].ToString();
                status = r["status"].ToString();
                group = r["group"].ToString();
                d_created = r["d_created"].ToString();
                d_last_accessed = r["d_last_accessed"].ToString();
                d_updated = r["d_updated"].ToString();
            }
        }
        else
        {
            err += ds.Tables["userdata"].Rows.Count + " Rows were retrieved for uid [" + svUID + "]";
        }

    }

    public void loadUserDataUP(String svUsername, String svPassword)
    {
        // loads up the variables for the user at svUID.

        sqltransaction s = new sqltransaction();
        String svQString = "SELECT * FROM users WHERE (username='" + svUsername + "')";
        DataSet ds = s.doQueryWithResults(svQString, "userdata");
        err = "User Not Found"; // protect against users not found
        if (ds.Tables["userdata"].Rows.Count >= 1)
        {
            foreach (DataRow r in ds.Tables["userdata"].Rows)
            {
                // looped because we need the user id to get the password
                if (saltedhash.ComputeHash(svPassword, "MD5", Encoding.UTF8.GetBytes(r["id"].ToString())) == r["password"].ToString())
                {
                    uid = r["id"].ToString();
                    first_name = r["first_name"].ToString();
                    last_name = r["last_name"].ToString();
                    altname = r["altname"].ToString();
                    username = r["username"].ToString();
                    password = r["password"].ToString();
                    company = r["company"].ToString();
                    company_url = r["company_url"].ToString();
                    email = r["email"].ToString();
                    phone = r["phone_number"].ToString();
                    street_address1 = r["street_address1"].ToString();
                    street_address2 = r["street_address2"].ToString();
                    city = r["city"].ToString();
                    prov_state = r["prov_state"].ToString();
                    country = r["country"].ToString();
                    postal_zip = r["postal_zip"].ToString();
                    role = r["role"].ToString();
                    status = r["status"].ToString();
                    statusText = "perform a soft get user by id for this info";
                    group = r["group"].ToString();
                    d_created = r["d_created"].ToString();
                    d_last_accessed = r["d_last_accessed"].ToString();
                    d_updated = r["d_updated"].ToString();
                    if (status == "100")
                    {
                        err = null; // all good, active user
                    }
                    else
                    {
                        // msgs for status conversion
                        switch (status)
                        {
                            case "200":
                                err = "This user is pending activation.";
                                break;
                            case "300":
                                err = "This user has been disabled.";
                                break;
                            case "400":
                                err = "This user has been deleted.";
                                break;
                            case "500":
                                err = "This user has been suspended.";
                                break;
                            default:
                                err = "This user is not active.";
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            err = ds.Tables["userdata"].Rows.Count + " rows were retrieved for user [" + svUsername + "]";
        }

    }

    public void loadUserDataUPbyID(String svID)
    {
        // loads up the variables for the user at svUID.
        sqltransaction s = new sqltransaction();
        String svQString = "SELECT u.*, s.name as statusText FROM users u INNER JOIN status s ON s.id=u.status WHERE u.id='" + svID + "'";
        DataSet ds = s.doQueryWithResults(svQString, "userdata");
        if (ds.Tables["userdata"].Rows.Count == 1)
        {
            foreach (DataRow r in ds.Tables["userdata"].Rows)
            {
                uid = r["id"].ToString();
                first_name = r["first_name"].ToString();
                last_name = r["last_name"].ToString();
                altname = r["altname"].ToString();
                username = r["username"].ToString();
                password = r["password"].ToString();
                company = r["company"].ToString();
                company_url = r["company_url"].ToString();
                email = r["email"].ToString();
                phone = r["phone_number"].ToString();
                street_address1 = r["street_address1"].ToString();
                street_address2 = r["street_address2"].ToString();
                city = r["city"].ToString();
                prov_state = r["prov_state"].ToString();
                country = r["country"].ToString();
                postal_zip = r["postal_zip"].ToString();
                role = r["role"].ToString();
                status = r["status"].ToString();
                statusText = r["statusText"].ToString();
                group = r["group"].ToString();
                d_created = r["d_created"].ToString();
                d_last_accessed = r["d_last_accessed"].ToString();
                d_updated = r["d_updated"].ToString();
                if (status == "300")
                {
                    err = "This user has been disabled.";
                }
            }
        }
        else
        {
            err += ds.Tables["userdata"].Rows.Count + " rows were retrieved for user [" + svID + "]";
        }

    }

    public void createUser(String firstname, String lastname, String nickname, String username, String password, String company, String company_website, String email, String phone, String address1, String address2, String city, String provstate, String country, String postalzip, String roleid, String statusid, String groupid)
    {
        // adds this user to the database.
        if (uid != "")
        {
            sqltransaction s = new sqltransaction();
            String svSQLQString = "INSERT INTO users ";
            svSQLQString += "(id, first_name, last_name, altname, username, password, company, company_url, email, phone_number, street_address1, street_address2, city, prov_state, country, postal_zip, role, status, [group], d_created)";
            svSQLQString += " VALUES ";
            svSQLQString += "('" + uid + "','" + firstname + "','" + lastname + "','" + nickname + "','" + username + "','" + saltedhash.ComputeHash(password, "MD5", Encoding.UTF8.GetBytes(uid)) + "','" + company + "','" + company_website + "','" + email + "','" + phone + "','" + address1 + "','" + address2 + "','" + city + "','" + provstate + "','" + country + "','" + postalzip + "','" + roleid + "','" + statusid + "','" + groupid + "',GETDATE())";
            Boolean userCreated = s.doQueryNoResults(svSQLQString);
        }
    }

    public void modifyUser(String id, String firstname, String lastname, String nickname, String username, String password, String company, String company_website, String email, String phone, String address1, String address2, String city, String provstate, String country, String postalzip, String roleid, String statusid, String groupid)
    {
        // updates the user with all information changes except id, d_created and d_last_accessed
        if (id != "")
        {
            uid = id;
            //sqltransaction s = new sqltransaction();
            //String svSQLQString = "UPDATE users SET ";
            //svSQLQString += "first_name='" + firstname + "', last_name='" + lastname + "', altname='" + nickname + "', username='" + username + "', password='" + saltedhash.ComputeHash(password, "MD5", Encoding.UTF8.GetBytes(uid)) + "', company='" + company + "', company_url='" + company_website + "', email='" + email + "', street_address1='" + address1 + "', street_address2='" + address2 + "', city='" + city + "', prov_state='" + provstate + "', country='" + country + "', postal_zip='" + postalzip + "', role='" + roleid + "', status='" + statusid + "', [group]='" + groupid + "', d_updated=GETDATE()";
            //svSQLQString += " WHERE ";
            //svSQLQString += " (id='" + uid + "')";
            //Boolean userCreated = s.doQueryNoResults(svSQLQString);

            if (password != "")
            {
                sqltransaction s = new sqltransaction();
                String svSQLQString = "UPDATE users SET ";
                svSQLQString += "first_name='" + firstname + "', last_name='" + lastname + "', altname='" + nickname + "', username='" + username + "', password='" + saltedhash.ComputeHash(password, "MD5", Encoding.UTF8.GetBytes(uid)) + "', company='" + company + "', company_url='" + company_website + "', email='" + email + "', phone_number='" + phone + "', street_address1='" + address1 + "', street_address2='" + address2 + "', city='" + city + "', prov_state='" + provstate + "', country='" + country + "', postal_zip='" + postalzip + "', role='" + roleid + "', status='" + statusid + "', [group]='" + groupid + "', d_updated=GETDATE()";
                svSQLQString += " WHERE ";
                svSQLQString += " (id='" + uid + "')";
                Boolean userCreated = s.doQueryNoResults(svSQLQString);
            }
            else
            {
                sqltransaction s = new sqltransaction();
                String svSQLQString = "UPDATE users SET ";
                svSQLQString += "first_name='" + firstname + "', last_name='" + lastname + "', altname='" + nickname + "', username='" + username + "', company='" + company + "', company_url='" + company_website + "', email='" + email + "', phone_number='" + phone + "', street_address1='" + address1 + "', street_address2='" + address2 + "', city='" + city + "', prov_state='" + provstate + "', country='" + country + "', postal_zip='" + postalzip + "', role='" + roleid + "', status='" + statusid + "', [group]='" + groupid + "', d_updated=GETDATE()";
                svSQLQString += " WHERE ";
                svSQLQString += " (id='" + uid + "')";
                Boolean userCreated = s.doQueryNoResults(svSQLQString);
            }
        }
    }

    public void softDeleteUser(String svUID)
    {
        // set the user's status to disabled.
        if (svUID != "")
        {
            sqltransaction s = new sqltransaction();
            String svSQLQString = "UPDATE users SET ";
            //svSQLQString += "status='300', d_updated=GETDATE()";
            svSQLQString += "status='400', d_updated=GETDATE()";
            svSQLQString += " WHERE ";
            svSQLQString += " (id='" + svUID + "')";
            Boolean userCreated = s.doQueryNoResults(svSQLQString);
        }
    }

    public void reActivateUser(String svUID)
    {
        // set the user's status to disabled.
        if (svUID != "")
        {
            sqltransaction s = new sqltransaction();
            String svSQLQString = "UPDATE users SET ";
            svSQLQString += "status='100', d_updated=GETDATE()";
            svSQLQString += " WHERE ";
            svSQLQString += " (id='" + svUID + "')";
            Boolean userCreated = s.doQueryNoResults(svSQLQString);
        }
    }

    public void hardDeleteUser(String svUID)
    {
        // delete the user's record.
        if (svUID != "")
        {
            // protect the built-in admin
            if (svUID != "965efdec-6254-4664-85e7-6b13e8f795d8")
            {
                sqltransaction s = new sqltransaction();
                String svSQLQString = "DELETE FROM users ";
                svSQLQString += " WHERE ";
                svSQLQString += " (id='" + svUID + "')";
                Boolean userCreated = s.doQueryNoResults(svSQLQString);
                // add any other special conditions here.
            }
        }
    }
}
