using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace WCF.Second
{
    class DuplexSender
    {
        //入口方法
        static void Main(string[] args)
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                using (Message message = Message.CreateMessage(binding.MessageVersion, "sendMessage", "Message Body"))
                {
                    //创建ChannelFactory
                    IChannelFactory<IDuplexChannel> factory = binding.BuildChannelFactory<IDuplexChannel>(new BindingParameterCollection());
                    factory.Open();
                    //这里创建IRequestChannel
                    IDuplexChannel duplexChannel = factory.CreateChannel(new EndpointAddress("net.tcp://localhost:9090/DuplexService/Point2"));
                    duplexChannel.Open();
                    //发送消息
                    duplexChannel.Send(message);
                    Console.WriteLine("已经成功发送消息！");
                    //var msg = duplexChannel.Receive();
                    //Console.WriteLine($"收到消息：{msg.GetBody<string>()}");
                    duplexChannel.Close();
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
