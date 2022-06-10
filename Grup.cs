﻿namespace TGbot
{
    struct HomeWork
    {
        private string _title = "", _message = "";
        private DateTime _date = new DateTime();
        private List<User> _users = new List<User>();
        public HomeWork(string title, string messege, DateTime date, List<User> users)
        {
            _users = users;
            _title = title;
            _message = messege;
            _date = date;
            _date = new DateTime(_date.Year, _date.Month, _date.Day);
        }

        public string Get_title()
        {
            return _title;
        }

        public bool PresenceUser(User user)
        {
            foreach (User item in _users)
            {
                if (item == user)
                    return true;
            }
            return false;
        }

        public HomeWork(string title, string messege, DateTime date, User user)
        {
            _users.Add(user);
            _title = title;
            _message = messege;
            _date = date;
            _date = new DateTime(_date.Year, _date.Month, _date.Day);
        }

    }

    public class Grup
    {
        public event EventHandler<MessageEvent> MessageAlert;
        private static EventHandler<MessageEvent> MessageEventUser;
        private string _name;
        private int _id;
        private List<int> let_id = new List<int>();
        private List<HomeWork> _homes = new List<HomeWork>();
        private List<User> _user = new List<User>();

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

        public string Get_Homework(User user)
        {
            string homework = "Задания\n";
            int count = 1;
            foreach(HomeWork homeWork in _homes)
            {
                if (homeWork.PresenceUser(user))
                {
                    homework += count.ToString() + ") " + homeWork.Get_title() + "\n";
                    count++;
                }
            }
            return homework;
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
        public void Add_let_id(int id)
        {
            let_id.Add(id);
        }
        public void Add_homework_Grup(string title, string messege, DateTime date)
        {
            _homes.Add(new HomeWork(title, messege, date,this._user));
        }
        public void Add_homework_ls(string title, string messege, DateTime date,User user)
        {
            _homes.Add(new HomeWork(title, messege, date, user));
        }
        public bool Search_let_id(int _id)
        {
            foreach (int id in let_id)
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
