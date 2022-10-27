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
    public class OrdersController : Controller
    {
        private readonly BookShopDemoContext _context;

        public OrdersController(BookShopDemoContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }



        public async Task<IActionResult> Create(int? id)
        {
            var book = await _context.Book.FindAsync(id);

            return View(book);
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, string bookCode, int bookquantity)
        {
            if(bookquantity == 0)
            {
                return RedirectToAction("catalogue", "books");
            }
            Orders order = new Orders();
            order.bookId = bookId;
            order.userId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            order.bookCode = bookCode;

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\'\\Documents\\bookShop.mdf;Integrated Security=True;Connect Timeout=30");
            string sql;
            sql = "UPDATE book  SET bookquantity  = bookquantity   - 1  where (id ='" + order.bookId + "' )";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();


            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(myorders));
      
        }
        public async Task<IActionResult> myorders()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var orItems = await _context.Orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);

        }



        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,int bookId)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'BookShopDemoContext.Orders'  is null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
                SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\'\\Documents\\bookShop.mdf;Integrated Security=True;Connect Timeout=30");
                string sql;
                sql = "UPDATE book  SET bookquantity  = bookquantity   + 1  where (id ='" + bookId + "' )";
                SqlCommand comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(myorders));
        }

        private bool OrdersExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
