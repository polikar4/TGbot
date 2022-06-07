namespace TGbot
{
     static class Send_To_Time
     {
        
        public static event EventHandler<TimeEvent> Send_Time;
        public static EventHandler<TimeEvent> Eventevent;

        public async static Task UpdateAsync()
        {
            while (true)
            {
                Eventevent = Send_Time;
                if (Eventevent != null)
                    Eventevent(null, new TimeEvent(10));

                await Task.Delay(60000);
                Console.WriteLine("prohla minuta");
            }
        }
    }

    class TimeEvent : EventArgs
    {
        public int _time_hous;
        public TimeEvent(int time_hous)
        {
            _time_hous = time_hous;
        }
    }
}
