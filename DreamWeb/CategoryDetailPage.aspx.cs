using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DreamLib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DreamWeb
{
    public partial class CategoryDetailPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Master.PopupWindowCatInfoClicked += new EventHandler(MasterCatInfoEdit);
        }

        protected void MasterCatInfoEdit(object sender, EventArgs e)
        {
            CCategory cat = ApplicationSession.category;
            lblDateOfFunction.Text = cat.OrderDate_ToString;
            lblNoOfPax.Text = cat.OrderQty_ToString;
            //UpdatePanel1.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                CCategory cat = ApplicationSession.category;
                if (cat.IsEmpty())
                {
                    lblMessage.Text = "No Record of Category";
                }
                else
                {
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    lblTitle.Text = cat.Caption;
                    List<CCatGroup> lst = cat.ListOfCategoryGroup(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                    lvwCatGroup.DataSource = lst;
                    lvwCatGroup.DataBind();

                    lblDateOfFunction.Text = cat.OrderDate_ToString;
                    lblNoOfPax.Text = cat.OrderQty_ToString;
                }
            }
        }


        protected void SlideImage_Click(object sender, EventArgs e) //not working yet
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            List<CCatDetail> lst = new List<CCatDetail>();
            foreach (ListViewDataItem grp in lvwCatGroup.Items)
            {
                HiddenField hf = (HiddenField)grp.FindControl("hf_GroupQty");
                Label lbl = (Label)grp.FindControl("lblCatGroupName");
                bool isNumeric = decimal.TryParse(hf.Value, out decimal dGroupQty);
                if (isNumeric)
                {
                    decimal dItemQty = 0;
                    List<CCatDetail> lstTemp = new List<CCatDetail>();
                    ListView lvwCatDetail = (ListView)grp.FindControl("lvwCatDetail");
                    foreach (ListViewDataItem item in lvwCatDetail.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkCatDetail");
                        if (chk.Checked)
                        {
                            dItemQty += 1;
                            if (dItemQty > dGroupQty)
                            {
                                lblMessage.Text = "Please only select " + string.Format("{0:#,0}", dGroupQty) + " items from the group of " + lbl.Text;
                                return;
                            }
                            else
                            {
                                HiddenField hf_CatDetail = (HiddenField)item.FindControl("hf_CatDetail");
                                CCatDetail catDetail = JsonConvert.DeserializeObject<CCatDetail>(hf_CatDetail.Value);
                                lstTemp.Add(catDetail);
                            }
                        }
                    }
                    if (dItemQty < dGroupQty)
                    {
                        lblMessage.Text = "Please select " + string.Format("{0:#,0}", dGroupQty) + " item(s) from the group of " + lbl.Text;
                        return;
                    }

                    lst.AddRange(lstTemp);
                }
            }

            CreateObjectMyCart(lst);
        }

        private void CreateObjectMyCart(List<CCatDetail> lstCatDetail)
        {
            CCategory cat = ApplicationSession.category;
            ApplicationSession.idx += 1;
            int flg = (int)CSalesDetail.EFlagStatus.TRANS_ITEM;
            CSalesDetail sd = new CSalesDetail(ApplicationSession.idx, cat.ID, "Category", cat.Caption, "", cat.OrderQty, cat.Price, 
                                               "", cat.Notes, "", 0, ApplicationSession.StoreID,
                                               ApplicationSession.OutletID, flg, ApplicationSession.ChairNo);

            flg = (flg | (int)CSalesDetail.EFlagStatus.TYPE_DETAIL);
            foreach (CCatDetail catDetail in lstCatDetail)
            {
                ApplicationSession.idx += 1;
                CSalesDetail obj = new CSalesDetail(ApplicationSession.idx, catDetail.ItemMasterID, catDetail.ItemCode, catDetail.ItemName, 
                                                    catDetail.GroupName, catDetail.ItemQty, catDetail.ItemPrice, catDetail.UnitSymbol, catDetail.Notes, "", 0, ApplicationSession.StoreID,
                                               ApplicationSession.OutletID, flg, ApplicationSession.ChairNo);
                sd.Children.Add(obj);
            }

            ApplicationSession.SalesMaster.CollectionSalesDetail().Add(sd);
            //Master.MyProperty = ApplicationSession.QtyCart;
            Master.DisplayOrderItem(sd);
        }

        protected void lvwCatGroup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    CCatGroup cg = (CCatGroup)item.DataItem;
                    ListView lvwCatDetail = (ListView)item.FindControl("lvwCatDetail");
                    List<CCatDetail> lst = cg.ListOfCategoryItems;
                    lvwCatDetail.DataSource = lst;
                    lvwCatDetail.DataBind();
                }
            }
        }

        protected void lvwCatDetail_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_CatDetail");
                    CCatDetail cd = (CCatDetail)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(cd);
                    hf.Value = sJson;
                }
            }
        }


        protected void chkCatDetail_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                //ListViewItem item = (ListViewItem)chk.Parent.Parent;
            }
        }

        
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Master.DisplayModalDialog_CategoryInfo();
        }

    }
}