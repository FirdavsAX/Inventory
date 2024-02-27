using GalaSoft.MvvmLight.Helpers;
using Lesson07.Data;
using Lesson07.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson07.Stores
{
    public  class SalesStore:IDisposable
    {
        private readonly InventoryDbContext database;
        public SalesStore()
        {
            database = new();
        }
        public async Task<List<Sale>> GetSales(int pageList,int currentPage,int? customerID = null,DateTime? saleDate=null)
        {
            List<Sale> sales = new();
            try
            {
                var query = database.Sales.
                    Include(x => x.Customer)
                    .Include(x => x.SaleProducts).AsQueryable();

                if(customerID is not null and > 0)
                {
                    query = query.Where(s => s.CustomerId == customerID);
                }
                if(saleDate is not null)
                {
                   query = query.Where(s => s.SaleDate <= saleDate);
                }

                sales = await query
                    .Skip((currentPage-1) * pageList)
                    .Take(pageList)
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }    
            return sales;
        }
        public async Task<List<Sale>> GetSales(int? customerID = null, DateTime? saleDate = null)
        {
            List<Sale> salesList = new List<Sale>();
            try
            {
                var query = database.Sales.Include(x => x.Customer).AsQueryable();

                if (customerID is not null and > 0)
                {
                    query = query.Where(s => s.CustomerId == customerID);
                }
                if (saleDate is not null)
                {
                    query = query.Where(s => s.SaleDate <= saleDate);
                }
                salesList = await query
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return salesList;
        }

        public async Task<int> GetCountSalesAsync()
        {
            int count = 0;
            try
            {
                count = await database.Sales.CountAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return count;
        }

        public async Task<int> GetCountSalesAsync(int? customerID = null, DateTime? saleDate = null)
        {
            int result = 0;
            try
            {
                var query = database.Sales.Include(x => x.Customer).AsQueryable();
                if (customerID is not null and > 0)
                {
                    query = query.Where(s => s.CustomerId == customerID);
                }
                if (saleDate is not null)
                {
                    query = query.Where(s => s.SaleDate <= saleDate);
                }
                result = await query
                    .CountAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }

        public async Task CreateSaleAsync(Sale sale)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(sale);

                if (EntityExists(sale.Id)) throw new InvalidOperationException();

                var createdSale = database.Sales.Add(sale);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<Sale> UpdateSale(Sale sale)
        {
            ArgumentNullException.ThrowIfNull(sale);

            if (!EntityExists(sale.Id)) throw new InvalidOperationException();

            var createdSale = database.Sales.Attach(sale);
            database.Entry(sale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            database.SaveChanges();

            return createdSale.Entity;
        }
        public void DeleteSale(Sale sale)
        {
            try
            {
                if (!EntityExists(sale.Id)) return;

                database.Sales.Remove(sale);
                database.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool EntityExists(int id) => database.Sales.Any(x => x.Id == id);
        public void Dispose() => database.Dispose();
    }
}
