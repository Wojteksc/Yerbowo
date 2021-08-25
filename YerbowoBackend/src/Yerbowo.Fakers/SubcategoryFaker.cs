using Bogus;
using Yerbowo.Domain.Subcategories;
using Yerbowo.Domain.Extensions;

namespace Yerbowo.Fakers
{
	public class SubcategoryFaker : Faker<Subcategory>
	{
		public SubcategoryFaker()
		{
			Ignore(p => p.Id);
			RuleFor(p => p.Name, f => f.Lorem.Word());
			RuleFor(p => p.Slug, f => f.Lorem.Word().ToSlug());
			RuleFor(p => p.Description, f => f.Lorem.Paragraph());
		}
	}
}
