using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;


namespace TGbot
{
    static class Program
    {
        static vkbot vk;
        static tgbot tg;
        static void Main(string[] args)
        {
            Send_To_Time.UpdateAsync(); // Event time

            vk = new vkbot(); // Start vk bot
            vk.StartProgtam();

            tg = new tgbot();
            

            Date.grups.Add(new Grup(228228228));       // Create test User
            Date.grups[0].Add_let_id(762150197, true);
            Date.grups[0].Add_let_id(769964603, true);
            Date.grups[0].Add_let_id(425505411, true);

            Console.ReadLine();
        }

        public static void Set_Messege(Message message, string _message)
        {
            tg.Set_Messege(message, _message);
        }

        public static User Search_User(int id)
        {
            foreach(Grup grup in Date.grups)
                foreach (User user in grup.Get_User())
                    if (user._user.From.Id == id)
                        return user;
            return null;
        }

        public static bool Have_Rules(int id, out Grup grup_return)
        {
            foreach (Grup grup in Date.grups)
                if (grup.Search_let_id(id))
                {
                    grup_return = grup;
                    return true;
                }
            grup_return = null;
            return false;
        }
    }
}