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
    public partial class OrderDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string strID = Request.QueryString["id"];
                if (strID == null)
                {
                    if (!ApplicationSession.SalesMaster.IsEmpty())
                    {
                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        CSalesMaster sm = new CSalesMaster(ApplicationSession.SalesMaster.ID, conn);
                        if (!sm.IsEmpty())
                        {
                            DisplayInfo(sm, conn);
                        }
                    }
                }
                else
                {
                    bool isNumeric = int.TryParse(strID, out int iID);
                    if (isNumeric)
                    {
                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        CSalesMaster sm = new CSalesMaster(iID, conn);
                        if (!sm.IsEmpty())
                        {
                            DisplayInfo(sm, conn);
                        }
                    }
                }
            }
        }

        private void DisplayInfo(CSalesMaster sm, MySqlConnection conn)
        {
            lblOrderNo.InnerText = sm.Number;
            lblTransDate.Text = sm.TransDate_ToString;
            //lblDelMode.Text = sm.DelModeName;

            lblPmtMode.Text = sm.PmtModeName;
            //lblPmtNo.Text = "";
            //lblPmtDate.Text = "";

            string sBillAddr = "";
            if (sm.LinkID > 0)
            {
                CPayment pmt = CSalesMaster.FetchPaymentRecord(conn, sm.LinkID);
                if (!pmt.isEmpty)
                {
                    lblPmtMode.Text = pmt.PaymentName;
                    //lblPmtNo.Text = pmt.Number;
                    //lblPmtDate.Text = pmt.TransDate_ToString;
                }
            }
            lblBillAddr.Text = sBillAddr;
            //lblDelAddr.Text = sm.Address;

            SalesDetailCollection col = sm.CollectionSalesDetail(true, conn);
            lvwOrder.DataSource = col;
            lvwOrder.DataBind();

            //lblDelFee.Text = sm.Charge_ToString;
            lblSubtotal.Text = sm.Subtotal_ToString;
            lblCharge.Text = "Charge";
            lblChargeAmt.Text = sm.Charge_ToString;
            lblTax.Text = "Tax";
            lblTaxAmt.Text = sm.Tax_ToString;
            lblTotal.Text = sm.SalesTotal_ToString;
            //lblNotes.Text = sm.Notes;

           // List<CDeliveryLog> lst = CSalesMaster.ListOfDeliveryLogs(conn, sm.ID);
            //lvwDelLog.DataSource = lst;
            //lvwDelLog.DataBind();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistoryPage.aspx");
        }
    }
}