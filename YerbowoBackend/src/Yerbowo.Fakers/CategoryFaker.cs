using Bogus;
using Yerbowo.Domain.Categories;
using Yerbowo.Domain.Extensions;

namespace Yerbowo.Fakers
{
	public class CategoryFaker : Faker<Category>
	{
		public CategoryFaker()
		{
			Ignore(p => p.Id);
			RuleFor(p => p.Name, f => f.Lorem.Word());
			RuleFor(p => p.Slug, f => f.Lorem.Word().ToSlug());
			RuleFor(p => p.Description, f => f.Lorem.Paragraph());
		}
	}
}
