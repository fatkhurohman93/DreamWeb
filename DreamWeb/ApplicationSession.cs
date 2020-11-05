using System;
using System.Web;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public static class ApplicationSession
    {
       
        //DBName
        public static string DBName
        {
            get
            {
                if (HttpContext.Current.Session["DBName"] != null && HttpContext.Current.Session["DBName"].ToString().Length > 0)
                {
                    return Convert.ToString(HttpContext.Current.Session["DBName"]);
                }
                else
                {
                    HttpContext.Current.Session["DBName"] = "";
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["DBName"] = value;
            }
        }

        //UserID
        private static string UserID
        {
            get
            {
                if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["UserID"].ToString().Length > 0)
                {
                    return Convert.ToString(HttpContext.Current.Session["UserID"]);
                }
                else
                {
                    HttpContext.Current.Session["UserID"] = "";
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        //StoreID
        public static int StoreID
        {
            get
            {
                if (HttpContext.Current.Session["StoreID"] != null && HttpContext.Current.Session["StoreID"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["StoreID"]);
                }
                else
                {
                    HttpContext.Current.Session["StoreID"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["StoreID"] = value;
            }
        }

        //OutletID
        public static int OutletID
        {
            get
            {
                if (HttpContext.Current.Session["OutletID"] != null && HttpContext.Current.Session["OutletID"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["OutletID"]);
                }
                else
                {
                    HttpContext.Current.Session["OutletID"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["OutletID"] = value;
            }
        }

        //SalesTypeID
        private static int SalesTypeID
        {
            get
            {
                if (HttpContext.Current.Session["SalesTypeID"] != null && HttpContext.Current.Session["SalesTypeID"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["SalesTypeID"]);
                }
                else
                {
                    HttpContext.Current.Session["SalesTypeID"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["SalesTypeID"] = value;
            }
        }

        //SalesType
        public static CSalesType SalesType
        {
            get
            {
                object obj = HttpContext.Current.Session["SalesType"];

                if (obj != null)
                {
                    return (CSalesType)obj;
                }
                else
                {
                    CSalesType salesType = new CSalesType();
                    HttpContext.Current.Session["SalesType"] = salesType;
                    return salesType;
                }
            }
            set
            {
                HttpContext.Current.Session["SalesType"] = value;
            }
        }

        //IsCatering
        private static bool IsCatering
        {
            get
            {
                if (HttpContext.Current.Session["IsCatering"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsCatering"]);
                }
                else
                {
                    HttpContext.Current.Session["IsCatering"] = false;
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsCatering"] = value;
            }
        }

        //ItemGroupID
        public static int ItemGroupID
        {
            get
            {
                if (HttpContext.Current.Session["ItemGroupID"] != null && HttpContext.Current.Session["ItemGroupID"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["ItemGroupID"]);
                }
                else
                {
                    HttpContext.Current.Session["ItemGroupID"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["ItemGroupID"] = value;
            }
        }

       

        //Counter
        public static int cnt
        {
            get
            {
                if (HttpContext.Current.Session["cnt"] != null && HttpContext.Current.Session["cnt"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["cnt"]);
                }
                else
                {
                    HttpContext.Current.Session["cnt"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["cnt"] = value;
            }
        }

        //MiniMember
        public static CMiniMember  member
        {
            get
            {
                object obj = HttpContext.Current.Session["member"];

                if (obj != null)
                {
                    return (CMiniMember)(obj);
                }
                else
                {
                    CMiniMember member = new CMiniMember();
                    HttpContext.Current.Session["member"] = member;
                    return member;
                }
            }
            set
            {
                HttpContext.Current.Session["member"] = value;
            }
        }


        public static CCategory category
        {
            get
            {
                object obj = HttpContext.Current.Session["category"];

                if (obj != null)
                {
                    return (CCategory)(obj);
                }
                else
                {
                    CCategory cat = new CCategory();
                    HttpContext.Current.Session["category"] = cat;
                    return cat;
                }
            }
            set
            {
                HttpContext.Current.Session["category"] = value;
            }
        }

        //setting
        public static CSetting SettingRecord
        {
            get
            {
                object obj = HttpContext.Current.Session["setting"];

                if (obj != null)
                {
                    return (CSetting)obj;
                }
                else
                {
                    MySqlConnection conn = CMain.GetConnection(DBName);
                    CSetting setting = new CSetting(conn, StoreID, OutletID);
                    return setting;
                }
            }
            set
            {
                HttpContext.Current.Session["setting"] = value;
            }
        }

        //Index
        public static int idx
        {
            get
            {
                if (HttpContext.Current.Session["idx"] != null && HttpContext.Current.Session["idx"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["idx"]);
                }
                else
                {
                    HttpContext.Current.Session["idx"] = 0;
                    return 0;
                }
                    
            }
            set
            {
                HttpContext.Current.Session["idx"] = value;
            }
        }


        //qty in cart
        public static int QtyCart
        {
            get
            {
                int iQty = 0;
                object obj = HttpContext.Current.Session["sm"];

                if (obj != null)
                {
                    CSalesMaster sm = (CSalesMaster)obj;
                    SalesDetailCollection col = sm.CollectionSalesDetail(QRcode == "");

                    foreach (CSalesDetail item in col.ToList())
                    {
                        iQty += Convert.ToInt32(item.Qty);
                    }

                }
                return iQty;
            }

        }

        //QRcode
        public static string QRcode
        {
            get
            {
                if (HttpContext.Current.Session["QRcode"] != null && HttpContext.Current.Session["QRcode"].ToString().Length > 0)
                {
                    return Convert.ToString(HttpContext.Current.Session["QRcode"]);
                }
                else
                {
                    HttpContext.Current.Session["QRcode"] = "";
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["QRcode"] = value;
            }
        }


        //TableNo
        public static string TableNo
        {
            get
            {
                if (HttpContext.Current.Session["TableNo"] != null && HttpContext.Current.Session["TableNo"].ToString().Length > 0)
                {
                    return Convert.ToString(HttpContext.Current.Session["TableNo"]);
                }
                else
                {
                    HttpContext.Current.Session["TableNo"] = CMain.TABLENO;
                    return CMain.TABLENO;
                }
            }
            set
            {
                HttpContext.Current.Session["TableNo"] = value;
            }
        }

        //ChairNo
        public static string ChairNo
        {
            get
            {
                if (HttpContext.Current.Session["ChairNo"] != null && HttpContext.Current.Session["ChairNo"].ToString().Length > 0)
                {
                    return Convert.ToString(HttpContext.Current.Session["ChairNo"]);
                }
                else
                {
                    HttpContext.Current.Session["ChairNo"] = "";
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["ChairNo"] = value;
            }
        }

        //SMID
        private static int SMID
        {
            get
            {
                if (HttpContext.Current.Session["SMID"] != null && HttpContext.Current.Session["SMID"].ToString().Length > 0)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["SMID"]);
                }
                else
                {
                    HttpContext.Current.Session["SMID"] = 0;
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["SMID"] = value;
            }
        }

        //cart
        public static CSalesMaster SalesMaster
        {
            get
            {
                object obj = HttpContext.Current.Session["sm"];

                if (obj != null)
                {
                    return (CSalesMaster)obj;
                }
                else
                {
                    CSalesMaster sm = new CSalesMaster();
                    HttpContext.Current.Session["sm"] = sm;
                    return sm;
                }
            }
            set
            {
                HttpContext.Current.Session["sm"] = value;
            }
        }
    }
}