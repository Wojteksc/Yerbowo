using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using Yerbowo.Domain.Addresses;
using Yerbowo.Domain.Categories;
using Yerbowo.Domain.Orders;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.Subcategories;
using Yerbowo.Domain.Users;

namespace Yerbowo.Infrastructure.Context
{
	public class YerbowoContextSeed
	{
		private readonly YerbowoContext _db;

		private Address _adminAddress1;
		private Address _adminAddress2;
		private Address _simpleUserAddress1;

		public YerbowoContextSeed(YerbowoContext db)
		{
			_db = db;
		}

		public void Seed()
		{
			_db.Database.EnsureCreated();

			if (_db.Users.Any())
				return;

			SeedUsers();

			SeedCategories();

			SeedSubcategories();

			SeedYerbaMate();

			SeedYerbaMateSets();

			SeedAddresses();

			SeedOrdersWithItems();

			UpdateProductStates();
		}

		private void SeedAddresses()
		{
			_adminAddress1 = new Address(
				1,
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
				1,
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
				2,
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
				_db.Addresses.Add(address);
			}

			_db.SaveChanges();
		}

		private void SeedUsers()
		{
			var admin = new User("Woytech", "Wojciechowski", "test@test.com", "", "admin", "", "", "Haslo123.");
			var simpleUser = new User("Adamn", "Nowak", "test2@test.com", "", "user", "", "", "Haslo123.");

			_db.Users.Add(admin);
			_db.Users.Add(simpleUser);

			_db.SaveChanges();
		}

		private void SeedCategories()
		{
			var Categories = new Category[]
			{
				new Category("Yerba Mate", "Kategoria 'Yerba Mate'", "yerba-mate.png"),
				new Category("Zestawy", "Kategoria 'Zestawy'", "zestawy.png"),
				new Category("Akcesoria", "Kategoria 'Akcesoria'", "akcesoria.png"),
				new Category("Herbaty", "Kategoria 'Herbaty'", "herbaty.png"),
				new Category("Zioła", "Kategoria 'Zioła'", "yerba-mate.png"),
			};

			foreach (Category c in Categories)
			{
				_db.Categories.Add(c);
				_db.SaveChanges();
			}
		}

		private void SeedSubcategories()
		{
			var subcategories = new Subcategory[]
			{
				new Subcategory(1,"Klasyczne","Klasyczne 'Yerba Mate'","yerba-klasyczne.png"),
				new Subcategory(1,"Wyselekcjonowane","Wyselekcjonowane 'Yerba Mate'","yerba-wyselekconowane.png"),
				new Subcategory(1,"Ekologiczne","Ekologiczne 'Yerba Mate'","yerba-ekologiczne.png"),
				new Subcategory(1,"Mocne","Mocne 'Yerba Mate'","yerba-mocne.png"),
				new Subcategory(1,"Z ziołami","Z ziołami 'Yerba Mate'","yerba-z-ziolami.png"),
				new Subcategory(1,"Green","Green 'Yerba Mate'","yerba-green.png"),
				new Subcategory(1,"Łagodne","Łagodne 'Yerba Mate'","yerba-lagodne.png"),
				new Subcategory(1,"Owocowe","Owocowe 'Yerba Mate'","yerba-owocowe.png"),
				new Subcategory(1,"Próbki","Próbki 'Yerba Mate'","yerba-probki.png"),
				new Subcategory(2,"Prezenty","Prezenty dla każdego","zestawy-prezenty.png"),
				new Subcategory(2,"Startowe","Coś dla początkujących","zestawy-startowe.png"),
				new Subcategory(2,"Dla yerbaholików","Prawdziwy Yerbaholik nie odmówi takim produktom","zestawy-dla-yerbaholikow.png"),
				new Subcategory(3,"Bombille","Bombille","akcesoria-bombille.png"),
				new Subcategory(3,"Naczynia","Naczynia","akcesoria-naczynia.png"),
				new Subcategory(3,"Naczynia zdobione","Naczynia zdobione","akcesoria-naczynia-zdobione.png"),
				new Subcategory(3,"Inne","Inne","akcesoria-inne.png"),
				new Subcategory(4,"Zielone","Herbaty zielone","herbaty-zielone.png"),
				new Subcategory(4,"Czerwone","Herbaty czerwone","herbaty-czerwone.png"),
				new Subcategory(4,"Czarne","Herbaty czarne","herbaty-czarne.png"),
				new Subcategory(4,"Białe","Herbaty białe","herbaty-biale.png"),
				new Subcategory(4,"Owocowe","Herbaty owocowe","herbaty-owocowe.png"),
				new Subcategory(5,"Lecznicze","Zioła lecznicze","ziola-lecznicze.png"),
			};

			foreach (Subcategory s in subcategories)
			{
				_db.Subcategories.Add(s);
				_db.SaveChanges();
			}
		}

		private void SeedYerbaMate()
		{
			SeedYerbaMateClassic();

			SeedYerbaMateSelected();

			SeedYerbaMateEco();

			SeedYerbaMateStrong();

			SeedYerbaMateWithHerbs();

			SeedYerbaMateGreen();

			SeedYerbaMateGentle();

			SeedYerbaMateFruits();
		}

		private void SeedYerbaMateClassic()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateClassic", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(1,"1234","AMANDA 1KG", rm.GetString("Amanda"), 38.45m, 38.45m, 43, ProductState.None,"amanda.jpg") {CreatedAt = DateTime.Now.AddMonths(0)},
				new Product(1,"2345","CAMPESINO CLASICA 500G", rm.GetString("CampesinoClasica"),21.70m,21.70m, 34, ProductState.None,"campesino-clasica.jpg" ) {CreatedAt = DateTime.Now.AddMonths(-1)},
				new Product(1,"3456","NOBLEZA GAUCHA MOLIENDA 500G",rm.GetString("NoblezaGauchaMolienda"),25.70m, 25.70m, 100, ProductState.None,"nobleza-gaucha-molienda.jpg") {CreatedAt = DateTime.Now.AddMonths(-2)},
				new Product(1,"5432","BARAO DE COTEGIPE PREMIUM 1 KG",rm.GetString("BaraoDeCotegipePremium"),59.45m, 63.45m, 44, ProductState.None, "barao-de-cotegipe-premium.jpg") {CreatedAt = DateTime.Now.AddMonths(0)},
				new Product(1,"4324","ROSAMONTE 1KG", rm.GetString("Rosamonte"),44.95m,44.95m,54, ProductState.None,"rosamonte.jpg") {CreatedAt = DateTime.Now.AddMonths(-4)},
				new Product(1,"8523","SARA ROJA TRADICIONAL SIN PALO 1KG", rm.GetString("SaraRojaTradicionalSinPalo"),47.24m,47.24m, 90, ProductState.None,"sara-roja-tradicional-sin-palo.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)},
				new Product(1,"5347","PIPORE TERERE 500G", rm.GetString("PiporeTerere"),23.95m,23.95m, 10, ProductState.None,"pipore-terere.jpg") {CreatedAt = DateTime.Now.AddMonths(-4)},
				new Product(1,"0987","AGUANTADORA TERERE 500G", rm.GetString("AguantadoraTerere"),24.70m,24.70m, 20, ProductState.None,"aguantadora-terere.jpg") {CreatedAt = DateTime.Now.AddMonths(0)}
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateSelected()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateSelected", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(2,"s331","PAJARITO SELECCION ESPECIAL 500G", rm.GetString("PajaritoSeleccionEspecial"), 20.95m, 20.95m, 40, ProductState.None, "pajarito-seleccion-especial.jpg") {CreatedAt = DateTime.Now.AddMonths(-5)},
				new Product(2,"s432","KRAUS GAUCHO SIN PALO 500G", rm.GetString("KrausGauchoSinPalo"),26.45m,26.45m, 50, ProductState.None,"kraus-gaucho-sin-palo.jpg") {CreatedAt = DateTime.Now.AddMonths(-1)},
				new Product(2,"s867","AGUANTADORA SELECCION ESPECIAL 500G", rm.GetString("AguantadoraSeleccionEspecial"),28.70m,28.70m, 90, ProductState.None,"aguantadora-seleccion-especial.jpg") {CreatedAt = DateTime.Now.AddMonths(-4)},
				new Product(2,"s690","PIPORE ESPECIAL 500G", rm.GetString("PiporeEspecial"),25.45m,25.45m, 31, ProductState.None,"pipore-especial.jpg") {CreatedAt = DateTime.Now.AddMonths(-8)},
				new Product(2,"s321","LA MERCED CAMPO&MONTE 500G", rm.GetString("LaMercedCampoMonte"),28.70m,31.45m, 25, ProductState.None,"la-merced-campo-monte.jpg") {CreatedAt = DateTime.Now.AddMonths(-4)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateEco()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateEco", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(3,"e211","UNION SUAVE 500G", rm.GetString("UnionSuave"), 21.70m, 21.70m, 90, ProductState.None, "union-suave.jpg") {CreatedAt = DateTime.Now.AddMonths(-1)},
				new Product(3,"e111","AMANDA ORGANICA 500G", rm.GetString("AmandaOrganica"), 31.96m, 31.96m, 100, ProductState.None, "amanda-organica.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)}
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateStrong()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateStrong", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(4,"s211","EL PAJARO DESPALADA BIO 350G", rm.GetString("ElPajaroDespaladaBio"), 20.70m, 23.70m, 43, ProductState.None, "el-pajaro-despalada-bio.jpg") {CreatedAt = DateTime.Now.AddMonths(0)},
				new Product(4,"s111","TARAGUI VITALITY DESPALADA 500G", rm.GetString("TaraguiVitalityDespalada"), 22.70m, 22.70m, 4, ProductState.None, "taragui-vitality-despalada.jpg") {CreatedAt = DateTime.Now.AddMonths(-1)}
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateWithHerbs()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateWithHerbs", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(5,"z211","PIPORE LISTO MENTA LIMON 500G", rm.GetString("PiporeListoMentaLimon"), 23.95m, 25.95m, 43, ProductState.None, "pipore-listo-menta-limon.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateGreen()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateGreen", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(6,"g211","MATE GREEN GUARANA 250G", rm.GetString("MateGreenGuarana"), 17.70m, 17.70m, 3, ProductState.None, "mate-green-guarana.jpg") {CreatedAt = DateTime.Now.AddMonths(-5)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateGentle()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateGentle", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(7,"ge211","LA TRANQUERA 500G", rm.GetString("LaTranquera"), 37.30m, 37.30m, 42, ProductState.None, "la-tranquera.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateFruits()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateFruits", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(8,"f211","CBSE SILUETA NARANJA POMARAŃCZOWA 500G", rm.GetString("CbseSiluetaNaranjaOrange"), 27.95m, 27.95m, 3, ProductState.None, "cbse-silueta-naranja-orange.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedYerbaMateSets()
		{
			var rm = new ResourceManager("Yerbowo.Infrastructure.Resources.YerbaMateSets", Assembly.GetExecutingAssembly());

			var products = new Product[]
			{
				new Product(11,"f211","ZESTAW YERBA MATE AMANDA ROJA", rm.GetString("AmandaRojaWithCeramicMateCupAndBombilla"), 81.48m, 81.48m, 2, ProductState.None, "amanda-roja-with-ceramic-mate-cup-and-bombilla.jpg") {CreatedAt = DateTime.Now.AddMonths(-3)},
			};

			foreach (Product p in products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges(isCurrentDate: false);
		}

		private void SeedOrdersWithItems()
		{
			var OrderItems1 = new List<OrderItem>()
			{
				new OrderItem(productId: 4, quantity: 4, price: 36.45m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 6, quantity: 1, price: 45m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 3, quantity: 2, price: 25.70m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 5, quantity: 1, price: 43m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 14, quantity: 3, price: 32m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
			};

			var OrderItems2 = new List<OrderItem>()
			{
				new OrderItem(productId: 9, quantity: 2, price: 20.95m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 3, quantity: 3, price: 25.70m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 6, quantity: 4, price: 45m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 10, quantity: 5, price: 150m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 10, quantity: 1, price: 26.45m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 12, quantity: 1, price: 25.45m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
				new OrderItem(productId: 15, quantity: 1, price: 31.96m) { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
			};

			var OrderItems3 = new List<OrderItem>()
			{
				new OrderItem(productId: 8, quantity: 1, price: 24.70m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 2, quantity: 2, price: 20.30m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 7, quantity: 1, price: 23.90m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
			};

			var OrderItems4 = new List<OrderItem>()
			{
				new OrderItem(productId: 16, quantity: 1, price: 24.70m) { CreatedAt = DateTime.Now.AddMonths(-3), UpdatedAt = DateTime.Now.AddMonths(-3) },
				new OrderItem(productId: 15, quantity: 2, price: 20.30m) { CreatedAt = DateTime.Now.AddMonths(-3), UpdatedAt = DateTime.Now.AddMonths(-3) },
				new OrderItem(productId: 7, quantity: 1, price: 23.95m) { CreatedAt = DateTime.Now.AddMonths(-3), UpdatedAt = DateTime.Now.AddMonths(-3) },
			};

			var OrderItems5 = new List<OrderItem>()
			{
				new OrderItem(productId: 12, quantity: 1, price: 22.70m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 16, quantity: 2, price: 21.30m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
				new OrderItem(productId: 7, quantity: 1, price: 26.95m) { CreatedAt = DateTime.Now.AddMonths(-2), UpdatedAt = DateTime.Now.AddMonths(-2) },
			};

			var orders = new Order[]
			{
				new Order(1,
				_db.Addresses.First(x => x.User.Id == 1).Id,
				OrderStatus.Delivered,
				OrderItems1.Sum(x => x.Quantity * x.Price),
				"Proszę o szybką realizację zamówienia",
				OrderItems1
				),
				new Order(2,
				_simpleUserAddress1.Id,
				OrderStatus.New,
				OrderItems2.Sum(x => x.Quantity * x.Price),
				"Proszę ładnie zapakować",
				OrderItems2
				),
				new Order(1,
				_adminAddress1.Id,
				OrderStatus.Completed,
				OrderItems3.Sum(x => x.Quantity * x.Price),
				"",
				OrderItems3
				),
				new Order(1,
				_adminAddress2.Id,
				OrderStatus.Completed,
				OrderItems4.Sum(x => x.Quantity * x.Price),
				"",
				OrderItems4
				),
				new Order(1,
				_adminAddress2.Id,
				OrderStatus.Completed,
				OrderItems5.Sum(x => x.Quantity * x.Price),
				"",
				OrderItems5
				)
			};

			foreach (var order in orders)
			{
				_db.Orders.Add(order);
			}

			_db.SaveChanges(isCurrentDate: false);
		}

		private void UpdateProductStates()
		{
			IEnumerable<Product> bestsellersProducts = GetBestsellersProducts();
			UpdateProducts(bestsellersProducts, ProductState.Bestseller);

			IEnumerable<Product> newsProduct = GetNewsProducts();
			UpdateProducts(newsProduct, ProductState.New);

			IEnumerable<Product> promotionProducts = GetPromotionsProducts();
			UpdateProducts(promotionProducts, ProductState.Promotion);
		}

		private IEnumerable<Product> GetBestsellersProducts()
		{
			var productIds = (from p in _db.Products
							  join oi in _db.OrderItems on p.Id equals oi.ProductId
							  select new { oi.ProductId, oi.Quantity } into s
							  group s by new { s.ProductId, s.Quantity } into g
							  orderby g.Sum(x => x.Quantity) descending
							  select g.Key.ProductId
								).Take(8).ToList();

			var products = _db.Products.Where(p => productIds.Contains(p.Id));

			return products;
		}

		private IEnumerable<Product> GetNewsProducts()
		{
			return _db.Products.OrderByDescending(x => x.Id).Take(4).ToList();
		}

		private IEnumerable<Product> GetPromotionsProducts()
		{
			return _db.Products.Where(p => p.Price != p.OldPrice).ToList();
		}

		private void UpdateProducts(IEnumerable<Product> products, ProductState state)
		{
			foreach (var product in products)
			{
				product.SetState(state);
			}

			_db.Products.UpdateRange(products);

			_db.SaveChanges();
		}
	}
}
