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
        private static ITelegramBotClient bot = new TelegramBotClient("5301941928:AAGNku_-noB7LNFOtAyKjxal34uaEMb8PHI");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                int user_id = (int)update.Message.From.Id;
                Telegram.Bot.Types.Message message = update.Message;
                User user = Search_User(user_id);
                if (user == null)
                {
                    if (Have_Rules(user_id, out Grup grup))
                    {
                        User _user = new User(user_id, grup, update.Message.From.Username);
                        grup.Add_User(_user);
                        await botClient.SendTextMessageAsync(message.Chat, "Вы подключены к группе с названием " + grup.Get_name());
                    }
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Ваш id - " + user_id.ToString() +
                            "\nНа данный момент вы не числетись не в одной группе" + 
                            "\nОтправте управляющим вашей группы ваш id");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, user.Handler_message(message.Text));
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Date.grups.Add(new Grup(228228228));
            Date.grups[0].Add_User(new User(769964603, Date.grups[0], "Vadim" ));
            Send_To_Time.UpdateAsync(); 
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }

        private static User Search_User(int id)
        {
            foreach(Grup grup in Date.grups)
                foreach (User user in grup.Get_User())
                    if (user._id == id)
                        return user;
            return null;
        }

        private static bool Have_Rules(int id, out Grup grup_return)
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