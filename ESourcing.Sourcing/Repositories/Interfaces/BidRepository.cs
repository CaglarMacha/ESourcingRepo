using ESourcing.Sourcing.Data.Interface;
using ESourcing.Sourcing.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Repositories.Interfaces
{
    public class BidRepository:IBidRepository
    {
        private readonly ISourcingContext _context;

        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);

            List<Bid> bids = await _context.Bids.Find(filter)
                                                .ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt)
                       .GroupBy(a => a.SellerUserName)
                       .Select(a => new Bid
                       {
                           AuctionId = a.FirstOrDefault().AuctionId,
                           Price = a.FirstOrDefault().Price,
                           CreatedAt = a.FirstOrDefault().CreatedAt,
                           SellerUserName = a.FirstOrDefault().SellerUserName,
                           ProductId = a.FirstOrDefault().ProductId,
                           Id = a.FirstOrDefault().Id
                       })
                       .ToList();


            return bids;

        }

        public Task<List<Bid>> GetBids()
        {
            throw new NotImplementedException();
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> auctions = await _context.Bids.Find(p => p.AuctionId == id).ToListAsync();
            List<Bid> bids = await GetBidByAuctionId(id);

            return bids.OrderByDescending(a => a.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);

        }
    }
}
