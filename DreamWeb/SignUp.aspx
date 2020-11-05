<%@ Page Language="C#" MasterPageFile="~/LogIn.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="DreamWeb.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lblKet {
            color: red;
            font-size: small;
            font-family: 'Century Schoolbook';
            margin: 10px;
            font-style: italic;
        }

        .lblTitle {
            font-size: larger;
            font-family: 'Century Schoolbook';
            margin: 10px;
            font-weight: bold;
        }

        .lblSubTitle {
            font-size: small;
            font-family: 'Century Schoolbook';
            margin: 10px;
            font-style: italic;
        }

        .txtAddr {
            margin: 5px;
            width: 50%;
            height: 35px;
            font-size: medium;
            font-family: 'Century Schoolbook';
        }

        .lblGenre {
            margin: 10px;
            width: 50%;
            font-size: medium;
            font-family: 'Century Schoolbook';
        }

        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row titlestyle">
            <h2>Register</h2>
            <p id="lblTitle" runat="server">Please Fill Information Below</p>
            <span>* Required</span>
        </div>

        <div class="row textboxStyle">
                <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Name*" OnTextChanged="Info_TextChanged"> </asp:TextBox>
                <br />
                <asp:TextBox ID="txtFullName" runat="server" class="form-control" placeholder="FullName" OnTextChanged="Info_TextChanged"> </asp:TextBox>
                <br />
                <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email*" TextMode="Email" OnTextChanged="Info_TextChanged"> </asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtPass" class="form-control" placeholder="Password*" TextMode="Password"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtPhone" class="form-control" placeholder="Phone*" TextMode="Phone" OnTextChanged="Info_TextChanged"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtAddr" class="form-control" placeholder="Address*" CssClass="txtAddr" OnTextChanged="Info_TextChanged"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtCity" class="form-control" placeholder="City" OnTextChanged="Info_TextChanged"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtZipCode" class="form-control" placeholder="Zip Code" OnTextChanged="Info_TextChanged"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtCountry" class="form-control" placeholder="County" OnTextChanged="Info_TextChanged"></asp:TextBox>
                <br />
                <asp:TextBox runat="server" ID="txtDoB" class="form-control" placeholder="Date Of Birth" OnTextChanged="Info_TextChanged" AutoCompleteType="Disabled" TextMode="Date"></asp:TextBox>
                <asp:Label runat="server" ID="lblSex" Text="Sex" CssClass="lblGenre"></asp:Label>
                <asp:RadioButton runat="server" ID="rbtnFemale" class="form-control" placeholder="" Text="Female" GroupName="sex" OnCheckedChanged="Info_TextChanged" />
                <asp:RadioButton runat="server" ID="rbtnMale" class="form-control" placeholder="" Text="Male" GroupName="sex" OnCheckedChanged="Info_TextChanged" />
                <br />
                <br />
                <asp:Button ID="btnReg" runat="server" OnClick="btnReg_Click" Text="Create Account" />
            </div>
        </div>
        <div class="row textboxStyle">
            <p>Have Account?</p>
            <a href="Default.aspx">LogIn</a>
        </div>
</asp:Content>