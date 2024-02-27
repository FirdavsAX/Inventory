using Lesson07.Data;
using Lesson07.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson07.Stores
{
    public class SaleProductDataStore : IDisposable
    {
        private readonly InventoryDbContext database;
        public SaleProductDataStore()
        {
            database = new();
        }
        public async Task<List<SaleProduct>> GetSaleProducts(int pageList, int currentPage,int? saleId = null)
        {
            List<SaleProduct> saleProductsList= new List<SaleProduct>();
            try
            {
                var query = database.SalesProducts.Include(x => x.Sale).AsQueryable();

                if(saleId is not null)
                {
                    query.Where(s => s.SaleId == saleId);
                }


                saleProductsList = await query
                    .Skip((currentPage - 1) * pageList)
                    .Take(pageList)
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return saleProductsList;
        }
        public async Task<int> GetTotalSaleProductsCount(int saleId) => 
            await database.SalesProducts
            .Include(sp => sp.Sale)
            .Include(sp => sp.Product)
            .CountAsync(x => x.SaleId == saleId);

        public async Task CreateSaleProduct(List<SaleProduct>saleProducts)
        {
            ArgumentNullException.ThrowIfNull(saleProducts);
            
            await database.SalesProducts.AddRangeAsync(saleProducts);
            database.SaveChanges();
            
        }
        public async Task UpdateSaleProducts(List<SaleProduct> saleProducts,int saleId)
        {
            ArgumentNullException.ThrowIfNull(saleProducts);

            if (!EntityExists(saleId)) throw new InvalidOperationException();

            database.SalesProducts.AttachRange(saleProducts);
            database.SaveChanges();
        }
        public void DeleteSale(Sale sale)
        {
            try
            {
                if (!EntityExists(sale.Id)) return;

                database.Sales.Remove(sale);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool EntityExists(int id) => database.Sales.Any(x => x.Id == id);
        public void Dispose() => database.Dispose();
    }
}
