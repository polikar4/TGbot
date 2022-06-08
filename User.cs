namespace TGbot
{
    public class User
    {
        public readonly bool _admin_root = true;
        public readonly Grup _grup;
        public readonly string FirstName = "", LastName = "", UserName = "";
        public readonly long id_tg = -1, id_vk = -1;
        private Bot_logic bot_Logic;

        public User(string FirstName, string LastName, string UserName, long id, bool tg_mess, Grup grup)
        {
            _grup = grup;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.UserName = UserName;
            if(tg_mess)
                this.id_tg = id;
            else
                this.id_vk = id;

            Send_To_Time.Send_Time += Send_debt; // Подписываемся на события 
            _grup.MessageAlert += AlertMessage;
            bot_Logic = new Bot_logic(this);
        }

        

        private void AlertMessage(object obj, MessageEvent messageEvent)
        {
            Program.Set_Messege( messageEvent.message + "\nОт " + FirstName + " " + LastName, id_tg, true);
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
