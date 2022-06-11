using System.IO;

namespace TGbot
{
    static class Date
    {
        public static List<Grup> grups = new List<Grup>();
        public const string directory = @"C:\Grups";

        public static void Load_base()
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                StreamReader sr = new StreamReader(file);

                int id_grup = Convert.ToInt32(sr.ReadLine().Split("=")[1]);
                string name_grup = sr.ReadLine().Split("=")[1];
                List<int> let_id = new List<int>();
                List<HomeWork> home_work = new List<HomeWork>();

                if (sr.ReadLine() == "{ let id")
                {
                    string let_id_str = sr.ReadLine();
                    while(let_id_str != "} end let id")
                    {
                        let_id.Add(Convert.ToInt32(let_id_str));
                        let_id_str = sr.ReadLine();
                    }
                }

                if(sr.ReadLine() == "{ homeworks")
                {
                    while (true)
                    {
                        string mess = sr.ReadLine();
                        if (mess != "{")
                            break;   

                        string title = sr.ReadLine();
                        string message = sr.ReadLine();
                        DateTime dateTime = ToDate(sr.ReadLine().Split(" ")[0].Split("."));
                        List<int> id_vk = new List<int>();
                        List<int> id_tg = new List<int>();

                        while (true)
                        {
                            string id = sr.ReadLine();
                            if (id == "}")
                                break;
                            if (id == "{")
                                continue;
                            id_vk.Add(Convert.ToInt32(id));
                        }
                        while (true)
                        {
                            string id = sr.ReadLine();
                            if (id == "}")
                                break;
                            if (id == "{")
                                continue;
                            id_tg.Add(Convert.ToInt32(id));
                        }

                        home_work.Add(new HomeWork(title, message, dateTime, id_vk, id_tg));
                    }
                }
                string mess12132 = sr.ReadLine();

                grups.Add(new Grup(name_grup, id_grup, let_id, home_work));


                if( sr.ReadLine() == "{ user")
                {
                    while (true)
                    {
                        string mess = sr.ReadLine();
                        if (mess != "{")
                            break;

                        UserStatus stat = new UserStatus();
                        string message = sr.ReadLine();
                        DateTime dateTime = ToDate(sr.ReadLine().Split(" ")[0].Split("."));
                        List<int> id_vk = new List<int>();
                        List<int> id_tg = new List<int>();

                        while (true)
                        {
                            string id = sr.ReadLine();
                            if (id == "}")
                                break;
                            if (id == "{")
                                continue;
                            id_vk.Add(Convert.ToInt32(id));
                        }
                        while (true)
                        {
                            string id = sr.ReadLine();
                            if (id == "}")
                                break;
                            if (id == "{")
                                continue;
                            id_tg.Add(Convert.ToInt32(id));
                        }

                        h
                }
            }
            //one grup = one .txt
            //for grups
                //create grup (id, name, letid)
                //cteate list homeworks ( title, mess, time, list id vk, list id tg)
                //add homeworks in grup
                //cteate list users (roll, first-last-username, vk-tg id, grup) 
                //add users in grup

        }

        private static DateTime ToDate(string[] a)
        {
            int day = Convert.ToInt32(a[0]);
            int monu = Convert.ToInt32(a[1]);
            int year = Convert.ToInt32(a[2]);
            return new DateTime(year, monu, day);
        }

        public static void Save_base()
        {
            foreach (Grup grup in grups)
            {
                object[] date = grup.Get_date_to_Base(); // { _name, _id, let_id, _homes, _user};

                StreamWriter sw = new StreamWriter(directory + @"\" + date[1] + ".txt");
                
                sw.WriteLine("id grup =" + date[1].ToString());    // id 
                sw.WriteLine("name grup =" + (string)date[0]); // name
                sw.WriteLine("{ let id");
                foreach(int id in (List<int>)date[2]) { // let_id
                    sw.WriteLine(id);
                }
                sw.WriteLine("} end let id");
                sw.WriteLine("{ homeworks");
                foreach (HomeWork homework in (List<HomeWork>)date[3])
                {
                    sw.WriteLine("{");
                    object[] hm = homework.Get_date_to_Base(); // { _title, _message, _date, _id_tg, _id_vk }
                    sw.WriteLine(hm[0]);
                    sw.WriteLine(hm[1]);
                    sw.WriteLine(hm[2]);
                    sw.WriteLine("{");
                    foreach (int id in (List<int>)hm[3])
                    { // let_id
                        sw.WriteLine(id);
                    }
                    sw.WriteLine("}");
                    sw.WriteLine("{");
                    foreach (int id in (List<int>)hm[4])
                    { // let_id
                        sw.WriteLine(id);
                    }
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                }
                sw.WriteLine("} end homeworks");
                sw.WriteLine("{ user");
                foreach (User user in (List<User>)date[4])
                {
                    //{ userStatus, FirstName, LastName , UserName , id_tg, id_vk }
                    object[] us = user.Get_date_to_Base();
                    sw.WriteLine("{");
                    sw.WriteLine(us[0]);
                    sw.WriteLine(us[1]);
                    sw.WriteLine(us[2]);
                    sw.WriteLine(us[3]);
                    sw.WriteLine(us[4]);
                    sw.WriteLine(us[5]);
                    sw.WriteLine("}");
                }
                sw.WriteLine("} end user");
                sw.Close();
            }
        }

        

    }
}
