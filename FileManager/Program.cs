using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    class Program
    {
        static void PageDirectories(string path, int userpage)
        {
            int Page = userpage * Properties.Settings.Default.PageSize;
            Console.Clear();
            string[] Dirs = Directory.GetDirectories(path);
            for (int i = Page; i < Page + Properties.Settings.Default.PageSize;)
            {
                try
                {
                    if (Dirs.Length <= i)
                        break;
                    Console.WriteLine(Dirs[i]);
                    i++;
                }
                catch { continue; }
            }
        }
        static void Main(string[] args)
        {
            int Page = 0;
            PageDirectories(Properties.Settings.Default.Path, Page);

            while (true)
            {
                string UserCommand = Console.ReadLine();
                switch(UserCommand)
                {
                    case "help":
                        Help();
                        break;
                    case "next":
                        Page++;
                        PageDirectories(Properties.Settings.Default.Path, Page);
                        break;
                    case "back":
                        Page--;
                        PageDirectories(Properties.Settings.Default.Path, Page);
                        break;
                    case "cd":
                        Console.WriteLine("Введите имя папки: ");
                        string InDir = Console.ReadLine();
                        Properties.Settings.Default.Path = Properties.Settings.Default.Path + InDir + @"\";
                        Properties.Settings.Default.Save();
                        PageDirectories(Properties.Settings.Default.Path, Page);
                        break;
                }
            }
            
        }
        static void Help()
        {
            Console.Clear();
            Console.WriteLine("next     следующая страница");
            Console.WriteLine("back     предыдущая страница");
        }
    }
}
