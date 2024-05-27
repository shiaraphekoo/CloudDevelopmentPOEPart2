using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudDevelopmentPOE.Data;
using CloudDevelopmentPOE.Models;

namespace CloudDevelopmentPOE.Controllers
{
    public class OrderController : Controller
    {
        private readonly KhumaloCraftEmporiumDBContext _context;

        public OrderController(KhumaloCraftEmporiumDBContext context)
        {
            _context = context;
        }

        public IActionResult Order()
        {
            return Redirect("AdminLogin");
        }


        //User Login View


        // GET: Order/AdminLogin
        public ActionResult AdminLogin()
        {
            return View();
        }

        // POST: Order/AdminLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Admin.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (admin != null)
                {
                    // Authentication successful
                    // You can store authentication token or session here if needed
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View();
                }
            }
            return View();
        }



        // GET: Order/UserLogin
        public ActionResult UserLogin()
        {
            return View();
        }

        // POST: Order/UserLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.User.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (admin != null)
                {
                    // Authentication successful
                    // You can store authentication token or session here if needed
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View();
                }
            }
            return View();
        }


        // GET: Order/Register
        public ActionResult UserRegister()
        {
            return View();
        }

        // POST: Order/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegister(User user)
        {
            if (ModelState.IsValid)
            {
                // Save user to the database
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! Please login to continue.";


                // Redirect to a confirmation page or login page
                return RedirectToAction("UserLogin", "Order");
            }
            return View(user);
        }






        // GET: Order
        public async Task<IActionResult> Index()
        {
            var khumaloCraftEmporiumDBContext = _context.Order.Include(o => o.Product).Include(o => o.User);
            return View(await khumaloCraftEmporiumDBContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "Address");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,UserID,ProductID,OrderDate,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", order.ProductID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "Address", order.UserID);
            return View(order);
        }


        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", order.ProductID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,UserID,ProductID,OrderDate,OrderStatus")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", order.ProductID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "Address", order.UserID);
            return View(order);
        }



        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }

    }
}
