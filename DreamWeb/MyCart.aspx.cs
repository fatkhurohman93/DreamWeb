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
    public partial class MyCart : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Master.PopupWindowQtyClicked += new EventHandler(MasterPropertyChanged);
        }

        protected void MasterPropertyChanged(object sender, EventArgs e)
        {
            CSalesDetail sd = (CSalesDetail)sender;
            if (sd != null)
            {
                foreach (ListViewItem item in lvwOrder.Items)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_TempID");
                    bool isNumeric = int.TryParse(hf.Value, out int iTempID);
                    if (isNumeric)
                    {
                        if (iTempID == sd.TempID)
                        {
                            UpdatePanel_OneItem(item, sd);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdatePanel_OneItem(ListViewItem item, CSalesDetail sd)
        {
            TextBox txt = (TextBox)item.FindControl("txtQty");
            txt.Text = sd.Qty_ToString;

            Label lblTot = (Label)item.FindControl("lblTotalPrice");
            lblTot.Text = sd.TotalPrice_ToString;

            //Label lblDesc = (Label)item.FindControl("lblDesc");
            // lblDesc.Text = sd.ItemDesc;

            RecalculateTotals();
            //UpdatePanel1.Update();
        }

        private void UpdateOneItem(ListViewItem item, CSalesDetail sd)
        {
            TextBox txt = (TextBox)item.FindControl("txtQty");
            txt.Text = sd.Qty_ToString;

            Label lblTot = (Label)item.FindControl("lblTotalPrice");
            lblTot.Text = sd.TotalPrice_ToString;

            RecalculateTotals();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCheckout.InnerText = (ApplicationSession.QRcode == "") ? "Checkout" : "Send Order";
                ShowListViewOrder();

                Master.SetInvisible_Basket();
            }
        }

        private void ShowListViewOrder()
        {
            SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
            lvwOrder.DataSource = col;
            lvwOrder.DataBind();

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            ApplicationSession.SalesMaster.Recalculate(ApplicationSession.SalesType);
            lblSubtotalAmt.Text = ApplicationSession.SalesMaster.Subtotal_ToString;
            lblTaxName.Text = (ApplicationSession.SalesType.TaxName == "") ? "Tax" : ApplicationSession.SalesType.TaxName;
            lblTaxAmt.Text = ApplicationSession.SalesMaster.Tax_ToString;
            lblTotalAmt.Text = ApplicationSession.SalesMaster.SalesTotal_ToString;
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            decimal dQty = decimal.Parse(txt.Text);
            ListViewDataItem item = (ListViewDataItem)txt.Parent;
            HiddenField hf = (HiddenField)item.FindControl("hf_TempID");
            bool isNumeric = int.TryParse(hf.Value, out int iTempID);
            if (isNumeric)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
                CSalesDetail sd = col.ToList().Find(obj => obj.TempID == iTempID);

                if (sd != null)
                {
                    sd.Qty = dQty;

                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    bool blnDetail = sd.HasDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                    if (blnDetail)
                    {
                        Master.DisplayOrderItem(sd);
                    }
                    else
                    {
                        UpdateOneItem(item, sd);
                    }
                }
            }

            Master.MyProperty = ApplicationSession.QtyCart;
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlButton btn = (System.Web.UI.HtmlControls.HtmlButton)sender;
            ListViewDataItem item = (ListViewDataItem)btn.Parent;
            HiddenField hf = (HiddenField)item.FindControl("hf_TempID");
            bool isNumeric = int.TryParse(hf.Value, out int iTempID);
            if (isNumeric)
            {
                ApplicationSession.SalesMaster.CollectionSalesDetail().RemoveAtTempID(iTempID);
                ShowListViewOrder();
            }
        }

        protected void btnShopping_Click(object sender, EventArgs e)
        {

            Response.Redirect("MainMenu.aspx");
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.QRcode == "")
            {
                Response.Redirect("Checkout.aspx");
            }
            else
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                CSalesMaster sm = ApplicationSession.SalesMaster;
                if (sm.IsEmpty())
                {
                    sm.CreateNewSales(ApplicationSession.StoreID, ApplicationSession.OutletID, ApplicationSession.SalesType.ID,
                                      1, "", 0, (int)CSalesMaster.EFlagStatus.STATUS_ORDER, 0, "", "", 0, ApplicationSession.TableNo, false);

                    if (sm.InsertRecord(conn))
                    {
                        sm.SetSalesMasterID(sm.ID);
                    }
                    else
                    {
                        //kasih message error
                    }
                }

                sm.Recalculate(ApplicationSession.SalesType);
                sm.UpdateRecord_SalesAmounts(conn);

                List<CSalesDetail> lst = sm.GetChildrenToBeSent;
                
                if (lst.Count > 0)
                {
                    foreach (CSalesDetail sd in lst)
                    {
                        if (sd.isEmpty)
                        {
                            sd.SalesMasterID = sm.ID;
                            sd.InsertRecord(conn, true);
                        }
                        else
                        {
                            sd.UpdateRecord_Send(conn);
                        }
                    }

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalConfirmed", "$(document).ready(function () {$('#ModalConfirmed').modal();});", true);
                }
                else
                {
                    //kasih message no record
                }
            }

        }

    }
}
