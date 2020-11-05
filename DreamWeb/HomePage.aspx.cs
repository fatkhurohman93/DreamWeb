using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!Page.IsPostBack)
            {
                //ApplicationSession.SalesMaster.CollectionSalesDetail() = new SalesDetailCollection();
                CStore store = CMain.GetStoreRecord(ApplicationSession.StoreID);
                if (store.IsEmpty())
                {
                    MessageBox.Show("Fail to retrieve Store Record");
                }
                else
                {
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);

                    List<COutlet> lst = store.ListOfOutlets(conn);
                    lvwOutlet.DataSource = lst;
                    lvwOutlet.DataBind();                 
                }
            }

        }
        

        protected void SlideImage_Click(object sender, EventArgs e) //not working yet
        {
            ImageButton imgBtn = (ImageButton)sender;
            string sImageURL = imgBtn.ImageUrl; //"./images/img1.jpg"
        }

    }
}

//contoh2 coding blom dipake
/*
//contoh coding untuk display image
private void ViewImage(byte[] buffer)
{
    System.IO.MemoryStream stream1 = new System.IO.MemoryStream(buffer, true);
    stream1.Write(buffer, 0, buffer.Length);
    Bitmap bitmap = (Bitmap)Bitmap.FromStream(stream1, true);
    Response.ContentType = "Image/jpeg";
    bitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
}

protected void Page_Init(object sender, EventArgs e)
{
    //lvwOutlet.ItemCommand += new EventHandler<ListViewCommandEventArgs>(lvwOutlet_ItemCommand);
}

void lvwOutlet_ItemCommand1(object sender, ListViewCommandEventArgs e)
{
    bool isNumeric = int.TryParse((string)e.CommandArgument, out int iOutletID);
    if (isNumeric)
    {
        ApplicationSession.OutletID = iOutletID;
        //masuk ke itemgroup page:
        string url = "ItemGroupPage.aspx";
        Response.Redirect(url);
    }

}

protected void Page_PreInit(object sender, EventArgs e)
{
    //Master.AlertContentPage += new EventHandler(Master_AlertContentPage);
}

private void Master_AlertContentPage(object sender, EventArgs e)
{
    if (sender is ImageButton)
    {
        ImageButton imgBtn = (ImageButton)sender;
        switch (imgBtn.ID)
        {
            case "btnAcc":
                Response.Redirect("AccounPage.aspx");
                break;

            case "btnCart":
                ModalPopupExtender1.Show();
                break;
        }

    }

}
*/
