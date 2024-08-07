namespace BlazorApp2Test
{
    internal class Helper
    {
        // File:
        internal const int R2_URL_EXPIRE_TIME = 30; //minutes
        internal const string R2_MEMO_FOLDER = "memoImg";


        // Limits:
        internal const int TextMaxLength = 1400;
        internal const int TextAreaMaxLength = 15000;
        internal const int FileMaxSize = 1024 * 1024 * 64;//- MB
        internal const int MaxFileUpload = 15;
        internal static readonly string[] AllowedImageContentTypes = new string[] { "image/png", "image/jpeg", "image/jpg" };


        // Error code:
    

        //Global Messages
        internal const string MSG_NOPERMISSION= "You do not have permission to access this page, try log in";
        internal const string MSG_NO_MEMO_PERMISSION = "Sorry, you don't have permission to view this, try other features";

        internal static int CountWords(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            return input.Split(new[] { ' ', '\n', '\r', '\t' },
                               StringSplitOptions.RemoveEmptyEntries).Length;
        }

        internal static int CountCharacters(string? input)
        {
            if (input == null)
            {
                return 0;
            }

            return input.Length;
        }
    }
}