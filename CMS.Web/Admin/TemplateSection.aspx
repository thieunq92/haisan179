<%@ Page language="c#" Codebehind="TemplateSection.aspx.cs" AutoEventWireup="false" Inherits="CMS.Web.Admin.TemplateSection" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>TemplateSection</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>Mẫu giao diện</h4>
				<table>
					<tr>
						<td style="WIDTH:130px">Mẫu</td>
						<td>
							<asp:label id="lblTemplate" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Vị trí</td>
						<td>
							<asp:label id="lblPlaceholder" runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
			<br/>
			<div class="group">
				<h4>Vùng phân hệ</h4>
				<table>
					<tr>
						<td style="WIDTH:130px">Các vùng phân hệ có</td>
						<td>
							<asp:dropdownlist id="ddlSections" runat="server"></asp:dropdownlist></td>
					</tr>
				</table>
			</div>
			<br/>
			<asp:button id="btnAttach" runat="server" text="Attach selected section"></asp:button>
			<asp:button id="btnBack" runat="server" text="Back"></asp:button>
		</form>
	</body>
</html>
