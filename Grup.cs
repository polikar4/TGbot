namespace TGbot
{
    public class Grup
    {
        public event EventHandler<MessageEvent> MessageAlert;

        private static EventHandler<MessageEvent> MessageEventUser;
        private string _name;
        private int _id;
        private List<int> let_id_tg = new List<int>();
        private List<int> let_id_vk = new List<int>();
        private List<HomeWork> _homes = new List<HomeWork>();
        private List<User> _user = new List<User>();

        struct HomeWork
        {
            private string _title = "", _message = "";
            private DateTime _date = new DateTime();
            public HomeWork(string title, string messege, DateTime date)
            {
                _title = title;
                _message = messege;
                _date = date;
                _date = new DateTime(_date.Year, _date.Month, _date.Day);
            }
        }

        public Grup(int id)
        {
            _id = id;
            _name = id.ToString();
        }

        public void AlertMessage(string messege)
        {
            MessageEventUser = MessageAlert;
            if (MessageEventUser != null)
                MessageEventUser(null, new MessageEvent(messege));
        }

        public string Get_name()
        {
            return _name;
        }

        public List<User> Get_User()
        {
            return _user;
        }
        public void Add_User(User user)
        {
            _user.Add(user);
        }

        public void Add_let_id(int id, bool tg)
        {
            if(tg)
                let_id_tg.Add(id);
            else
                let_id_vk.Add(id);
        }

        public void Add_homework(string title, string messege, DateTime date)
        {
            _homes.Add(new HomeWork(title, messege, date));
        }

        public bool Search_let_id(int _id)
        {
            foreach (int id in let_id_tg)
                if (id == _id)
                    return true;
            return false;
        }
    }

    public class MessageEvent : EventArgs
    {
        public string message;

        public MessageEvent(string message)
        {
            this.message = message;
        }
    }
}
