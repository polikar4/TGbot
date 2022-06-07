namespace TGbot
{
    public class User
    {
        public readonly bool _admin_root = true;
        public readonly Grup _grup;
        public readonly Telegram.Bot.Types.Message _user;
        private Bot_logic bot_Logic;

        public User(Telegram.Bot.Types.Message user, Grup grup)
        {
            _grup = grup;
            _user = user;
            Send_To_Time.Send_Time += Send_debt;
            bot_Logic = new Bot_logic(this);
            _grup.MessageAlert += AlertMessage;
            
        }

        private void AlertMessage(object obj, MessageEvent messageEvent)
        {
            Program.Set_Messege(_user, messageEvent.message + "\nОт " + _user.From.FirstName + " " + _user.From.LastName);
        }

        public string Handler_message(string message)
        {
            return bot_Logic.Message(message);
        }

        private void Send_debt(object obj, TimeEvent timeEvent)
        {
            //Console.WriteLine(timeEvent._time_hous.ToString()); 
        }
    }
}
