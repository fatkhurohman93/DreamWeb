using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;


namespace DreamWeb
{
    public partial class SalesTypePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sOutletID = Request.QueryString["oid"];
                if (sOutletID != null)
                {
                    bool isNumeric = int.TryParse(sOutletID, out int iOutletID);
                    if (isNumeric)
                    {
                        CMain.InitParams(iOutletID);
                    }
                }

                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                List<CSalesType> lst = COutlet.ListOfSalesTypes(conn, ApplicationSession.StoreID, ApplicationSession.OutletID);
                lvwSalesType.DataSource = lst;
                lvwSalesType.DataBind();
            }
        }

        protected void lvwSalesType_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (ListViewDataItem item = (ListViewDataItem)e.Item)
            {
                if (item != null)
                {
                    HiddenField hf = (HiddenField)item.FindControl("hf_STobject");
                    CSalesType st = (CSalesType)item.DataItem;
                    string sJson = JsonConvert.SerializeObject(st);
                    hf.Value = sJson;
                }
            }
        }

        protected void SalesType_Clicked(object sender, EventArgs e)
        {
            HtmlAnchor a = (HtmlAnchor)sender;
            //ListViewItem item0 = (ListViewItem)a.Parent.Parent;
            HiddenField hf0 = (HiddenField)a.FindControl("hf_STobject");
            CSalesType st = JsonConvert.DeserializeObject<CSalesType>(hf0.Value);

            if (!st.isEmpty)
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                st.FetchListOfOrderPromos(conn, ApplicationSession.StoreID, ApplicationSession.OutletID, CSetting.GetFlagOfToday());
                ApplicationSession.SalesType = st;
                if (st.IsCatering())
                {
                    Response.Redirect("CategoryPage.aspx");
                }
                else 
                {
                    if (st.IsType(CSalesType.EFlagType.TYPE_DINEIN))
                    {
                        //CreateOrder:
                        //>>input: CoverAmt & TableNo

                        CSalesMaster sm = new CSalesMaster();
                        sm.CreateNewSales(ApplicationSession.StoreID, ApplicationSession.OutletID, ApplicationSession.SalesType.ID, 
                            1, "", 0, (int)CSalesMaster.EFlagStatus.STATUS_ORDER, 0, "", "", 0, "", true);

                        if (sm.InsertRecord(conn))
                        {
                            string str = "MainMenu.aspx?qr=010101&sm=" + sm.ID.ToString();
                            Response.Redirect(str);

                            //System.Drawing.Bitmap imgQR = CMain.CreateQRCode(str);
                            //Response.ContentType = "Image/jpeg";
                            //imgQR.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            //imgQR.Save(Server.MapPath("~/images/QRcode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                            //imgQRcode.ImageUrl = "~/images/QRcode.jpg";
                            //aOrderNo.InnerText = str;
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalConfirmed", "$(document).ready(function () {$('#ModalConfirmed').modal();});", true);
                            //Master.DisplayModalMessageBox("testing...");
                        }
                    }
                    else
                    {
                        Response.Redirect("ItemGroupPage.aspx");
                    }
                }
                
            }
        }


    }
}