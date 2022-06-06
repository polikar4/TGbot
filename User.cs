namespace TGbot
{
    public class User
    {
        public readonly bool _admin_root = true;
        public readonly int _id;
        public readonly Grup _grup;
        public readonly string _name_user;
        private Bot_logic bot_Logic;

        public User(int id, Grup grup, string name_user)
        {
            _id = id;
            _grup = grup;
            Send_To_Time.Send_Time += Send_debt;
            _name_user = name_user;
            bot_Logic = new Bot_logic(this);
        }

        public string Handler_message(string message)
        {
            return bot_Logic.Message(message);
        }
        private void Send_debt(object obj, EventArgs EventArgs)
        {

        }
    }
}
