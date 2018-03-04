using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace JsonRpcOverTcp.ServiceModel
{
    public class JsonRpcEndpointBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(JsonRpcEndpointBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new JsonRpcEndpointBehavior();
        }
    }
}
