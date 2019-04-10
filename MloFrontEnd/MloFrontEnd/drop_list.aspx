<%@ Page Title="" Language="C#" MasterPageFile="~/pickup.master" AutoEventWireup="true" CodeBehind="drop_list.aspx.cs" Inherits="MloFrontEnd.drop_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PickupHead" runat="server">
    <title>Drop List</title><script>
		$(window).unbind('resize.myEvents').bind('resize.myEvents', function () {
        var outerwidth = $('#grid').width();
        $('#jqGrid').setGridWidth(outerwidth);
		var outerheight = (($('#grid').height())/1.245);
        $('.ui-jqgrid-bdiv').height(outerheight);
		});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PickupBody" runat="server">
    <div id="grid" class="jqGrid tablediv">
    <table id="jqGrid"></table>
    <div id="jqGridPager"></div>

						<button class="drpbutton" onclick="add()" runat="server" >
							Drop All
						</button>
						
		

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
						label: 'Order Id',
                        name: 'OrderId',
						sorttype:'integer',
						align: 'center',
                        width: 30
                    },
					{
						label: 'Wholesaler',
                        name: 'wholesaler',
                        width: 60,		
						sorttype:'string',						
						align: 'center'
						
                    },
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
                        width: 30
                    },
					
					{
						label: 'Quantity',
                        name: 'Quantity',
						sorttype:'integer',
						align: 'center',
                        width: 30
                    },
					
					
                   
                                    
                    {
						label: 'Action',                        
                        width: 80,
						align: 'center',
						sortable: false,
						formatter:function (cellvalue, options, rowObject) {    
						
						var btnHtml = "<a href='javascript:void(0)' onclick='Print(" + JSON.stringify(rowObject)  + ")' class='button'><u>Picked Up</u></a> <a href='javascript:void(0)' onclick='Print(" + JSON.stringify(rowObject)  + ")' class='button'><u>Cancel</u></a>";
						//var btnHtml = "<input type='button' value='Print' onclick='Print("+rowObject+");'\>";
						return btnHtml;
}
                    }
                ],


                viewrecords: true, // show the current page, data rang and total records on the toolbar
                
                height: (($('#grid').height())/1.245),
                rowNum: -1,
				rownumbers: true, // show row numbers
                rownumWidth: 20,
				datatype: 'local',
                pager: "#jqGridPager",
				caption: "V.K Sharma Wholesaler",
				autowidth: true,  // set 'true' here
                shrinkToFit: true,
                pgbuttons: false,
                pginput: false,
                pgtext:"", 
				grouping: true,
                groupingView: {
                    groupField: ["wholesaler"],
                    groupColumnShow: [false],
                    groupText: ["<b>{0}</b>"],
                    groupOrder: ["asc"],
                    groupSummary: [false],
                    groupCollapse: false
                    
                }
            });

            fetchGridData();

            function fetchGridData() {
			
                
                var gridArrayData = [];
				// show loading message
				$("#jqGrid")[0].grid.beginReq();
                $.ajax({
                    url: "http://192.168.1.19:6001/OrderManagement/OrderManagement.svc/GetOrderForUser",
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            var item = result[i];
                            gridArrayData.push({
                                Challan: item.challan,
                                Quantity: item.noi,
                                
                                wholesaler :item.dest,
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
	
</asp:Content>
