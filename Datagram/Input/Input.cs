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
                BindingElement[] bindingElements = new BindingElement[3];
                bindingElements[0] = new TextMessageEncodingBindingElement();
                bindingElements[1] = new OneWayBindingElement();
                bindingElements[2] = new HttpTransportBindingElement();
                CustomBinding binding = new CustomBinding(bindingElements);
                //建立ChannelListner
                IChannelListener<IInputChannel> listener = binding.BuildChannelListener<IInputChannel>(new Uri("http://localhost/InputService"), new BindingParameterCollection());
                listener.Open();
                //创建IInputChannel
                IInputChannel inputChannel = listener.AcceptChannel();
                inputChannel.Open();
                Console.WriteLine("开始接受消息。。。");
                //接受并打印消息
                Message message = inputChannel.Receive();
                Console.WriteLine("接受到一条消息，action为：{0}，body为：{1}", message.Headers.Action, message.GetBody<String>());
                message.Close();
                inputChannel.Close();
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
