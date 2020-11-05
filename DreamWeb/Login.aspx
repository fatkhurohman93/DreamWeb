<%@ Page Title="" Language="C#" MasterPageFile="~/LogIn.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DreamWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .card-container.card {
            max-width: 350px;
            padding: 40px 40px;
        }

        .btn {
            font-weight: 700;
            height: 36px;
            -moz-user-select: none;
            -webkit-user-select: none;
            user-select: none;
            cursor: default;
        }

        /*
 * Card component
 */
        .card {
            background-color: #F7F7F7;
            /* just in case there no content*/
            padding: 20px 25px 30px;
            margin: 0 auto 25px;
            margin-top: 50px;
            /* shadows and rounded borders */
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
            -moz-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
        }

        .profile-img-card {
            width: 96px;
            height: 96px;
            margin: 0 auto 10px;
            display: block;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            border-radius: 50%;
        }

        /*
 * Form styles
 */
        .profile-name-card {
            font-size: 16px;
            font-weight: bold;
            text-align: center;
            margin: 10px 0 0;
            min-height: 1em;
        }

        .reauth-email {
            display: block;
            color: #404040;
            line-height: 2;
            margin-bottom: 10px;
            font-size: 14px;
            text-align: center;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin #inputEmail,
        .form-signin #inputPassword {
            direction: ltr;
            height: 44px;
            font-size: 16px;
        }

        .form-signin input[type=email],
        .form-signin input[type=password],
        .form-signin input[type=text],
        .form-signin button {
            width: 100%;
            display: block;
            margin-bottom: 10px;
            z-index: 1;
            position: relative;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin .form-control:focus {
            border-color: rgb(104, 145, 162);
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
        }

        .btn.btn-signin {
            /*background-color: #4d90fe; */
            background-color: rgb(104, 145, 162);
            /* background-color: linear-gradient(rgb(104, 145, 162), rgb(12, 97, 33));*/
            padding: 0px;
            font-weight: 700;
            font-size: 14px;
            height: 36px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            border: none;
            -o-transition: all 0.218s;
            -moz-transition: all 0.218s;
            -webkit-transition: all 0.218s;
            transition: all 0.218s;
        }

            .btn.btn-signin:hover,
            .btn.btn-signin:active,
            .btn.btn-signin:focus {
                background-color: rgb(12, 97, 33);
            }

        .forgot-password {
            color: rgb(104, 145, 162);
        }

            .forgot-password:hover,
            .forgot-password:active,
            .forgot-password:focus {
                color: rgb(12, 97, 33);
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="container">
        <div class="card card-container">
            <img id="profile-img" class="profile-img-card" src="images/avatar_2x.png" />
            <p id="profile-name" class="profile-name-card"></p>

            <span id="reauth-email" class="reauth-email"></span>
            <input runat="server" type="email" id="txtUserID" class="form-control" placeholder="Email address" required autofocus>
            <input runat="server" type="password" id="txtPswd" class="form-control" placeholder="Password" required>
            <div id="remember" class="checkbox">
                <label>
                    <input type="checkbox" value="remember-me">
                    Remember me
                   
                </label>
            </div>
            <button runat="server" id="btnLogin" onserverclick="btnLogin_Click" class="btn btn-lg btn-primary btn-block btn-signin">Sign In</button>

            <!-- /form -->
            <a href="#ModalResetPswd" data-target="#ModalResetPswd" data-toggle="modal">Forgot Password?</a>
            <a href="SignUp.aspx?status=new">SignUp</a>
        </div>
        <!-- /card-container -->


        <!-- Modal -->
        <div class="modal fade" id="ModalResetPswd" role="dialog" data-backdrop="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <div class="row">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <div class="text-center">
                                            <h3><i class="fa fa-lock fa-4x"></i></h3>
                                            <h2 class="text-center">Forgot Password?</h2>
                                            <p>You can reset your password here.</p>
                                            <div class="panel-body">
                                                <fieldset>
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="glyphicon glyphicon-envelope color-blue"></i></span>
                                                            <input id="emailInput" runat="server" placeholder="email address" class="form-control" type="email" oninvalid="setCustomValidity('Please enter a valid email address!')" onchange="try{setCustomValidity('')}catch(e){}" required="">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <button class="btn btn-primary btn-block" value="Send Password" type="submit" runat="server" onserverclick="btnSendPassword_Click" />
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /container -->
    </div>


</asp:Content>
