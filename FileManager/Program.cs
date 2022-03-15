using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace FileManager
{
    //Почему-то юзерская переманная пути записана не верно. Не знаю, где ее сбросить
    //Если выводить путь на экран- папки выводятся через один знак \
    //А если навести мышку на переменную путь, то там через два \\
    class Program
    {
        static void PrintFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                try
                {
                    Console.WriteLine($"------{file}");
                }
                catch { continue; }
            }
        }
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
            string Path = @"C:\";
            PageDirectories(Properties.Settings.Default.Path, Page);

            while (true)
            {
                Console.WriteLine($"Это мой путь {Properties.Settings.Default.Path}");
                string UserCommand = Console.ReadLine();
                if(UserCommand == "exit")
                {
                    Properties.Settings.Default.Path = Path;
                    Properties.Settings.Default.Save();
                    break;
                }
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
                        Page = 0;
                        Properties.Settings.Default.Path = Properties.Settings.Default.Path + InDir + @"\";
                        Properties.Settings.Default.Save();
                        PageDirectories(Properties.Settings.Default.Path, Page);
                        break;
                    case "cd ..":
                        string[] arr = Properties.Settings.Default.Path.Split('\\');
                        Page = 0;
                        Properties.Settings.Default.Path = "";
                        for(int i = 0; i < arr.Length - 2; i++)
                        {
                            Properties.Settings.Default.Path = $"{Path}{arr[i]}\\";
                        }
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
            Console.WriteLine("cd       подняться на уровень выше");
            Console.WriteLine("cd ..    вернуться на уровень ниже");
            Console.WriteLine("exit     выйти с сохранением прогресса");
        }
    }
}
