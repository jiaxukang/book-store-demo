using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShopDemo.Data;
using BookShopDemo.Models;
using Microsoft.Data.SqlClient;

namespace BookShopDemo.Controllers
{
    public class UsersAccountsController : Controller
    {
        private readonly BookShopDemoContext _context;

        public UsersAccountsController(BookShopDemoContext context)
        {
            _context = context;
        }

        // GET: UsersAccounts
        public async Task<IActionResult> Index()
        {
              return View(await _context.UsersAccounts.ToListAsync());
        }



        // GET: UsersAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        // POST: UsersAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,pass")] UsersAccounts usersAccounts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usersAccounts);
        }

        // GET: UsersAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsersAccounts == null)
            {
                return NotFound();
            }

            var usersAccounts = await _context.UsersAccounts.FindAsync(id);
            if (usersAccounts == null)
            {
                return NotFound();
            }
            return View(usersAccounts);
        }

        // POST: UsersAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,pass")] UsersAccounts usersAccounts)
        {
            if (id != usersAccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersAccountsExists(usersAccounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usersAccounts);
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string na, string pa)
        {
            SqlConnection conn1 = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\'\\Documents\\bookShop.mdf;Integrated Security=True;Connect Timeout=30");
            string sql;
            sql = "SELECT * FROM usersaccounts where name ='" + na + "' and  pass ='" + pa + "' ";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {
 
                string id = Convert.ToString((int)reader["Id"]);
                HttpContext.Session.SetString("Name", na);

                HttpContext.Session.SetString("userid", id);
                reader.Close();
                conn1.Close();
                
                return RedirectToAction("catalogue", "books");

                

            }
            else
            {
                ViewData["Message"] = "wrong user name password";
                return View();
            }
        }


        private bool UsersAccountsExists(int id)
        {
          return _context.UsersAccounts.Any(e => e.Id == id);
        }
    }
}
