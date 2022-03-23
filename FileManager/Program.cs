using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace FileManager
{
    
    class Program
    {
        /// <summary>
        /// Окно предупреждения
        /// </summary>
        static void CommandDontFound()
        {
            Console.Clear();
            PageDirectories();
            InformationWindow(Properties.Settings.Default.Path);
            Frame("up");
            Console.WriteLine(Properties.Settings.Default.Format, "Команда не распознана. Введите help");
            Frame("down");

            File.AppendAllText(Properties.Settings.Default.PathFErr, "\nКоманда не распознана");
        }
        /// <summary>
        /// Оформление окон
        /// </summary>
        /// <param name="side"></param>
        static void Frame(string side)
        {
            if(side == "up")
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");

            if(side == "down")
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");

            if (side == "trans")
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");

            if (side == "trans1")
                Console.WriteLine(Properties.Settings.Default.Format, "");

            if(side == "message")
            {
                Frame("up");
                Console.WriteLine(Properties.Settings.Default.Format, "Отказано в доступе по пути");
                Frame("down");

                File.AppendAllText(Properties.Settings.Default.PathFErr, "\nОтказано в доступе по указанному пути");
            }
        }
        /// <summary>
        /// Окно информации
        /// </summary>
        /// <param name="path"></param>
        static void InformationWindow(string path)
        {
            if(path == @"C:\\")
            {
                DriveInfo[] drive = DriveInfo.GetDrives();
                Frame("trans");
                Console.WriteLine(Properties.Settings.Default.Format, $"Имя диска: {drive[0].Name}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Тип диска: {drive[0].DriveType}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Имя файловой системы: {drive[0].DriveFormat}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Доступно памяти: {drive[0].AvailableFreeSpace}Byte");
                Console.WriteLine(Properties.Settings.Default.Format, $"Размер диска: {drive[0].TotalSize} Byte");
                Frame("down");
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(path);
                Frame("trans");
                Console.WriteLine(Properties.Settings.Default.Format, "Information:");
                Console.WriteLine(Properties.Settings.Default.Format, $"Имя: {di.Name}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Путь: {di.FullName}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Время создания: {di.CreationTime}");
                Console.WriteLine(Properties.Settings.Default.Format, $"Последнее изменение: {di.LastWriteTime}");
                Frame("down");
            }
        }
        /// <summary>
        /// Распознование сложных команд
        /// </summary>
        /// <param name="uc"></param>
        static void Recognition(string uc)
        {
            string[] arr = uc.Split(' ');
            //вывод информации о файле
            if(arr[0] == "info")
            {
                string[] obj = uc.Split('/');
                string path = $@"{Properties.Settings.Default.Path}{obj[1]}";
                PageDirectories();
                InformationWindow(path);
            }
            //удаление файла
            else if(arr[0] == "delete")
            {
                string[] obj = uc.Split('/');
                string file = $@"{Properties.Settings.Default.Path}{obj[1]}";
                if (File.Exists(file)) 
                {
                    File.Delete(file);
                    PageDirectories();
                    InformationWindow(Properties.Settings.Default.Path);
                }
                else
                {
                    CommandDontFound();
                }
            }
            //копирование файлов
            else if(arr[0] == "copy")
            {
                string[] obj = uc.Split('/');
                string s1 = $@"{Properties.Settings.Default.Path}{obj[1]}";
                string s2 = $@"{Properties.Settings.Default.Path}{obj[3]}";
                if (File.Exists(s1))
                {
                    File.Copy(s1, s2);
                    PageDirectories();
                    InformationWindow(Properties.Settings.Default.Path);
                }
                else
                {
                    CommandDontFound();
                }
            }
            else if(arr[0] == "cd") 
            {
                //выход из папки
                if (arr[1] == "..")
                {
                    //Проверка на попытку выйти на несуществующий уровень
                    if (Properties.Settings.Default.Path == @"C:\")
                    {
                        Console.Clear();
                        PageDirectories();
                        InformationWindow(Properties.Settings.Default.Path);
                        Frame("up");
                        Console.WriteLine(Properties.Settings.Default.Format, "Выход на уровень выше невозможен");
                        Frame("down");
                        File.AppendAllText(Properties.Settings.Default.PathFErr, "\nВыход из диска невозможен");
                    }
                    else
                    {
                        string[] arr1 = Properties.Settings.Default.Path.Split('\\');
                        Properties.Settings.Default.Page = 0;
                        Properties.Settings.Default.Path = "";

                        for (int i = 0; i < arr1.Length - 2; i++)
                        {
                            Properties.Settings.Default.Path = $"{Properties.Settings.Default.Path}{arr1[i]}\\";
                        }
                        Properties.Settings.Default.Save();

                        PageDirectories();
                        InformationWindow(Properties.Settings.Default.Path);
                    }
                }
                //вход в указанную папку
                else
                {
                    Properties.Settings.Default.Page = 0;
                    string path = Properties.Settings.Default.Path + arr[1] + $"\\";
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (di.Exists)
                    {
                        Properties.Settings.Default.Path = path;
                        PageDirectories();
                        InformationWindow(Properties.Settings.Default.Path);
                    } else
                    {
                        CommandDontFound();
                    }
                }            
            }
            else
            {
                CommandDontFound();
            }
        }
        /// <summary>
        /// Вывод файлов
        /// </summary>
        static void PrintFiles()
        {

            string[] files = Directory.GetFiles(Properties.Settings.Default.Path);

            Console.WriteLine(@"╠{0, -56}║", "══════╗");
            string sep = "╠";
            for (int i = 0; i < files.Length;)
            {
                try
                {
                    if (files.Length <= 1)
                        break;
                    string[] allname = files[i].Split('\\');

                    if (i == files.Length - 1)
                        sep = "╚";

                    Console.WriteLine(Properties.Settings.Default.Format, $"      {sep}{allname[allname.Length - 1]}");
                    i++;
                }
                catch
                { 
                    continue;
                    File.AppendAllText(Properties.Settings.Default.PathFErr, "\nФайл не доступен. Требуются права администратора");
                }
            }
            Frame("trans1");
        }
        /// <summary>
        /// Постраничный вывод папок
        /// </summary>
        static void PageDirectories()
        {
            //Проверка на попытку выйти на несуществующую страницу
            if (Properties.Settings.Default.Page < 0)
            {
                Properties.Settings.Default.Page = 0;
                Properties.Settings.Default.Save();
            }
            int skip = Properties.Settings.Default.Page * Properties.Settings.Default.PageSize + Properties.Settings.Default.PageSize;
            
            try {
                string[] Dirs = Directory.GetDirectories(Properties.Settings.Default.Path);
                Frame("up");
                for (int i = Properties.Settings.Default.Page * Properties.Settings.Default.PageSize; i < skip;)
                {
                    try
                    {
                        if (Dirs.Length <= i)
                            break;
                        string[] dirname = Dirs[i].Split('\\');
                        Console.WriteLine(Properties.Settings.Default.Format, $" {dirname[dirname.Length - 1]}");
                        
                        i++;
                    }
                    catch 
                    { 
                        continue;
                        File.AppendAllText(Properties.Settings.Default.PathFErr, "\nПапка не доступна. Требуются права администратора");
                    }
                }
                PrintFiles();
            }
            catch
            {
                Console.Clear();
                Properties.Settings.Default.Path = @"C:\";
                Properties.Settings.Default.Save();
                PageDirectories();
                InformationWindow(Properties.Settings.Default.Path);
                Frame("message");
            }

            
        }
        static void Main(string[] args)
        {
            Console.Title = "File Manager";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetWindowSize(59, 30);


            Properties.Settings.Default.Page = 0;
            PageDirectories();
            InformationWindow(Properties.Settings.Default.Path);

            while (true)
            {
                string UserCommand = Console.ReadLine();
                Console.Clear();
                //простые команды
                switch(UserCommand)
                {
                    //вызов списка команд
                    case "help":
                        Help();
                        break;
                    //следующая страница
                    case "next":
                        Properties.Settings.Default.Page++;
                        PageDirectories();
                        InformationWindow(Properties.Settings.Default.Path);
                        Properties.Settings.Default.Save();
                        break;
                    //предыдущая страница
                    case "back":
                        Properties.Settings.Default.Page--;
                        PageDirectories();
                        InformationWindow(Properties.Settings.Default.Path);
                        break;
                    default:
                        try { Recognition(UserCommand); }
                        catch { CommandDontFound(); }
                        break;
                }
            }
            
        }
        /// <summary>
        /// Список команд
        /// </summary>
        static void Help()
        {
            Console.Clear();
            Frame("up");
            Console.WriteLine(Properties.Settings.Default.Format, "Следующая страница");
            Console.WriteLine(Properties.Settings.Default.Format, "     next");
            Console.WriteLine(Properties.Settings.Default.Format, "Предыдущая страница");
            Console.WriteLine(Properties.Settings.Default.Format, "     back");
            Console.WriteLine(Properties.Settings.Default.Format, "Открыть папку");
            Console.WriteLine(Properties.Settings.Default.Format, "     cd имя_папки");
            Console.WriteLine(Properties.Settings.Default.Format, "Выйти из текущей папки");
            Console.WriteLine(Properties.Settings.Default.Format, "     cd ..");
            Console.WriteLine(Properties.Settings.Default.Format, "Копировать файл");
            Console.WriteLine(Properties.Settings.Default.Format, "*имя файла указывается с расширением");
            Console.WriteLine(Properties.Settings.Default.Format, "     copy /имя_файла/ /имя_копии/");
            Console.WriteLine(Properties.Settings.Default.Format, "Удаление файла");
            Console.WriteLine(Properties.Settings.Default.Format, "     delete /имя_файла/");
            Console.WriteLine(Properties.Settings.Default.Format, "Просмотр информации о файле");
            Console.WriteLine(Properties.Settings.Default.Format, "     info /имя_файла/");
            Frame("down");
        }
    }
}
