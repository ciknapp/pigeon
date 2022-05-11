<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" AutoEventWireup="true" CodeBehind="Team.aspx.cs" Inherits="CS414_Team2.Team" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Our Team</title>
    <style>
        body {
            height: 100vh;
            background: rgb(2,0,36);
            background: linear-gradient(360deg, rgba(2,0,36,1) 0%, rgba(9,92,121,1) 0%, rgba(175,231,241,1) 21%);
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <section style="height: 55px; color: white;">
        <nav style="background-color: #4A87C6;">
            <div class="container" style="width: 100%; margin: 0;">
                <a href="Nest.aspx" style="text-decoration: none; color: white;">
                    <img src="Resources/Images/FullLogo2.png" height="54" width="74" style="width: auto;" />
                    <i class="fa-solid fa-arrow-left"></i>
                    <b>&nbsp;Back to Nests</b>
                </a>
                </a>
                <div class="create" style="position: absolute; right: 0; margin-right: 20px;">
                    <asp:Label ID="lblNavBarUsername" Text="" runat="server" />
                    <div>
                        <button type="button" class="btn btn-secondary dropdown-toggle dropdown-toggle-split" style="background-color: #4A87C6; border: none;" id="MenuReference" data-bs-toggle="dropdown" aria-expanded="false" data-bs-reference="parent">
                            <span class="visually-hidden">Toggle Dropdown</span>
                            <img class="profile-photo" style="border: 2px solid white;" id="imgNavProfilePicture" runat="server" src="Resources/Images/FullLogo.png" />
                        </button>
                        <div class="dropdown-menu" aria-labelledby="MenuReference">
                            <a class="dropdown-item" href="UserSettings.aspx"><i class="fa fa-cog" aria-hidden="true"></i>User settings</a>
                            <hr class="dropdown-divider">
                            <asp:LinkButton ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" CssClass="dropdown-item" Style="text-decoration: none;">
                                <i class="fa fa-sign-out" aria-hidden="true"></i>
                                &nbsp;Logout
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    </section>

    <header>
        <div class="team-area">
            <div class="container">
                <div class="row">
                    <div class="col-lg-3 col-md-6 d-flex align-items-stretch">
                        <div class="single-team">
                            <div class="img-area">
                                <img src="Resources/Images/1.jpg" class="img-responsive" alt="Caleb">
                            </div>
                            <div class="img-text">
                                <h4>Caleb Knapp</h4>
                                <h5>Project Manager (PM)</h5>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 d-flex align-items-stretch">
                        <div class="single-team">
                            <div class="img-area">
                                <img src="Resources/Images/2.png" class="img-responsive" alt="Himprawa">
                            </div>
                            <div class="img-text">
                                <h4>Himprawa Khattri</h4>
                                <h5>Quality Assurance Engineer (QA)</h5>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 d-flex align-items-stretch">
                        <div class="single-team">
                            <div class="img-area">
                                <img src="Resources/Images/3.jpeg" class="img-responsive" alt="Timothy">
                            </div>
                            <div class="img-text">
                                <h4>Timothy De Jesus</h4>
                                <h5>Database Administrator (DBA)</h5>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 d-flex align-items-stretch">
                        <div class="single-team">
                            <div class="img-area">
                                <img src="Resources/Images/4.jpeg" class="img-responsive" alt="Mason">
                            </div>
                            <div class="img-text">
                                <h4>Mason Phillips</h4>
                                <h5>Cybersecurity Specialist (CYS) </h5>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
</asp:Content>
