<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="appEntergable2._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="row">
            <p>
                Ingrese los Datos Requeridos para validar su TDC.
            </p>
            <div class="form-outline mb-4">
                <asp:HiddenField runat="server" ID="hidenContador" Value="0" />
                <asp:HiddenField runat="server" ID="hidenFechaBloqueo" Value="" />
                <asp:TextBox runat="server" ValidationGroup="Guardar" ID="txtNombre" PlaceHolder="Nombre Completo" />
                <asp:RequiredFieldValidator ValidationGroup="Guardar" ControlToValidate="txtNombre" Display="Dynamic"
                    ErrorMessage="Nombres Obligatorio." runat="server" ForeColor="#cc3300">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ValidationGroup="Guardar" ControlToValidate="txtNombre"
                    Display="Dynamic" ErrorMessage="Nombre Completo no válido" ForeColor="#cc3300"
                    ValidationExpression="[a-zA-Z ]{2,254}" />
            </div>
            <div class="form-outline mb-4">
                <asp:TextBox runat="server" ValidationGroup="Guardar" ID="txtTarjeta" PlaceHolder="Tarjeta de Credito"
                    MaxLength="16" TextMode="Password" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                    TargetControlID="txtTarjeta" />
                <asp:RegularExpressionValidator runat="server" ValidationGroup="Guardar" ControlToValidate="txtTarjeta"
                    Display="Dynamic" ErrorMessage="TDC no válido" ForeColor="#cc3300"
                    ValidationExpression="[0-9]{16,16}" />
                <%-- <asp:MaskedEditExtender runat="server"
                    TargetControlID="txtTarjeta"
                    Mask="000000000000" />--%>
            </div>
            <div class="form-outline mb-4">
                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" type="submit" ValidationGroup="Guardar"
                    class="btn btn-primary btn-block" OnClick="btnAceptar_Click" />

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Guardar" />
            </div>

            <div class="col-lg-12">
                <asp:TextBox runat="server" Enabled="false" ID="txtResult" TextMode="MultiLine" Width="100%" />
            </div>
        </div>
    </main>


</asp:Content>
