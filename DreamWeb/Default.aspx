<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/LogIn.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DreamWeb._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txtStyle{
            margin:10px;
            width:300px;
            height:50px;
            font-size:large;
        }
       
        .ForgetPass{
            width:50%;
            margin:5px;
        }
        .SignUp{
            width:50%;
            margin:5px;
            font-style:italic;
        }
        
    </style>
</asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="jumbotron" style="background-color:navy;margin:10px;">
        <asp:Panel runat="server" HorizontalAlign="Center">
            <img src="./images/LogoDreamWeb.jpg" alt="" runat="server" width="230" height="70"/>
        </asp:Panel>
    </div>
    <hr />
    <asp:Panel runat="server" HorizontalAlign="Center">
        
        <div class="divstyle">
            <asp:Panel runat="server" HorizontalAlign="Center">
                <asp:Label ID="lblMessage" runat="server"  > </asp:Label>
                
            </asp:Panel>
            <asp:Panel runat="server" HorizontalAlign="Center" CssClass="btnpanel">
                <div>
                    <asp:textbox ID="txtUserID" runat="server" placeholder="Email" CssClass="txtStyle" > </asp:textbox>

                </div>
            </asp:Panel>
            <asp:Panel runat="server" HorizontalAlign="Center" CssClass="btnpanel">
                <div>
                    <asp:textbox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password" CssClass="txtStyle"> </asp:textbox>
                </div>
            </asp:Panel>

            <asp:button ID="btnLogin" runat="server" Text="Login" CssClass="txtStyle" OnClick="btnLogin_Click"></asp:button>
            <br />
            <br />
             <asp:Panel runat="server" HorizontalAlign="Center">
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Forget Password?" OnClick="btnForgetPass_Click" CssClass="ForgetPass"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" CssClass="SignUp"></asp:LinkButton>
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>



