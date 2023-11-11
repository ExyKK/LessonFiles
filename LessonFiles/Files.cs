using System;
using System.IO.Compression;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace LessonFiles
{
    internal class FileManipulator
    {
        protected readonly string FilePath;
        protected readonly string? FileDir;

        public FileManipulator(string path)
        {
            FilePath = path;
            FileDir = Path.GetDirectoryName(FilePath);
        }

        public void Write(string? input)
        {
            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);

            using (StreamWriter writer = new(FilePath))
            {
                writer.Write(input);
            }
        }      

        public virtual void Read()
        {
            using (StreamReader reader = new(FilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
            }
        }

        public virtual void Delete()
        {
            File.Delete(FilePath);
        }

        public string GetFilePath()
        {
            return FilePath;
        }
    }

    internal abstract class StructuredManipulator : FileManipulator
    {
        public StructuredManipulator(string path) : base(path) { }

        public abstract void Write(string[] input);
    }

    internal class JsonManipulator : StructuredManipulator
    {
        public JsonManipulator(string path) : base(path) { }

        public override void Write(string[] input)
        {
            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);

            SampleModel jsonObject = new(int.Parse(input[0]), input[1], input[2]);
            string json = JsonSerializer.Serialize(jsonObject);

            using (StreamWriter writer = new(FilePath))
            {
                writer.Write(json);
            }
        }

        public override void Read()
        {
            string? line = "";
            using (StreamReader reader = new(FilePath))
            {
                line = reader.ReadLine();
            }

            SampleModel? restoredObject = JsonSerializer.Deserialize<SampleModel>(line);
            Console.WriteLine($"Id: {restoredObject.Id}\nName: {restoredObject.Name}\nDescription: {restoredObject.Description}\n");
        }
    }

    internal class XmlManipulator : StructuredManipulator
    {
        public XmlManipulator(string path) : base(path) { }

        public override void Write(string[] input)
        {
            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }

            SampleModel xmlObject = new(int.Parse(input[0]), input[1], input[2]);
            XmlSerializer xmlSerializer = new(typeof(SampleModel));

            using (FileStream stream = new(FilePath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(stream, xmlObject);
            }
        }

        public override void Read()
        {
            SampleModel xmlObj = new();
            XmlDocument xDoc = new();
            xDoc.Load(FilePath);

            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name == "Id")
                        xmlObj.Id = int.Parse(xnode.InnerText);
                    if (xnode.Name == "Name")
                        xmlObj.Name = xnode.InnerText;
                    if (xnode.Name == "Description")
                        xmlObj.Description = xnode.InnerText;
                }

                Console.WriteLine($"Id: {xmlObj.Id}\nName: {xmlObj.Name}\nDescription: {xmlObj.Description}\n");
            }
        }
    }

    internal class Archiver : FileManipulator
    {
        private readonly string? _compressedPath;
        private readonly string? _fileName;

        public Archiver(string filePath) : base(filePath) 
        {
            if (!(FilePath == ""))
                _fileName = Path.GetFileNameWithoutExtension(FilePath);
            if (!(FileDir == null || FileDir == string.Empty))
                _compressedPath = Path.Combine(FileDir, _fileName + ".gz");
        }

        public void Write() // Архивирование
        {
            using (FileStream sourceStream = new(FilePath, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(_compressedPath))
                {
                    using (GZipStream compressionStream = new(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                        Console.WriteLine($"Сжатие файла завершено\nСозданный архив: {_compressedPath}\n");
                    }
                }
            }
        }

        public override void Read() // Разархивация
        {
            string restoredPath = Path.Combine(FileDir, _fileName + ".txt");

            using (FileStream sourceStream = new(_compressedPath, FileMode.OpenOrCreate))
            {                
                using (FileStream targetStream = File.Create(restoredPath))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);                      
                    }
                }
            }
            FileInfo source = new(restoredPath);
            FileInfo compressed = new(_compressedPath);
            Console.WriteLine($"Восстановлен файл: {restoredPath}\n" +
                              $"Исходный размер: {source.Length}\n" +
                              $"Сжатый размер: {compressed.Length}\n");
        }

        public override void Delete()
        {
            base.Delete();
            if (File.Exists(_compressedPath))
                File.Delete(_compressedPath);
        }
    }
}