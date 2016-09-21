using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Runtime.Serialization;
namespace WCF.Second
{
    class Output
    {
        //入口方法
        static void Main(string[] args)
        {
            try
            {
                //建立自定义的通道栈
                BindingElement[] bindingElements = new BindingElement[2];
                bindingElements[0] = new TextMessageEncodingBindingElement();
                bindingElements[1] = new HttpTransportBindingElement();
                CustomBinding binding = new CustomBinding(bindingElements);
                using (Message message = Message.CreateMessage(binding.MessageVersion, "sendMessage","Message Body"))
                {
                    //创建ChannelFactory
                    IChannelFactory<IRequestChannel> factory = binding.BuildChannelFactory<IRequestChannel>(new BindingParameterCollection());
                    factory.Open();
                    //这里创建IRequestChannel
                    IRequestChannel requestChannel = factory.CreateChannel(new EndpointAddress("http://localhost/RequestReplyService"));
                    requestChannel.Open();
                    //发送消息
                    Message response = requestChannel.Request(message);
                    Console.WriteLine("已经成功发送消息！");
                    //查看返回消息
                    Console.WriteLine("接受到一条返回消息，action为：{0}，body为：{1}", response.Headers.Action, response.GetBody<String>());
                    requestChannel.Close();
                    factory.Close();
                }
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
