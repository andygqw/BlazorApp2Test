﻿using BlazorApp2Test.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2Test.Data
{
    public class UserDbContext : DbContext
    {

        public DbSet<User>? Users { get; set; }
        public DbSet<Memo>? Memos { get; set; }
        public DbSet<Reply>? Replies { get; set; }
        public DbSet<Log>? Log { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}