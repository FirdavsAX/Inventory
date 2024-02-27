using Bogus;
using Lesson07.Data;
using Lesson07.Models;

namespace Lesson07.TestDataCreator;

public class DatabaseSeeder
{
    public static void SeedDatabase()
    {
        // product.Name;
        using var context = new InventoryDbContext();
        CreateCategories(context);
        CreateProducts(context);
        CreateSuppliers(context);
        CreateSupplies(context);
        CreateSupplyProducts(context);
        CreateCustomers(context);
        CreateSales(context);

        /*
        var supplies = context.Supplies.ToList();
        int g = 0;
        */
    }

    private static void CreateCategories(InventoryDbContext context)
    {
        if (context.Categories.Any()) return;

        var faker = new Faker();
        var categoryNames = faker.Commerce.Categories(150).Distinct();

        foreach (var categoryName in categoryNames)
        {
            context.Categories.Add(new Category { Name = categoryName });
        }

        context.SaveChanges();
    }

    private static void CreateProducts(InventoryDbContext context)
    {
        if (context.Products.Any()) return;
        
        var faker = new Faker();
        var productFaker = ProductTestDataCreator.CreateFaker();

        var categories = context.Categories.ToList();

        foreach (var category in categories)
        {
            var randomCount = faker.Random.Int(1, 50);

            for (int i = 0; i < randomCount; i++)
            {
                var product = productFaker.Generate();
                product.CategoryId = category.Id;

                context.Products.Add(product);
            }
        }

        context.SaveChanges();
    }

    private static void CreateSuppliers(InventoryDbContext context)
    {
        if (context.Suppliers.Any()) return;

        var faker = new Faker();

        for(int i = 0; i < 50; i++)
        {
            var supplier = new Supplier();
            supplier.FirstName = faker.Name.FirstName();
            supplier.LastName = faker.Name.LastName();
            supplier.Company = faker.Company.CompanyName();
            supplier.PhoneNumber = faker.Phone.PhoneNumber("+998#########");

            context.Suppliers.Add(supplier);
        }

        context.SaveChanges();
    }

    private static void CreateSupplies(InventoryDbContext context)
    {
        if (context.Supplies.Any()) return;

        var faker = new Faker();
        var suppliers = context.Suppliers.ToList();

        foreach(var supplier in suppliers)
        {
            var suppliesCount = faker.Random.Int(10, 50);

            for(int i = 0; i < suppliesCount; i++)
            {
                var totalDue = faker.Random.Decimal(100_000, 10_000_000);
                var totalPaid = faker.Random.Decimal(100_000, totalDue);
                var date = faker.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(2));

                var supply = new Supply
                {
                    Date = date,
                    TotalDue = totalDue,
                    TotalPaid = totalPaid,
                    SupplierId = supplier.Id
                };

                context.Supplies.Add(supply);
            }
        }

        context.SaveChanges();
    }

    private static void CreateSupplyProducts(InventoryDbContext context)
    {
        if(context.SupplyProducts.Any()) return;

        var faker = new Faker();
        var supplies = context.Supplies.ToList();
        var products = context.Products.ToArray();

        foreach(var supply in supplies)
        {
            var supplyItemsCount = faker.Random.Int(5, 25);

            for(int i = 0; i < supplyItemsCount; i++)
            {
                var randomProduct = faker.Random.ArrayElement(products);

                var supplyProduct = new SupplyProduct
                {
                    Quantity = faker.Random.Int(5, 100),
                    UnitPrice = faker.Random.Decimal(randomProduct.Price, randomProduct.Price + 100_000),
                    ProductId = randomProduct.Id,
                    SupplyId = supply.Id
                };

                context.SupplyProducts.Add(supplyProduct);
            }
        }

        context.SaveChanges();
    }

    private static void CreateCustomers(InventoryDbContext context)
    {
        if (context.Customers.Any()) return;

        var faker = CustomerFaker.Fake();

        for(int i = 0; i < 50; i++)
        {
            var customer = faker.Generate();
            context.Customers.Add(customer);
        }

        context.SaveChanges();
    }

    private static void CreateSales(InventoryDbContext context)
    {
        if (context.Sales.Any()) return;

        var customerIds = context.Customers
            .Select(x => x.Id)
            .ToArray();
        var faker = SaleFaker.GetFaker(customerIds);

        for(int i = 0; i < 1500; i++)
        {
            var fakeSale = faker.Generate();
            context.Sales.Add(fakeSale);
        }

        context.SaveChanges();
    }
}