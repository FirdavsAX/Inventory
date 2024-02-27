using Bogus;
using Lesson07.Models;

namespace Lesson07.TestDataCreator;

internal class CustomerFaker
{
    public static Faker<Customer> Fake()
    {
        var customerFaker = new Faker<Customer>()
            .RuleFor(x => x.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(x => x.LastName, (f, u) => f.Name.LastName())
            .RuleFor(x => x.PhoneNumber, (f, u) => f.Phone.PhoneNumber("+998#########"))
            .RuleFor(x => x.Address, (f, u) => f.Address.FullAddress());

        return customerFaker;
    }
}
