using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TRMWebHttpCors
{
    // Based on part of the Cross-Origin Resource Sharing documentation: http://www.w3.org/TR/cors/

    public class CustomOperationInvoker : IOperationInvoker
    {
        IOperationInvoker _innerInvoker = null;
        public CustomOperationInvoker(IOperationInvoker innerInvoker)
        {
            _innerInvoker = innerInvoker;
        }

        public object[] AllocateInputs()
        {
            return _innerInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            // Check if the unhandled request is due to preflight checks (OPTIONS header)
            if (OperationContext.Current.Extensions.Find<PreflightDetected>() != null)
            {
                // Override the standard error handling, so the request won't contain an error
                outputs = null;
                return null;
            }
            else
            {
                // No preflight - probably a missed call (wrong URI or method)
                return _innerInvoker.Invoke(instance, inputs, out outputs);
            }
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            // Not supported - an exception will be thrown
            return _innerInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            // Not supported - an exception will be thrown
            return _innerInvoker.InvokeEnd(instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get
            {
                return _innerInvoker.IsSynchronous;
            }
        }
    }

    public class PreflightDetected : IExtension<OperationContext>
    {
        string _requestedHeaders = null;

        public PreflightDetected(string requestedHeaders)
        {
            RequestedHeaders = requestedHeaders;
        }

        public string RequestedHeaders
        {
            get
            {
                return _requestedHeaders ?? string.Empty;
            }
            set
            {
                _requestedHeaders = value;
            }
        }
        public void Attach(OperationContext owner)
        {
        }

        public void Detach(OperationContext owner)
        {
        }
    }

    public class CorsMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = request.Properties["httpRequest"] as HttpRequestMessageProperty;

            // Check if the client sent an "OPTIONS" request
            if (httpRequest != null)
            {
                if (httpRequest.Method == "OPTIONS")
                {
                    // Store the requested headers
                    OperationContext.Current.Extensions.Add(new PreflightDetected(
                        httpRequest.Headers["Access-Control-Request-Headers"]));
                }
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
			HttpResponseMessageProperty property = null;

			if (reply == null)
			{
				// This will usually be for a preflight response
				reply = Message.CreateMessage(MessageVersion.None, null);
				property = new HttpResponseMessageProperty();
				reply.Properties[HttpResponseMessageProperty.Name] = property;
				property.StatusCode = HttpStatusCode.OK;
			}
			else
			{
				property = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;
			}

			PreflightDetected preflightRequest = OperationContext.Current.Extensions.Find<PreflightDetected>();
			if (preflightRequest != null)
			{
				// Add allow HTTP headers to respond to the preflight request
				if (preflightRequest.RequestedHeaders == string.Empty)
					property.Headers.Add("Access-Control-Allow-Headers", "Accept");
				else
					property.Headers.Add("Access-Control-Allow-Headers", preflightRequest.RequestedHeaders + ", Accept");

				property.Headers.Add("Access-Control-Allow-Methods", "*");

			}

			// Add allow-origin header to each response message, because client expects it
			property.Headers.Add("Access-Control-Allow-Origin", "*");
		}
    }

    public class CorsSupportBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Register a message inspector, and an operation invoker for undhandled operations
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsMessageInspector());

            IOperationInvoker invoker = endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.Invoker;
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.Invoker =
                new CustomOperationInvoker(invoker);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // Make sure that the behavior is applied to an endpoing with WebHttp binding
            if (!(endpoint.Binding is WebHttpBinding))
                throw new InvalidOperationException("The CorsSupport behavior can only be used in WebHttpBinding endpoints");
        }
    }

    public class CorsSupportBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CorsSupportBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new CorsSupportBehavior();
        }
    }

}