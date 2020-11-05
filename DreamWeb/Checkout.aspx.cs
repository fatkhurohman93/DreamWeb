using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCheckout.Attributes["PayModeID"] = "";
                btnCheckout.Attributes["PayModeName"] = "";

                CSalesMaster sm = ApplicationSession.SalesMaster;
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                lvwOrder.DataSource = sm.CollectionSalesDetail(true, conn);
                lvwOrder.DataBind();

                lblSubtotal.Text = sm.Subtotal_ToString;
                lblTaxName.Text = (ApplicationSession.SalesType.TaxName == "") ? "Tax" : ApplicationSession.SalesType.TaxName;
                lblTaxAmt.Text = sm.Tax_ToString;
                lblTotal.Text = sm.SalesTotal_ToString;
                btnCheckout.InnerText = "PAY ";
                checkoutAmt.InnerText = sm.SalesTotal_ToString;
            }
        }

        protected void img_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string sCmdName = btn.CommandName;

            switch (sCmdName)
            {
                case "mastercard":
                    divMastercard.Attributes["class"] = "row d-flex px-3 radio";
                    divVisa.Attributes["class"] = "row d-flex px-3 radio gray";
                    divEWallet.Attributes["class"] = "row d-flex px-3 radio gray mb-3";

                    pMastercard.Attributes["class"] = "my-auto font-weight-bold";
                    pVisa.Attributes["class"] = "my-auto";
                    pEWallet.Attributes["class"] = "my-auto";

                    //spanCheckout.InnerText = "Mastercard";
                    btnCheckout.InnerText = "Mastercard";
                    btnCheckout.Attributes["PayModeID"] = "1";
                    btnCheckout.Attributes["PayModeName"] = "Mastercard";
                    break;

                case "visa":
                    divMastercard.Attributes["class"] = "row d-flex px-3 radio gray";
                    divVisa.Attributes["class"] = "row d-flex px-3 radio";
                    divEWallet.Attributes["class"] = "row d-flex px-3 radio gray mb-3";

                    pMastercard.Attributes["class"] = "my-auto";
                    pVisa.Attributes["class"] = "my-auto font-weight-bold";
                    pEWallet.Attributes["class"] = "my-auto";

                    //spanCheckout.InnerText = "VISA";
                    btnCheckout.InnerText = "VISA";
                    btnCheckout.Attributes["PayModeID"] = "2";
                    btnCheckout.Attributes["PayModeName"] = "Visa";
                    break;

                case "ewallet":
                    divMastercard.Attributes["class"] = "row d-flex px-3 radio gray";
                    divVisa.Attributes["class"] = "row d-flex px-3 radio gray";
                    divEWallet.Attributes["class"] = "row d-flex px-3 radio mb-3";

                    pMastercard.Attributes["class"] = "my-auto";
                    pVisa.Attributes["class"] = "my-auto";
                    pEWallet.Attributes["class"] = "my-auto font-weight-bold";

                    //spanCheckout.InnerText = "e-Wallet";
                    btnCheckout.InnerText = "e-Wallet";
                    btnCheckout.Attributes["PayModeID"] = "3";
                    btnCheckout.Attributes["PayModeName"] = "e-Wallet";
                    break;
            }

            //bool bln = divMastercard.Attributes["class"].Contains("gray");

        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
            if (col.Count == 0)
            {
                Master.DisplayModalMessageBox("Your cart is empty"); 
            }
            else
            {
                string s = btnCheckout.Attributes["PayModeID"];
                bool isNumeric = int.TryParse(btnCheckout.Attributes["PayModeID"], out int iPayModeID);
                if (isNumeric)
                {
                    CSalesMaster sm = new CSalesMaster();
                    sm.CreateNewSales(ApplicationSession.StoreID, ApplicationSession.OutletID, ApplicationSession.SalesType.ID, 1, "",
                        ApplicationSession.member.ID, (int)CSalesMaster.EFlagStatus.STATUS_ORDER,
                        iPayModeID, btnCheckout.Attributes["PayModeName"], "", 0, ApplicationSession.SalesMaster.TableNo, false);

                    if (ApplicationSession.SalesType.IsCatering()) { sm.FromDate = ApplicationSession.category.OrderDate; }

                    try
                    {
                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        if (sm.InsertRecord(conn))
                        {
                            sm.SetSalesMasterID(sm.ID);
                            sm.InsertChildrenRecords(conn, false);

                            aOrderNo.InnerText = sm.Number;
                            aOrderNo.Attributes["smid"] = sm.ID.ToString();

                            ApplicationSession.SalesMaster.RefreshCollection();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalConfirmed", "$(document).ready(function () {$('#ModalConfirmed').modal();});", true);
                        }
                        else
                        {
                            //lblMessage.Text = "Fail to save order. Please try again";
                        }
                    }
                    catch
                    {
                        //lblMessage.Text = "Fail to save order. Please try again";
                    }
                }
            }
        }

        protected void aOrderNo_Click(object sender, EventArgs e)
        {
            string sAtt = aOrderNo.Attributes["smid"];
            bool isNumeric = int.TryParse(sAtt, out int iSMID);
            if (isNumeric)
            {
                Response.Redirect("OrderDetail.aspx?id=" + sAtt);
            }
        }


    }
}