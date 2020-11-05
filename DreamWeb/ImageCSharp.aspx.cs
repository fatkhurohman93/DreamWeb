using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class ImageCSharp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strName = Request.QueryString["name"];
            string strID = Request.QueryString["id"];
            if (strName != null)
            {
                if (strID != null)
                {
                    bool isNumeric = int.TryParse(strID, out int iID);
                    if (isNumeric)
                    {
                        if (iID == 0)
                        {
                            if (strName == "outlet") { iID = ApplicationSession.OutletID; }
                        }
                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        byte[] bPhoto = FetchImage(strName, iID, conn);

                        
                        if (bPhoto == null)
                        {
                            string noImageURL = "~/images/noimage.jpg";
                            bPhoto = File.ReadAllBytes(Server.MapPath(noImageURL));
                        }


                        if (bPhoto != null)
                        {
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "image/jpeg";
                            //Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["Name"].ToString());
                            Response.BinaryWrite(bPhoto);
                            Response.Flush();
                            Response.End();
                        }
                        
                    }

                }
            }


        }

        private byte[] FetchImage(string sTableName, int iID, MySqlConnection conn)
        {
            byte[] img = null;
            using (MySqlCommand cmd = new MySqlCommand("SELECT Photo FROM " + sTableName + " WHERE ID = @ID", conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new MySqlParameter("@ID", iID));

                try
                {
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if (obj is DBNull) { } else { img = (byte[])obj; }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally { cmd.Connection.Close(); }
            }
            return img;

        }
    }

  

}