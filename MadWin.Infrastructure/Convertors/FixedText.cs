namespace Shop2City.Core.Convertors
{
    public class FixedText
    {
        public static string FixedEmail(string email)
        {
            return email
                .Trim()
                .ToLower();
        }
    }
}
