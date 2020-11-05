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
    public partial class MainMenu : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string sScanQR = Request.QueryString["qr"];
                if (sScanQR == null)
                {
                    if (ApplicationSession.OutletID == 0)
                    {
                        ApplicationSession.QRcode = "";
                        //TEMPORARY:
                        //DisplayItemGroups();
                        Response.Redirect("Login.aspx"); ;
                    }
                    else
                    {
                        DisplayItemGroups();
                    }
                    
                }
                else
                {
                    ApplicationSession.QRcode = sScanQR;
                    if (InitFromQRCode(sScanQR))
                    {
                        string sSMID = Request.QueryString["sm"];
                        if (sSMID != null)
                        {
                            if (InitFromSMID(sSMID)) { DisplayItemGroups(); }
                        }
                        else
                        {
                            /* with tableNo & chairNo*/
                            string sTableNo = Request.QueryString["tbl"];
                            if (sTableNo != null)
                            {
                                InitFromTableNo(sTableNo);

                                string sChairNo = Request.QueryString["chr"];
                                if (sChairNo == null) { sChairNo = ""; }
                                ApplicationSession.ChairNo = sChairNo;

                                DisplayItemGroups();
                            }
                            
                        }
                    }
                }
            }
        }

        private void DisplayItemGroups()
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            COutlet outlet = new COutlet(ApplicationSession.OutletID, conn);
            lblTitle.InnerText = outlet.Name;
            lblSlogan.Text = "Welcome!";

            List<CItemGroup> lst = COutlet.ListOfItemGroups(conn, outlet.StoreID, outlet.ID);
            lvItemGroup.DataSource = lst;
            lvItemGroup.DataBind();
        }

        private bool InitFromQRCode(string sQR)
        {
            bool bln = false;
            int iStoreID = 0;
            int iOutletID = 0;
            int iSalesTypeID = 0;

            if (sQR.Length == 6)
            {
                //char 1&2: StoreID
                string sStoreID = sQR.Substring(0, 2);
                bool isNumeric = int.TryParse(sStoreID, out int storeID);
                if (isNumeric) { iStoreID = storeID; }

                if (iStoreID == 0) { iStoreID = CMain.STOREID; }

                CStore store = CMain.GetStoreRecord(iStoreID);
                if (store.IsEmpty())
                {
                    MessageBox.Show("Store Record is not found");
                }
                else
                {
                    ApplicationSession.DBName = store.DBName;
                    ApplicationSession.StoreID = iStoreID;
                    ApplicationSession.member = new CMiniMember("walk-in", 0, "");

                    //char 3&4: OutletID
                    string sOutletID = sQR.Substring(2, 2);
                    isNumeric = int.TryParse(sOutletID, out int outletID);
                    if (isNumeric) { iOutletID = outletID; }
                    if (iOutletID == 0) { iOutletID = CMain.OUTLETID; }
                    CMain.InitParams(iOutletID);

                    //char 5&6: SalesTypeID
                    string sSalesTypeID = sQR.Substring(4, 2);
                    isNumeric = int.TryParse(sSalesTypeID, out int stID);
                    if (isNumeric) { iSalesTypeID = stID; }
                    if (iSalesTypeID == 0) { iSalesTypeID = CMain.SALESTYPEID; }
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    ApplicationSession.SalesType = CSalesType.FetchSalesType(conn, iSalesTypeID);

                    bln = true;
                }
            }
            else
            {
                MessageBox.Show("Parameter is not in the right format");
            }

            return bln;

        }

        private bool InitFromSMID(string sSMID)
        {
            bool bln = false;
            bool isNumeric = int.TryParse(sSMID, out int iSMID);
            if (isNumeric)
            {
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);

                CSalesMaster sm = new CSalesMaster(iSMID, conn);
                if (!sm.IsEmpty())
                {
                    sm.RetrieveCollection(conn);
                    ApplicationSession.SalesMaster = sm;
                    bln = true;
                }
            }
            else
            {
                MessageBox.Show("Sales Record is not found");
            }

            return bln;
        }

        private void InitFromTableNo(string sTableNo)
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);

            CSalesMaster sm = new CSalesMaster(ApplicationSession.StoreID, ApplicationSession.OutletID, sTableNo, conn);
            if (!sm.IsEmpty())
            {
                sm.RetrieveCollection(conn);
            }
            ApplicationSession.SalesMaster = sm;
            ApplicationSession.TableNo = sTableNo;  
        }

    }


}