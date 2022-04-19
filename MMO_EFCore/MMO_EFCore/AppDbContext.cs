using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMO_EFCore
{
    public class AppDbContext : DbContext
    {
        // DbSet<Item> -> EFCore한테 알려준다
        public DbSet<Item> Items { get; set; }
    }
}
