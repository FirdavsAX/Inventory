using Bogus;
using Lesson07.Models;

namespace Lesson07.TestDataCreator;

public class ProductTestDataCreator
{
    public static Faker<Product> CreateFaker()
    {
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, (f, u) => f.Commerce.ProductName())
            .RuleFor(p => p.Description, (f, u) => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, (f, u) => f.Random.Decimal(10_000, 1_000_000))
            .RuleFor(p => p.ExpireDate, (f, u) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1)));

        return productFaker;
    }
}
