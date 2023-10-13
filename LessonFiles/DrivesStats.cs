namespace LessonFiles
{
    internal class DrivesStats
    {
        public static void Show() 
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип диска: {drive.DriveType}");
                Console.WriteLine($"Тип файловой системы: {drive.DriveFormat}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize >> 30}GB");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace >> 30}GB");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
        }
    }
}
