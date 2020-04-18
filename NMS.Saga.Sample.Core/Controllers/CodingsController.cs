using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NMS.Saga.Sample.Contracts;
using NMS.Saga.Sample.Contracts.Models;
using NMS.Saga.Sample.Core.Courier;
using NMS.Saga.Sample.Core.DataLayer;

namespace NMS.Saga.Sample.Core.Controllers
{
    public class CodingsController : Controller
    {
        private readonly CoreDbContext _context;
        private readonly RoutingSlipPublisher __pub;

        public CodingsController(CoreDbContext context , RoutingSlipPublisher pub)
        {
            _context = context;
            __pub = pub;
        }

        // GET: Codings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coding.ToListAsync());
        }

        // GET: Codings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coding = await _context.Coding
                .FirstOrDefaultAsync(m => m.IdCoding == id);
            if (coding == null)
            {
                return NotFound();
            }

            return View(coding);
        }

        // GET: Codings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Codings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCoding,Title")] Coding coding)
        {
            if (ModelState.IsValid)
            {
                await __pub.PublishInsertCoding(new Coding
                {
                    IdCoding = coding.IdCoding,
                    Title = coding.Title
                });
                return RedirectToAction(nameof(Index));
            }
            return View(coding);
        }

        // GET: Codings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coding = await _context.Coding.FindAsync(id);
            if (coding == null)
            {
                return NotFound();
            }
            return View(coding);
        }

        // POST: Codings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCoding,Title")] Coding coding)
        {
            if (id != coding.IdCoding)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodingExists(coding.IdCoding))
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
            return View(coding);
        }

        // GET: Codings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coding = await _context.Coding
                .FirstOrDefaultAsync(m => m.IdCoding == id);
            if (coding == null)
            {
                return NotFound();
            }

            return View(coding);
        }

        // POST: Codings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coding = await _context.Coding.FindAsync(id);
            _context.Coding.Remove(coding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodingExists(int id)
        {
            return _context.Coding.Any(e => e.IdCoding == id);
        }
    }
}
