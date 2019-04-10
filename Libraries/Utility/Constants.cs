using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;


namespace Utility
{
    public static class Constants
    {
        public const string Initiated = "Initiated";
        public const string Accepted = "Accepted";
        public const string Pickedup = "Pickedup";
        public const string Dropped = "Dropped";
        public const string OutForDelivery = "Out For Delivery";
        public const string Delivered = "Delivered";
        public const string Undelivered = "Undelivered";
        public const string Rescheduled = "Rescheduled";
        public const string RescheduleOFD = "RescheduleOFD";
        public const string Cancelled = "Cancelled";
        public const string Halted = "Halted";

    }

    public static class CustExceptn 
    {
        public static ErrorConstants GET_LOCN = new ErrorConstants("E001", "Unable to get all locations");
        public static string GET_LOCN_STR =  "Unable to get all locations";
    }

    public class ErrorConstants
    {
        public string code { get; set; }
        public string desc { get; set; }
        public ErrorConstants() { }
        public ErrorConstants(string code, string desc) { }

       // 
    }


    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message) : base(message) { }
        public UserFriendlyException(string message, Exception innerException) : base(message, innerException) { }
    }

    //public class UserFriendlyExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    //{
    //    public override void OnException(HttpActionExecutedContext context)
    //    {
    //        var friendlyException = context.Exception as UserFriendlyException;
    //        if (friendlyException != null)
    //        {
    //            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = friendlyException.Message });
    //        }
    //    }
    //}
}