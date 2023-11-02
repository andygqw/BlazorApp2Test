using System;
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
            if (File.Exists(Helper.MemoJSONset))
            {
                var existingData = File.ReadAllText(Helper.MemoJSONset);
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

            if (MemoList.Contains(memo))
            {
                MemoList.Remove(memo);

                var serializedData = JsonSerializer.Serialize(MemoList);
                await File.WriteAllTextAsync(Helper.MemoJSONset, serializedData);
            }
            else
            {
                throw new Exception("Delete Memo which doesn't exist");
            }
        }
    }
}
