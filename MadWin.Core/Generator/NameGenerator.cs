using System;

namespace Shop2City.Core.Generator
{
    public class NameGenerator
    {
        public static string GenerateUniqCode()
        {
            return Guid
                .NewGuid()
                .ToString()
                .Replace("-", "");
        }

        public static string GenerateDateTime()
        {
            return DateTime.Now.ToLongTimeString().Replace("-", "");
        }
    }
}
