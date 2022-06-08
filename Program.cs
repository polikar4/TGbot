using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;

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
            Date.grups[0].Add_let_id(762150197);
            Date.grups[0].Add_let_id(769964603);
            Date.grups[0].Add_let_id(425505411);
            Date.grups[0].Add_let_id(340275666);

            Console.ReadLine();
        }

        public static async Task Get_messageAsync(Update update, vkmess vkmessage)
        {
            if (update != null && update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                Treatment_message(update.Message.From.Id, update.Message.Text, true, update);
            }
            else
            {
                Treatment_message(vkmessage.userid, vkmessage.message, false);
            }
        }

        public static void Treatment_message(long? id, string message, bool tg_mess,Update update = null)
        {
            User user = Program.Search_User(id);

            if(user == null)
            {
                if (Program.Have_Rules(id, out Grup grup))
                {
                    if (tg_mess)
                    {
                        user = new User(update.Message.From.FirstName,
                            update.Message.From.LastName,
                            update.Message.From.Username,
                            (long)id, true, grup);
                    }
                    else
                    {
                        var dateuser = vk.vk.Users.Get(new long[] { (long)id }).FirstOrDefault();
                        user = new User(dateuser.FirstName,
                            dateuser.LastName,
                            dateuser.ScreenName,
                            (long)id, true, grup);
                    }
                    grup.Add_User(user);
                    Set_Messege("Вы подключены к группе\n\n" + user.Handler_message("-1"), (long)id, tg_mess);

                }
                else
                {
                    message = "Ваш id - " + id.ToString() +
                        "\nНа данный момент вы не числетись не в одной группе" +
                        "\nОтправте управляющим вашей группы ваш id" +
                        "\nИли если вы зареганы в другом мессенжере, то добавте это id";

                    Set_Messege(message, (long)id, tg_mess);
                }  
                
            }
            else
            {
                Set_Messege(user.Handler_message(message),(long)id, tg_mess);
            }

            /*int user_id = (int)update.Message.From.Id;
            Telegram.Bot.Types.Message message = update.Message;
            
            if (user == null)
            {
                if ()
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

        public static void Set_Messege(string _message, long id, bool tg_mess)
        {
            if (tg_mess)
                tg.Set_Messege(id, _message);
            else
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

        public static bool Have_Rules(long? id, out Grup grup_return)
        {
            foreach (Grup grup in Date.grups)
                if (grup.Search_let_id((int)id))
                {
                    grup_return = grup;
                    return true;
                }
            grup_return = null;
            return false;
        }
    }
}