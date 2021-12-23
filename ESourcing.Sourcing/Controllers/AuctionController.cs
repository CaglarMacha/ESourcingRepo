using DnsClient.Internal;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Events;
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
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<AuctionController> _logger;

        public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger , IBidRepository bidRepository)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _bidRepository = bidRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auction = await _auctionRepository.GetAuctions ();
            return Ok(auction);
        }
        [HttpGet("id:lenght(24)",Name ="GetAuction")]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id: {id} has not been found in db");
                return NotFound();
            }
            return Ok(auction);
        }
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> CreateAuction ([FromBody]Auction auction)
        {
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAction", new {id=auction.Id },auction);
        }
        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));  
        }
        [HttpDelete("id:lenght(24)")]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> DeleteAuction(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }
        [HttpPost("ComplateAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            Auction auction = await _auctionRepository.GetAuction(id);

            if (auction == null)
                return NotFound();
            if(auction.Status!=(int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if (bid == null)
                return NotFound();


            OrderCreateEvent evetMessage = new OrderCreateEvent() ;
            
        } 

    }

}