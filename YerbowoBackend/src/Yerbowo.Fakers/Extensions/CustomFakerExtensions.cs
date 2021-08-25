using Bogus;
using System;

namespace Yerbowo.Fakers.Extensions
{
    public static class CustomFakerExtensions
    {
        public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
        {
            return faker.CustomInstantiator(f => Activator.CreateInstance(typeof(T), nonPublic: true) as T);
        }
    }
}
