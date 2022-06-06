namespace TGbot
{
    static class Send_To_Time
    {
        public static event EventHandler Send_Time;

        public async static Task UpdateAsync()
        {
            while (true)
            {
                if (Send_Time != null)
                    Send_Time(null, new EventArgs());

                Console.WriteLine("Kek");
                await Task.Delay(405005);
            }
        }
    }
}
