<%@ Page Title="" Language="C#" MasterPageFile="~/warehouse.master" AutoEventWireup="true" CodeBehind="warehouse_status.aspx.cs" Inherits="MloFrontEnd.warehouse_status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="WarehouseHead" runat="server">
    <title>Warehouse Status</title>
    <script>
		$(window).unbind('resize.myEvents').bind('resize.myEvents', function () {
        var outerwidth = $('#grid').width();
        $('#jqGrid').setGridWidth(outerwidth);
		var outerheight = (($('#grid').height())/1.245);
        $('.ui-jqgrid-bdiv').height(outerheight);
		});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="WarehouseBody" runat="server">
    <div style="margin: 15px; color:#0e485e">

Group By: <select id="chngroup" class="btn dropdown-toggle" style=" margin-left:5px;height:30px; font-size:15px;padding-top:0;padding-bottom:0; color: #0e485e;background-color: #b8ccca;    /* border-color: #6c757d; */ ">
        <option value="wholesaler" >Wholesaler</option>
        <option value="destination">Destination</option>
		<option value="pickup_by">Pickup by</option>
		<option value="Driver">Driver</option>
		<option value="State" selected="selected">Status</option>
		<option value="activeq">Active</option>
        <option value="clear">Remove Grouping</option>    
        </select>
	

	</div>	
		
		
<div id="grid" class="jqGrid tablediv">
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
						label: 'Order Id',
                        name: 'OrderId',width: 80,
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
                        name: 'Challan',width: 80,
						sorttype:'integer',
						//formatter: 'number',
						align: 'center',
                        
                    },
					
					{
						label: 'Quantity',
                        name: 'Quantity',width: 80,
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
                
                height: (($('#grid').height())/1.245),
                rowNum: 20,
				rownumbers: true, // show row numbers
                rownumWidth: 20,
				datatype: 'local',
                pager: "#jqGridPager",
				caption: "V.K Sharma Wholesaler",
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
			jQuery("#chngroup").change(function(){
				var vl = $(this).val();
				if(vl) {
					if(vl === "clear") {
						jQuery("#jqGrid").jqGrid('groupingRemove',true);
					} else {
						jQuery("#jqGrid").jqGrid('groupingGroupBy',vl);
					}
    }
}); 
          
		  
        });
			
		
		
		
    

    </script>
	
</asp:Content>
