namespace TGbot
{
    public enum UserStatus
    {
        user = 1,
        moder = 2,
        admin = 3
    }

    public class User
    {
        public readonly UserStatus userStatus;
        public readonly Grup _grup;
        public readonly string FirstName = "", LastName = "", UserName = "";
        public long id_tg = -1, id_vk = -1;
        private Bot_logic bot_Logic;

        public User(string FirstName, string LastName, string UserName, long id, bool tg_mess, Grup grup)
        {
            userStatus = UserStatus.admin;
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
            try
            {
                if (id_tg != -1)
                    Program.Set_Messege(messageEvent.message, id_tg, true);
            } catch {}

            try
            {
                if (id_vk != -1)
                Program.Set_Messege(messageEvent.message, id_vk, false);
            } catch { }
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
