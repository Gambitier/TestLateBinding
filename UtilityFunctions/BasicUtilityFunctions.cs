using System;

namespace UtilityFunctions
{
    [Information(Description ="This class contains basic utility functions")]
    public class BasicUtilityFunctions
    {
        [Information(Description = "This methos returns a welcome message")]
        public string WriteWelcomeMessage()
        {
            return "hello world";
        }

        [Information(Description = "This method concatenetes 3 strings in given order")]
        public string ConctThreeStrings(string string1, string string2, string string3)
        {
            return $"{string1} {string2} {string3}";
        }
    }
}
