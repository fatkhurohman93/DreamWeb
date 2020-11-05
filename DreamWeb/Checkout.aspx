<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="DreamWeb.Checkout" %>

<%@ MasterType VirtualPath="~/MainPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            color: #000;
            overflow-x: hidden;
            height: 100%;
            background-color: #fff;
            background-repeat: no-repeat
        }

        .plus-minus {
            position: relative
        }

        .plus {
            position: absolute;
            top: -4px;
            left: 2px;
            cursor: pointer
        }

        .minus {
            position: absolute;
            top: 8px;
            left: 5px;
            cursor: pointer
        }

        .vsm-text:hover {
            color: #FF5252
        }

        .book,
        .book-img {
            width: 120px;
            height: 180px;
            border-radius: 5px
        }

        .book {
            margin: 20px 15px 5px 15px
        }

        .border-top {
            border-top: 1px solid #EEEEEE !important;
            margin-top: 20px;
            padding-top: 15px
        }

        .card {
            margin: 40px 0px;
            padding: 40px 50px;
            border-radius: 20px;
            border: none;
            box-shadow: 1px 5px 10px 1px rgba(0, 0, 0, 0.2)
        }

        input,
        textarea {
            background-color: #F3E5F5;
            padding: 8px 15px 8px 15px;
            width: 100%;
            border-radius: 5px !important;
            box-sizing: border-box;
            border: 1px solid #F3E5F5;
            font-size: 15px !important;
            color: #000 !important;
            font-weight: 300
        }

            input:focus,
            textarea:focus {
                -moz-box-shadow: none !important;
                -webkit-box-shadow: none !important;
                box-shadow: none !important;
                border: 1px solid #9FA8DA;
                outline-width: 0;
                font-weight: 400
            }

        button:focus {
            -moz-box-shadow: none !important;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
            outline-width: 0
        }

        .pay {
            width: 80px;
            height: 40px;
            border-radius: 5px;
            border: 1px thin #673AB7;
            margin: 10px 20px 10px 0px;
            cursor: pointer;
            box-shadow: 1px 5px 10px 1px rgba(0, 0, 0, 0.2)
        }

        .gray {
            -webkit-filter: grayscale(100%);
            -moz-filter: grayscale(100%);
            -o-filter: grayscale(100%);
            -ms-filter: grayscale(100%);
            filter: grayscale(100%);
            color: #E0E0E0
        }

            .gray .pay {
                box-shadow: none
            }

        #tax {
            border-top: 1px lightgray solid;
            margin-top: 10px;
            padding-top: 10px
        }

        .btn-blue {
            border: none;
            border-radius: 10px;
            background-color: #673AB7;
            color: #fff;
            padding: 8px 15px;
            margin: 20px 0px;
            cursor: pointer
        }

            .btn-blue:hover {
                background-color: #311B92;
                color: #fff
            }

        #checkout {
            float: left
        }

        #check-amt {
            float: right
        }

        @media screen and (max-width: 768px) {

            .book,
            .book-img {
                width: 100px;
                height: 150px
            }

            .card {
                padding-left: 15px;
                padding-right: 15px
            }

            .mob-text {
                font-size: 13px
            }

            .pad-left {
                padding-left: 20px
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <!-- bradcam_area_start -->
    <div class="bradcam_area breadcam_bg overlay">
        <h3>Checkout</h3>
    </div>
    <!-- bradcam_area_end -->

    <!-- best_burgers_area_start  -->
    <div class="best_burgers_area" runat="server">
        <div class="container px-4 py-1 mx-auto">
            <asp:ListView runat="server" ID="lvwOrder" GroupPlaceholderID="groupPlaceholder1"
                ItemPlaceholderID="itemPlaceholder1">
                <LayoutTemplate>
                    <table style="width: 100%">
                        <tr id="groupPlaceholder1" runat="server"></tr>
                        <tr id="itemPlaceholder1" runat="server"></tr>
                        <thead>
                            <tr style="border-bottom: solid; border-top: solid; border-right: 1px solid; border-left: 1px solid; border-color: lightgrey; background-color: lightgrey">
                                <th style="width: 150px">Product</th>
                                <th style="width: 100px">Qty</th>
                                <th style="width: 100px;">Price</th>
                                <th style="width: 100px;">Total</th>
                            </tr>
                        </thead>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <%--<div runat="server" style="float: left" class="itemlist">--%>
                    <div runat="server" style="float: left">
                        <tr style="border-bottom: 2px solid; border-color: lightgrey;">
                            <td>
                                <%# Eval("TransName")%>
                            </td>
                            <td>
                                <%# Eval("Qty_ToString")%>
                            </td>
                            <td>
                                <%# Eval("UnitPrice_ToString")%>
                            </td>
                            <td>
                                <%# Eval("TotalPrice_ToString")%>
                            </td>
                        </tr>

                    </div>
                </ItemTemplate>
            </asp:ListView>

            <!--   <asp:UpdatePanel ID="UP_PayMode" runat="server" UpdateMode="Conditional">
                <ContentTemplate> -->
            <div class="row justify-content-center">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="row">
                            <div class="col-lg-3 radio-group">
                                <div class="row d-flex px-3 radio gray" runat="server" id="divMastercard">
                                    <asp:ImageButton runat="server" class="pay" src="images/paymode/mastercard.jpg" OnClick="img_Click" CommandName="mastercard" />
                                    <p id="pMastercard" runat="server" class="my-auto">Mastercard</p>
                                </div>
                                <div class="row d-flex px-3 radio gray" runat="server" id="divVisa">
                                    <asp:ImageButton runat="server" class="pay" src="images/paymode/visa.jpg" OnClick="img_Click" CommandName="visa" />
                                    <p id="pVisa" runat="server" class="my-auto">VISA</p>
                                </div>
                                <div class="row d-flex px-3 radio gray mb-3" runat="server" id="divEWallet">
                                    <asp:ImageButton runat="server" class="pay" src="images/paymode/ewallet.jpg" OnClick="img_Click" CommandName="ewallet" />
                                    <p id="pEWallet" runat="server" class="my-auto">e-Wallet</p>
                                </div>
                            </div>
                            <div class="col-lg-5">
                                <div class="row px-2">
                                    <div class="form-group col-md-6">
                                        <label class="form-control-label">Name on Card</label>
                                        <input type="text" id="cname" name="cname" placeholder="Johnny Doe">
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label class="form-control-label">Card Number</label>
                                        <input type="text" id="cnum" name="cnum" placeholder="1111 2222 3333 4444">
                                    </div>
                                </div>
                                <div class="row px-2">
                                    <div class="form-group col-md-6">
                                        <label class="form-control-label">Expiration Date</label>
                                        <input type="text" id="exp" name="exp" placeholder="MM/YYYY">
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label class="form-control-label">CVV</label>
                                        <input type="text" id="cvv" name="cvv" placeholder="***">
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 mt-2">
                                <div class="row d-flex justify-content-between px-4">
                                    <p class="mb-1 text-left">Subtotal</p>
                                    <h6 class="mb-1 text-right">
                                        <asp:Label ID="lblSubtotal" runat="server" Font-Bold="true" Text="0" />
                                    </h6>
                                </div>
                                <div class="row d-flex justify-content-between px-4">
                                    <p class="mb-1 text-left">
                                        <asp:Label ID="lblTaxName" runat="server" Text="Tax" />
                                    </p>
                                    <h6 class="mb-1 text-right">
                                        <asp:Label ID="lblTaxAmt" runat="server" Font-Bold="true" Text="0" />
                                    </h6>
                                </div>
                                <div class="row d-flex justify-content-between px-4" id="tax">
                                    <p class="mb-1 text-left">TOTAL</p>
                                    <h6 class="mb-1 text-right">
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Text="0" />
                                    </h6>
                                </div>
                                <button class="btn-block btn-blue" runat="server" id="btnCheckout" type="button" onserverclick="btnCheckout_Click">
                                  <!--  <span id="checkoutAmt" runat="server"></span> -->
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!--     </ContentTemplate>
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
                                <a class="btn btn-link" runat="server" id="aOrderNo" onserverclick="aOrderNo_Click" />
                                <hr />
                                <a class="btn btn-block" href="MainMenu.aspx?qr=010101" id="close-modal">Close</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
