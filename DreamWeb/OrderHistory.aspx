<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="DreamWeb.OrderHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="best_burgers_area" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><strong>Order summary</strong></h3>
                        </div>
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-condensed">
                                    <thead>
                                        <tr>
                                            <td><strong>Date</strong></td>
                                            <td class="text-center"><strong>Total</strong></td>
                                            <td class="text-center"><strong>Status</strong></td>
                                            <td class="text-right"><strong>Action</strong></td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:ListView runat="server" ID="lvwOrderHistory" GroupPlaceholderID="groupPlaceholder1" ItemPlaceholderID="itemPlaceholder1">
                                            <ItemTemplate>
                                                <div runat="server" style="float: left">
                                                    <asp:Label runat="server" ID="lblOrderNo"></asp:Label>

                                                    <tr style="border-bottom: 2px solid; border-color: lightgrey; width: 100%;">
                                                        <td style="text-align: left; margin: 10px">
                                                            <asp:Label runat="server" ID="lblDate" CssClass="lbldetail"><%# Eval("TransDate_ToString") %></asp:Label>
                                                        </td>
                                                        <td style="text-align: right; margin: 10px">
                                                            <asp:Label runat="server" ID="lblTotal" CssClass="lbldetail"><%# Eval("SalesTotal_ToString") %></asp:Label>
                                                        </td>
                                                        <td style="text-align: right; margin: 10px">
                                                            <asp:Label runat="server" ID="lblStatus" CssClass="lbldetail"><%# Eval("Status_ToString") %></asp:Label>
                                                        </td>
                                                        <td style="text-align: right; margin: 10px">
                                                            <asp:LinkButton runat="server" CssClass="lbldetail" ID="linkView" Text="View" OnClick="linkView_Click" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>

                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
