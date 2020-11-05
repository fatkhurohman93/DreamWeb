<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CategoryPage.aspx.cs" Inherits="DreamWeb.CategoryPage" %>

<%@ MasterType VirtualPath="~/MainPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .slideimg {
            height: 250px;
            width: 100%;
        }

        .titlestyle {
            background-color: #E5E5FE;
            margin: 10px;
        }

        /*.namestyle {
            color: #006600;
            font-size: 26px;
            font-weight: bold;
            margin: 10px;
        }*/
        .pricestyle {
            color: black;
            font-size: 20px;
            font-weight: bold;
            margin: 5px;
        }

        .btnsubmit {
            margin: 10px;
            width: 100px;
        }

        .lstviewstyle {
            margin-right: 0px;
            margin-bottom: 49px;
            width: 100%;
            height: auto;
        }

        .lblket {
            font-family: 'Century Schoolbook';
            font-size: small;
            font-style: italic;
        }
    </style>
    <script type="text/javascript">
        var i = 1;
        function fun() {
            debugger;
            i++;
            document.getElementById("<%=img1.ClientID%>").src = "./images/img" + i + ".jpg";
            if (i == 3) { i = 0; }

        }
        setInterval("fun()", 2000);

        function checkDate(sender, args) {
            if (sender._selectedDate < new Date()) {
                alert("You cannot select a day earlier than today!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="css/dreamstyle.css" rel="stylesheet" />
            <asp:Panel runat="server" HorizontalAlign="Center">
                <%--<asp:Image ID="img1" runat="server"  src="./images/img1.png" CssClass="slideimg"/>--%>
                <asp:ImageButton ID="img1" runat="server" ImageUrl="./images/img1.jpg" CssClass="slideimg" OnClick="SlideImage_Click" />
            </asp:Panel>
            <asp:Panel runat="server" HorizontalAlign="Center">
                <asp:Label ID="lblMessage" runat="server" CssClass="labelMessage"></asp:Label>
            </asp:Panel>
            <asp:Label ID="lblTitle" runat="server" CssClass="titlestyle"></asp:Label>
            <asp:Panel runat="server" HorizontalAlign="Center" CssClass="lstviewstyle">
                <asp:ListView ID="lvwCategory" runat="server" OnItemDataBound="lvwCategory_ItemDataBound" OnItemCommand="lvwCategory_ItemCommand">
                    <LayoutTemplate>
                        <asp:Panel ID="itemPlaceholder" runat="server"></asp:Panel>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <asp:Panel runat="server" CssClass="itemlistsimple" Width="100%">
                            <asp:Label ID="lblName" runat="server" CssClass="namestyle"> <%# Eval("Caption") %> </asp:Label>
                            <br />
                            <div runat="server" style="float: left; margin: 5px;">
                                <asp:ImageButton ImageUrl='<%# Eval("ID", "ImageCSharp.aspx?name=category&id={0}")%>' runat="server"
                                    CommandName="view" CommandArgument='<%# Eval("ID") %>' Width="100" Height="100" />
                            </div>
                            <%--<asp:Panel runat="server" Width="100%">--%>
                            <asp:Label ID="lblPrice" runat="server"> <%# Eval("UnitPrice_ToString") %> </asp:Label>
                            <br />
                            <asp:Label ID="lblNotes" runat="server" CssClass="labelCondiment"> <%# Eval("Notes") %> </asp:Label>
                            <br />
                            <asp:Label ID="lblDesc" runat="server" CssClass="lblket"> </asp:Label>
                            <%--</asp:Panel>--%>

                            <asp:HiddenField runat="server" ID="hf_Category" />
                            <div runat="server" style="margin: 5px;">
                                <asp:LinkButton ID="btnDetail" runat="server" Text="view" CommandName="view" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                            </div>
                        </asp:Panel>
                        <%--<div runat="server" style="float: left;width:100%" class="itemlistsimple" >

                     
                     
                </div>--%>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>

          
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
