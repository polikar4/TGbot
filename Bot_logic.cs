using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGbot
{
    class Bot_logic
    {
        private Status _status;
        private User _user;
        private string _message;
        private int _nomber_mess;
        enum Status
        {
            No_status = 0,
            Wtite_homework = 1,
            Grup_Edit = 2,
            Add_User = 3,
            My_info = 4,
            Alert_message = 5
        }
        public Bot_logic(User user)
        {
            _user = user;
            _status = Status.No_status;
            _message = "";
            _nomber_mess = -1;
        }

        public string Message(string message)
        {
            _message = message;
            _nomber_mess = -1;
            int.TryParse(message, out _nomber_mess);

            switch (_status)
            {
                case Status.Add_User:
                    Add_user();
                    break;

                case Status.Grup_Edit:
                    Edit_grup();
                    break;

                case Status.No_status:
                    No_status();
                    break;

                case Status.Wtite_homework:
                    Write_homework();
                    break;

                case Status.My_info:
                    My_info();
                    break;

                case Status.Alert_message:
                    Alert_message();
                    break;
            }

            if (message == _message)
                return "Нет такой команды /help";
            else
                return _message;
        }

        private void Alert_message()
        {
            
            if (_message == "Назад" || _nomber_mess == 2)
            {
                Perehod(Status.No_status);
                _message = "Вы отменили написание срочного сообщения\n\n" + _message;
            }
            else
            {
                _user._grup.AlertMessage(_message);
                Perehod(Status.No_status);
                _message = "Вы написали срочное сообщение группе. Надеюсь за него вас анально не покарают\n\n" + _message;
            }
        }
        private void No_status()
        {
            if (_message == "Добавить задание" || _nomber_mess == 2)
            {
                if (_user._admin_root)
                {
                    Perehod(Status.Wtite_homework);
                }
                else
                {
                    _message = "У вас нет прав для создания заданий для группы";
                }
            }
            else if (_message == "Группа" || _nomber_mess == 1)
            {
                Perehod(Status.Grup_Edit);
            }
            else if (_message == "Срочное сообщение группе" || _nomber_mess == 3)
            {
                if (_user._admin_root)
                {
                    Perehod(Status.Alert_message);
                }
                else
                {
                    Perehod(Status.No_status);
                    _message = "У вас нет прав писать срочные сообщения\n\n" + _message;
                }
                    
            }
            else if (_message == "Обо мне" || _nomber_mess == 4)
            {
                Perehod(Status.My_info);
            }
            else
                Perehod(Status.No_status);

        }
        private void My_info()
        {
            Perehod(Status.No_status);
            string info = _user.FirstName + " " + _user.LastName + "\n"
                + "Username - " + _user.UserName + "\n"
                + "Id - " + _user.id_tg.ToString() + "\n" 
                + "Наличие прав админа - " + (_user._admin_root ? "Есть" : "Нет");

            _message = info + "\n\n" + _message;
        }
        private void Edit_grup()
        {
            if (_message == "Список группы" || _nomber_mess == 1)
            {
                Perehod(Status.No_status);
                string list = "";
                int count = 1;
                foreach (User user in _user._grup.Get_User())
                {
                    list += count.ToString() + ") " + user.UserName + " id - "
                        + user.id_tg.ToString() + "  Aдмин - " +
                        (_user._admin_root ? "Да" : "Нет") + "\n";
                    count++;
                }
                Perehod(Status.Grup_Edit);
                _message = list + "\n" +_message;
            }
            else if (_message == "Добавить user" || _nomber_mess == 2)
            {
                Perehod(Status.Add_User);
            }
            else if (_message == "Дать права админа" || _nomber_mess == 3)
            {
                No_realis();
            }
            else if (_message == "Назад" || _nomber_mess == 4)
            {
                Perehod(Status.No_status);
            }
            else
            {
                Perehod(Status.Grup_Edit);
            }
        }
        private void Add_user()
        {
            int id = 0;
            if (_message == "Назад" || _nomber_mess == 2)
            {
                Perehod(Status.Grup_Edit);
                _message = "Вы отменили добавление пользователя в группу\n\n" + _message;
            }
            else if (int.TryParse(_message, out id))
            {
                Perehod(Status.Grup_Edit);
                _user._grup.Add_let_id(id);
                _message = "Пользователь добавлен\n\n" + _message;
            }
            else
            {
                _message = "Не верный id, введите ещё раз или выйдите в окно (Написать Назад или 2)";
            }
        }
        private void Write_homework()
        {
            Console.WriteLine(_message);
            Perehod(Status.No_status);
            _message = "Добавленно\n\n" + _message;
        }

        private void Perehod(Status status)
        {
            List<string> com_nostatus = new List<string> { "Группа", "Добавить задание", "Срочное сообщение группе", "Обо мне" };
            List<string> com_homework = new List<string> { "Введите дату", "Короткое описание задания", "Дополнительная информация" };
            List<string> com_grupedit = new List<string> { "Список группы ", "Добавить user", "Дать права админа", "Назад" };
            List<string> com_adduser = new List<string> { "Напишите id пользователя или (Назад или 2)" };
            List<string> com_alert = new List<string> { "Напишите срочное сообщение или (Назад или 2)" };

            switch (status) 
            {
                case Status.No_status:
                    _status = Status.No_status;
                    Write_Command(com_nostatus);
                    break;

                case Status.Wtite_homework:
                    _status = Status.Wtite_homework;
                    Write_Command(com_homework);
                    break;

                case Status.Grup_Edit:
                    _status = Status.Grup_Edit;
                    Write_Command(com_grupedit);
                    break;

                case Status.Add_User:
                    _status = Status.Add_User;
                    Write_Command(com_adduser);
                    break;

                case Status.My_info:
                    _status = Status.My_info;
                    My_info();
                    break;

                case Status.Alert_message:
                    _status = Status.Alert_message;
                    Write_Command(com_alert);
                    break;
            }
        }

        private void Write_Command(List<string> strings)
        {
            if (strings.Count == 1)
            {
                _message = strings[0];
            }
            else
            {
                string str = "";
                int i = 1;
                foreach (string s in strings)
                {
                    str += i.ToString() + ") " + s + "\n";
                    i++;
                }
                _message = str;
            }
        }

        private void No_realis()
        {
            _message = "На данный момент не реалезовано";
        }
    }
}
