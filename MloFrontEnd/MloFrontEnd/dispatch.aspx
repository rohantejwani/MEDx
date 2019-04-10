<%@ Page Title="" Language="C#" MasterPageFile="~/warehouse.master" AutoEventWireup="true" CodeBehind="dispatch.aspx.cs" Inherits="MloFrontEnd.dispatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="WarehouseHead" runat="server">
    <title>Dispatch</title>
    <script>
		$(window).unbind('resize.myEvents').bind('resize.myEvents', function () {
        var outerwidth = $('#driver').width();
        $('#jqGrid').setGridWidth(outerwidth);
		var outerheight = (($('#driver').height())/1.235);
        $('#jqGrid').setGridHeight(outerheight);
		var outerwidth1 = $('#tsp').width();
        $('#jqGrid1').setGridWidth(outerwidth1);
		var outerheight1 = (($('#tsp').height())/1.235);
        $('#jqGrid1').setGridHeight(outerheight1);
		});
</script>    <style>

.tsp {
    
    width: 75%;
    height: 100%;
    margin: 10px;
    justify-content: center;
    align-items: center;
	
    
}
.driver {
    
    width: 25%;
    height: 50%;
    margin: 10px;
    justify-content: center;
    align-items: center;
	min-width: 230px;
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="WarehouseBody" runat="server">
    <div style="display: flex; height:87.25%;">
<div id="driver" class="driver">
	 <table id="jqGrid"></table>
    <div id="jqGridPager"></div>
		 
</div>
<div id="tspside" class="tsp">
 <div id="tsp" style="height:50%;">
 <table id="jqGrid1"></table>
 <div id="jqGridPager1"></div>
 <button class="drpbutton" onclick="add()" runat="server" >Create Route</button>
 </div>
 <div id="tsp" style="height:50%;padding-top:140px;">
 <table class="table table-hover table-light table-bordered">
		<caption><div align="right"><button class="print-btn">
							Print
						</button></div>
	</div></caption>
		
		
        <thead style="background-color:black">
            <tr style="color:white">
                <td>Order Id</td>
               	<td>Retailer Address</td>
				<td>Challan No</td>
				<td>Number of Items</td>
				<td>Wholesaler Address</td>	
            </tr>
        </thead>
        
        </table>

 </div>
</div>
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
			$("#jqGrid1").jqGrid({
                colModel: [
				    {
						label: 'Order Id',
                        name: 'OrderId',width: 30,
						sorttype:'integer',
						align: 'center'
                        
                    },
					{
						label: 'Wholesaler',
                        name: 'wholesaler',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
                    },
					{
						label: 'Destination',
                        name: 'destination',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
                    }, 
					
					{
						label: 'Challan',
                        name: 'Challan',width: 40,
						sorttype:'integer',
						//formatter: 'number',
						align: 'center',
                        
                    },
					
					{
						label: 'Quantity',
                        name: 'Quantity',width: 30,
						sorttype:'integer',
						align: 'center',
                        
                    },
					{
						label: 'Picked By',
                        name: 'pickup_by',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
                    },
					{
						label: 'Delivered By',
                        name: 'Driver',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
                    },
					{
						label: 'Status',
                        name: 'State',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
                    },
					{
						label: 'Active',
                        name: 'activeq',
                        width: 30,
						sorttype:'string',						
						align: 'center'
						
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
                
                height: (($('#tsp').height())/1.245),
                rowNum: 13,
				rownumbers: true, // show row numbers
                rownumWidth: 20,
				datatype: 'local',
                pager: "#jqGridPager1",
				caption: "Routing Table",
				autowidth: true,  // set 'true' here
                shrinkToFit: true,
				grouping: true,
                groupingView: {
                    groupField: ["State"],
                    groupColumnShow: [true],
                    groupText: ["<b>{0}</b>"],
                    groupOrder: ["asc"],
                    groupSummary: [false],
                    groupCollapse: false
                    
                }
            });

            fetchGridData1();

            function fetchGridData1() {
			
                
                var gridArrayData = [];
				// show loading message
				$("#jqGrid1")[0].grid.beginReq();
                $.ajax({
                    url: "http://192.168.1.12:8080/OrderManagement/GetOrderForUser?userId=&state=",
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            var item = result[i];
                            gridArrayData.push({
                                Challan: item.challan,
                                Quantity: item.noi,
                                destination:item.dest,
								pickup_by:item.pb,
								Driver:item.driver,
								State:item.state,
								activeq:item.active,
                                wholesaler :item.dest,
                                OrderId: item.id
                            });                            
                        }
						// set the new data
						$("#jqGrid1").jqGrid('setGridParam', { data: gridArrayData});
						// hide the show message
						$("#jqGrid1")[0].grid.endReq();
						// refresh the grid
						$("#jqGrid1").trigger('reloadGrid');
						
                    }
                });
            }

            function formatTitle(cellValue, options, rowObject) {
                return cellValue.substring(0, 50) + "...";
            };

            function formatLink(cellValue, options, rowObject) {
                return "<a href='" + cellValue + "'>" + cellValue.substring(0, 25) + "..." + "</a>";
            };
			$("#jqGrid1").jqGrid("navGrid","#jqGridPager1",{add:false, edit:false, del:false});
			// on chang select value change grouping
          
		  
       
            $("#jqGrid").jqGrid({
                colModel: [
				    {
						label: 'Id',
                        name: 'DriverId',
						width: 30,
						sorttype:'integer',
						align: 'center'
                        
                    },
					{
						label: 'Driver Name',
                        name: 'DriverName',
                        width: 80,
						sorttype:'string',						
						align: 'center'
						
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
                
                height: (($('#driver').height())/1.245),
                rowNum: 20,
				rownumbers: true, // show row numbers
                rownumWidth: 20,
				datatype: 'local',
                pager: "#jqGridPager",
				caption: "Drivers",
				autowidth: true,  // set 'true' here
                shrinkToFit: true
				
            });

            fetchGridData();

            function fetchGridData() {
			
                
                var gridArrayData = [];
				// show loading message
				$("#jqGrid")[0].grid.beginReq();
                $.ajax({
                    url: "http://192.168.1.12:8080/UserManagement/GetDriver",
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            var item = result[i];
                            gridArrayData.push({
                                
								DriverId:item.Id,
								DriverName:item.Name
								
								
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
			$("#jqGrid").jqGrid("navGrid","#jqGridPager",{add:false, edit:false, del:false});
			// on chang select value change grouping
			
          
		  
        });
 </script>
</asp:Content>
