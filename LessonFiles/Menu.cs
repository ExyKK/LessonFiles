using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LessonFiles
{
    internal class Menu
    {
        private readonly string[] menuItems = new string[] { "Информация о дисках", "Работа с файлами", "Архивирование", "Выход" };
        private readonly string[] menuFileItems = new string[] { "Работа с .txt файлом", "Работа с .json файлом", "Работа с .xml файлом", "Назад" };
        private readonly string[] menuFileOptions = new string[] { "Записать", "Прочитать", "Удалить", "Назад" };

        public void Run()
        {
            Console.WriteLine("Меню\n");
            ShowMenuItems(menuItems);

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        ClearAndWrite(menuItems[0]);
                        DrivesStats.Show();
                        ShowMenuItems(menuItems);
                        break;

                    case ConsoleKey.D2:
                        ClearAndWrite(menuItems[1]);
                        ShowMenuItems(menuFileItems);
                        bool exit = false;
                        while (!exit)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.D1:
                                    ClearAndWrite(menuFileItems[0]); // Работа с .txt файлом
                                    ShowMenuItems(menuFileOptions);
                                    bool exitTxt = false;
                                    while (!exitTxt)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.D1:
                                                WritingTxt(); // Запись                                                
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new FileManipulator(GetPath()));
                                                break;

                                            case ConsoleKey.D3:
                                                Deletion(); // Удаление
                                                break;

                                            case ConsoleKey.D4:
                                                Exit(ref exitTxt, menuFileOptions[3], menuFileItems);
                                                break;
                                        }
                                    }
                                    break;

                                case ConsoleKey.D2:
                                    ClearAndWrite(menuFileItems[1]); // Работа с .json файлом
                                    ShowMenuItems(menuFileOptions);
                                    bool exitJson = false;
                                    while (!exitJson) 
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.D1:
                                                ClearAndWrite("Запись\n\nВведите полный путь до файла:"); // Запись
                                                WritingStructured(new JsonManipulator(GetPath()));
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new JsonManipulator(GetPath()));
                                                break;

                                            case ConsoleKey.D3:
                                                Deletion(); // Удаление                                             
                                                break;

                                            case ConsoleKey.D4:
                                                Exit(ref exitJson, menuFileOptions[3], menuFileItems);
                                                break;
                                        }
                                    }
                                    break;

                                case ConsoleKey.D3:
                                    ClearAndWrite(menuFileItems[2]); // Работа с .xml файлом
                                    ShowMenuItems(menuFileOptions);
                                    bool exitXml = false;
                                    while (!exitXml)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.D1:
                                                ClearAndWrite("Запись\n\nВведите полный путь до файла:"); // Запись
                                                WritingStructured(new XmlManipulator(GetPath()));
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new XmlManipulator(GetPath()));
                                                break;

                                            case ConsoleKey.D3:
                                                Deletion(); // Удаление
                                                break;

                                            case ConsoleKey.D4:
                                                Exit(ref exitXml, menuFileOptions[3], menuFileItems);
                                                break;
                                        }
                                    }
                                    break;

                                case ConsoleKey.D4:
                                    Exit(ref exit, menuFileItems[3], menuItems);
                                    break;
                            }
                        }
                        break;

                    case ConsoleKey.D3:
                        ClearAndWrite(menuItems[2]); // Архивирование
                        ShowMenuItems(menuFileOptions);
                        bool exitArchive = false;
                        while (!exitArchive)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.D1:
                                    Compress(); // Сжатие
                                    break;

                                case ConsoleKey.D2:
                                    ClearAndWrite("Разархивация\n\nВведите полный путь до файла:"); // Разархивация
                                    Reading(new Archiver(GetPath()));
                                    break;

                                case ConsoleKey.D3:
                                    Deletion(); // Удаление
                                    break;

                                case ConsoleKey.D4:
                                    Exit(ref exitArchive, menuFileOptions[3], menuFileItems);
                                    break;
                            }
                        }
                        break;

                    case ConsoleKey.D4:
                        ClearAndWrite(menuItems[3]); // Выход                 
                        return;

                    default:
                        ClearAndWrite("Введите цифру для выбора");
                        ShowMenuItems(menuItems);
                        break;
                }
            }
        }

        private void ShowMenuItems(string[] menuItems)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]}");
            }
            Console.WriteLine();
        }

        private void ClearAndWrite(string value)
        {
            Console.Clear();
            Console.WriteLine(value + '\n');
        }

        private void Exit(ref bool exit, string value, string[] items)
        {
            ClearAndWrite(value);
            ShowMenuItems(items);
            exit = true;
        }

        private string GetPath()
        {
            string? path = Console.ReadLine();
            if (path == "")
                throw new Exception();
            return path;
        }

        private void WritingTxt()
        {
            ClearAndWrite("Запись\n\nВведите полный путь до файла:");                                           
            try
            {
                string path = GetPath();
                FileManipulator manipulator = new(path);
                ClearAndWrite($"Запись в файл\n\n{path}");
                manipulator.Write(Console.ReadLine());
                ClearAndWrite("Запись успешна");
                ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuFileOptions);
            }
        }

        private void WritingStructured(StructuredManipulator manipulator)
        {            
            try
            {
                ClearAndWrite($"Запись в файл\n\n{manipulator.GetFilePath()}");

                string[] input = new string[3];
                Console.WriteLine($"Введите Id:");
                input[0] = Console.ReadLine();
                Console.WriteLine($"Введите Name:");
                input[1] = Console.ReadLine();
                Console.WriteLine($"Введите Description:");
                input[2] = Console.ReadLine();

                manipulator.Write(input);
                ClearAndWrite("Запись успешна");
                ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuFileOptions);
            }
        }

        private void Compress()
        {
            ClearAndWrite("Сжатие\n\nВведите полный путь до файла:");
            try
            {
                string filePath = GetPath();
                Archiver archiver = new(filePath);

                ClearAndWrite($"Архивирование\n\n{filePath}");
                archiver.Write();
                ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuFileOptions);
            }
        }

        private void Reading(FileManipulator manipulator)
        {            
            try
            {
                Console.WriteLine();
                manipulator.Read();
                ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuFileOptions);
            }
        }

        private void Deletion()
        {
            ClearAndWrite("Удаление\n\nВведите полный путь до файла:");
            try
            {
                Archiver manipulator = new(GetPath());
                manipulator.Delete();
                ClearAndWrite("Файл удалён");
                ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuFileOptions);
            }
        }
    }
}
