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
                BindingElement[] bindingElements = new BindingElement[3];
                bindingElements[0] = new TextMessageEncodingBindingElement();
                //OneWayBindingElement可以使得传输通道支持数据报模式
                bindingElements[1] = new OneWayBindingElement();
                bindingElements[2] = new HttpTransportBindingElement();
                CustomBinding binding = new CustomBinding(bindingElements);
                using (Message message = Message.CreateMessage(binding.MessageVersion, "sendMessage","Message Body"))
                {
                    //创建ChannelFactory
                    IChannelFactory<IOutputChannel> factory = binding.BuildChannelFactory<IOutputChannel>(new BindingParameterCollection());
                    factory.Open();
                    //这里创建IOutputChannel
                    IOutputChannel outputChannel = factory.CreateChannel(new EndpointAddress("http://localhost/InputService"));
                    outputChannel.Open();
                    //发送消息
                    outputChannel.Send(message);
                    Console.WriteLine("已经成功发送消息！");
                    outputChannel.Close();
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
