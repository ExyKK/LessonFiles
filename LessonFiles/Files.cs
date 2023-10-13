using System.IO.Compression;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace LessonFiles
{
    internal class FileManipulator
    {
        protected readonly string FilePath;
        protected readonly string FileDir;

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
                XDocument xDocument = new();
                xDocument.Add(new XElement("models"));
                xDocument.Save(FilePath);
            }

            XmlDocument xDoc = new();
            xDoc.Load(FilePath);

            XmlElement? xRoot = xDoc.DocumentElement;

            XmlElement modelElem = xDoc.CreateElement("model");
            XmlAttribute idAttr = xDoc.CreateAttribute("id");
            XmlElement nameElem = xDoc.CreateElement("name");
            XmlElement descriptionElem = xDoc.CreateElement("description");

            XmlText idText = xDoc.CreateTextNode(input[0]);
            XmlText nameText = xDoc.CreateTextNode(input[1]);
            XmlText descriptionText = xDoc.CreateTextNode(input[2]);

            idAttr.AppendChild(idText);
            nameElem.AppendChild(nameText);
            descriptionElem.AppendChild(descriptionText);

            modelElem.Attributes.Append(idAttr);
            modelElem.AppendChild(nameElem);
            modelElem.AppendChild(descriptionElem);

            xRoot?.AppendChild(modelElem);
            xDoc.Save(FilePath);
        }

        public override void Read()
        {
            XmlDocument xDoc = new();
            xDoc.Load(FilePath);

            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode? attr = xnode.Attributes.GetNamedItem("id");
                    Console.WriteLine($"{attr?.Name}: {attr?.Value}");

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        Console.WriteLine($"{childnode.Name}: {childnode.InnerText}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    internal class Archiver : FileManipulator
    {
        private readonly string _compressedPath;
        private readonly string _fileName;

        public Archiver(string filePath) : base(filePath) 
        {
            _fileName = Path.GetFileNameWithoutExtension(FilePath);
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
                        Console.WriteLine($"Сжатие файла {FilePath} завершено\nИсходный размер: {sourceStream.Length}\nСжатый размер: {targetStream.Length}\n");
                    }
                }
            }
        }

        public override void Read() // Разархивация
        {
            using (FileStream sourceStream = new(_compressedPath, FileMode.OpenOrCreate))
            {
                string restoredPath = Path.Combine(FileDir, _fileName + ".txt");
                using (FileStream targetStream = File.Create(restoredPath))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine($"Восстановлен файл: {restoredPath}");
                    }
                }
            }
        }

        public override void Delete()
        {
            base.Delete();
            if (File.Exists(_compressedPath))
                File.Delete(_compressedPath);
        }
    }
}