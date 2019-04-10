using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class WebHttpErrHandler : WebHttpBehavior
    {
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // clear default erro handlers.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            // add our own error handler.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new ErrorHandler());
        }
    }

    class ErrorHandler : System.ServiceModel.Dispatcher.IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Logger.Log(Level.Error, "ErrorHandler.. ");
            if (error is FaultException)
            {
                // extract the our FaultContract object from the exception object.
                var detail = error.GetType().GetProperty("Detail").GetGetMethod().Invoke(error, null);

                // create a fault message containing our FaultContract object
                fault = Message.CreateMessage(version, "", detail, new DataContractJsonSerializer(detail.GetType()));

                // tell WCF to use JSON encoding rather than default XML
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);
                // return custom error code.
                var rmp = new HttpResponseMessageProperty();
                rmp.StatusCode = (System.Net.HttpStatusCode)error.GetType().GetProperty("StatusCode").GetGetMethod().Invoke(error, null);
                // put appropraite description here..

                rmp.StatusDescription = "See fault object for more information.";
                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
            else
            {
                var msg = error.GetType().GetProperty("Message").GetGetMethod().Invoke(error, null);
                var stacktrace = error.GetType().GetProperty("StackTrace").GetGetMethod().Invoke(error, null);

                Logger.Log(Level.Error, "Error Message " + msg + " Caused by :: " + stacktrace);
                fault = Message.CreateMessage(version, "", msg, new DataContractJsonSerializer(typeof(string)));
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);
                // return custom error code.
                var rmp = new HttpResponseMessageProperty();
                rmp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                // put appropraite description here..
                rmp.StatusDescription = "Unkown exception";
                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
        }
    }
}
