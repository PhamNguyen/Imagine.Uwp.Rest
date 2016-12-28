using System;

namespace JsonUWP.Attributes
{
    public class SignAttribute : Attribute
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsList { get; set; }


        public SignAttribute(string name)
        {
            Name = name;
        }
    }
}