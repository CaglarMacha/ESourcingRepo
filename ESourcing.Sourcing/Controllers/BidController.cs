using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly ILogger _logger;

        public BidController(IBidRepository bidRepository, ILogger logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBid(Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }
        [HttpGet("GetBidByAuctionId")]
        [ProducesResponseType(typeof(IEnumerable<Bid>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidByAuctionId (string id)
        {
            IEnumerable<Bid> bids = await _bidRepository.GetBidByAuctionId(id);

            return Ok(bids);

        }
        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid ( string id)
        {
           Bid bid = await _bidRepository.GetWinnerBid(id);

            return Ok(bid);
        }
    }
}
