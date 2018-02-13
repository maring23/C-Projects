using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace MyClassCollection
{
    class MySQLDBUtilities
    {
        MySqlConnection conn;
        MySqlCommand com;
        MySqlDataAdapter myAdapter;
        ConnectionStringSolution css = new ConnectionStringSolution();

        public MySQLDBUtilities()
        {
            com = new MySqlCommand();
        }

        public MySqlConnection OpenConnection()
        {
            string connString = "SERVER=localhost;" +
                                "PORT=3306;" +
                                "DATABASE=scoring_db;" +
                                "USERNAME=root;" +
                                "PASSWORD=qwerty;";
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            try
            {
                conn.Open();
                return conn;
            }
            catch
            {
                return null;
            }
        }

        public void CloseConnection()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string GetSettings()
        {
            string temp = "";
            try
            {
                temp = css.ReadRegistryKey("ConnectionString", "mysqllao");
            }
            catch (Exception e)
            {

            }
            return temp;
        }

        public DataTable SelectTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                myAdapter = new MySqlDataAdapter(query, OpenConnection());
                myAdapter.Fill(dt);
                myAdapter.Dispose();
                CloseConnection();
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public void InsertQuery(string query)
        {
            com.CommandText = query;
            try
            {
                com.Connection = OpenConnection();
                int res = com.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /*public string[] LoadComboBox(string query, string colname)
        {
            List<string> temp = new List<string>();
            DataTable dt = SelectTable(query);
            foreach (DataRow r in dt.Rows)
                temp.Add(r[colname].ToString());
            if (temp.Count == 0)
                temp.Add("");
            return temp.ToArray();
        }
        public string[] GetSingleColumnDatas(string query, string colname)
        {
            string[] temp = LoadComboBox(query, colname);
            string[] tel = { "" };
            if (temp.Length == 0)
                temp = new string[] { "" };
            return temp;
        }*/
        public long GetID(string query, string colname)
        {
            DataTable dt = SelectTable(query);
            if (dt.Rows.Count != 0)
            {
                DataRow r = dt.Rows[0];
                long i = Convert.ToInt64(r[colname].ToString());
                return i;
            }
            else
                return 0;
        }
        public void BindDataToComboBox(string query,ComboBox cmb,string columnKey,string columnValue)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DataTable dt = SelectTable(query);
            foreach(DataRow r in dt.Rows)
            {
                dictionary.Add(r[columnKey].ToString(), r[columnValue].ToString());
            }
            if (dt.Rows.Count > 0)
            {
                cmb.DataSource = new BindingSource(dictionary, null);
                cmb.ValueMember = "key";
                cmb.DisplayMember = "value";
            }
        }

        //Current Project based Function

        public void SPInsertUser(string username, string userpassword, string usertype, long judgeid,bool IsInsert,long userid)
        {
            com.Parameters.Clear();
            string procedurename = "insert_user";
            com.CommandText = procedurename;
            com.Connection = OpenConnection();
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Add(new MySqlParameter("?username1",username));
            com.Parameters.Add(new MySqlParameter("?userpassword1",userpassword));
            com.Parameters.Add(new MySqlParameter("?usertype1",usertype));
            com.Parameters.Add(new MySqlParameter("?judgeid1",judgeid));
            com.Parameters.Add(new MySqlParameter("?isinsert", (IsInsert?1:0)));
            com.Parameters.Add(new MySqlParameter("?userid1", userid));

            com.ExecuteNonQuery();
            CloseConnection();
        }
        public void SPInsertJudge(string fullname,string remarks, bool IsInsert, long judgeid)
        {
            com.Parameters.Clear();
            string procedurename = "insert_judge";
            com.CommandText = procedurename;
            com.Connection = OpenConnection();
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Add(new MySqlParameter("?fullname1", fullname));
            com.Parameters.Add(new MySqlParameter("?remarks1", remarks));
            com.Parameters.Add(new MySqlParameter("?isinsert", (IsInsert?1:0)));
            com.Parameters.Add(new MySqlParameter("?judgeid1", judgeid));

            com.ExecuteNonQuery();
            CloseConnection();
        }
        public void SPInsertContestant(string fullname, string photopath, string contestantno, string remarks, bool IsInsert, string contestantid)
        {
            com.Parameters.Clear();
            string procedurename = "insert_contestant";
            com.CommandText = procedurename;
            com.Connection = OpenConnection();
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Add(new MySqlParameter("?fullname1", fullname));
            com.Parameters.Add(new MySqlParameter("?photopath1", photopath));
            com.Parameters.Add(new MySqlParameter("?contestantno1", contestantno));
            com.Parameters.Add(new MySqlParameter("?remarks1", remarks));
            com.Parameters.Add(new MySqlParameter("?isinsert", (IsInsert?1:0)));
            com.Parameters.Add(new MySqlParameter("?contestantid1", contestantid));

            com.ExecuteNonQuery();
            CloseConnection();
        }
        public void SPInsertCriteria(string criterianame, string percentage, bool isInsert, string criteriaid)
        {
            com.Parameters.Clear();
            string procedurename = "insert_criteria";
            com.CommandText = procedurename;
            com.Connection = OpenConnection();
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Add(new MySqlParameter("?criterianame1", criterianame));
            com.Parameters.Add(new MySqlParameter("?percentage1", percentage));
            com.Parameters.Add(new MySqlParameter("?isinsert", (isInsert?"1":"0")));
            com.Parameters.Add(new MySqlParameter("?criteriaid1", criteriaid));

            com.ExecuteNonQuery();
            CloseConnection();
        }
        public long GetLastIndex(string tablename,string columnname)
        { 
            DataTable dt = SelectTable("SELECT * FROM " + tablename);
            try
            {
                if (dt != null)
                {
                    DataRow r = dt.Rows[dt.Rows.Count - 1];
                    string s = r[columnname].ToString();
                    return Convert.ToInt64(s);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public void InsertMultiple(List<string> queries)
        {
            OpenConnection();
            MySqlCommand command = conn.CreateCommand();
            MySqlTransaction trans = conn.BeginTransaction();

            command.Connection = conn;
            command.Transaction = trans;
            try
            {
                for (int i = 0; i < queries.Count; i++)
                {
                    command.CommandText = queries[i].ToString();
                    command.ExecuteNonQuery();
                }
                trans.Commit();
                MessageBox.Show("Committed successfully!");
            }
            catch (Exception e)
            {
                try
                {
                    trans.Rollback();
                }
                catch (MySqlException ex)
                {
                    if (trans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                                          " was encountered while attempting to roll back the transaction.");
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }
    }
    class InteractionAddOns
    {
        Point formPoint;
        Point mousePoint;
        public bool isMoving;

        public InteractionAddOns()
        {
            formPoint = new Point();
            mousePoint = new Point();
            isMoving = false;
        }
        public Point FormMove(ref object sender, ref MouseEventArgs e, Point mouseXY)
        {
            Point m = mouseXY;
            int x, y;
            x = m.X - mousePoint.X;
            y = m.Y - mousePoint.Y;
            x = formPoint.X + x;
            y = formPoint.Y + y;
            m = new Point(x, y);
            return m;
        }
        public void FormDrag(ref object sender, ref MouseEventArgs e, Point mouseXY, Point formXY)
        {
            if (e.Button == MouseButtons.Left)
            {
                formPoint = formXY;
                mousePoint = mouseXY;
                isMoving = true;
            }
        }
        public void FormDrop(ref object sender, ref MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMoving = false;
        }
        
    }
    class HelperMethods
    {
        public string CorrectCasing(string source)
        {
            string retval = "";
            if (source.Contains(' '))
            {
                string[] temp = source.Split(' ');
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Length > 1)
                        retval += temp[i].Substring(0, 1).ToUpper() + temp[i].Substring(1).ToLower() + " ";
                    else
                        retval += temp[i].ToUpper();
                }
            }
            else
                retval += source.Substring(0, 1).ToUpper() + source.Substring(1).ToLower() + " ";
            retval = retval.Trim();
            return retval;
        }
        public Image GetCopyImage(string path)
        {
            using (Image im = Image.FromFile(path))
            {
                Bitmap bm = new Bitmap(im);
                return bm;
            }
        }
        public void TextHandle(ref object sender, ref KeyPressEventArgs e, bool IsDigit)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && IsDigit)
                e.Handled = true;
            if (!char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsSymbol(e.KeyChar) && !IsDigit)
                e.Handled = true;
        }
        public void DecimalHandle(ref object sender, ref KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text.Contains(".") && e.KeyChar == '.')
                e.Handled = true;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }
        public void LimitTo(ref object sender, ref KeyPressEventArgs e, double count)
        {
            TextBox t = (TextBox)sender;
            if (!t.Text.Equals("") && !t.Text.Equals(".") && !char.IsControl(e.KeyChar))
            {
                try
                {
                    double temp = Convert.ToDouble(t.Text + e.KeyChar);
                    if (temp > count)
                        e.Handled = true;
                }
                catch
                {
                    e.Handled = true;
                }
            }
        }
    }
    class ConnectionStringSolution
    {
        public string AES_Encrypt(string clearText, string passkey)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            string encryptedString = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passkey));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(clearText);
                encryptedString = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return "";
            }
            return encryptedString;
        }
        public string AES_Decrypt(string encryptedText, string passkey)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            string decryptedString = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passkey));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = CipherMode.ECB;
                ICryptoTransform DESDecrypter = AES.CreateDecryptor();
                byte[] buffer = Convert.FromBase64String(encryptedText);
                decryptedString = ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return "";
            }
            return decryptedString;
        }
        public void CreateRegistryKey(string key, string value)
        {
            value = AES_Encrypt(value, "mysqllao");
            try
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software", true);
                regKey.CreateSubKey("Scoring System");
                regKey = regKey.OpenSubKey("Scoring System", true);
                regKey.SetValue(key, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public string ReadRegistryKey(string key, string passkey)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software", true);
            regKey.CreateSubKey("Scoring System");
            regKey = regKey.OpenSubKey("Scoring System", true);
            return AES_Decrypt(regKey.GetValue(key).ToString(), passkey);

        }
    }
}
