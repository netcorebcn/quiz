using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quiz.Domain
{
    public abstract class Enumeration : IComparable
    {
        protected Enumeration(string name) => Name = name;

        public string Name { get; }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Name.Equals(otherValue.Name);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public int CompareTo(object other) => Name.CompareTo(((Enumeration)other).Name);
    }
}