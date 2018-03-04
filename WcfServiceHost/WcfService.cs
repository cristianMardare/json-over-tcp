using JsonRpcOverTcp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceHost
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        string Collect(string message);
    }

    [ServiceContract]
    public interface IUntypedWcfService
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message Process(Message input);
    }



    public class WcfService : IWcfService, IUntypedWcfService
    {
        public Message Process(Message input)
        {
            Console.WriteLine("[service] input = {0}", input);
            byte[] bytes = Formatting.MessageToBytes(input);
            Debugging.PrintBytes(bytes);

            return Formatting.BytesToMessage(bytes);
        }

        public string Collect(string message)
        {
            Debug.WriteLine($"Collected message: {message}");
            return "ok";
        }
    }
}
