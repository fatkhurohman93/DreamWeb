<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="DreamWeb.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- bradcam_area_start -->
            <div class="bradcam_area breadcam_bg overlay">
                <h3>OUTLETS</h3>
            </div>
            <!-- bradcam_area_end -->

            <!-- best_burgers_area_start  -->
            <div class="best_burgers_area" runat="server">
                <div class="container" runat="server">
                    <asp:ListView ID="lvwOutlet" runat="server">
                        <LayoutTemplate>
                            <div class="row" runat="server">
                                <div id="itemPlaceholder" runat="server">
                                </div>
                            </div>
                        </LayoutTemplate>
                        <EmptyDataTemplate>
                            <div class="row" runat="server">
                                <div class="sub" id="itemPlaceholder" runat="server">
                                    No data was returned.
                                </div>
                            </div>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <div class="col-xl-4 col-md-4 col-lg-4 img-thumbnail">
                                <a href='<%# Eval("ID", "SalesTypePage.aspx?oid={0}")%>'>
                                    <img src='<%# Eval("ID", "ImageCSharp.aspx?name=outlet&id={0}")%>' />
                                    <h1><%# Eval("Name") %></h1>
                                    <h4><%# Eval("Address") %></h4>
                                    <h4><%# Eval("Phone") %></h4>
                                    <h4><%# Eval("Email") %></h4>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


