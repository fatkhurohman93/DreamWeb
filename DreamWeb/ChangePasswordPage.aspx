<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="ChangePasswordPage.aspx.cs" Inherits="DreamWeb.ChangePasswordPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pnlStyle{
            margin:20px;
        }
        .footer{
            width: 100%;
            float:left;
        }
        .lblChangePass{
            font-family:'Century Schoolbook';
            font-size:xx-large;
            font-weight:bold;
        }
        .required{
            color:red;
            font-family:'Century Schoolbook';
            font-size:small;
        }
        .lblPass{
            font-family:'Century Schoolbook';
            font-size:medium;
            width:100%;
            margin:10px;
        }
        
        .lblyourpass{
             font-family:'Century Schoolbook';
            font-size:large;
            font-weight:bold;
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:Panel runat="server" HorizontalAlign="Center" CssClass="pnlStyle">
        <asp:Label runat="server" Text="Change Password" CssClass="lblChangePass"></asp:Label>
    </asp:Panel>
    <asp:Panel runat="server">
        <asp:Label runat="server" Text="Your Password" CssClass="lblyourpass"></asp:Label>
        <hr />
    </asp:Panel>
    <asp:Panel runat="server">
        <asp:Label runat="server" Text="*" CssClass="required"></asp:Label>
        <asp:Label runat="server" Text="Password" CssClass="lblPass"></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="txtPass" TextMode="Password" 
            CssClass="lblPass" placeholder="Password"></asp:TextBox>
        <br />
        <asp:Label runat="server" Text="*" CssClass="required"></asp:Label>
        <asp:Label runat="server" Text="Password Confirm" CssClass="lblPass"></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="txtConfirm" TextMode="Password" 
            CssClass="lblPass" placeholder="Password"></asp:TextBox>
        <br />
        <asp:Panel runat="server">
            <asp:Button runat="server" Text="Cancel" ID="btnBack" OnClick="btnCancel_Click" CssClass="lblPass"/>
            <asp:Button runat="server" Text="Continue" ID="btnChange" OnClick="btnChange_Click" CssClass="lblPass" />
        </asp:Panel>
    </asp:Panel>

    <div class="footer">
        <p>&copy; <%: DateTime.Now.Year %> - DreamWeb</p>
    </div>
</asp:Content>
