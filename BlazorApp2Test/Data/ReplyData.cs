using System.Text.Json;
using System.Linq;
using BlazorApp2Test.Models;
using BlazorApp2Test.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2Test.Data
{
    public class ReplyData
    {
        private readonly UserDbContext _context;
        private readonly UserService _userService;

        public ReplyData(UserDbContext context, UserService s)
        {
            _context = context;
            _userService = s;
        }

        public async Task<List<ReplyModel>> GetReplies(int memoId)
        {
            if(_context.Replies != null)
            {
                var list = _context.Replies.Where(r => r.memoId == memoId).ToList();

                List<ReplyModel> result = new List<ReplyModel>();

                if (list != null)
                {
                    foreach (var rep in list)
                    {
                        ReplyModel newReply = new ReplyModel();

                        newReply.UpdateFromRawData(rep);

                        newReply.CreatedBy = await _userService.GetUsername(rep.userId);

                        if (rep.replyId != null)
                        {
                            newReply.RepliedBy = await GetReplyAuthorById(rep.replyId.Value);
                        }

                        result.Add(newReply);
                    }
                }

                return result;
            }
            else
            {
                throw new Exception("Something wrong with db Replies");
            }
        }

        public async Task AddReply(ReplyModel model)
        {
            if(!string.IsNullOrEmpty(model.Content))
            {
                Reply rep = new Reply();

                if (model.UserId == null)
                    rep.userId = _userService.GetUserId();
                else
                    rep.userId = model.UserId.Value;
                rep.memoId = model.MemoId;
                rep.content = model.Content;
                rep.replyId = model.ReplyId;
                rep.create_time = DateTime.Now;

                if(_context.Replies != null)
                {
                    _context.Replies.Add(rep);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("AddReply: Invalid Replay passed in");
            }
        }

        public async Task DeleteReply(int id)
        {
           if(_context.Replies != null)
            {
                var r = await _context.Replies.FindAsync(id);

                if (r != null)
                {
                    _context.Replies.Remove(r);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Can't find the reply to be deleted");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Replies");
            }
        }

        public async Task<string> GetReplyAuthorById(int id)
        {
            if(_context.Replies != null)
            {
                var r = await _context.Replies.FindAsync(id);

                if (r != null)
                {
                    return await _userService.GetUsername(r.userId);
                }
                else
                {
                    throw new Exception("ReplayData: can't find user with id " + id);
                }
            }
            else
            {
                throw new Exception("Something wrong with db Replies");
            }
        }
    }
}