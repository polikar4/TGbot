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
        public readonly VkApi vk = new VkApi();

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


        public async void Resendler()
        {
            vkmess vkmessage = await GetMessage();
            if (vkmessage.userid != 0)
            {
                Program.Get_messageAsync(null, vkmessage);
                
            }
            /*
            Console.WriteLine(vk.userid.ToString());
            if(vk.userid != 0)
                SengMessage(vk.message, vk.userid, null);
            */
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

        public async Task<vkmess> GetMessage()
        {
            return await Get_mess(await vk.Messages.GetDialogsAsync(new MessagesDialogsGetParams
            {
                Count = 1,
                Unread = true
            }));
        }  

        public async Task<vkmess> Get_mess(MessagesGetObject messages)
        {
            if (messages.Messages.Count > 0)
            {

                 string message = "";
                 string keyname = "";
                 long? userid = messages.Messages[0].UserId.Value;

                vk.Messages.MarkAsRead(messages.Messages[0].UserId.Value.ToString());

                if (messages.Messages[0].Body != null)
                    message = messages.Messages[0].Body.ToString();
                else
                    message = "";


                if (messages.Messages[0].Payload != null)
                    keyname = messages.Messages[0].Payload.ToString();
                else
                    keyname = "";

                vkmess vkmessage = new vkmess(message, keyname, userid);
                return vkmessage;
            }
            return new vkmess("", "", 0);
        }
    }
}
