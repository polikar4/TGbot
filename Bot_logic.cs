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
            Task = 1,
            Grup_Edit = 2,
            Add_User = 3,
            Alert_message = 5,
            Aboutme = 6,
            Set_id = 7,
            Add_HomeWork_grup = 8,
            Add_HomeWork_me = 9
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

                case Status.Task:
                    Task();
                    break;

                case Status.Alert_message:
                    Alert_message();
                    break;

                case Status.Aboutme:
                    Aboutme();
                    break;

                case Status.Set_id:
                    Set_id();
                    break;

                case Status.Add_HomeWork_grup:
                    Add_HomeWork_grup();
                    break;

                case Status.Add_HomeWork_me:
                    Add_HomeWork_me();
                    break;
            }

            if (message == _message)
                return "Если ты видешь это сообщение, это значит ты нашёл дырку в машине состояний. Рекомендую написать @polikar4"+
                    "\nПриложи скрин как ты дошёл до этого состояния, желательно последние 10-15 команд";
            else
                return _message;
        }

        private void Set_id()
        {
            long id = 0;
            if (_message == "Назад" || _nomber_mess == 2)
            {
                Perehod(Status.Aboutme);
                _message = "Вы отменили добавление пользователя в группу\n\n" + _message;
            }
            else if (long.TryParse(_message, out id))
            {
                Perehod(Status.Aboutme);
                if (_user.id_tg == -1)
                    _user.id_tg = id;
                else
                    _user.id_vk = id;

                _message = "Пользователь добавлен\n\n" + _message;
            }
            else
            {
                _message = "Не верный id, введите ещё раз или выйдите в окно (Написать Назад или 2)";
            }
        }
        private void Aboutme()
        {
            string info = _user.FirstName + " " + _user.LastName + "\n"
                + "Username - " + _user.UserName + "\n"
                + "Id telegram - " + _user.id_tg.ToString() + "\n"
                + "Id vk - " + _user.id_vk.ToString() + "\n"
                + "Роль - " + _user.userStatus.ToString();

            //"Добавть tg id", "Добавть vk id", "Назад"
            if (_message == "Добавть tg id" || _nomber_mess == 1)
            {
                if(_user.id_tg != -1)
                {
                    Perehod(Status.Aboutme);
                    _message = "У вас уже добавлен tg id\n\n" + _message;
                }
                else
                {
                    Perehod(Status.Set_id);
                }
            }
            else if (_message == "Добавть vk id" || _nomber_mess == 2)
            {
                if (_user.id_vk != -1)
                {
                    Perehod(Status.Aboutme);
                    _message = "У вас уже добавлен vk id\n\n" + _message;
                }
                else
                {
                    Perehod(Status.Set_id);
                } 
            }
            else if (_message == "Назад" || _nomber_mess == 3)
            {
                Perehod(Status.No_status);
            }
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
                _user._grup.AlertMessage(_message +"\n от "+ _user.FirstName + " " + _user.LastName);
                Perehod(Status.No_status);
                _message = "Вы написали срочное сообщение группе. Надеюсь за него вас анально не покарают\n\n" + _message;
            }
        }
        private void No_status()
        {
            if (_message == "Задания" || _nomber_mess == 2)
            {
                if (_user.userStatus > 0)
                {
                    Perehod(Status.Task);
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
                if (_user.userStatus > 0)
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
                string info = _user.FirstName + " " + _user.LastName + "\n"
                + "Username - " + _user.UserName + "\n"
                + "Id telegram - " + _user.id_tg.ToString() + "\n"
                + "Id vk - " + _user.id_vk.ToString() + "\n"
                + "Роль - " + _user.userStatus.ToString();

                Perehod(Status.Aboutme);

                _message = info + "\n\n" + _message; 
            }
            else
                Perehod(Status.No_status);

        }
        private void Edit_grup()
        {
            if (_message == "Список группы" || _nomber_mess == 1)
            {
                string rol = "";
                if (_user.userStatus == UserStatus.user)
                    rol = "User";
                if (_user.userStatus == UserStatus.moder)
                    rol = "Moder";
                if (_user.userStatus == UserStatus.admin)
                    rol = "Admin";
                Perehod(Status.No_status);
                string list = "";
                int count = 1;
                foreach (User user in _user._grup.Get_User())
                {
                    list += count.ToString() + ") " + user.LastName + " id - "
                        + user.id_tg.ToString() + "  Роль - " +
                         rol + "\n";
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
        private void Add_HomeWork_grup()
        {
            if (_message == "Назад" || _nomber_mess == 2)
            {
                Perehod(Status.Task);
                _message = "Вы отменили написание задания \n\n" + _message;
                return;
            }
            string[] text = _message.Split('\n');
            if (text.Length == 2)
            {
                string[] date = text[0].Split('.');
                if (date.Length == 2)
                {
                    int year = 0;
                    if (DateTime.Now < new DateTime(DateTime.Now.Year, int.Parse(date[1]), int.Parse(date[0])))
                        year = DateTime.Now.Year;
                    else
                        year = DateTime.Now.Year + 1;
                    _user._grup.Add_homework_Grup(text[1], "", new DateTime(year, int.Parse(date[1]), int.Parse(date[0])));
                    Perehod(Status.Task);
                    _message = "Задание создано\n" + _message;
                    return;
                }

                Perehod(Status.Task);
                _message = "Неверный ввод задания" + _message;
                return;
            }
            if (text.Length == 3)
            {
                string[] date = text[0].Split('.');
                if(date.Length == 2)
                {
                    int year = 0;
                    if(DateTime.Now < new DateTime(DateTime.Now.Year, int.Parse(date[1]),int.Parse(date[0]) ))
                        year = DateTime.Now.Year;
                    else
                        year = DateTime.Now.Year + 1;
                    _user._grup.Add_homework_Grup(text[1], text[2], new DateTime(year, int.Parse(date[1]), int.Parse(date[0])));
                    Perehod(Status.Task);
                    _message = "Задание создано\n" + _message;
                    return;
                }

                Perehod(Status.Task);
                _message = "Неверный ввод задания" + _message;
                return;
            }
            Perehod(Status.Task);
            _message = "Неверный ввод задания" + _message;
            return;
        }
        private void Add_HomeWork_me()
        {
            if (_message == "Назад" || _nomber_mess == 2)
            {
                Perehod(Status.Task);
                _message = "Вы отменили написание задания \n\n" + _message;
            }
            _user._grup.Add_homework_ls(_message, "Kek", new DateTime(2023, 01, 01),_user);
            Perehod(Status.Task);
        }
        private void Task()
        {
            if (_message == "Список заданий" || _nomber_mess == 1)
            {
                Perehod(Status.Task);
                _message = _user._grup.Get_Homework(_user) + "\n\n" + _message;
            }
            else if (_message == "Добавить задание группе" || _nomber_mess == 2)
            {
                Perehod(Status.Add_HomeWork_grup);
                _message = "Напишите задание в видеn\n" +
                   "Дата в формате день.месяц\n" +
                   "Краткое описание задания\n" +
                   "Более детальное описание (при необходимости можно не писать)\n\n";
            }
            else if (_message == "Добавить задание себе" || _nomber_mess == 3)
            {
                Perehod(Status.Add_HomeWork_me);
                _message = "Напишите задание в видеn\n" +
                    "Дата в формате день.месяц\n" +
                    "Краткое описание задания\n" +
                    "Более детальное описание (при необходимости можно не писать)\n\n";
            }
            else if (_message == "Назад" || _nomber_mess == 4)
            {
                Perehod(Status.No_status);
            }
            else
            {
                Perehod(Status.Task);
            }
        }

        private void Perehod(Status status)
        {
            List<string> com_nostatus = new List<string> { "Группа", "Задания", "Срочное сообщение группе", "Обо мне" };
            List<string> com_homework = new List<string> { "Список заданий", "Добавить задание группе", "Добавить задание себе", "Назад" };
            List<string> com_grupedit = new List<string> { "Список группы ", "Добавить user", "Дать права админа", "Назад" };
            List<string> com_adduser = new List<string> { "Напишите id пользователя или (Назад или 2)" };
            List<string> com_alert = new List<string> { "Напишите срочное сообщение или (Назад или 2)" };
            List<string> com_aboutme = new List<string> { "Добавть tg id", "Добавть vk id", "Назад"};
            List<string> com_setid = new List<string> { "Напишите id пользователя или (Назад или 2)" };
            List<string> Add_HomeWork_grup = new List<string> { "Напишите задание группе или (Назад или 2)" };
            List<string> Add_HomeWork_me = new List<string> { "Напишите задание себе или (Назад или 2)" };


            switch (status) 
            {
                case Status.No_status:
                    _status = Status.No_status;
                    Write_Command(com_nostatus);
                    break;

                case Status.Task:
                    _status = Status.Task;
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

                case Status.Alert_message:
                    _status = Status.Alert_message;
                    Write_Command(com_alert);
                    break;

                case Status.Aboutme:
                    _status = Status.Aboutme;
                    Write_Command(com_aboutme);
                    break;

                case Status.Set_id:
                    _status = Status.Set_id;
                    Write_Command(com_setid);
                    break;

                case Status.Add_HomeWork_grup:
                    _status = Status.Add_HomeWork_grup;
                    Write_Command(Add_HomeWork_grup);
                    break;

                case Status.Add_HomeWork_me:
                    _status = Status.Add_HomeWork_me;
                    Write_Command(Add_HomeWork_me);
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
