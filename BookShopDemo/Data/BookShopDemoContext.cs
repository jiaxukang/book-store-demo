using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShopDemo.Models;

namespace BookShopDemo.Data
{
    public class BookShopDemoContext : DbContext
    {
        public BookShopDemoContext (DbContextOptions<BookShopDemoContext> options)
            : base(options)
        {
        }

        public DbSet<BookShopDemo.Models.Book> Book { get; set; } = default!;

        public DbSet<BookShopDemo.Models.UsersAccounts> UsersAccounts { get; set; }

        public DbSet<BookShopDemo.Models.Orders> Orders { get; set; }

    }
}
