﻿<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="MenuItem.aspx.cs" Inherits="DreamWeb.MenuItem" %>

<%@ MasterType VirtualPath="~/MainPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>DreamWeb</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- <link rel="manifest" href="site.webmanifest"> -->
    <link rel="shortcut icon" type="image/x-icon" href="img/dreamweb.png">
    <!-- Place favicon.ico in the root directory -->

    <!-- CSS here -->
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" href="css/owl.carousel.min.css">
    <link rel="stylesheet" href="css/magnific-popup.css">
    <link rel="stylesheet" href="css/font-awesome.min.css">
    <link rel="stylesheet" href="css/themify-icons.css">
    <link rel="stylesheet" href="css/nice-select.css">
    <link rel="stylesheet" href="css/flaticon.css">
    <link rel="stylesheet" href="css/animate.css">
    <link rel="stylesheet" href="css/slicknav.css">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/dreamstyle.css">

    <!-- <link rel="stylesheet" href="css/responsive.css"> -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <!--  <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate> -->
    <!-- bradcam_area_start -->
    <div class="bradcam_area breadcam_bg overlay">
        <h3 id="lblMenu" runat="server"></h3>
    </div>
    <!-- bradcam_area_end -->

    <!-- best_burgers_area_start  -->
    <div class="best_burgers_area" runat="server">
        <div class="container" runat="server">

            <asp:ListView runat="server" ID="lvwMenuItem" ItemPlaceholderID="itemPlaceholder">

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
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-6 img-thumbnail text-center">
                        <a href='#' runat="server" onserverclick="MenuItem_Click" customdata='<%# Eval("ID") %>'>
                            <img src='<%# Eval("ID", "images/itemmaster/{0}.jpg")%>' />
                            <p><%# Eval("Name") %></p>
                            <p><%# Eval("UnitPrice_toString") %></p>
                            <asp:LinkButton ID="linkMinus" Text="-" CssClass="buttonClass" OnClick="btnPlusMinus_Click" CommandArgument='<%# Eval("ID") %>' CommandName="minus" runat="server" />
                            <asp:Label Width="10px" ID="lblQty" runat="server" CommandArgument='<%# Eval("ID") %>'><%# GetOrderQty(Eval("ID")) %></asp:Label>
                            <asp:LinkButton Text="+" CssClass="buttonClass" OnClick="btnPlusMinus_Click" CommandArgument='<%# Eval("ID") %>' CommandName="plus" runat="server" />
                        </a>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <!-- best_burgers_area_end  -->

    <!-- features_room_startt -->

    <!-- features_room_end -->

    <!-- testimonial_area_start  -->

    <!-- testimonial_area_ned  -->

    <!-- instragram_area_start -->
    <div class="instragram_area">
        <div class="container">
            <div class="row">
                <div class="col-lg-3 col-md-6">
                    <div class="single_instagram">
                        <img src="img/instragram/1.png" alt="">
                        <div class="ovrelay">
                            <a href="#">
                                <i class="fa fa-instagram"></i>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="single_instagram">
                        <img src="img/instragram/2.png" alt="">
                        <div class="ovrelay">
                            <a href="#">
                                <i class="fa fa-instagram"></i>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="single_instagram">
                        <img src="img/instragram/3.png" alt="">
                        <div class="ovrelay">
                            <a href="#">
                                <i class="fa fa-instagram"></i>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="single_instagram">
                        <img src="img/instragram/4.png" alt="">
                        <div class="ovrelay">
                            <a href="#">
                                <i class="fa fa-instagram"></i>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- instragram_area_end -->

    <!--       </ContentTemplate>        
    </asp:UpdatePanel>   -->
    <!-- JS here -->
    <script src="js/vendor/modernizr-3.5.0.min.js"></script>
    <script src="js/vendor/jquery-1.12.4.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/owl.carousel.min.js"></script>
    <script src="js/isotope.pkgd.min.js"></script>
    <script src="js/ajax-form.js"></script>
    <script src="js/waypoints.min.js"></script>
    <script src="js/jquery.counterup.min.js"></script>
    <script src="js/imagesloaded.pkgd.min.js"></script>
    <script src="js/scrollIt.js"></script>
    <script src="js/jquery.scrollUp.min.js"></script>
    <script src="js/wow.min.js"></script>
    <script src="js/nice-select.min.js"></script>
    <script src="js/jquery.slicknav.min.js"></script>
    <script src="js/jquery.magnific-popup.min.js"></script>
    <script src="js/plugins.js"></script>

    <!--contact js-->
    <script src="js/contact.js"></script>
    <script src="js/jquery.ajaxchimp.min.js"></script>
    <script src="js/jquery.form.js"></script>
    <script src="js/jquery.validate.min.js"></script>
    <script src="js/mail-script.js"></script>
    <script src="js/main.js"></script>



</asp:Content>

