using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;


namespace TGbot
{
    class tgbot
    {
        private ITelegramBotClient bot;

        public tgbot()
        {
            bot = new TelegramBotClient("5301941928:AAGNku_-noB7LNFOtAyKjxal34uaEMb8PHI");
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
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                int user_id = (int)update.Message.From.Id;
                Telegram.Bot.Types.Message message = update.Message;
                User user = Program.Search_User(user_id);
                if (user == null)
                {
                    if (Program.Have_Rules(user_id, out Grup grup))
                    {
                        User _user = new User(update.Message, grup);
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

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public void Set_Messege(Message message, string _message)
        {
            bot.SendTextMessageAsync(message.Chat, _message);
        }

    }
}
