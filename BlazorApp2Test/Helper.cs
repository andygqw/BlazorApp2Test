﻿namespace BlazorApp2Test
{
    internal class Helper
    {
        // File:
        internal const string UploadFolderPath = "wwwroot/UploadFile";
        internal const string DownloadPath = "UploadFile/@filename";
        internal const string UploadFolder = "/files";
        internal const string WebConfigFile = "web.config";

        // Memo:
        internal const string MemoImgset = "wwwroot/Memos";
        internal const string MemoImgget = "/Memos";

        internal const string MemoJSONset = "wwwroot/Memos/memos.JSON";

        // Limits:
        internal const int TextMaxLength = 1400;
        internal const int TextAreaMaxLength = 15000;
        internal const int FileMaxSize = 1024 * 1024 * 64;//- MB
        internal const int MaxFileUpload = 25;
        internal static string[] AllowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };


        // Error code:
        internal const int ReturnGood = 40;
        internal const int ReturnFailed = 50;

        //Global Messages
        internal const string MSG_NOPERMISSION= "You do not have permission to access this page, try log in";

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