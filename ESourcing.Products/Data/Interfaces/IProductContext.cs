using ESourcing.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Products.Data.Interfaces
{
    public interface IProductContext
    {
        //Mongo Db Context işlemini yapıyoruz Mongo Client e bağlanıyoruz
        IMongoCollection<Product> Products { get; }

    }
}
