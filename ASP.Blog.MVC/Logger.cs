using System;

namespace ASP.Blog.MVC
{
    public class Logger : ILogger
    {
        public void WriteEvent(string errorMessage)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen; 
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(errorMessage);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteError(string eventMessage)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(eventMessage);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
