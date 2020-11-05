<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CategoryDetailPage.aspx.cs" Inherits="DreamWeb.CategoryDetailPage" %>

<%@ MasterType VirtualPath="~/MainPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lblket {
            font-family: 'Century Schoolbook';
            font-size: small;
            font-style: italic;
        }

        .btncheckout {
            border: 2px solid;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            font-family: 'Century Schoolbook';
            font-size: medium;
            background-color: burlywood;
            border-color: burlywood;
            float: right;
            margin: 20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <link href="css/dreamstyle.css" rel="stylesheet" />
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <asp:Panel runat="server" HorizontalAlign="Center">
        <%--<asp:Image ID="img1" runat="server"  src="./images/img1.png" CssClass="slideimg"/>--%>
        <asp:ImageButton ID="img1" runat="server" ImageUrl="./images/img1.jpg" CssClass="slideimg" OnClick="SlideImage_Click" />
    </asp:Panel>
    <asp:Panel runat="server" HorizontalAlign="Center">
        <asp:Label ID="lblMessage" runat="server" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <asp:Label ID="lblTitle" runat="server" CssClass="titlestyle"></asp:Label>
    <br />
    <br />
    <asp:Panel runat="server" BorderStyle="Ridge" BorderWidth="1">
        <asp:Label runat="server">Date of Function: </asp:Label>
            <asp:Label runat="server" ID="lblDateOfFunction" Width="40%"></asp:Label>
        
            <asp:Label runat="server">Number of Pax: </asp:Label>
            <asp:Label runat="server" ID="lblNoOfPax" Width="10%" ></asp:Label>
        <asp:LinkButton ID="btnEdit" runat="server" Text="edit" CommandName="edit" Width="20%" CssClass="lblket" OnClick="btnEdit_Click" ></asp:LinkButton>
    </asp:Panel>

   
            <div style="float: left" runat="server">
                <asp:ListView runat="server" ID="lvwCatGroup" GroupPlaceholderID="groupPlaceholder2" ItemPlaceholderID="itemPlaceholder2" OnItemDataBound="lvwCatGroup_ItemDataBound">
                    <LayoutTemplate>
                        <table>
                            <tr id="groupPlaceholder2" runat="server"></tr>
                            <tr id="itemPlaceholder2" runat="server"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div runat="server" style="float: left" cssclass="itemlistsimple">
                            <%--<table border="1" style="border:solid">--%>
                            <tr>
                                <td style="text-align: left">
                                    <hr style="border-color: gray" />
                                    <asp:Label ID="lblCatGroupName" runat="server" CssClass="labellvtitle"><%# Eval("GroupName") %></asp:Label>
                                    <asp:HiddenField runat="server" ID="hf_GroupQty" Value='<%# Eval("GroupQty")%>' />
                                    <asp:Label ID="lblDesc" runat="server" Width="200px" CssClass="lblket"><%# Eval("GroupDesc") %></asp:Label>
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lvwCatDetail" runat="server" GroupPlaceholderID="groupPlaceholder2" ItemPlaceholderID="itemPlaceholder2" OnItemDataBound="lvwCatDetail_ItemDataBound">
                                        <LayoutTemplate>
                                            <table>
                                                <tr id="groupPlaceholder2" runat="server"></tr>
                                                <tr id="itemPlaceholder2" runat="server"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <div runat="server" style="float: left" cssclass="itemlistsimple">
                                                <asp:CheckBox runat="server" ID="chkCatDetail" Checked="false" AutoPostBack="true"
                                                    OnCheckedChanged="chkCatDetail_OnCheckedChanged" />
                                                <asp:HiddenField runat="server" ID="hf_CatDetail" />
                                                <asp:Label ID="Label2" runat="server"
                                                    Text='<%#Eval("ItemName")%>' Width="150px" CssClass="labelCondiment"></asp:Label>

                                                <%--<hr />--%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <%--</table>--%>

                            <%--<br />--%>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>

            <br />
            <div runat="server" style="float: right;">
                <asp:Button ID="btnOK" runat="server" Text="OK" CommandName="ok" OnClick="btnOK_Click" CssClass="btncheckout" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
