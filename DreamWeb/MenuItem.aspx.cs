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
    public partial class MenuItem : System.Web.UI.Page
    {
       
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack) --cannot place this! otherwise event will not be fired.
            {
                //Master.myPropertyChanged += new EventHandler(MasterPropertyChanged);
                Master.PopupWindowQtyClicked += new EventHandler(MasterPropertyChanged);
                //lvwMenuItem.ItemCommand += new EventHandler<ListViewCommandEventArgs>(lvwMenuItem_ItemCommand);
            }
        }

        protected void MasterPropertyChanged(object sender, EventArgs e)
        {
            CSalesDetail sd = (CSalesDetail)sender;
            if (sd != null)
            {
                foreach (ListViewDataItem item in lvwMenuItem.Items)
                {
                    LinkButton btn = (LinkButton)item.FindControl("linkMinus");
                    string sArg = Convert.ToString(btn.CommandArgument);
                    bool isNumeric = int.TryParse(sArg, out int iItemMasterID);
                    if (isNumeric)
                    {
                        if (iItemMasterID == sd.TransID)
                        {
                            Label lbl = (Label)item.FindControl("lblQty");
                            lbl.Text = GetOrderQty(iItemMasterID);
                            break;
                        }
                    }
                }
            }
        }


        protected void btnPlusMinus_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ListViewDataItem item = (ListViewDataItem)btn.Parent.Parent;
            Label lbl = (Label)item.FindControl("lblQty");
            string sArg = Convert.ToString(btn.CommandArgument);
            bool isNumeric = int.TryParse(sArg, out int iItemMasterID);
            if (isNumeric)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail(ApplicationSession.QRcode == "");
                CSalesDetail sd = col.ToList().Find(obj => obj.TransID == iItemMasterID);

                if (btn.CommandName == "plus")
                {
                    if (sd is null)
                    {
                        sd = AddItemToCart(iItemMasterID, 1);
                    }
                    else
                    {
                        sd.Qty += 1;
                    }
                }
                else
                {
                    if (sd is null) { }
                    else
                    {
                        if (sd.Qty >= 1)
                        {
                            sd.Qty -= 1;
                        }

                    }
                }

                if (sd != null)
                {
                    lbl.Text = sd.Qty_ToString;
                    if (sd.Qty > 0)
                    {
                        ShowMasterModalPopup_DisplayOrderItem_IfItemHasDetail(sd);
                    }
                }
            }
            Master.MyProperty = ApplicationSession.QtyCart;
        }


        private void ShowMasterModalPopup_DisplayOrderItem_IfItemHasDetail(CSalesDetail sd)
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            bool blnDetail = sd.HasDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            if (blnDetail)
            {
                Master.DisplayOrderItem(sd);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string strID = Request.QueryString["id"];
                if (strID != null)
                {
                    bool isNumeric = int.TryParse(strID, out int iID);
                    if (isNumeric) { ApplicationSession.ItemGroupID = iID; }
                }
                DisplayMenuItem();
            }
        }

        public string GetOrderQty(object sItemMasterID)
        {
            string s = "0";
            bool bln = int.TryParse(sItemMasterID.ToString(), out int iItemMasterID);
            if (bln)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail(ApplicationSession.QRcode == "");
                CSalesDetail sd = col.ToList().Find(obj => obj.TransID == iItemMasterID);
                if (sd is null) { }
                else
                {
                    if (ApplicationSession.QRcode == "")
                    {
                        s = sd.Qty.ToString();
                    }
                    else
                    {
                        if (sd.IsNotSent) { s = sd.Qty.ToString(); }
                    }
                }
            }
            return s;
        }

        private void DisplayMenuItem()
        {
            int iItemGroupID = ApplicationSession.ItemGroupID;
            if (iItemGroupID > 0)
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);

                CItemGroup ig = new CItemGroup(iItemGroupID, conn);
                lblMenu.InnerText = ig.Name;
                
                List<CItemMaster> lst = ig.ListOfItemMasters(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                lvwMenuItem.DataSource = lst;
                lvwMenuItem.DataBind();
            }
        }

        protected void MenuItem_Click(object sender, EventArgs e) 
        {
            HtmlAnchor anchor = (HtmlAnchor)sender;
            string sArg = anchor.Attributes["customdata"];
            bool isNumeric = int.TryParse(sArg, out int iItemMasterID);
            if (isNumeric)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail(ApplicationSession.QRcode == "");
                CSalesDetail sd = col.ToList().Find(obj => obj.TransID == iItemMasterID);

                if (sd is null)
                {
                    sd = AddItemToCart(iItemMasterID, 0);
                }

                Master.DisplayOrderItem(sd);
                //ClientScript.RegisterStartupScript(typeof(Page), "autoPostback", ClientScript.GetPostBackEventReference(this, String.Empty), true);


            }
        }


        private CSalesDetail AddItemToCart(int iItemMasterID, decimal dQty)
        {
            CSalesDetail sd = new CSalesDetail();
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            CItemMaster im = new CItemMaster(iItemMasterID, conn);
            //masuk ke dalam cart
            if (im.Name != "")
            {
                ApplicationSession.idx += 1;
                int flg = (int)CSalesDetail.EFlagStatus.TRANS_ITEM;
                sd = new CSalesDetail(ApplicationSession.idx, im.ID, im.Code, im.Name, im.ItemGroupName, dQty, im.Price, im.UnitSymbol, 
                                     "", "", 0, ApplicationSession.StoreID, ApplicationSession.OutletID, flg, ApplicationSession.ChairNo);

                //Master.MyProperty = ApplicationSession.QtyCart;
                List<CPromo> lstPromo = CItemMaster.ListOfItemPromos(conn, ApplicationSession.SalesType.ID, iItemMasterID, im.ItemGroupID,
                                                                     ApplicationSession.StoreID, ApplicationSession.OutletID, CSetting.GetFlagOfToday());

                //auto promo:
                IEnumerable<CPromo> lstAuto = lstPromo.Where(promo => promo.IsAutoPromo());
                if (lstAuto.ToList().Count > 0)
                {
                    List<CMiniItem> lst = sd.ApplyListOfItemPromo(lstAuto.ToList());
                    foreach (CMiniItem item in lst)
                    {
                        if (item.Name == "item")
                        {
                            ApplicationSession.idx += 1;
                            CSalesDetail child = new CSalesDetail(ApplicationSession.idx, item.ID, "promo", item.Name, "", item.Qty, item.UnitPrice, 
                                                                  "", "", "", 0, ApplicationSession.StoreID, ApplicationSession.OutletID, 0, "");
                            sd.Children.Add(child);
                        }
                    }
                }

                //manual promo:
                IEnumerable<CPromo> lstManual = lstPromo.Where(promo => promo.IsAutoPromo() == false);
                if (lstManual.ToList().Count > 0)
                {
                    //show promos & add it to sd.children
                }

                ApplicationSession.SalesMaster.CollectionSalesDetail().Add(sd);
            }
            return sd;
        }



    }
}