<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="MyCart.aspx.cs" Inherits="DreamWeb.MyCart" %>

<%@ MasterType VirtualPath="~/MainPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <!-- bradcam_area_start -->
    <div class="bradcam_area breadcam_bg overlay">
        <h3 id="lblMenu" runat="server"></h3>
    </div>
    <!-- bradcam_area_end -->

    <!-- best_burgers_area_start  -->
    <div class="best_burgers_area" runat="server">
        <!--   <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate> -->
        <div class="container">
            <div class="row">
                <div class="col-sm-12 col-md-10 col-md-offset-1">
                    <asp:ListView runat="server" ID="lvwOrder" ItemPlaceholderID="itemPlaceholder1">
                        <LayoutTemplate>
                            <table class="table table-hover">
                                <tr id="itemPlaceholder1" runat="server"></tr>
                                <thead>
                                    <tr>
                                        <th>Product</th>
                                        <th>Quantity</th>
                                        <th class="text-center">Price</th>
                                        <th class="text-center">Total</th>
                                        <th></th>
                                    </tr>
                                </thead>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="col-sm-8 col-md-6">
                                    <div class="media">
                                        <a class="thumbnail pull-left" href="#">
                                            <img class="media-object" src='<%# Eval("TransID", "images/itemmaster/{0}.jpg")%>' style="width: 120px; height: 100px;">
                                        </a>
                                        <div class="media-body">
                                            <h4 class="media-heading"><a href="#"><%# Eval("TransName") %></a></h4>
                                            <h5 class="media-heading"><a href="#"><%# Eval("ItemDesc") %></a></h5>
                                            <!--    <span>Status: </span><span class="text-success"><strong>In Stock</strong></span> -->
                                        </div>
                                    </div>
                                </td>
                                <td class="col-sm-1 col-md-1">
                                    <asp:HiddenField runat="server" ID="hf_TempID" Value='<%# Eval("TempID")%>'></asp:HiddenField>
                                    <asp:TextBox ID="txtQty" Enabled='<%# Eval("IsNotSent") %>' runat="server" TextMode="Number" Text='<%# Eval("Qty_ToString") %>' OnTextChanged="txtQty_TextChanged" AutoPostBack="true" />
                                </td>
                                <td class="col-sm-1 col-md-1 text-center"><strong><%# Eval("UnitPrice_ToString")%></strong></td>
                                <td class="col-sm-1 col-md-1 text-center">
                                    <asp:Label ID="lblTotalPrice" runat="server" Font-Bold="true" Text='<%# Eval("TotalPrice_ToString") %>' />
                                </td>
                                <td class="col-sm-1 col-md-1">
                                    <div>
                                        <button runat="server" id="btnRemove" type="button" visible='<%# Eval("IsNotSent") %>' class="btn btn-danger"
                                            onserverclick="btnRemove_Click">
                                            <span class="glyphicon glyphicon-remove"></span>Remove
                                        </button>
                                    </div>
                                    <div>
                                        <asp:Label runat="server" Visible='<%# Eval("IsSent") %>' Text="Sent" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="row text-center">
                                Your cart is empty.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <table>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <h5>Subtotal</h5>
                            </td>
                            <td class="text-right">
                                <h5>
                                    <asp:Label ID="lblSubtotalAmt" runat="server" Font-Bold="true" Text="0" />
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <h5>
                                    <asp:Label ID="lblTaxName" runat="server" Text="Tax" />
                                </h5>
                            </td>
                            <td class="text-right">
                                <h5>
                                    <asp:Label ID="lblTaxAmt" runat="server" Font-Bold="true" Text="0" />
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <h3>TOTAL</h3>
                            </td>
                            <td class="text-right">
                                <h3>
                                    <asp:Label ID="lblTotalAmt" runat="server" Font-Bold="true" Text="0" />
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <button type="button" class="btn btn-default" runat="server" onserverclick="btnShopping_Click">
                                    <span class="glyphicon glyphicon-shopping-cart"></span>Continue Browsing
                                </button>
                            </td>
                            <td>
                                <button type="button" class="btn btn-success" runat="server" id="btnCheckout" onserverclick="btnCheckout_Click">
                                    <span class="glyphicon glyphicon-play"></span>
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <!--      </ContentTemplate>
        </asp:UpdatePanel> -->


        <!-- Modal -->
        <div id="ModalConfirmed" class="modal fade" role="dialog" data-backdrop="false">
            <div class="container d-flex justify-content-center">
                <div class="col-md-6">
                    <div class="card text-center">
                        <div class="card-body" style="background-color: lightgray">
                            <img src="img/dreamweb.png">
                            <h5 class="title">Thank YOU!</h5>
                            <p class="description">We have received your order</p>
                            <a class="btn btn-link" runat="server" href="MainMenu.aspx" />
                            <hr />
                            <a class="btn btn-block" href="MainMenu.aspx" id="close-modal">Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
