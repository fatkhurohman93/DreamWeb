using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class OrderHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                List<CMiniSM> lst = CMember.ListOfPastOrders(conn, ApplicationSession.member.ID, ApplicationSession.StoreID);
                lvwOrderHistory.DataSource = lst;
                lvwOrderHistory.DataBind();
            }
        }

        protected void linkView_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            string sArg = lbtn.CommandArgument;
            bool isNumeric = Int32.TryParse(sArg, out int iSMID);
            if (isNumeric)
            {
                string url = "OrderDetail.aspx?id=" + iSMID.ToString();
                Response.Redirect(url);
            }

        }
    }
}