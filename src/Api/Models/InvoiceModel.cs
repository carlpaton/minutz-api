using System.Collections.Generic;

namespace Api.Models
{
    public class InvoiceModel
    {
        public string Number { get; private set; }
        public CompanyModel Seller { get; private set; }
        public CompanyModel Buyer { get; private set; }
        public IEnumerable<ItemModel> Items { get; private set; }

        public static InvoiceModel Example()
        {
            return new InvoiceModel()
            {
                Number = "123",
                Seller = new CompanyModel()
                {
                    Name = "Next Step Webs, Inc.",
                    Road = "12345 Sunny Road",
                    Country = "Sunnyville, TX 12345"
                },
                Buyer = new CompanyModel()
                {
                    Name = "Acme Corp.",
                    Road = "16 Johnson Road",
                    Country = "Paris, France 8060"
                },
                Items = new List<ItemModel>()
                {
                    new ItemModel()
                    {
                        Name = "Website design",
                        Price = 300
                    },
                    new ItemModel()
                    {
                        Name = "Implementing specific components",
                        Price = 600
                    },
                    new ItemModel()
                    {
                        Name = "Maintenance and support",
                        Price = 150
                    }
                }
            };
        }
    }

    public class CompanyModel
    {
        public string Name { get; set; }
        public string Road { get; set; }
        public string Country { get; set; }
    }

    public class ItemModel
    {
        public string Name { get; set; }
        public long Price { get; set; }
    }
}