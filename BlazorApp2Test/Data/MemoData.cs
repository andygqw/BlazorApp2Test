using System.Linq;
using BlazorApp2Test.Models;
using BlazorApp2Test.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2Test.Data
{
    public class MemoData
    {
        private readonly UserDbContext _context;
        private readonly UserService _userService;
        private readonly ReplyData _replyData;

        public MemoData(UserDbContext context, UserService s, ReplyData r)
        {
            _context = context;
            _userService = s;
            _replyData = r;
        }

        public async Task SaveMemo(MemoModel memo)
        {
            Memo newMemo = new Memo();

            if (_userService.GetAuth())
            {
                newMemo.userId = _userService.GetUserId();
                newMemo.createTime = DateTime.Now;
                if (memo.Name != null) newMemo.name = memo.Name;
                if (memo.Description != null) newMemo.description = memo.Description;
                if (memo.Image != null) newMemo.image = memo.Image;
            }
            else
            {
                throw new Exception("Memo: Failed to get user info");
            }

            if(_context.Memos != null)
            {
                _context.Memos.Add(newMemo);
            }
            

            await _context.SaveChangesAsync();
        }

        public async Task<List<MemoModel>> LoadMemos()
        {
            if (_userService.GetAuth())
            {
                List<MemoModel> list = new List<MemoModel>();

                if(_context.Memos != null)
                {
                    var memos = await _context.Memos
                                        .OrderByDescending(m => m.id)
                                        .ToListAsync();
                    if (memos != null)
                    {
                        foreach (var memo in memos)
                        {
                            MemoModel m = new MemoModel();

                            m.UpdateFromRawData(memo);

                            m.CreatedBy = await _userService.GetUsername(m.UserId);

                            m.Replies = await _replyData.GetReplies(m.Id);

                            list.Add(m);
                        }

                        return list;
                    }
                    else
                    {
                        throw new Exception("Can't retrieve memo");
                    }
                }
                else
                {
                    throw new Exception("Something wrong with db Users");
                }
            }
            else
            {
                throw new Exception("Not logged in");
            }
        }

        public async Task EditMemo(MemoModel memo)
        {
            if(_context.Memos != null)
            {
                var m = await _context.Memos.FindAsync(memo.Id);

                if (m != null)
                {
                    if (m.image != null)
                    {
                        DeleteImg(m.image);
                    }

                    m.name = memo.Name;
                    m.description = memo.Description;
                    m.image = memo.Image;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Can't find memo to be edited");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users");
            }
        }

        public async Task DeleteMemo(MemoModel memo)
        {
            if(memo.Image != null)
            {
                DeleteImg(memo.Image);
            }

            if(_context.Memos != null)
            {
                var m = await _context.Memos.FindAsync(memo.Id);

                if (m != null)
                {
                    if(_context.Replies != null)
                    {
                        var r = await _context.Replies.Where(i => i.memoId == m.id).ToListAsync();

                        if (r != null)
                        {
                            foreach (var d in r)
                            {
                                _context.Replies.Remove(d);
                            }
                            await _context.SaveChangesAsync();
                        }

                        _context.Memos.Remove(m);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Something wrong with db Replies");
                    }
                }
                else
                {
                    throw new Exception("No matching memo to be deleted");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users");
            }
        }

        public async Task SaveMemoImage(IBrowserFile selectedFile)
        {
            var fileExtension = Path.GetExtension(selectedFile.Name).ToLowerInvariant();

            if (!Helper.AllowedExtensions.Contains(fileExtension))
            {
                throw new Exception("File type is not supported.");
            }

            var path = Path.Combine(Helper.MemoImgset, selectedFile.Name);

            // Save the file to the server's local folder
            await using FileStream fs = new(path, FileMode.Create);
            await selectedFile.OpenReadStream(selectedFile.Size).CopyToAsync(fs);
            fs.Close();
        }

        private void DeleteImg(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                char separator = '\\';
                string[] parts = path.Split(separator);
                var name = parts[parts.Length - 1];

                var filePath = Helper.MemoImgset + '/' + name;

                File.Delete(filePath);
            }
        }
    }
}