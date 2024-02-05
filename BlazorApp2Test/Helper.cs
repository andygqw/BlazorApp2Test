namespace BlazorApp2Test
{
    public class Helper
    {
        public const string UploadFolderPath = "wwwroot/UploadFile";

        public const string DownloadPath = "UploadFile/@filename";


        public const string MemoImgset = "wwwroot/Memos";
        public const string MemoImgget = "/Memos";


        public const string MemoJSONset = "wwwroot/Memos/memos.JSON";


        // Limits:
        public const int TextMaxLength = 1400;
        public const int TextAreaMaxLength = 15000;
        public const int FileMaxSize = 1024 * 1024 * 30;//- 30MB
        public static string[] AllowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };


        // Error code:
        public const int ReturnGood = 40;
        public const int ReturnFailed = 50;

        public static int CountWords(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            return input.Split(new[] { ' ', '\n', '\r', '\t' },
                               StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static int CountCharacters(string? input)
        {
            if (input == null)
            {
                return 0;
            }

            return input.Length;
        }
    }
}