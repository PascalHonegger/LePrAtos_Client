﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LePrAtos.Service_References.HelloWorldService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://webservices.javapostsforlearning.arpit.org", ConfigurationName="HelloWorldService.HelloWorld")]
    public interface HelloWorld {
        
        // CODEGEN: Generating message contract since element name name from namespace http://webservices.javapostsforlearning.arpit.org is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        sayHelloWorldResponse sayHelloWorld(sayHelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<sayHelloWorldResponse> sayHelloWorldAsync(sayHelloWorldRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sayHelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="sayHelloWorld", Namespace="http://webservices.javapostsforlearning.arpit.org", Order=0)]
        public sayHelloWorldRequestBody Body;
        
        public sayHelloWorldRequest() {
        }
        
        public sayHelloWorldRequest(sayHelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.javapostsforlearning.arpit.org")]
    public partial class sayHelloWorldRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string name;
        
        public sayHelloWorldRequestBody() {
        }
        
        public sayHelloWorldRequestBody(string name) {
            this.name = name;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sayHelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="sayHelloWorldResponse", Namespace="http://webservices.javapostsforlearning.arpit.org", Order=0)]
        public sayHelloWorldResponseBody Body;
        
        public sayHelloWorldResponse() {
        }
        
        public sayHelloWorldResponse(sayHelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.javapostsforlearning.arpit.org")]
    public partial class sayHelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sayHelloWorldReturn;
        
        public sayHelloWorldResponseBody() {
        }
        
        public sayHelloWorldResponseBody(string sayHelloWorldReturn) {
            this.sayHelloWorldReturn = sayHelloWorldReturn;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface HelloWorldChannel : HelloWorld, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HelloWorldClient : System.ServiceModel.ClientBase<HelloWorld>, HelloWorld {
        
        public HelloWorldClient() {
        }
        
        public HelloWorldClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HelloWorldClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HelloWorldClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HelloWorldClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        sayHelloWorldResponse HelloWorld.sayHelloWorld(sayHelloWorldRequest request) {
            return base.Channel.sayHelloWorld(request);
        }
        
        public string sayHelloWorld(string name) {
            sayHelloWorldRequest inValue = new sayHelloWorldRequest();
            inValue.Body = new sayHelloWorldRequestBody();
            inValue.Body.name = name;
            sayHelloWorldResponse retVal = ((HelloWorld)(this)).sayHelloWorld(inValue);
            return retVal.Body.sayHelloWorldReturn;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<sayHelloWorldResponse> HelloWorld.sayHelloWorldAsync(sayHelloWorldRequest request) {
            return base.Channel.sayHelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<sayHelloWorldResponse> sayHelloWorldAsync(string name) {
            sayHelloWorldRequest inValue = new sayHelloWorldRequest();
            inValue.Body = new sayHelloWorldRequestBody();
            inValue.Body.name = name;
            return ((HelloWorld)(this)).sayHelloWorldAsync(inValue);
        }
    }
}
