using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace JsonRpcOverTcp.Channels
{
    public class SizedTcpTransportBindingExtensionElement : BindingElementExtensionElement
    {
        public override Type BindingElementType
        {
            get
            {
                return typeof(SizedTcpTransportBindingElement);
            }
        }

        protected override BindingElement CreateBindingElement()
        {
            return new SizedTcpTransportBindingElement();
        }
    }
}
