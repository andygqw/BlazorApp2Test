using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml;
using BlazorApp2Test.Models;

namespace BlazorApp2Test.Data
{
    public class MemoData
    {

        public async Task SaveMemo(MemoModel memo)
        {
            List<MemoModel> memos = new List<MemoModel>();

            // If the file exists, read its content
            if (File.Exists(Helper.MemoFilePath))
            {
                var existingData = File.ReadAllText(Helper.MemoFilePath);
                memos = JsonSerializer.Deserialize<List<MemoModel>>(existingData) ?? new List<MemoModel>();
            }

            MemoModel lastElement;

            if (memos.Count > 0)
            {
                lastElement = new MemoModel(memos[memos.Count - 1]);
            }
            else
            {
                lastElement = new MemoModel();
                lastElement.Id = 0;
            }

            memo.Id = lastElement.Id + 1;

            // Add the new memo to the list
            memos.Add(memo);

            // Serialize and save the updated list
            var serializedData = JsonSerializer.Serialize(memos);
            await File.WriteAllTextAsync(Helper.MemoFilePath, serializedData);
        }

        public async Task<List<MemoModel>> LoadMemos()
        {
            if (File.Exists(Helper.MemoFilePath))
            {
                var jsonString = await File.ReadAllTextAsync(Helper.MemoFilePath);
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return new List<MemoModel>();
                }
                var results = JsonSerializer.Deserialize<List<MemoModel>>(jsonString);
                if(results != null)
                {
                    return results;
                }
            }
            return new List<MemoModel>();
        }
    }
}
