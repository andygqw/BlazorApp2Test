using System;
using System.IO;
using System.Text.Json;
using BlazorApp2Test.Models;

namespace BlazorApp2Test.Data
{
    public class MemoData
    {
        public async Task SaveMemo(MemoModel memo)
        {
            var memos = await LoadMemos();

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
            await File.WriteAllTextAsync(Helper.MemoJSONset, serializedData);

        }

        public async Task<List<MemoModel>> LoadMemos()
        {
            if (File.Exists(Helper.MemoJSONset))
            {
                var jsonString = await File.ReadAllTextAsync(Helper.MemoJSONset);
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

        public async Task DeleteMemo(MemoModel memo)
        {
            var MemoList = await LoadMemos();

            // Dunno why List.Contains() and .Remove() doesnt work
            if (MemoList.Any(m => m.Id == memo.Id))
            {
                int index = MemoList.FindIndex(m => m.Id == memo.Id);

                MemoList.RemoveAt(index);

                if(MemoList.Any(m => m.Id == memo.Id))
                {
                    throw new Exception("Remove failed");
                }

                var serializedData = JsonSerializer.Serialize(MemoList);
                await File.WriteAllTextAsync(Helper.MemoJSONset, serializedData);

                if (memo.Image != null)
                {
                    char separator = '\\';
                    string[] parts = memo.Image.Split(separator);
                    var name = parts[parts.Length - 1];

                    var filePath = Helper.MemoImgset + '/' + name;

                    File.Delete(filePath);
                }
            }
            else
            {
                throw new Exception("Delete Memo which doesn't exist");
            }
        }
    }
}
