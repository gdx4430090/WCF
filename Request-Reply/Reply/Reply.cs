using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace WCF.Second
{
    class Input
    {
        static void Main(string[] args)
        {
            try
            {
                //建立和发送端相同的通道栈
                BindingElement[] bindingElements = new BindingElement[2];
                bindingElements[0] = new TextMessageEncodingBindingElement();
                bindingElements[1] = new HttpTransportBindingElement();
                CustomBinding binding = new CustomBinding(bindingElements);
                //建立ChannelListner
                IChannelListener<IReplyChannel> listener = binding.BuildChannelListener<IReplyChannel>(new Uri("http://localhost/RequestReplyService"), new BindingParameterCollection());
                listener.Open();
                //创建IReplyChannel
                IReplyChannel replyChannel = listener.AcceptChannel();
                replyChannel.Open();
                Console.WriteLine("开始接受消息。。。");
                //接受并打印消息
                RequestContext requestContext = replyChannel.ReceiveRequest();
                Console.WriteLine("接受到一条消息，action为：{0}，body为：{1}", requestContext.RequestMessage.Headers.Action, requestContext.RequestMessage.GetBody<String>());
                //创建返回消息
                Message response = Message.CreateMessage(binding.MessageVersion, "response", "reponse body");
                //发送返回消息
                requestContext.Reply(response);
                //关闭
                requestContext.Close();
                replyChannel.Close();
                listener.Close();
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.Read();
            }
        }
    }
}
