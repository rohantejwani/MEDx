<%@ Page Title="" Language="C#" MasterPageFile="~/wholesaler.master" AutoEventWireup="true" CodeBehind="request_delivery.aspx.cs" Inherits="MloFrontEnd.request_delivery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="WholesalerHead" runat="server">
<title>Request Delivery</title>
<script>$(window).unbind('resize.myEvents').bind('resize.myEvents', function () {
        var outerwidth = $('#grid').width();
        $('#jqGrid').setGridWidth(outerwidth);
    });
</script>
<style>
.rdcontainer{
    display: flex;
    flex-wrap: wrap;
    margin: 10px;
    position: absolute;
    padding-top: 110px;
    width: 100%;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="WholesalerBody" runat="server">
<div class="limiter">
<div  class="rdcontainer">
<div class="column left">
		<form class="login100-form validate-form p-l-110 p-r-110" autocomplete="off">
					<span class="request100-form-title p-b-20">
						Create A Request
					</span>
					<div class="wrap-input100 validate-input m-b-16" data-validate="Please enter Retailer Name">
						<input class="input100" type="text" name="rname" placeholder="Retailer Firm Name" list="retailer_list" autocomplete="off">
						<span class="focus-input100"></span>
					</div>
					<datalist id="retailer_list">
						  <option value="HTML">
						  <option value="CSS">
						  <option value="JavaScript">
						  <option value="Java">
						  <option value="Ruby">
						  <option value="PHP">
						  <option value="Go">
						  <option value="Erlang">
						  <option value="Python">
						  <option value="C">
						  <option value="C#">
						  <option value="C++">
					</datalist>
					
					<div class="wrap-input100 validate-input m-b-16" data-validate="Please enter Challan No">
						<input class="input100" type="number" name="cno" placeholder="Challan No"  autocomplete="off">
						<span class="focus-input100"></span>
					</div>
					
					<div class="wrap-input100 validate-input m-b-20" data-validate="Please enter no of items">
						<input class="input100" type="number" name="ino" placeholder="Number of Items" autocomplete="off">
						<span class="focus-input100"></span>
					</div>

					

					<div class="container-login100-form-btn">
						<button class="request100-form-btn" onclick="add()" runat="server" >
							Request
						</button>
					</div>
				
					
					
					
		</form>
		
		</div>
		
		
		
    <div id="grid" class="jqGrid" style="position:relative;width:100%;max-width:880px; min-width:320px;margin:auto;">

    <table id="jqGrid" style="margin:auto"></table>
    <div id="jqGridPager"></div>
	</div>
		

    <script type="text/javascript"> 
	   function Print(row) {
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');


            printWindow.document.write('<table align="center" height=80% width=80% cellspacing="10" style=" border: 1px solid black;">');
            
            printWindow.document.write('<tr><td align="center">Order Id</td><td>' + row.OrderId + '</td></tr>');
			printWindow.document.write('<tr><td align="center">Challan</td><td>' + row.Challan + '</td></tr>');
			printWindow.document.write('<tr><td align="center">Destination</td><td>' + row.Destination + '</td></tr>');
			
			
            
            printWindow.document.write('</table>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
            };

    
        $(document).ready(function () {
            $("#jqGrid").jqGrid({
                colModel: [
				    {
						label: 'Destination',
                        name: 'Destination',
                        width: 100,		
						sorttype:'string',						
						align: 'center'
						
                    }, 
					
					{
						label: 'Challan',
                        name: 'Challan',
						sorttype:'integer',
						//formatter: 'number',
						align: 'center',
                        width: 50
                    },
					
					{
						label: 'Quantity',
                        name: 'Quantity',
						sorttype:'integer',
						align: 'center',
                        width: 50
                    },
					
					{
						label: 'Order id',
                        name: 'OrderId',
						sorttype:'integer',
						align: 'center',
                        width: 50
                    },
                   
                                    
                    {
						label: 'Action',                        
                        width: 60,
						align: 'center',
						sortable: false,
						formatter:function (cellvalue, options, rowObject) {    
						
						var btnHtml = "<a href='javascript:void(0)' onclick='Print(" + JSON.stringify(rowObject)  + ")' class='button'><u>Print</u></a> <a href='javascript:void(0)' onclick='Print(" + JSON.stringify(rowObject)  + ")' class='button'><u>Cancel</u></a>";
						//var btnHtml = "<input type='button' value='Print' onclick='Print("+rowObject+");'\>";
						return btnHtml;
}
                    }
                ],

                viewrecords: true, // show the current page, data rang and total records on the toolbar
                autowidth: true,
                shrinktofit: true,
                height: 200,
                rowNum: 10,
				rownumbers: true, // show row numbers
                rownumWidth: 20,
				datatype: 'local',
                pager: "#jqGridPager",
				caption: "V.K Sharma Wholesaler"
            });

            fetchGridData();

            function fetchGridData() {
			
                
                var gridArrayData = [];
				// show loading message
				$("#jqGrid")[0].grid.beginReq();
                $.ajax({
                    url: "http://localhost:6001/OrderManagement/OrderManagement.svc/GetOrderForUser",
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            var item = result[i];
                            gridArrayData.push({
                                Challan: item.challan,
                                Quantity: item.noi,
                                Destination: item.dest,
                                
                                OrderId: item.id
                            });                            
                        }
						// set the new data
						$("#jqGrid").jqGrid('setGridParam', { data: gridArrayData});
						// hide the show message
						$("#jqGrid")[0].grid.endReq();
						// refresh the grid
						$("#jqGrid").trigger('reloadGrid');
						
                    }
                });
            }

            function formatTitle(cellValue, options, rowObject) {
                return cellValue.substring(0, 50) + "...";
            };

            function formatLink(cellValue, options, rowObject) {
                return "<a href='" + cellValue + "'>" + cellValue.substring(0, 25) + "..." + "</a>";
            };

          
        });

    </script>
	</div>
	</div>
</asp:Content>
