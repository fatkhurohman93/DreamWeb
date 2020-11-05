<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="SalesTypePage.aspx.cs" Inherits="DreamWeb.SalesTypePage" %>

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

        .namestyle {
            color: #006600;
            font-size: 26px;
            font-weight: bold;
            margin: 10px;
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
            align-content: center;
        }

        .btnstyle {
            border: 2px groove;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            float: inherit;
            padding: 5px;
            margin: 10px;
            box-shadow: 5px 10px #888888;
            font-family: 'Century Schoolbook';
            font-size: large;
        }
    </style>

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
    <link rel="stylesheet" href="css/dreamstyle.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- bradcam_area_start -->
            <div class="bradcam_area breadcam_bg overlay">
                <h3>Sales Types</h3>
            </div>
            <!-- bradcam_area_end -->

            <!-- best_burgers_area_start  -->
            <div class="best_burgers_area" runat="server">
                <div class="container">
                    <asp:ListView ID="lvwSalesType" runat="server" OnItemDataBound="lvwSalesType_ItemDataBound">
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
                            <div class="col-xl-6 col-md-6 col-lg-6 img-thumbnail">
                                <a href='#' runat="server" onserverclick="SalesType_Clicked">
                                    <asp:HiddenField runat="server" ID="hf_STobject" />
                                    <img src='<%# Eval("ID", "images/salestype/{0}.jpg")%>' />
                                    <span><h3><%# Eval("Name") %></h3></span>
                                </a>
                            </div>
                        </ItemTemplate>
                       
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal -->
    <div id="ModalConfirmed" class="modal fade" role="dialog" data-backdrop="false">
        <div class="container d-flex justify-content-center">
            <div class="col-md-6">
                <div class="card text-center">
                    <div class="card-body" style="background-color: lightgray">
                        <img src="img/dreamweb.png">
                        <h5 class="title">Thank YOU!</h5>
                        <p class="description">We have received your order</p>
                        <a class="btn btn-link" runat="server" id="aOrderNo" />
                        <hr />
                        <a class="btn btn-block" href="MainMenu.aspx?qr=010101" id="close-modal">Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


