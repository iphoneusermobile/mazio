namespace mazio.util
{
    class Rot13
    {
        // taken from http://dotnetperls.com/Content/ROT13.aspx
        public static string Rotate(string toRotate)
        {
            // Modifies the characters in-place using ToCharArray.
            // Far more efficient, and simpler too. Use this type of code.
            char[] charArray = toRotate.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                int thisInt = charArray[i];
                if (thisInt >= 65 && thisInt < 91)
                {
                    thisInt += 13;
                    if (thisInt >= 91)
                    {
                        thisInt -= 26;
                    }
                }
                else if (thisInt >= 97 && thisInt < 123)
                {
                    thisInt += 13;

                    if (thisInt >= 123)
                    {
                        thisInt -= 26;
                    }
                }
                charArray[i] = (char)thisInt;
            }
            return new string(charArray);
        }
    }
}
