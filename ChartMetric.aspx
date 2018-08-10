<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartMetric.aspx.cs" Inherits="ChartMetric" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <table>
        <tr>
            <td align="left" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"><b>Provider: <asp:Literal runat="server" ID="ltProvider"></asp:Literal></b></td>
            <td align="right" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"><b>Metric: <asp:Literal runat="server" ID="ltMetric"></asp:Literal></b></td>         
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td colspan="2">
                <asp:Chart ID="MetricChart" runat="server" Width="862px" Height="263px" OnDataBound="MetricChart_DataBound">
                    <Series>
                        <asp:Series BorderWidth="2" Color="black" Name="Series1" ChartType="Line" ChartArea="MainChartArea"></asp:Series>
                        <asp:Series BorderWidth="2" Color="green" Name="Series2" ChartType="Line" ChartArea="MainChartArea"></asp:Series>
                        <asp:Series BorderWidth="2" Color="red" Name="Series3" ChartType="Line" BorderDashStyle="Dash" ChartArea="MainChartArea"></asp:Series>
                        <asp:Series BorderWidth="2" Color="red" Name="Series4" ChartType="Line" BorderDashStyle="Dash" ChartArea="MainChartArea"></asp:Series>
                    </Series>
            
                    <ChartAreas>
                        <asp:ChartArea Name="MainChartArea">
                            <AxisX IntervalAutoMode="VariableCount" IsLabelAutoFit="False" 
                                MaximumAutoSize="100">
                                <MajorGrid Interval="Auto" />
                                <MajorTickMark Interval="Auto" />
                                <LabelStyle Angle="90" Font="Microsoft Sans Serif, 8.25pt" />
                            </AxisX>
                            
                            <AxisY2 IntervalAutoMode="VariableCount">
                                <MajorGrid Interval="Auto" IntervalOffset="Auto" IntervalOffsetType="Auto" 
                                    IntervalType="Auto" />
                                <MinorGrid Enabled="True" />
                                <MajorTickMark Interval="Auto" IntervalOffset="Auto" IntervalOffsetType="Auto" 
                                    IntervalType="Auto" />
                                <MinorTickMark Enabled="True" />
                                <ScaleBreakStyle Enabled="True" />
                            </AxisY2>
                          
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
        </tr>
    </table> 
    <!--
	<table cellpadding="4">
		<tr>
			<td align="left" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"> Logarithmic:</td>
			<td><asp:checkbox id="Logarithmic" runat="server"></asp:checkbox></td>
		</tr>
		<tr>
			<td align="left" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"> Auto Scale:</td>
				<td>
					<p><asp:checkbox id="AutoScale" runat="server" Checked="True"></asp:checkbox></p>
				</td>
		</tr>
		<tr>
			<td align="left" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"> Maximum:</td>
			<td>
				<asp:dropdownlist id="Maximum" runat="server" cssclass="spaceright">
					<asp:listitem Value="3000">3000</asp:listitem>
					<asp:listitem Value="5000" Selected="True">5000</asp:listitem>
					<asp:listitem Value="10000">10000</asp:listitem>
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td align="left" style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size: 14px;"> Minimum:</td>
			<td>
				<asp:dropdownlist id="Minimum" runat="server" cssclass="spaceright">
					<asp:listitem Value="0">0</asp:listitem>
					<asp:listitem Value="250">250</asp:listitem>
					<asp:listitem Value="500">500</asp:listitem>
				    </asp:dropdownlist></td>
		</tr>
	</table>
    -->
    </form>
    <div style="padding-top: 4px; padding-left: 0px; float: left; text-align: left; font-size: x-small; font-family: Arial, Helvetica, sans-serif; width: 778px; position: relative; top: 0px; left: 0px;">
        <% Response.WriteFile("Version.inc"); %>
    </div>
</body>
</html>
