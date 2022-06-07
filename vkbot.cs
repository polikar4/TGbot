using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace TGbot
{
    class vkbot
    {
        private VkApi vk = new VkApi();

        public vkbot()
        {
            vk.Authorize(new ApiAuthParams
            {
                AccessToken = "4fe4f7ea8bd109c748888b0552e570c08dff79562536e67b99f24fda24cfbccf299496ddcc2ca2228dd31"
            });
           Console.WriteLine("Create vk bot");
        }

        public async Task StartProgtam()
        {
            Console.WriteLine("Start vk bot");
            while (true)
            {
                await Task.Delay(1000);
                await Task.Run(() => Resendler());
            }
            
        }


        public void Resendler()
        {
            object[] obj = GetMessage();
            if (obj != null)
            {
                Console.WriteLine(obj[2].ToString());
                SengMessage(obj[0].ToString(), Convert.ToInt32(obj[2]), null);
            }
        }


        public void SengMessage(string message, long? userid, MessageKeyboard keyboard)
        {
            vk.Messages.Send(new MessagesSendParams
            {
                Message = message,
                PeerId = userid,
                RandomId = new Random().Next(),
                Keyboard = keyboard
            });
        }

        public object[] GetMessage()
        {
            string message = "";
            string keyname = "";
            long? userid = 0;


              var messages = vk.Messages.GetDialogs(new MessagesDialogsGetParams
            {
                Count = 1,
                Unread = true
            });

            if (messages.Messages.Count > 0)
            {

                if (messages.Messages[0].Body != null)
                    message = messages.Messages[0].Body.ToString();
                else
                    message = "";

                if (messages.Messages[0].Payload != null)
                    keyname = messages.Messages[0].Payload.ToString();
                else
                    keyname = "";


                    userid = messages.Messages[0].UserId.Value;
                vk.Messages.MarkAsRead(userid.ToString());
                return new object[3] { message, keyname, userid }; 
            }
            else
            {
                return null;
            }


        }

        
    }
}
