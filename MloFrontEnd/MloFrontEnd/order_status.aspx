<%@ Page Title="" Language="C#" MasterPageFile="~/wholesaler.master" AutoEventWireup="true" CodeBehind="order_status.aspx.cs" Inherits="MloFrontEnd.order_status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="WholesalerHead" runat="server">
<title>Delivery Status</title>
    <script>$(window).unbind('resize.myEvents').bind('resize.myEvents', function () {
        var outerwidth = $('#grid').width();
        $('#jqGrid').setGridWidth(outerwidth);
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="WholesalerBody" runat="server">
    <div id="grid" class="jqGrid" style="position:relative;padding-top:5%;max-width:1180px; min-width:320px;margin:auto;">

    <table id="jqGrid"></table>
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
						label: 'Status',
                        name: 'Status',
						sorttype:'string',
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
                height: 300,
                rowNum: 20,
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
                                Status: item.state,

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
</asp:Content>
