namespace LessonFiles
{
    internal class Menu
    {
        private readonly string[] menuItems = new string[] { "Информация о дисках", "Работа с файлами", "Архивирование", "Выход" };
        private readonly string[] menuFileItems = new string[] { "Работа с .txt файлом", "Работа с .json файлом", "Работа с .xml файлом", "Назад" };
        private readonly string[] menuFileOptions = new string[] { "Записать", "Прочитать", "Удалить", "Назад" };
        private readonly string[] menuArchiveOptions = new string[] { "Сжать файл", "Разархивировать", "Удалить файл и архив", "Назад" };

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
                                                ClearAndWrite("Запись\n\nВведите полный путь до файла:"); // Запись
                                                Writing(new FileManipulator(Console.ReadLine()));                                                
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new FileManipulator(Console.ReadLine()));
                                                break;

                                            case ConsoleKey.D3:
                                                ClearAndWrite("Удаление\n\nВведите полный путь до файла:"); // Удаление
                                                Deletion(new FileManipulator(Console.ReadLine()));
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
                                                Writing(new JsonManipulator(Console.ReadLine()));
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new JsonManipulator(Console.ReadLine()));
                                                break;

                                            case ConsoleKey.D3:
                                                ClearAndWrite("Удаление\n\nВведите полный путь до файла:"); // Удаление
                                                Deletion(new JsonManipulator(Console.ReadLine()));                                           
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
                                                Writing(new XmlManipulator(Console.ReadLine()));
                                                break;

                                            case ConsoleKey.D2:
                                                ClearAndWrite("Чтение\n\nВведите полный путь до файла:"); // Чтение
                                                Reading(new XmlManipulator(Console.ReadLine()));
                                                break;

                                            case ConsoleKey.D3:
                                                ClearAndWrite("Удаление\n\nВведите полный путь до файла:"); // Удаление
                                                Deletion(new XmlManipulator(Console.ReadLine()));
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
                        ShowMenuItems(menuArchiveOptions);
                        bool exitArchive = false;
                        while (!exitArchive)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.D1:
                                    ClearAndWrite("Сжатие\n\nВведите полный путь до файла:"); // Сжатие
                                    Compression(new Archiver(Console.ReadLine()));
                                    break;

                                case ConsoleKey.D2:
                                    ClearAndWrite("Разархивация\n\nВведите полный путь до архива:"); // Разархивация
                                    Reading(new Archiver(Console.ReadLine()));
                                    break;

                                case ConsoleKey.D3:
                                    ClearAndWrite("Удаление\n\nВведите полный путь до файла:"); // Удаление
                                    Deletion(new Archiver(Console.ReadLine()));
                                    break;

                                case ConsoleKey.D4:
                                    Exit(ref exitArchive, menuArchiveOptions[3], menuItems);
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

        private void Compression(Archiver archiver)
        {            
            try
            {
                ClearAndWrite($"Архивирование\n\n{archiver.GetFilePath()}");
                archiver.Write();
                ShowMenuItems(menuArchiveOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                ShowMenuItems(menuArchiveOptions);
            }
        }

        private void Writing(FileManipulator manipulator)
        {
            try
            {
                ClearAndWrite($"Запись в файл\n\n{manipulator.GetFilePath()}");

                if (manipulator is JsonManipulator || manipulator is XmlManipulator)
                {
                    string[] input = new string[3];
                    Console.WriteLine($"Введите Id:");
                    input[0] = Console.ReadLine();
                    Console.WriteLine($"Введите Name:");
                    input[1] = Console.ReadLine();
                    Console.WriteLine($"Введите Description:");
                    input[2] = Console.ReadLine();

                    var structuredManipulator = (StructuredManipulator) manipulator;
                    structuredManipulator.Write(input);
                }
                else
                {
                    manipulator.Write(Console.ReadLine());
                }

                ClearAndWrite("Запись успешна");
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
                if (manipulator.GetType() == typeof(Archiver))
                    ShowMenuItems(menuArchiveOptions);
                else
                    ShowMenuItems(menuFileOptions);
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                if (manipulator.GetType() == typeof(Archiver))
                    ShowMenuItems(menuArchiveOptions);
                else
                    ShowMenuItems(menuFileOptions);
            }
        }

        private void Deletion(FileManipulator manipulator)
        {            
            try
            {
                manipulator.Delete();
                if (manipulator.GetType() == typeof(Archiver))
                {
                    ClearAndWrite("Файл и архив удалены");
                    ShowMenuItems(menuArchiveOptions);
                }
                else
                {
                    ClearAndWrite("Файл удалён");
                    ShowMenuItems(menuFileOptions);
                }
            }
            catch (Exception)
            {
                ClearAndWrite("Проверьте корректнось введённого пути");
                if (manipulator.GetType() == typeof(Archiver))
                    ShowMenuItems(menuArchiveOptions);
                else
                    ShowMenuItems(menuFileOptions);
            }
        }
    }
}
