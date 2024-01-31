using Microsoft.AspNetCore.Components;


namespace BlazorApp2Test
{
    public class Helper
    {
        public const string UploadFolderPath = "wwwroot/UploadFile";

        public const string DownloadPath = "UploadFile/@filename";


        public const string MemoImgset = "wwwroot/Memos";
        public const string MemoImgget = "/Memos";


        public const string MemoJSONset = "wwwroot/Memos/memos.JSON";

        public const string FileTransfer_DeleteAll = "INTRACOMMAND_DELETE_ALL_CODE_$51L#(D";


        // Limits:
        public const int TextMaxLength = 1400;
        public const int TextAreaMaxLength = 15000;
    }
}