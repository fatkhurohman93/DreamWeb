<%@ Page Title="" Language="C#" MasterPageFile="~/LogIn.Master" AutoEventWireup="true" CodeBehind="AccountPage.aspx.cs" Inherits="DreamWeb.AccountPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .footer {
            width: 100%;
            float: left;
        }

        .lblMyAcc {
            font-family: 'Century Schoolbook';
            font-size: xx-large;
            font-weight: bold;
        }


        .rewardstyle {
            border: 2px solid;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            padding: 5px;
            background-color: burlywood;
        }

        .lblreward {
            font-family: 'Century Schoolbook';
            font-size: medium;
            font-weight: bold;
        }

        .btnpanel {
            border: 2px solid;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            padding: 5px;
            margin: 10px;
        }

        .divstyle {
            margin-right: 20px;
            margin-left: 20px;
            margin-top: 50px;
            margin-bottom: 20px;
            text-align:center;
        }

        .lblmenu {
            margin-left: 10px;
            font-style: italic;
            font-size: small;
            font-family: 'Century Schoolbook';
        }

        .lblbtnmenu {
            font-family: 'Century Schoolbook';
            font-size: medium;
            font-weight: bold;
            border-style: none;
            background-color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row titlestyle">
            <h2>My Account</h2>
        </div>
        <div class="rewardstyle lblreward divstyle">
            <p>Your Rewards Balance Is: </p>
            <span id="spanReward"></span>
        </div>
        <div class="divstyle">
            <a href="SignUp.aspx?status=member">
                <div class="btnpanel">
                    <img src="./images/usericonblack.png" width="20" height="20" />
                    <span class="lblbtnmenu">Update My Profile</span>
                    <p class="lblmenu">Access Your Account Details</p>
                </div>
            </a>
            <a href="ChangePasswordPage.aspx">
                <div class="btnpanel">
                    <img src="./images/iconchangepass.png" width="20" height="20" />
                    <span class="lblbtnmenu">Update My Password</span>
                    <p class="lblmenu">Keep Your Security Acccesses In Check</p>
                </div>
            </a>
            <a href="OrderHistory.aspx">
                <div class="btnpanel">
                    <img src="./images/iconhistory.png" width="20" height="20" />
                    <span class="lblbtnmenu">My Order History</span>
                    <p class="lblmenu">Keep Tracks Your Order</p>
                </div>
            </a>
            <a href="Login.aspx">
                <div class="btnpanel">
                    <img src="./images/iconlogout.png" width="20" height="20" />
                    <span class="lblbtnmenu">Logout</span>
                </div>
            </a>

        </div>
    </div>

    <div class="footer">
        <p>&copy; <%: DateTime.Now.Year %> - DreamWeb</p>
    </div>
</asp:Content>
