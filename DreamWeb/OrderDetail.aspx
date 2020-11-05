<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="DreamWeb.OrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>

    <style type="text/css">
        .invoice-title h2, .invoice-title h3 {
            display: inline-block;
        }

        .table > tbody > tr > .no-line {
            border-top: none;
        }

        .table > thead > tr > .no-line {
            border-bottom: none;
        }

        .table > tbody > tr > .thick-line {
            border-top: 2px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="best_burgers_area" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="invoice-title">
                        <h2>Order Details</h2>
                        <h3 class="pull-right">Order No:
                            <label runat="server" id="lblOrderNo"></label>
                        </h3>
                    </div>
                    <hr>
                    <div class="row">
                        <div class="col-xs-6">
                            <address>
                                <strong>Billed To:</strong><br>
                                <asp:Label runat="server" ID="lblBillAddr"></asp:Label>
                            </address>
                        </div>
                        <div class="col-xs-6 text-right">
                            <address>
                                <strong>Shipped To:</strong><br>
                                <asp:Label runat="server" ID="lblDelAddr"></asp:Label>
                            </address>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <address>
                                <strong>Payment Method:</strong><br>
                                <asp:Label runat="server" ID="lblPmtMode"></asp:Label>
                            </address>
                        </div>
                        <div class="col-xs-6 text-right">
                            <address>
                                <strong>Order Date:</strong><br>
                                <asp:Label runat="server" ID="lblTransDate"></asp:Label>
                                <br>
                            </address>
                        </div>
                    </div>
                </div>
            </div>

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
                                            <td><strong>Item</strong></td>
                                            <td class="text-center"><strong>Quantity</strong></td>
                                            <td class="text-center"><strong>UnitPrice</strong></td>
                                            <td class="text-right"><strong>Total</strong></td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:ListView runat="server" ID="lvwOrder" GroupPlaceholderID="groupPlaceholder1" ItemPlaceholderID="itemPlaceholder1">
                                            <ItemTemplate>
                                                <div runat="server" style="float: left">
                                                    <tr>
                                                        <td style="width: 200px; text-align: left" class="tblproductdetail">
                                                            <%# Eval("TransName")%>
                                                        </td>

                                                        <td style="width: 100px; text-align: right" class="tblproductdetail">
                                                            <%# Eval("Qty_ToString")%>
                            
                                                        </td>
                                                        <td style="width: 200px; text-align: right" class="tblproductdetail">
                                                            <%# Eval("UnitPrice_ToString")%>
                                                        </td>
                                                        <td style="width: 200px; text-align: right" class="tblproductdetail">
                                                            <%# Eval("TotalPrice_ToString")%>
                                                        </td>

                                                    </tr>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <tr>
                                            <td class="thick-line"></td>
                                            <td class="thick-line"></td>
                                            <td class="thick-line text-center"><strong>Subtotal</strong></td>
                                            <td class="thick-line text-right"><strong>
                                                <asp:Label runat="server" ID="lblSubtotal" CssClass="lbldetail" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="no-line"></td>
                                            <td class="no-line"></td>
                                            <td class="no-line text-center"><strong>
                                                <asp:Label runat="server" Text="Charge" ID="lblCharge" CssClass="lbldetail" />
                                            </strong></td>
                                            <td class="no-line text-right"><strong>
                                                <asp:Label runat="server" ID="lblChargeAmt" CssClass="lbldetail" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="no-line"></td>
                                            <td class="no-line"></td>
                                            <td class="no-line text-center"><strong>
                                                <asp:Label runat="server" Text="Tax" ID="lblTax" CssClass="lbldetail" />
                                            </strong></td>
                                            <td class="no-line text-right"><strong>
                                                <asp:Label runat="server" ID="lblTaxAmt" CssClass="lbldetail" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="no-line"></td>
                                            <td class="no-line"></td>
                                            <td class="no-line text-center"><strong>TOTAL</strong></td>
                                            <td class="no-line text-right"><strong>
                                                <asp:Label runat="server" ID="lblTotal" CssClass="lbldetail" /></strong>
                                            </td>
                                        </tr>
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
