using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management.StaticClass
{
    public static class StringHandler
    {
        public static string SpaceAndLowcaseGenerator(string str)
        {
            string returnString = str.ToLower();
            return String.Concat(returnString.Where(c => !Char.IsWhiteSpace(c)));
        }
        public static bool IsTextAllowed(string text)
        {
            return !StaticEvents.StaticEventHandler._regex.IsMatch(text);
        }
        public static bool IsNumberAndSymbolAllowed(string text)
        {
            return StaticEvents.StaticEventHandler._regex.IsMatch(text);
        }
        public static bool IsDateTime(string text)
        {
            DateTime date;
            return DateTime.TryParse(text, out date);
        }
        public static string GenerateIdCode(int idCode)
        { 
            string newIdCode = Convert.ToString(idCode);
            if(newIdCode.Length<6)
            {
                int rest = 6 - newIdCode.Length;
                for (int i = 6; i > rest; i--)
                {
                    newIdCode = newIdCode.Insert(0, "0");
                }
            }
            return newIdCode;
        }
    }
}
