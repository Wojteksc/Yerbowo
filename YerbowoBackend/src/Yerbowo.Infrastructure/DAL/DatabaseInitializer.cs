namespace Yerbowo.Infrastructure.DAL;

[ExcludeFromCodeCoverage]
public class DatabaseInitializer(YerbowoContext db, IPasswordManager passwordManager)
{
    private Address _adminAddress1;
    private Address _adminAddress2;
    private Address _simpleUserAddress1;

    public async Task Seed()
    {
        await db.Database.EnsureCreatedAsync();

        if (db.Users.Any())
            return;

        await SeedUsers();

        await SeedCategories();

        await SeedSubcategories();

        await SeedYerbaMate();

        await SeedYerbaMateSets();

        await SeedAddresses();

        await SeedOrdersWithItems();

        await UpdateProductStates();
    }

    private async Task SeedAddresses()
    {
        _adminAddress1 = new Address(
            AddressIds.AdminHome,
            UserIds.Admin,
            "Dom",
            "Adam",
            "Nowak",
            "Belwedera",
            "31A",
            "23",
            "Warszawa",
            "00-001",
            "343242678",
            "test4324test@test.com"
            );

        _adminAddress2 = new Address(
           AddressIds.AdminWork,
           UserIds.Admin,
           "Praca",
           "Adam",
           "Nowak",
           "Narutowicza",
           "43",
           "",
           "Dąbrowa Górnicza",
           "41-200",
           "546455754",
           "testdsfsdfas45534test@test.com"
       );

        _simpleUserAddress1 = new Address(
            AddressIds.SimpleUserHome,
            UserIds.SimpleUser,
            "Dom",
            "Bogdan",
            "Kowalski",
            "Sokolska",
            "10",
            "",
            "Katowice",
            "40-300",
            "746886432",
            "tesdty4535235tgdvxc@test.com"
        );

        var addresses = new Address[] { _adminAddress1, _adminAddress2, _simpleUserAddress1 };

        foreach (var address in addresses)
        {
            await db.Addresses.AddAsync(address);
        }

        await db.SaveChangesAsync();
    }

    private async Task SeedUsers()
    {
        var admin = new User(UserIds.Admin, "Woytech", "Wojciechowski", "user@example.com", "admin");
        var simpleUser = new User(UserIds.SimpleUser, "Adam", "Nowak", "user2@example.com", "user");
        var testUser = new User(UserIds.IntegrationTestUser, "FirstName", "LastName", "yerbowoTestUser@IntegrationTestYerbowo.com", "admin");

        var securedPassword = passwordManager.Secure("Haslo123.");

        admin.SetVerificationDate(DateTime.UtcNow);
        admin.SetVerificationToken("Token");
        admin.SetPassword(securedPassword);

        simpleUser.SetVerificationDate(DateTime.UtcNow);
        simpleUser.SetVerificationToken("Token");
        simpleUser.SetPassword(securedPassword);

        testUser.SetVerificationDate(DateTime.UtcNow);
        testUser.SetVerificationToken("Token");
        testUser.SetPassword(securedPassword);

        await db.Users.AddAsync(admin);
        await db.Users.AddAsync(simpleUser);
        await db.Users.AddAsync(testUser);

        await db.SaveChangesAsync();
    }

    private async Task SeedCategories()
    {
        var Categories = new Category[]
        {
            new Category(CategoryIds.YerbaMate, "Yerba Mate", "Kategoria 'Yerba Mate'", "yerba-mate.png"),
            new Category(CategoryIds.Sets, "Zestawy", "Kategoria 'Zestawy'", "zestawy.png"),
            new Category(CategoryIds.Accessories, "Akcesoria", "Kategoria 'Akcesoria'", "akcesoria.png"),
            new Category(CategoryIds.Teas, "Herbaty", "Kategoria 'Herbaty'", "herbaty.png"),
            new Category(CategoryIds.Herbs, "Zioła", "Kategoria 'Zioła'", "yerba-mate.png"),
        };

        await db.AddRangeAsync(Categories);
        await db.SaveChangesAsync();
    }

    private async Task SeedSubcategories()
    {
        var subcategories = new Subcategory[]
        {
            new Subcategory(SubcategoryIds.Classic, CategoryIds.YerbaMate, "Klasyczne", "Klasyczne 'Yerba Mate'", "yerba-klasyczne.png"),
            new Subcategory(SubcategoryIds.Selected, CategoryIds.YerbaMate, "Wyselekcjonowane", "Wyselekcjonowane 'Yerba Mate'", "yerba-wyselekconowane.png"),
            new Subcategory(SubcategoryIds.Eco, CategoryIds.YerbaMate, "Ekologiczne", "Ekologiczne 'Yerba Mate'", "yerba-ekologiczne.png"),
            new Subcategory(SubcategoryIds.Strong, CategoryIds.YerbaMate, "Mocne", "Mocne 'Yerba Mate'", "yerba-mocne.png"),
            new Subcategory(SubcategoryIds.WithHerbs, CategoryIds.YerbaMate, "Z ziołami", "Z ziołami 'Yerba Mate'", "yerba-z-ziolami.png"),
            new Subcategory(SubcategoryIds.Green, CategoryIds.YerbaMate, "Green", "Green 'Yerba Mate'", "yerba-green.png"),
            new Subcategory(SubcategoryIds.Gentle, CategoryIds.YerbaMate, "Łagodne", "Łagodne 'Yerba Mate'", "yerba-lagodne.png"),
            new Subcategory(SubcategoryIds.Fruits, CategoryIds.YerbaMate, "Owocowe", "Owocowe 'Yerba Mate'", "yerba-owocowe.png"),
            new Subcategory(SubcategoryIds.Samples, CategoryIds.YerbaMate, "Próbki", "Próbki 'Yerba Mate'", "yerba-probki.png"),
            new Subcategory(SubcategoryIds.Gifts, CategoryIds.Sets, "Prezenty", "Prezenty dla każdego", "zestawy-prezenty.png"),
            new Subcategory(SubcategoryIds.StarterSets, CategoryIds.Sets, "Startowe", "Coś dla początkujących", "zestawy-startowe.png"),
            new Subcategory(SubcategoryIds.YerbaholicSets, CategoryIds.Sets, "Dla yerbaholików", "Prawdziwy Yerbaholik nie odmówi takim produktom", "zestawy-dla-yerbaholikow.png"),
            new Subcategory(SubcategoryIds.Bombillas, CategoryIds.Accessories, "Bombille", "Bombille", "akcesoria-bombille.png"),
            new Subcategory(SubcategoryIds.Vessels, CategoryIds.Accessories, "Naczynia", "Naczynia", "akcesoria-naczynia.png"),
            new Subcategory(SubcategoryIds.DecoratedVessels, CategoryIds.Accessories, "Naczynia zdobione", "Naczynia zdobione", "akcesoria-naczynia-zdobione.png"),
            new Subcategory(SubcategoryIds.OtherAccessories, CategoryIds.Accessories, "Inne", "Inne", "akcesoria-inne.png"),
            new Subcategory(SubcategoryIds.GreenTea, CategoryIds.Teas, "Zielone", "Herbaty zielone", "herbaty-zielone.png"),
            new Subcategory(SubcategoryIds.RedTea, CategoryIds.Teas, "Czerwone", "Herbaty czerwone", "herbaty-czerwone.png"),
            new Subcategory(SubcategoryIds.BlackTea, CategoryIds.Teas, "Czarne", "Herbaty czarne", "herbaty-czarne.png"),
            new Subcategory(SubcategoryIds.WhiteTea, CategoryIds.Teas, "Białe", "Herbaty białe", "herbaty-biale.png"),
            new Subcategory(SubcategoryIds.FruitTea, CategoryIds.Teas, "Owocowe", "Herbaty owocowe", "herbaty-owocowe.png"),
            new Subcategory(SubcategoryIds.MedicinalHerbs, CategoryIds.Herbs, "Lecznicze", "Zioła lecznicze", "ziola-lecznicze.png"),
        };

        await db.Subcategories.AddRangeAsync(subcategories);
        await db.SaveChangesAsync();
    }

    private async Task SeedYerbaMate()
    {
        await SeedYerbaMateClassic();

        await SeedYerbaMateSelected();

        await SeedYerbaMateEco();

        await SeedYerbaMateStrong();

        await SeedYerbaMateWithHerbs();

        await SeedYerbaMateGreen();

        await SeedYerbaMateGentle();

        await SeedYerbaMateFruits();
    }

    private async Task SeedYerbaMateClassic()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateClassic", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.Amanda1Kg, SubcategoryIds.Classic, "1234", "AMANDA 1KG", rm.GetString("Amanda"), 38.45m, 38.45m, 43, ProductState.None, "amanda.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(0) },
            new Product(ProductIds.CampesinoClasica500g,SubcategoryIds.Classic, "2345", "CAMPESINO CLASICA 500G", rm.GetString("CampesinoClasica"), 21.70m, 21.70m, 34, ProductState.None, "campesino-clasica.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(-1) },
            new Product(ProductIds.NoblezaGauchaMolienda500g,SubcategoryIds.Classic, "3456", "NOBLEZA GAUCHA MOLIENDA 500G", rm.GetString("NoblezaGauchaMolienda"), 25.70m, 25.70m, 100, ProductState.None, "nobleza-gaucha-molienda.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(-2) },
            new Product(ProductIds.BaraoDeCotegipePremium1Kg,SubcategoryIds.Classic, "5432", "BARAO DE COTEGIPE PREMIUM 1 KG", rm.GetString("BaraoDeCotegipePremium"), 59.45m, 63.45m, 44, ProductState.None, "barao-de-cotegipe-premium.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(0) },
            new Product(ProductIds.Rosamonte1Kg,SubcategoryIds.Classic, "4324", "ROSAMONTE 1KG", rm.GetString("Rosamonte"), 44.95m, 44.95m, 54, ProductState.None, "rosamonte.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(-4) },
            new Product(ProductIds.SaraRojaTradicionalSinPalo1Kg,SubcategoryIds.Classic, "8523", "SARA ROJA TRADICIONAL SIN PALO 1KG", rm.GetString("SaraRojaTradicionalSinPalo"), 47.24m, 47.24m, 90, ProductState.None, "sara-roja-tradicional-sin-palo.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(-3) },
            new Product(ProductIds.PiporeTerere500g,SubcategoryIds.Classic, "5347", "PIPORE TERERE 500G", rm.GetString("PiporeTerere"), 23.95m, 23.95m, 10, ProductState.None, "pipore-terere.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(-4) },
            new Product(ProductIds.AguantadoraTerere500g,SubcategoryIds.Classic, "0987", "AGUANTADORA TERERE 500G", rm.GetString("AguantadoraTerere"), 24.70m, 24.70m, 20, ProductState.None, "aguantadora-terere.jpg") { CreatedAt = DateTime.UtcNow.AddMonths(0) }
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync();
    }

    private async Task SeedYerbaMateSelected()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateSelected", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.PajaritoSeleccionEspecial500g,SubcategoryIds.Selected,"s331","PAJARITO SELECCION ESPECIAL 500G", rm.GetString("PajaritoSeleccionEspecial"), 20.95m, 20.95m, 40, ProductState.None, "pajarito-seleccion-especial.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-5)},
            new Product(ProductIds.KrausGauchoSinPalo500g,SubcategoryIds.Selected,"s432","KRAUS GAUCHO SIN PALO 500G", rm.GetString("KrausGauchoSinPalo"),26.45m,26.45m, 50, ProductState.None,"kraus-gaucho-sin-palo.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-1)},
            new Product(ProductIds.AguantadoraSeleccionEspecial500g,SubcategoryIds.Selected,"s867","AGUANTADORA SELECCION ESPECIAL 500G", rm.GetString("AguantadoraSeleccionEspecial"),28.70m,28.70m, 90, ProductState.None,"aguantadora-seleccion-especial.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-4)},
            new Product(ProductIds.PiporeEspecial500g,SubcategoryIds.Selected,"s690","PIPORE ESPECIAL 500G", rm.GetString("PiporeEspecial"),25.45m,25.45m, 31, ProductState.None,"pipore-especial.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-8)},
            new Product(ProductIds.LaMercedCampoMonte500g,SubcategoryIds.Selected,"s321","LA MERCED CAMPO&MONTE 500G", rm.GetString("LaMercedCampoMonte"),28.70m,31.45m, 25, ProductState.None,"la-merced-campo-monte.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-4)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync();
    }

    private async Task SeedYerbaMateEco()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateEco", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.UnionSuave500g,SubcategoryIds.Eco,"e211","UNION SUAVE 500G", rm.GetString("UnionSuave"), 21.70m, 21.70m, 90, ProductState.None, "union-suave.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-1)},
            new Product(ProductIds.AmandaOrganica500g,SubcategoryIds.Eco,"e111","AMANDA ORGANICA 500G", rm.GetString("AmandaOrganica"), 31.96m, 31.96m, 100, ProductState.None, "amanda-organica.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-3)}
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateStrong()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateStrong", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.ElPajaroDespaladaBio350g,SubcategoryIds.Strong,"s211","EL PAJARO DESPALADA BIO 350G", rm.GetString("ElPajaroDespaladaBio"), 20.70m, 23.70m, 43, ProductState.None, "el-pajaro-despalada-bio.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(0)},
            new Product(ProductIds.TaraguiVitalityDespalada500g,SubcategoryIds.Strong,"s111","TARAGUI VITALITY DESPALADA 500G", rm.GetString("TaraguiVitalityDespalada"), 22.70m, 22.70m, 4, ProductState.None, "taragui-vitality-despalada.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-1)}
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateWithHerbs()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateWithHerbs", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.PiporeListoMentaLimon500g,SubcategoryIds.WithHerbs,"z211","PIPORE LISTO MENTA LIMON 500G", rm.GetString("PiporeListoMentaLimon"), 23.95m, 25.95m, 43, ProductState.None, "pipore-listo-menta-limon.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-3)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateGreen()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateGreen", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.MateGreenGuarana250g,SubcategoryIds.Green,"g211","MATE GREEN GUARANA 250G", rm.GetString("MateGreenGuarana"), 17.70m, 17.70m, 3, ProductState.None, "mate-green-guarana.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-5)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateGentle()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateGentle", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.LaTranquera500g,SubcategoryIds.Gentle,"ge211","LA TRANQUERA 500G", rm.GetString("LaTranquera"), 37.30m, 37.30m, 42, ProductState.None, "la-tranquera.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-3)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateFruits()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateFruits", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.CbseSiluetaNaranja500g,SubcategoryIds.Fruits,"f211","CBSE SILUETA NARANJA POMARAŃCZOWA 500G", rm.GetString("CbseSiluetaNaranjaOrange"), 27.95m, 27.95m, 3, ProductState.None, "cbse-silueta-naranja-orange.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-3)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedYerbaMateSets()
    {
        var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateSets", Assembly.GetExecutingAssembly());

        var products = new Product[]
        {
            new Product(ProductIds.AmandaRojaSet,SubcategoryIds.YerbaholicSets,"f211","ZESTAW YERBA MATE AMANDA ROJA", rm.GetString("AmandaRojaWithCeramicMateCupAndBombilla"), 81.48m, 81.48m, 2, ProductState.None, "amanda-roja-with-ceramic-mate-cup-and-bombilla.jpg") {CreatedAt = DateTime.UtcNow.AddMonths(-3)},
        };

        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task SeedOrdersWithItems()
    {
        var OrderItems1 = new List<OrderItem>()
        {
            new OrderItem(OrderItemIds.OrderItem1, productId: ProductIds.BaraoDeCotegipePremium1Kg, quantity: 4, price: 36.45m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem2, productId: ProductIds.SaraRojaTradicionalSinPalo1Kg, quantity: 1, price: 45m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem3, productId: ProductIds.NoblezaGauchaMolienda500g, quantity: 2, price: 25.70m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem4, productId: ProductIds.Rosamonte1Kg, quantity: 1, price: 43m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem5, productId: ProductIds.UnionSuave500g, quantity: 3, price: 32m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
        };

        var OrderItems2 = new List<OrderItem>()
        {
            new OrderItem(OrderItemIds.OrderItem6, productId: ProductIds.PajaritoSeleccionEspecial500g, quantity: 2, price: 20.95m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItem(OrderItemIds.OrderItem7, productId:  ProductIds.NoblezaGauchaMolienda500g, quantity: 3, price: 25.70m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItem(OrderItemIds.OrderItem8, productId:  ProductIds.SaraRojaTradicionalSinPalo1Kg, quantity: 4, price: 45m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItem(OrderItemIds.OrderItem9, productId:  ProductIds.KrausGauchoSinPalo500g, quantity: 5, price: 150m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItem(OrderItemIds.OrderItem10, productId:  ProductIds.PiporeEspecial500g, quantity: 1, price: 25.45m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItem(OrderItemIds.OrderItem11, productId:  ProductIds.AmandaOrganica500g, quantity: 1, price: 31.96m) { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        var OrderItems3 = new List<OrderItem>()
        {
            new OrderItem(OrderItemIds.OrderItem12, productId: ProductIds.AguantadoraTerere500g, quantity: 1, price: 24.70m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem13, productId: ProductIds.CampesinoClasica500g, quantity: 2, price: 20.30m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem14, productId: ProductIds.PiporeTerere500g, quantity: 1, price: 23.90m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
        };

        var OrderItems4 = new List<OrderItem>()
        {
            new OrderItem(OrderItemIds.OrderItem15, productId: ProductIds.ElPajaroDespaladaBio350g, quantity: 1, price: 24.70m) { CreatedAt = DateTime.UtcNow.AddMonths(-3), UpdatedAt = DateTime.UtcNow.AddMonths(-3) },
            new OrderItem(OrderItemIds.OrderItem16, productId: ProductIds.AmandaOrganica500g, quantity: 2, price: 20.30m) { CreatedAt = DateTime.UtcNow.AddMonths(-3), UpdatedAt = DateTime.UtcNow.AddMonths(-3) },
            new OrderItem(OrderItemIds.OrderItem17, productId: ProductIds.PiporeTerere500g, quantity: 1, price: 23.95m) { CreatedAt = DateTime.UtcNow.AddMonths(-3), UpdatedAt = DateTime.UtcNow.AddMonths(-3) },
        };

        var OrderItems5 = new List<OrderItem>()
        {
            new OrderItem(OrderItemIds.OrderItem18, productId: ProductIds.PiporeEspecial500g, quantity: 1, price: 22.70m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem19, productId: ProductIds.ElPajaroDespaladaBio350g, quantity: 2, price: 21.30m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
            new OrderItem(OrderItemIds.OrderItem20, productId: ProductIds.PiporeTerere500g, quantity: 1, price: 26.95m) { CreatedAt = DateTime.UtcNow.AddMonths(-2), UpdatedAt = DateTime.UtcNow.AddMonths(-2) },
        };

        var orders = new Order[]
        {
            new Order(
                OrderIds.Order1,
                UserIds.Admin,
                db.Addresses.First(x => x.User.Id == UserIds.Admin).Id,
                OrderStatus.Delivered,
                OrderItems1.Sum(x => x.Quantity * x.Price),
                "Proszę o szybką realizację zamówienia",
                OrderItems1
            ),
            new Order(
                OrderIds.Order2,
                UserIds.SimpleUser,
                _simpleUserAddress1.Id,
                OrderStatus.New,
                OrderItems2.Sum(x => x.Quantity * x.Price),
                "Proszę ładnie zapakować",
                OrderItems2
            ),
            new Order(
                OrderIds.Order3,
                UserIds.Admin,
                _adminAddress1.Id,
                OrderStatus.Completed,
                OrderItems3.Sum(x => x.Quantity * x.Price),
                "",
                OrderItems3
            ),
            new Order(
                OrderIds.Order4,
                UserIds.Admin,
                _adminAddress2.Id,
                OrderStatus.Completed,
                OrderItems4.Sum(x => x.Quantity * x.Price),
                "",
                OrderItems4
            ),
            new Order(
                OrderIds.Order5,
                UserIds.Admin,
                _adminAddress2.Id,
                OrderStatus.Completed,
                OrderItems5.Sum(x => x.Quantity * x.Price),
                "",
                OrderItems5
            )
        };

        foreach (var order in orders)
        {
            await db.Orders.AddAsync(order);
        }

        await db.SaveChangesAsync(isCurrentDate: false);
    }

    private async Task UpdateProductStates()
    {
        IEnumerable<Product> bestsellersProducts = await GetBestsellersProducts();
        await UpdateProducts(bestsellersProducts, ProductState.Bestseller);

        IEnumerable<Product> newsProduct = await GetNewsProducts();
        await UpdateProducts(newsProduct, ProductState.New);

        IEnumerable<Product> promotionProducts = await GetPromotionsProducts();
        await UpdateProducts(promotionProducts, ProductState.Promotion);
    }

    private async Task<IEnumerable<Product>> GetBestsellersProducts()
    {
        var productIds = await (from p in db.Products
                          join oi in db.OrderItems on p.Id equals oi.ProductId
                          select new { oi.ProductId, oi.Quantity } into s
                          group s by new { s.ProductId, s.Quantity } into g
                          orderby g.Sum(x => x.Quantity) descending
                          select g.Key.ProductId
                            ).Take(8).ToListAsync();

        var products = db.Products.Where(p => productIds.Contains(p.Id));

        return products;
    }

    private async Task<IEnumerable<Product>> GetNewsProducts()
    {
        return await db.Products.OrderByDescending(x => x.Id).Take(4).ToListAsync();
    }

    private async Task<IEnumerable<Product>> GetPromotionsProducts()
    {
        return await db.Products.Where(p => p.Price != p.OldPrice).ToListAsync();
    }

    private async Task UpdateProducts(IEnumerable<Product> products, ProductState state)
    {
        foreach (var product in products)
        {
            product.SetState(state);
        }

        db.Products.UpdateRange(products);

        await db.SaveChangesAsync();
    }
}