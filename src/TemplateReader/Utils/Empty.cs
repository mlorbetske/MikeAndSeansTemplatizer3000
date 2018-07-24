using System.Collections.Generic;

namespace TemplateReader.Utils
{
    public static class Empty<T>
    {
        public static class List
        {
            // ReSharper disable once StaticMemberInGenericType
            public static readonly IReadOnlyList<T> Value = new List<T>();
        }

        public static class Array
        {
            // ReSharper disable once StaticMemberInGenericType
            public static T[] Value = new T[0];
        }
    }
}
