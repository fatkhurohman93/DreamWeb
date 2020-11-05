<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="NotifResetPass.aspx.cs" Inherits="DreamWeb.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .userstyle{
            margin:10px;
            font-size:medium;
            font-family:'Century Schoolbook';
            width:100%;
        }
        .resendstyle{
            margin:30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:Panel runat="server" HorizontalAlign="Center" CssClass="resendstyle">
        <asp:Label runat="server" ID="lblUserID" Text="UserID" CssClass="userstyle"></asp:Label>
        <asp:TextBox runat="server" ID="txtUserID" TextMode="Email" CssClass="userstyle"></asp:TextBox>
        <asp:Button runat="server" ID="btnSend" Text="Send" CssClass="userstyle" OnClick="btnSend_Click" BorderStyle="Ridge"/>
    </asp:Panel>
    <asp:Panel runat="server" HorizontalAlign="Center" CssClass="resendstyle">
        <asp:Label runat="server" ID="lblNotif"></asp:Label>
    </asp:Panel>
    <asp:Panel runat="server" HorizontalAlign="Center">
        <asp:Label runat="server" ID="lblStatus"></asp:Label>
        <asp:LinkButton runat="server" Font-Italic="true" CssClass="userstyle" ID="linkBtn" OnClick="linkBtn_Click"></asp:LinkButton>
        <br />       
        <asp:Button runat="server" ID="btnLogin" BorderStyle="Ridge" Text="Back to Login screen" CssClass="userstyle" OnClick="btnLogin_Click"/>
    </asp:Panel>

</asp:Content>
