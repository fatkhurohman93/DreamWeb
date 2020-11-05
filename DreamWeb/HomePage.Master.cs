using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DreamWeb
{
    public partial class HomePage : System.Web.UI.MasterPage
    {
        private const int CONST_POPUP_WIDTH = 500;
        private const int CONST_POPUP_HEIGHT = 700;
        private const int CONST_POPUP_WIDTH_MOBILE = 360;
        private const int CONST_POPUP_HEIGHT_MOBILE = 500;

        public event EventHandler PopupWindowQtyClicked;
        public event EventHandler myPropertyChanged;
        public delegate void MyPropertyChanged(object sender, EventArgs e);
        private int _myProperty;
        public int MyProperty
        {
            get
            {
                return _myProperty;
            }
            set
            {
                _myProperty = value;
                if (myPropertyChanged != null)
                {
                    myPropertyChanged(this, new EventArgs());
                    lblQty.Text = _myProperty.ToString();
                }
            }
        }

        public event EventHandler PopupWindowCatInfoClicked;


        System.Web.HttpBrowserCapabilities browser;
        protected void Page_Load(object sender, EventArgs e)
        {
            browser = Request.Browser;
            if (browser.IsMobileDevice == true)
            {
                txtSearch.Visible = true;
            }
            else
            {
                txtSearch.Visible = false;
            }

            if (!IsPostBack)
            {
                btnAcc.Visible = (ApplicationSession.QRcode == "");
                lblQty.Text = ApplicationSession.QtyCart.ToString();
                DisplayDropdownMenu();
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            browser = Request.Browser;
            if (browser.IsMobileDevice == false) txtSearch.Visible = true;
        }

        protected void btnBasket_Click(object sender, ImageClickEventArgs e)
        {
            DisplayMyCart();
        }

        private void DisplayMyCart()
        {
            SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
            lvwOrder.DataSource = col;
            lvwOrder.DataBind();

            PanelPopup_MyCart.Width = browser.IsMobileDevice == true ? CONST_POPUP_WIDTH_MOBILE : CONST_POPUP_WIDTH;
            PanelPopup_MyCart.Height = browser.IsMobileDevice == true ? CONST_POPUP_HEIGHT_MOBILE : CONST_POPUP_HEIGHT;
            ModalPopupExtender_MyCart.Show();
        }

        protected void btnCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("CartPage.aspx");
        }

        protected void btnAcc_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AccountPage.aspx");
        }

        public void DisplayOrderItem(CSalesDetail item) //only display item desc etc, user then can add item
        {
            ApplicationSession.SalesMaster.CollectionSalesDetail().SetFocus(item.TempID);
            InitAndShowModalPopupExtender_OrderItem();
        }

       
        protected void EditItem_Click(object sender, EventArgs e) //linkItem
        {
            LinkButton btn = (LinkButton)sender;
            string sArg = btn.CommandArgument;
            bool isNumeric = int.TryParse(sArg, out int iTempID);
            if (isNumeric)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
                CSalesDetail sd = col.ToList().Find(obj => obj.TempID == iTempID);
                if (sd != null)
                {
                    DisplayOrderItem(sd);
                }
            }
        }

        private void InitAndShowModalPopupExtender_OrderItem()
        {
            CSalesDetail sd = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();
            if (sd != null)
            {
                lblName.Text = sd.TransName;
                lblPrice.Text = sd.UnitPrice_ToString;
                lblTotal.Text = sd.TotalPrice_ToString;
                string sID = sd.TransID.ToString();
                imgItem.ImageUrl = "ImageCSharp.aspx?name=itemmaster&id=" + sID;
                popup_txtQty.Text = sd.Qty_ToString;

                if (ApplicationSession.SalesType.IsCatering())
                {
                    lblCondItem.Text = sd.Notes;
                }
                else
                {                   
                    lblCondItem.Text = sd.Condiments_ToString;                   
                }
               

                SalesDetailCollection children = sd.Children;
                if (children.Count > 0)
                {
                    grd.DataSource = children;
                    grd.DataBind();
                }
                else
                {
                    grd.DataSource = new SalesDetailCollection();
                    grd.DataBind();
                }

                ShowModalPopupExtender_OrderItem();
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ApplicationSession.SalesType.IsCatering())
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblQty = (Label)e.Row.FindControl("lblQty1");
                    lblQty.Text = "";
                }
            }           
        }

        private void ShowModalPopupExtender_OrderItem()
        {
            CSalesDetail sd = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();

            if (ApplicationSession.SalesType.IsCatering())
            {
                btnCondiment.Text = "Continue Ordering";
                btnCondiment.CommandName = "menu";
                btnItemDetail.Text = "Checkout";
                btnItemDetail.CommandName = "checkout";
                btnOK.Text = "Cancel";
                btnOK.CommandName = "cancel";
            }
            else
            {
                btnCondiment.Text = "Condiments";
                btnCondiment.CommandName = "condiment";
                btnItemDetail.Text = "Choose Options";
                btnItemDetail.CommandName = "option";

                btnCondiment.Enabled = false;
                btnItemDetail.Enabled = false;

                if (sd != null)
                {
                    if (sd.Qty > 0)
                    {
                        btnCondiment.Enabled = true;

                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        bool blnDetail = sd.HasDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                        btnItemDetail.Enabled = blnDetail;
                    }
                }
            }
                

            PanelPopup_OrderItem.Width = browser.IsMobileDevice == true ? CONST_POPUP_WIDTH_MOBILE : CONST_POPUP_WIDTH;
            PanelPopup_OrderItem.Height = browser.IsMobileDevice == true ? CONST_POPUP_HEIGHT_MOBILE : CONST_POPUP_HEIGHT;
            ModalPopupExtender_OrderItem.Show();
        }

        private void ShowModalPopupExtender_Condiment()
        {
            PanelPopup_Condiment.Width = browser.IsMobileDevice == true ? CONST_POPUP_WIDTH_MOBILE : CONST_POPUP_WIDTH;
            PanelPopup_Condiment.Height = browser.IsMobileDevice == true ? CONST_POPUP_HEIGHT_MOBILE : CONST_POPUP_HEIGHT;
            ModalPopupExtender_Condiment.Show();
        }

        protected void popup_btnPlusMinus_Click(object sender, EventArgs e)
        {
            int iTempID = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().TempID;
            if (iTempID > 0)
            {
                SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
                CSalesDetail sd = col.ToList().Find(obj => obj.TempID == iTempID);
                if (sd != null)
                {
                    string sQty = popup_txtQty.Text;
                    bool bln = decimal.TryParse(sQty, out decimal dQty);
                    if (bln)
                    {
                        Button btn = (Button)sender;
                        if (btn.CommandName == "plus")
                        {
                            dQty += 1;
                        }
                        else
                        {
                            if (dQty >= 1)
                            {
                                dQty -= 1;
                            }
                        }

                        sd.Qty = dQty;
                        lblTotal.Text = sd.TotalPrice_ToString;
                        popup_txtQty.Text = dQty.ToString();
                        MyProperty = ApplicationSession.QtyCart;
                    }

                    /*
                    if (dQty > 0)
                    {
                        btnCondiment.Enabled = true;

                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        bool blnDetail = sd.HasDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                        btnItemDetail.Enabled = blnDetail;
                    }
                    */
                }

                lblMessage_OrderItem.Text = "";
                //btnPlusMinusClicked(sd, new EventArgs());
            }

            ShowModalPopupExtender_OrderItem();
        }


        protected void btnItemDetail_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.SalesType.IsCatering())
            {
                Response.Redirect("CartPage.aspx");
            }
            else
            {
                int iTempID = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().TempID;
                if (iTempID > 0)
                {
                    SalesDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail();
                    CSalesDetail sd = col.ToList().Find(obj => obj.TempID == iTempID);
                    if (sd != null)
                    {
                        SalesDetailCollection children = sd.Children;
                        if (children.Count == 0)
                        {
                            AddAutomaticSelectionGroup(sd.Qty);
                        }

                        lblMessage_OrderItem.Text = "";
                        DisplayItemDetail();
                    }
                }
            }          
        }


        private void AddAutomaticSelectionGroup(decimal dQty)
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            ItemDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().ItemDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            List<CItemDetail> lst = col.AutomaticSelection();
            foreach (CItemDetail id in lst)
            {
                if (!id.IsEmpty())
                {
                    ApplicationSession.idx += 1;
                    int flg = (int)(CSalesDetail.EFlagStatus.TRANS_ITEM | CSalesDetail.EFlagStatus.TYPE_DETAIL);
                    CSalesDetail sd = new CSalesDetail(ApplicationSession.idx, id.ID, id.ItemCode, id.ItemName, id.SelGrp.ToString(), 
                                                       (id.Qty * dQty), id.Price, id.UnitSymbol, "", "", 0,  ApplicationSession.StoreID, 
                                                       ApplicationSession.OutletID, flg, ApplicationSession.ChairNo);
                    ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Children.Add(sd);
                }
            }
        }

        private void DisplayItemDetail(int grp = 1)
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            ItemDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().ItemDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            List<CItemDetail> lst = col.ListBySelectionGroup(grp);
            lvwItemDetail.DataSource = lst;
            lvwItemDetail.DataBind();

            btnPrev.Enabled = (grp > col.LBoundIndex());
            btnPrev.Attributes.Add("idx", grp.ToString());
            btnNext.Enabled = (grp < col.UBoundIndex());
            btnNext.Attributes.Add("idx", grp.ToString());

            lblMasterName.Text = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().TransName;
            ShowModalPopupExtender_ListDetail();

        }

        private void ShowModalPopupExtender_ListDetail(bool blnShowQty = true)
        {
            if (blnShowQty)
            {
                decimal dRmnQty = GetRemainingQty_currentGroup();
                string sRmnQty = string.Format("{0:#,0}", dRmnQty);
                lblMasterQty.Text = "Choose " + sRmnQty + " item(s)";
            }

            PanelPopup_ListDetail.Width = browser.IsMobileDevice == true ? CONST_POPUP_WIDTH_MOBILE : CONST_POPUP_WIDTH;
            PanelPopup_ListDetail.Height = browser.IsMobileDevice == true ? CONST_POPUP_HEIGHT_MOBILE : CONST_POPUP_HEIGHT;
            ModalPopupExtender_ListDetail.Show();
        }

        private decimal GetRemainingQty_currentGroup()
        {
            List<CItemDetail> lst = new List<CItemDetail>();

            string sGrp = btnNext.Attributes["idx"];
            bool isNumeric = int.TryParse(sGrp, out int iGrp);
            if (isNumeric)
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                ItemDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().ItemDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                lst = col.ListBySelectionGroup(iGrp);
            }

            decimal dTotItem = TotalItem_oneGroup(lst);
            decimal dRmnQty = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Qty - dTotItem;
            return dRmnQty;
        }

        private decimal TotalItem_oneGroup(List<CItemDetail> lstItemDetail)
        {
            decimal dQty = 0;
            CSalesDetail sd = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();
            if (sd != null)
            {
                SalesDetailCollection col = sd.Children;
                if (col.Count > 0)
                {
                    foreach (CItemDetail id in lstItemDetail)
                    {
                        CSalesDetail sd0 = col.ToList().Find(obj => obj.TransID == id.ID);
                        if (sd0 != null)
                        {
                            if (sd0.Qty > 0)
                            {
                                if (id.Qty > 0)
                                {
                                    decimal dReqQty = sd0.Qty / id.Qty;
                                    dQty += dReqQty;
                                }

                            }
                        }
                    }
                }
            }

            return dQty;
        }


        public string fn_GetOrderDetailQty(object sItemDetailID)
        {
            decimal dQty = 0;
            bool bln = int.TryParse(sItemDetailID.ToString(), out int iItemDetailID);
            if (bln)
            {
                CSalesDetail sd = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();
                if (sd != null)
                {
                    SalesDetailCollection col = sd.Children;
                    if (col.Count > 0)
                    {
                        CSalesDetail sd0 = col.ToList().Find(obj => obj.TransID == iItemDetailID);
                        if (sd0 is null) { }
                        else
                        {
                            dQty = sd0.Qty;
                        }
                    }
                }
            }
            return string.Format("{0:#,0}", dQty);
        }

        protected void btnPrevNext_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string sIdx = btn.Attributes["idx"];
            bool isNumeric = int.TryParse(sIdx, out int idx);
            if (isNumeric)
            {
                decimal dRmnQty = GetRemainingQty_currentGroup();
                if (dRmnQty != 0)
                {
                    lblMessage_ListDetail.Text = "please choose " + string.Format("{0:#,0}", dRmnQty) + " more item(s) from this selection group";
                    ShowModalPopupExtender_ListDetail();
                }
                else
                {
                    if (btn.CommandName == "prev")
                    {
                        if (idx > 1) { idx -= 1; }
                    }
                    else
                    {
                        idx += 1;
                    }

                    DisplayItemDetail(idx);
                }

            }
        }


        protected void lvwItemDetail_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "plus":
                    decimal dRmnQty = GetRemainingQty_currentGroup();
                    if (dRmnQty > 0)
                    {
                        goto case "minus";
                    }
                    else
                    {
                        ShowModalPopupExtender_ListDetail();
                        break;
                    }

                case "minus":
                    ListViewItem item = e.Item;
                    HiddenField hf = (HiddenField)item.FindControl("hf_ID");
                    bool isNumeric = int.TryParse(hf.Value, out int iItemDetailID);
                    if (isNumeric)
                    {
                        Label lbl = (Label)item.FindControl("lblQty_Detail");
                        CSalesDetail sd = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();
                        if (sd != null)
                        {
                            SalesDetailCollection col = sd.Children;
                            if (col.Count > 0)
                            {
                                CSalesDetail sd0 = col.ToList().Find(obj => obj.TransID == iItemDetailID);
                                if (e.CommandName == "plus")
                                {
                                    if (sd0 is null)
                                    {
                                        decimal dQty = AddItemDetailToComboItem(iItemDetailID);
                                        lbl.Text = string.Format("{0:#,0}", dQty);
                                    }
                                    else
                                    {
                                        sd0.Qty += 1;
                                        lbl.Text = sd0.Qty_ToString;
                                    }
                                }
                                else
                                {
                                    if (sd0 is null) { }
                                    else
                                    {
                                        if (sd0.Qty >= 1)
                                        {
                                            sd0.Qty -= 1;
                                            lbl.Text = sd0.Qty_ToString;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (e.CommandName == "plus")
                                {
                                    decimal dQty = AddItemDetailToComboItem(iItemDetailID);
                                    lbl.Text = string.Format("{0:#,0}", dQty);
                                }

                            }

                        }

                    }
                    lblMessage_ListDetail.Text = "";
                    ShowModalPopupExtender_ListDetail();
                    break;
            }

        }


        private decimal AddItemDetailToComboItem(int iItemDetailID)
        {
            decimal dQty = 0;
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            CItemDetail id = new CItemDetail(iItemDetailID, conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            //masuk ke dalam child
            if (!id.IsEmpty())
            {
                ApplicationSession.idx += 1;
                dQty = id.Qty;
                int flg = (int)(CSalesDetail.EFlagStatus.TRANS_ITEM | CSalesDetail.EFlagStatus.TYPE_DETAIL);
                CSalesDetail sd = new CSalesDetail(ApplicationSession.idx, id.ID, id.ItemCode, id.ItemName, id.SelGrp.ToString(), 
                                                   dQty, id.Price, id.UnitSymbol, "", "", 0, ApplicationSession.StoreID,
                                                   ApplicationSession.OutletID, flg, ApplicationSession.ChairNo);
                ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Children.Add(sd);
            }
            return dQty;
        }

        protected void btnOK_ListDetail_Click(object sender, EventArgs e)
        {
            //disini cek apakah semua selgrp sudah dipilih(?)
            CMiniItem mini = CheckIfAnyItemDetailMissing();
            if (mini.isEmpty)
            {
                InitAndShowModalPopupExtender_OrderItem();
            }
            else
            {
                DisplayItemDetail(mini.ID);
                lblMessage_ListDetail.Text = "please choose " + string.Format("{0:#,0}", mini.Qty) + " more item(s) from this selection group";
                ShowModalPopupExtender_ListDetail(false);
            }
        }

        private CMiniItem CheckIfAnyItemDetailMissing()
        {
            CMiniItem miniItem = new CMiniItem();
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            ItemDetailCollection col = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().ItemDetails(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            int lbound = col.LBoundIndex();
            int ubound = col.UBoundIndex();

            for (int grp = lbound; grp <= ubound; grp++)
            {
                List<CItemDetail> lst = col.ListBySelectionGroup(grp);
                decimal dTotItem = TotalItem_oneGroup(lst);
                decimal dRmnQty = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Qty - dTotItem;
                if (dRmnQty > 0)
                {
                    miniItem = new CMiniItem(grp, "", dRmnQty, 0);
                    break;
                }
            }

            return miniItem;
        }

        protected void btnOK_OrderItem_Click(object sender, EventArgs e)
        {
            if (btnOK.CommandName != "cancel")
            {
                CMiniItem mini = CheckIfAnyItemDetailMissing();
                if (!mini.isEmpty)
                {
                    lblMessage_OrderItem.Text = "Please choose your preferred options before closing";
                    ShowModalPopupExtender_OrderItem();
                }
                else
                {
                    //CHECK: suka error disini
                    PopupWindowQtyClicked(ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem(), new EventArgs());
                }
            }
        }

        protected void btnCondiment_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.SalesType.IsCatering())
            {
                Response.Redirect("CategoryPage.aspx");
            }
            else
            {
                CSalesDetail item = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem();
                if (item != null)
                {
                    lblItemName.Text = item.TransName;

                    lvwItemCond.DataSource = item.Condiments;
                    lvwItemCond.DataBind();

                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    List<CCondGroup> lstCG = CItemMaster.ListOfCondGroups(conn, item.TransID, ApplicationSession.StoreID, ApplicationSession.OutletID);

                    lvwCondGroup.DataSource = lstCG;
                    lvwCondGroup.DataBind();

                    lblMessage_OrderItem.Text = "";
                    ShowModalPopupExtender_Condiment();
                }
            }                     
        }



        protected void lvwCondGroup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_CGObject");
                    CCondGroup cg = (CCondGroup)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(cg);
                    hf.Value = sJson;

                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    List<CCondiment> lst = cg.ListOfCondiments(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);

                    ListView lvw = (ListView)item.FindControl("lvwCondiment");
                    if (lst.Count > 0)
                    {
                        lvw.DataSource = lst;
                        lvw.DataBind();
                    }

                }
            }
        }

        protected void lvwCondiment_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_CondObject");
                    CCondiment cond = (CCondiment)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(cond);
                    hf.Value = sJson;

                    CheckBox chk = (CheckBox)item.FindControl("chkCond");
                    bool blnPicked = IsThisCondPicked(cond.ID);
                    chk.Checked = blnPicked;
                    chk.Enabled = !blnPicked;
                }
            }
        }

        protected void lvwItemCond_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_CondObject");
                    CCondiment cond = (CCondiment)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(cond);
                    hf.Value = sJson;
                }
            }
        }

        private bool IsThisCondPicked(int iCondID)
        {
            bool bln = false;
            List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;
            foreach (CCondiment cond in lst)
            {
                if (cond.ID == iCondID)
                {
                    bln = true;
                    break;
                }
            }
            return bln;
        }

        protected void btnOK_Condiment_Click(object sender, EventArgs e)
        {
            CCondGroup obj = new CCondGroup();
            foreach (ListViewDataItem item in lvwCondGroup.Items)
            {
                HiddenField hf = (HiddenField)item.FindControl("hf_CGObject");
                CCondGroup cg = JsonConvert.DeserializeObject<CCondGroup>(hf.Value);
                if ((cg != null) && (!cg.isEmpty))
                {
                    if (cg.IsTypeRequired())
                    {
                        bool bln = false;
                        List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;
                        foreach (CCondiment cond in lst)
                        {
                            if (cond.CondGroupID == cg.ID)
                            {
                                bln = true;
                                break;
                            }
                        }
                        if (!bln)
                        {
                            obj = cg;
                            break;
                        }
                    }
                }
            }

            if (obj.isEmpty)
            {
                InitAndShowModalPopupExtender_OrderItem();
            }
            else
            {
                //MessageBox.Show("Please choose at least one condiment from " + obj.Name); >>gak muncul, mgk karena popup
                lblMessage_Condiment.Text = "Please choose at least one condiment from " + obj.Name;
                ShowModalPopupExtender_Condiment();
            }

        }

        protected void btnDeleteItemCond_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ListViewItem item0 = (ListViewItem)btn.Parent.Parent;
            HiddenField hf0 = (HiddenField)item0.FindControl("hf_CondObject");
            CCondiment cond0 = JsonConvert.DeserializeObject<CCondiment>(hf0.Value);

            if (!cond0.isEmpty)
            {
                ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().RemoveCondiment(cond0.ID);
                List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;
                lvwItemCond.DataSource = lst;
                lvwItemCond.DataBind();

                foreach (ListViewDataItem item1 in lvwCondGroup.Items)
                {
                    HiddenField hf1 = (HiddenField)item1.FindControl("hf_CGObject");
                    CCondGroup cg = JsonConvert.DeserializeObject<CCondGroup>(hf1.Value);
                    if (cg != null)
                    {
                        if (cond0.CondGroupID == cg.ID)
                        {
                            ListView lvw = (ListView)item1.FindControl("lvwCondiment");
                            foreach (ListViewDataItem item2 in lvw.Items)
                            {
                                HiddenField hf2 = (HiddenField)item2.FindControl("hf_CondObject");
                                CCondiment cond2 = JsonConvert.DeserializeObject<CCondiment>(hf2.Value);
                                if (cond2.ID == cond0.ID)
                                {
                                    CheckBox chk = (CheckBox)item2.FindControl("chkCond");
                                    chk.Checked = false;
                                    chk.Enabled = true;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                ShowModalPopupExtender_Condiment();
            }
        }



        protected void chkCond_OnCheckedChanged(object sender, EventArgs e)
        {
            lblMessage_Condiment.Text = "";
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                ListViewItem item = (ListViewItem)chk.Parent.Parent;
                HiddenField hf = (HiddenField)item.FindControl("hf_CondObject");
                CCondiment cond = JsonConvert.DeserializeObject<CCondiment>(hf.Value);

                if (!cond.isEmpty)
                {
                    ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments.Add(cond);
                    List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;

                    lvwItemCond.DataSource = lst;
                    lvwItemCond.DataBind();

                    chk.Enabled = false;

                    ShowModalPopupExtender_Condiment();
                }
            }
        }



        public void DisplayDropdownMenu()
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);

            if (ApplicationSession.SalesType.IsCatering())
            {
                //Category:
                List<CCategory> lst = CCategory.ListOfCategoryZero(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                foreach (CCategory cat in lst)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    tabs.Controls.Add(li);
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    string url = "CategoryPage.aspx?id=" + cat.ID.ToString();
                    anchor.Attributes.Add("href", url);
                    //anchor.Attributes.Add("runat", "server");
                    //anchor.Attributes.Add("onclick", "return MyFunction();");
                    anchor.InnerText = cat.Caption;
                    li.Controls.Add(anchor);
                }
            }
            else
            {
                //ItemGroups:
                List<CItemGroup> lst = COutlet.ListOfItemGroups(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                foreach (CItemGroup ig in lst)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    tabs.Controls.Add(li);
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    string url = "MenuItemPage.aspx?id=" + ig.ID.ToString();
                    anchor.Attributes.Add("href", url);
                    //anchor.Attributes.Add("runat", "server");
                    //anchor.Attributes.Add("onclick", "return MyFunction();");
                    anchor.InnerText = ig.Name;
                    li.Controls.Add(anchor);
                }
            }

            //DeliveryMethods:
            List<CDeliveryMode> lstDelMode = COutlet.ListOfDeliveryModes(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
            foreach (CDeliveryMode delMode in lstDelMode)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                delTabs.Controls.Add(li);
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                //string url = "MenuItemPage.aspx?id=" + ig.ID.ToString();
                //anchor.Attributes.Add("href", url);
                //anchor.Attributes.Add("runat", "server");
                //anchor.Attributes.Add("onclick", "return MyFunction();");
                anchor.InnerText = delMode.Name;
                li.Controls.Add(anchor);
            }




        }






        //old coding -just for reference
        protected void chkCond_OnCheckedChanged1(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            ListViewItem item0 = (ListViewItem)chk.Parent.Parent;
            HiddenField hf0 = (HiddenField)item0.FindControl("hf_CondObject");
            CCondiment cond0 = JsonConvert.DeserializeObject<CCondiment>(hf0.Value);

            if (!cond0.isEmpty)
            {
                bool blnFound = false;
                foreach (ListViewItem item1 in lvwItemCond.Items)
                {
                    HiddenField hf1 = (HiddenField)item1.FindControl("hf_CondObject");
                    CCondiment cond1 = (CCondiment)JsonConvert.DeserializeObject<CCondiment>(hf1.Value);
                    if (!cond1.isEmpty)
                    {
                        if (cond0.ID == cond1.ID)
                        {
                            blnFound = true;
                            if (!chk.Checked)
                            {
                                lvwItemCond.Items.RemoveAt(item1.DisplayIndex);
                                ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments.Remove(cond1);
                            }
                            break;
                        }

                    }
                }

                if (!blnFound)
                {
                    if (chk.Checked)
                    {
                        ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments.Add(cond0);
                        List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;

                        lvwItemCond.DataSource = lst;
                        lvwItemCond.DataBind();

                        chk.Enabled = false;
                    }
                }


            }

            ShowModalPopupExtender_Condiment();

        }

        protected void btnDeleteItemCond_Click1(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string sArg = btn.CommandArgument;
            bool isNumeric = int.TryParse(sArg, out int iCondID);
            if (isNumeric)
            {
                ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().RemoveCondiment(iCondID);
                List<CCondiment> lst = ApplicationSession.SalesMaster.CollectionSalesDetail().SelectedItem().Condiments;
                lvwItemCond.DataSource = lst;
                lvwItemCond.DataBind();


                ShowModalPopupExtender_Condiment();
            }
        }



        protected void btnOK_CategoryInfo_Clicked(object sender, EventArgs e)
        {
            bool isNumeric = decimal.TryParse(txtQty_CategoryInfo.Text, out decimal dQty);
            if (isNumeric)
            {
                ApplicationSession.category.OrderQty = dQty;

                DateTime dt;
                bool isDate = DateTime.TryParseExact(txtDate_CategoryInfo.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                if (isDate)
                {
                    string s = dt.ToString("yyyy-MM-dd") + " " + txtTime_CategoryInfo.Text;
                    DateTime dtOrder;
                    isDate = DateTime.TryParseExact(s, "yyyy-MM-dd HH\\:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOrder);
                    if (isDate)
                    {
                        ApplicationSession.category.OrderDate = dtOrder;
                    }
                }

                PopupWindowCatInfoClicked(null, new EventArgs());
               
            }
        }

        
        protected void popup_CategoryInfo_btnPlusMinus_Click(object sender, EventArgs e)
        {
            bool isNumeric = decimal.TryParse(txtQty_CategoryInfo.Text, out decimal dQty);
            if (!isNumeric) { dQty = 0; }

            Button btn = (Button)sender;
            if (btn.CommandName == "plus")
            {
                dQty += 1;
            }
            else
            {
                decimal dMinQty = ApplicationSession.category.MinQty;
                if (dQty > dMinQty)
                {
                    dQty -= 1;
                }
            }

            txtQty_CategoryInfo.Text = dQty.ToString();
            ModalPopupExtender_CategoryInfo.Show();
        }

        public void DisplayModalPopupExtender_CategoryInfo()
        {
            CCategory cat = ApplicationSession.category;

            lblName_CategoryInfo.Text = cat.Caption;
            if (cat.OrderDate != null)
            {
                DateTime dt = (DateTime)cat.OrderDate;
                txtDate_CategoryInfo.Text = dt.ToString("yyyy-MM-dd");
                txtTime_CategoryInfo.Text = dt.ToString("hh\\:mm");
            }
            
            if (cat.OrderQty == 0) { cat.OrderQty = cat.MinQty; }
            txtQty_CategoryInfo.Text = cat.OrderQty_ToString;

            if (cat.MinQty > 0)
            {
                lblMinQty_CategoryInfo.Visible = true;
                lblMinQty_CategoryInfo.Text = "Minimum Order: " + cat.MinQty_ToString + " pax";
            }
            else
            {
                lblMinQty_CategoryInfo.Visible = false;
            }
            

            ModalPopupExtender_CategoryInfo.Show();
        }
        

    }
}