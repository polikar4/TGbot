using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGbot
{
    class tgbot
    {
        public readonly ITelegramBotClient bot;

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
            await Program.Get_messageAsync(update, new vkmess("","",0));
        }
        
        

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public void Set_Messege(long id, string _message)
        {

            List<InlineKeyboardButton> buttons = new();
            for (int nomber = 0; nomber < 10; nomber++)
                if (_message.Contains(nomber.ToString() + ")"))
                    buttons.Add(InlineKeyboardButton.WithCallbackData(nomber.ToString(), nomber.ToString()));
            bot.SendTextMessageAsync(new ChatId(id), _message, replyMarkup: new InlineKeyboardMarkup(buttons));
        }

    }
}
