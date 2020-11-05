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
    public partial class CategoryPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Master.PopupWindowCatInfoClicked += new EventHandler(MasterCatInfoEdit);
        }

        protected void MasterCatInfoEdit(object sender, EventArgs e)
        {
            string url = "CategoryDetailPage.aspx";
            Response.Redirect(url);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strID = Request.QueryString["id"];
                if ((strID == null) || (strID == "0"))
                {
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    List<CCategory> lst = CCategory.ListOfCategoryZero(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                    lvwCategory.DataSource = lst;
                    lvwCategory.DataBind();
                }
                else
                {
                    bool isNumeric = int.TryParse(strID, out int iCategoryID);
                    if (isNumeric)
                    {
                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                        CCategory cat = new CCategory(iCategoryID, conn);
                        lblTitle.Text = cat.Caption;
                        List<CCategory> lst = cat.ListOfChildren(conn);
                        lvwCategory.DataSource = lst;
                        lvwCategory.DataBind();
                    }
                }


            }
        }
        protected void SlideImage_Click(object sender, EventArgs e) //not working yet
        {

        }


        protected void lvwCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_Category");
                    CCategory cat = (CCategory)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(cat);
                    hf.Value = sJson;

                    Label lblPrice = (Label)item.FindControl("lblPrice");
                    if (cat.Price == 0)
                    {
                        lblPrice.Visible = false;
                    }
                    else
                    {
                        lblPrice.Visible = true;
                        lblPrice.Text = cat.UnitPrice_ToString;
                    }

                    LinkButton btn = (LinkButton)item.FindControl("btnDetail");
                    btn.Visible = true;

                    if (cat.HasDetail)
                    {
                        if (cat.MinQty > 0)
                        {
                            Label lblDesc = (Label)item.FindControl("lblDesc");
                            lblDesc.Text = "Minimum Order: " + cat.MinQty_ToString + " pax";
                        }
                        btn.Text = "order";
                    }
                    else
                    {
                        btn.Text = "view";
                    }
                }
            }
        }


        protected void lvwCategory_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListViewItem item = e.Item;
            HiddenField hf = (HiddenField)item.FindControl("hf_Category");
            CCategory cat = JsonConvert.DeserializeObject<CCategory>(hf.Value);

            if ((cat != null) && (!cat.IsEmpty()))
            {
                ApplicationSession.category = cat;
                if (cat.HasDetail)
                {
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    if (cat.HasMenuOption(conn, ApplicationSession.StoreID, ApplicationSession.OutletID))
                    {
                        Master.DisplayModalDialog_CategoryInfo();
                    }
                }
                else
                {
                    string url = "CategoryPage.aspx?id=" + cat.ID.ToString();
                    Response.Redirect(url);
                }
            }
        }


  
       
    }
}