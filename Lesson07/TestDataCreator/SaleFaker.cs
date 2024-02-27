using Bogus;
using Lesson07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson07.TestDataCreator
{
    internal class SaleFaker
    {
        public static Faker<Sale> GetFaker(int[] customerIds)
        {
            var faker = new Faker();
            var total = faker.Random.Decimal(10_000, 1_000_000);
            var totalPaid = faker.Random.Decimal(10_000, total);
            var totalDiscount = faker.Random.Decimal(10_000, total / 10);

            var saleFaker = new Faker<Sale>()
                .RuleFor(x => x.TotalDue, (f, u) => total)
                .RuleFor(x => x.TotalPaid, (f, u) => totalPaid)
                .RuleFor(x => x.TotalDiscount, (f, u) => totalDiscount)
                .RuleFor(x => x.SaleDate, (f, u) => f.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now))
                .RuleFor(x => x.CustomerId, (f, u) => f.Random.ArrayElement(customerIds));

            return saleFaker;
        }
    }
}
