<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="MloFrontEnd.profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Profile</title><style>
.center-div
{
    position: relative;
     margin: auto;
     top: 15%;
     right: 0;
     bottom: 12%;
     left: 0;
	 width:50%;
height:60%;
	min-width: 320px;
}
.d{
position:absolute;

z-index:0;
border-top-left-radius:15px;
border-top-right-radius:15px;
border-bottom-left-radius:15px;
border-bottom-right-radius:15px;
min-width: 320px;

}
.color{
background-color:white;
}
.top{
position:absolute;
background-color:#b8ccca;
width:100%;
height:15%;
z-index:1;
border-top-right-radius:15px;
border-top-left-radius:15px;
text-align:center;
line-height:55px;
color:#0e485e

}
.img{
position:absolute;
    left:6%;
    top: 38px;
	border-rounded:15px;
	border-radius:15px;
    z-index: 2;
}
.table1{
table-layout:fixed;width:100%;
}

.rounded{
border-radius:15px;
}
tr{
	border-top:0px;
    border-bottom: 1px solid #e6e6e6;
	height: 35px;
    vertical-align: bottom;
	width:40px;
	
}
.tbmaster{
position:absolute;
background-color:white;
top:17%;
left:35%;
right:5%;
bottom:5%;

}
.ptable{
position:absolute;
left:10px;right:10px;top:10px;bottom:auto;
background-color:white;
width:100%;
height:100%;



}
.pbutton{
position:absolute;
width:100%;
left:4%;bottom:10%;right:4%;top:90%;
height:30px;

}
.sp1{
font-size:17px; position:absolute;
}
@media (max-width: 800px){
.center-div{
position:relative;
bottom:5px;top:20px;width:100%;
height:90%;
}
.img{
position:absolute;
margin-left: auto;
margin-right: auto;
left:30%;
right:30%;
top:10%;
border:1px solid #b8ccca;
border-rounded:15px;
border-radius:15px;
   z-index: 2;
width:132px;height:132px;
}
.sp1{
position:static;
}
.tbmaster{
position:absolute;
width:100%;
bottom:10%;right:0;top:35%;left:0;
}
.ptable{
position:absolute;
left:5%;right:5;top:15%;bottom:15%;
background-color:white;
table-layout:fixed;
max-width:90%;
text-align:center;
display: inline-block;



}
.pbutton{
position:absolute;
width:100%;

height:30px;

}
}


</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="d color center-div">
<div class="top"><span class="sp1" >Profile Details</span></div>
<div class="img"><img style="border-radius:15px;" src="a.jpg" alt="Responsive image" width="132" height="132"></div>
<div class="tbmaster">
<div class="ptable"><table class="table table-hover  table-borderless  table1">
  
  <tbody>
    <tr>
      
      <td>Mark</td>
      <td>Otto</td>
      
    </tr>
    <tr>
      
      <td>Jacob</td>
      <td>Thornton</td>
      
    </tr>
    <tr>
      
      <td>Larry</td>
      <td>the Bird</td>
      
    </tr>
  </tbody>
</table>
<div class="pbutton">
<ul >
						<li style="display:inline;">
						<a class="newbutton" style="min-width: 30%; height: 30px;"   >
							Edit
						</a>
						</li>
						<li style="display:inline;padding-left:20%;">
						<a class="newbutton" style="min-width: 30%;height: 30px;">
							Change Password
						</a>
						</li>			
</ul>
</div>
</div>



</div>
</div>
</asp:Content>
