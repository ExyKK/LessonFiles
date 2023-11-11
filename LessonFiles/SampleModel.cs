﻿namespace LessonFiles
{
    public class SampleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public SampleModel() { }

        public SampleModel(int id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
