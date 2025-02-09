using ECommerceSkinet.WebAPI.Controllers;
using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceSkinet.Core.DTO;
using AutoMapper;

namespace Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class BasketController : CustomControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            CustomerBasket basket = await _basketRepository.GetBasketAsync(id);
            //return Ok(basket ?? new CustomerBasket { Id = id });
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            CustomerBasket customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            CustomerBasket udpatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
            return Ok(udpatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
