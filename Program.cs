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

        public static async Task Get_messageAsync(Update update, vkmess vkmessage)
        {
            if (update != null && update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                Treatment_message(update.Message.From.Id, update.Message.Text, true, update.Message.Chat);
            }
            else
            {
                
                Treatment_message(vkmessage.userid, vkmessage.message, false, null);
            }
        }

        public static void Treatment_message(long? id, string message, bool tg_mess, Chat chat)
        {
            User user = Program.Search_User(id);

            if(user == null)
            {
                if (tg_mess)
                    Set_Messege("Вас в базе нет, пошли нахуй", chat);
                else
                {
                    Set_Messege("Вас в базе нет, пошли нахуй",  id);
                }
            }

            /*int user_id = (int)update.Message.From.Id;
            Telegram.Bot.Types.Message message = update.Message;
            
            if (user == null)
            {
                if (Program.Have_Rules(user_id, out Grup grup))
                {
                    User _user = new User(update.Message, grup);
                    grup.Add_User(_user);
                    await tg.bot.SendTextMessageAsync(message.Chat, "Вы подключены к группе с названием " + grup.Get_name());
                }
                else
                    await tg.bot.SendTextMessageAsync(message.Chat, "Ваш id - " + user_id.ToString() +
                        "\nНа данный момент вы не числетись не в одной группе" +
                        "\nОтправте управляющим вашей группы ваш id");
            }
            else
            {
                await tg.bot.SendTextMessageAsync(message.Chat, user.Handler_message(message.Text));
            }*/
        }

        public static void Set_Messege(string _message, Chat chat)
        {
              tg.Set_Messege(chat, _message);
        }

        public static void Set_Messege(string _message, long? id)
        {
              vk.SengMessage(_message, id, null);
        }

        public static User Search_User(long? id)
        {
            foreach(Grup grup in Date.grups)
                foreach (User user in grup.Get_User())
                    if (user.id_tg == id)
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