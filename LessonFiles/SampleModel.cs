namespace LessonFiles
{
    internal class SampleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public SampleModel(int id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
