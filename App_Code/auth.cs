using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for auth
/// </summary>
[WebService(Namespace = "http://www.edcetratraining.com/ET_TOOLS")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class auth : System.Web.Services.WebService {

    public auth () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public user getUser(String username, String password, String id, String accessHash)
    {
       user u = new user();
       if (u.validateAccessHash(accessHash))
        {
            if (id == "")
            {
                u.loadUserDataUP(username, password);
            }
            else
            {
                u.loadUserDataUPbyID(id);
            }
        }
        return u;

    }

    [WebMethod]
    public void renewUser(String userId)
    {
        // updates a user account after renewal
        sqltransaction s = new sqltransaction();
        String svSQLQString = "UPDATE users SET d_created=GETDATE() WHERE (id='" + userId + "')";
        Boolean renewed = s.doQueryNoResults(svSQLQString);
    }

    [WebMethod]
    public void userOptedOut(String userId)
    {
        // updates a user account after renewal
        sqltransaction s = new sqltransaction();
        String svSQLQString = "UPDATE users SET optout=1 WHERE (id='" + userId + "')";
        Boolean renewed = s.doQueryNoResults(svSQLQString);
    }

    [WebMethod]
    public void userOptedIn(String userId)
    {
        // updates a user account after renewal
        sqltransaction s = new sqltransaction();
        String svSQLQString = "UPDATE users SET optout=0 WHERE (id='" + userId + "')";
        Boolean renewed = s.doQueryNoResults(svSQLQString);
    }


    [WebMethod]
    public user setUser(String firstname, String lastname, String nickname, String username,
        String password, String company, String company_website, String email, String phone, 
        String address1, String address2, String city, String provstate, String country,
        String postalzip, String roleid, String statusid, String groupid, String id, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            u = getUser(username, password, id, accessHash);

            if (u.err == "" || u.err == null)
            {
                u.modifyUser(id, firstname, lastname, nickname, username, password, company, company_website, email, phone, address1, address2, city, provstate, country, postalzip, roleid, statusid, groupid);
            }
            else
            {
                u.createUser(firstname, lastname, nickname, username, password, company, company_website, email, phone, address1, address2, city, provstate, country, postalzip, roleid, statusid, groupid);
            }

            return u;
        }
        else
        {
            return u;
        }
    }

    [WebMethod]
    public user removeUser(String id, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            u.softDeleteUser(id);
            u.err = "User with id [" + id + "] disabled";
        }

        return u;
    }

    [WebMethod]
    public user deleteUser(String id, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            u.hardDeleteUser(id);
            u.err = "User with id [" + id + "] deleted permanently";
        }

        return u;
    }

    [WebMethod]
    public Boolean updateLastAccessUser(String id, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            // updates the user with all information changes except id, d_created and d_last_accessed
            sqltransaction s = new sqltransaction();
            String svSQLQString = "UPDATE users SET d_last_accessed=GETDATE()";
            svSQLQString += " WHERE ";
            svSQLQString += " (id='" + id + "')";
            Boolean userCreated = s.doQueryNoResults(svSQLQString);

            return userCreated;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public Boolean initialUpdatePass(String id, String pass, String accessHash)
    {
         user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            sqltransaction s = new sqltransaction();
            String svSQLQString = "UPDATE users SET password='" + pass + "'";
            svSQLQString += " WHERE ";
            svSQLQString += " (id='" + id + "')";
            Boolean userUpdate = s.doQueryNoResults(svSQLQString);

            return userUpdate;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public DataSet getGroups(String lang, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            if (lang == "")
            {
                lang = "en";
            }
            String tn = "groups";
            String qs = "SELECT id, name FROM groups WHERE language='" + lang + "' ORDER BY name";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public string addGroup(String name, String lang, int parentid, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String newId = "Error";
            sqltransaction s = new sqltransaction();
            String qs = "SELECT MAX(id) as maxid FROM groups";
            String tn = "maxid";
            DataSet ds = s.doQueryWithResults(qs, tn);
            // get the highest id, then add 100
            int maxid = int.Parse(ds.Tables[tn].Rows[0][tn].ToString());
            int newid = maxid + 100;
            qs = "INSERT INTO groups (id, name, language, parent) VALUES (" + newid + ", '" + name + "','" + lang + "', " + parentid + ")";
            bool c = s.doQueryNoResults(qs);
            if (c)
            {
                newId = newid.ToString();
            }

            return newId;
        }
        else
        {
            return "";
        }
    }

    [WebMethod]
    public bool setGroup(int id, String name, String lang, int parentid, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            sqltransaction s = new sqltransaction();
            String qs = "UPDATE groups SET name='" + name + "', language='" + lang + "', parent=" + parentid + " WHERE id=" + id + "";
            bool c = s.doQueryNoResults(qs);

            return c;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public bool removeGroup(int id, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            sqltransaction s = new sqltransaction();
            String qs = "DELETE FROM groups WHERE id=" + id + "";
            bool c = s.doQueryNoResults(qs);

            //will need to put in code for if removing group with children then to update all children to root parent

            return c;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public DataSet getRoles(String lang, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            if (lang == "")
            {
                lang = "en";
            }
            String tn = "roles";
            String qs = "SELECT id, name FROM roles WHERE language='" + lang + "' ORDER BY id DESC";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public String getRolesName(String lang, String roleid, String accessHash)
    {
        user u = new user();
        String myRoleName = "";
        if (u.validateAccessHash(accessHash))
        {
            if (lang == "")
            {
                lang = "en";
            }
            String tn = "roles";
            String qs = "SELECT name FROM roles WHERE (language='" + lang + "') AND (id='" + roleid + "')";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);
            if (ds.Tables[tn].Rows.Count > 0)
            {
                myRoleName = ds.Tables[tn].Rows[0]["name"].ToString();
            }
            
        }
        return myRoleName;
    }

    [WebMethod]
    public DataSet getStatus(String lang, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            if (lang == "")
            {
                lang = "en";
            }
            String tn = "status";
            String qs = "SELECT id, name FROM status WHERE language='" + lang + "' ORDER BY name";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public DataSet getActiveUserList(String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT * FROM users WHERE status=100 ORDER BY username";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public DataSet searchActiveUserList(String searchTerm, String accessHash)
    {

        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT * FROM users WHERE status=100 AND (first_name LIKE '%" + searchTerm + "%' OR last_name LIKE '%" + searchTerm + "%') ORDER BY username";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public String getActiveUserIdByEmail(String emailAddress, String accessHash)
    {

        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT TOP (1) * FROM users WHERE status=100 AND email='" + emailAddress + "'";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    [WebMethod]
    public DataSet searchActiveUserListByFacet(String firstName, String lastName, String email, String username, String city,
        String prov, String country, String userType, String companyId, String accessHash)
    {

        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT * FROM users WHERE (status=100) ";
            String fa = "AND ";
            if (firstName == "" && lastName == "" && email == "" && username == "" && city == "" && prov == "" && country == "" && userType == "" && companyId == "")
            {
                return new DataSet();
            }
            else
            {
                if (firstName != "")
                {
                    qs += fa + "(first_name LIKE '%" + firstName + "%') ";
                    fa = "AND ";
                }
                if (lastName != "")
                {
                    qs += fa + "(last_name LIKE '%" + lastName + "%') ";
                    fa = "AND ";
                }
                if (email != "")
                {
                    qs += fa + "(email='" + email + "') ";
                    fa = "AND ";
                }
                if (username != "")
                {
                    qs += fa + "(username LIKE '%" + username + "%') ";
                    fa = "AND ";
                }
                if (city != "")
                {
                    qs += fa + "(city='" + city + "') ";
                    fa = "AND ";
                }
                if (prov != "")
                {
                    qs += fa + "(prov_state='" + prov + "') ";
                    fa = "AND ";
                }
                if (country != "")
                {
                    qs += fa + "(country='" + country + "') ";
                    fa = "AND ";
                }
                if (userType != "")
                {
                    qs += fa + "(role='" + userType + "') ";
                    fa = "AND ";
                }
                if (companyId != "")
                {
                    int x;
                    if (int.TryParse(companyId, out x))
                    {
                        qs += fa + "(company='" + companyId + "') ";
                    }
                    else
                    {
                        qs += fa + "(company LIKE '%" + companyId + "%') ";
                    }
                    
                    fa = "AND ";
                }
                qs += "ORDER BY last_name";
                sqltransaction s = new sqltransaction();
                DataSet ds = s.doQueryWithResults(qs, tn);

                return ds;
            }
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public DataSet getAllUserList(String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT * FROM users ORDER BY username";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public DataSet getNonActiveUserList(String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT * FROM users WHERE (status>100) ORDER BY d_created";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }

    [WebMethod]
    public bool compareUserPass(String id, String password, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            u.loadUserDataUPbyID(id);
            bool passCompared = false;

            if (saltedhash.ComputeHash(password, "MD5", Encoding.UTF8.GetBytes(u.uid.ToString())) == u.password.ToString())
            {
                passCompared = true;
            }

            return passCompared;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public DataSet getUsersByCompany(String companyid, String accessHash)
    {
        user u = new user();
        if (u.validateAccessHash(accessHash))
        {
            String tn = "users";
            String qs = "SELECT u.*, r.name as roleName FROM users u INNER JOIN roles r ON u.role=r.id WHERE u.company='" + companyid + "' AND r.language='en' AND u.status='100' ORDER BY u.last_name";
            sqltransaction s = new sqltransaction();
            DataSet ds = s.doQueryWithResults(qs, tn);

            return ds;
        }
        else
        {
            return new DataSet();
        }
    }
}

