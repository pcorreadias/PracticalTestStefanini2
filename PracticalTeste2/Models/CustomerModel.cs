using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalTeste2.Models
{
    public class CustomerModel
    {
        public class CustomerSearchModel
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public int? Gender { get; set; }
            public int? City { get; set; }
            public int? Region { get; set; }
            public DateTime? LastPurchaseFrom { get; set; }
            public DateTime? LastPurchaseTo { get; set; }
            public int? Classification { get; set; }
            public int? Seller { get; set; }
        }

        //public class Gender
        //{
        //    public int? Id { get; set; }
        //    public string Name { get; set; }
        //}

        //public class Gender
        //{
        //    public int? Id { get; set; }
        //    public string Name { get; set; }
        //}

        private DatabaseEntities Context;
        public CustomerModel()
        {
            Context = new DatabaseEntities();
        }

        public IQueryable<Customer> GetCustomers(CustomerSearchModel searchModel)
        {
            var result = Context.Customer.AsQueryable();

            if (searchModel != null)
            {
                if (searchModel.Id.HasValue)
                    result = result.Where(x => x.Id == searchModel.Id);
                if (!string.IsNullOrEmpty(searchModel.Name))
                    result = result.Where(x => x.Name.Contains(searchModel.Name));
                if (searchModel.Gender.HasValue)
                    result = result.Where(x => x.GenderId == searchModel.Gender);
                if (searchModel.City.HasValue)
                    result = result.Where(x => x.CityId == searchModel.City);
                if (searchModel.Region.HasValue)
                    result = result.Where(x => x.RegionId == searchModel.Region);
                if (searchModel.LastPurchaseFrom.HasValue && searchModel.LastPurchaseTo.HasValue)
                    result = result.Where(x => x.LastPurchase >= searchModel.LastPurchaseFrom && x.LastPurchase <= searchModel.LastPurchaseTo);
                if (searchModel.Classification.HasValue)
                    result = result.Where(x => x.ClassificationId == searchModel.Classification);
                if (searchModel.Seller.HasValue)
                    result = result.Where(x => x.UserId == searchModel.Seller);
            }
            return result;
        }
    }
}