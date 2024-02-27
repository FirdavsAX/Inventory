using Lesson07.Data;
using Lesson07.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson07.Stores
{
    internal class CategoriesStore
    {
        private readonly InventoryDbContext _context;

        public CategoriesStore()
        {
            _context = new InventoryDbContext();
        }

        public async Task<List<Category>> GetCategoriesAsync() 
            => await _context.Categories.ToListAsync();
    }
}
