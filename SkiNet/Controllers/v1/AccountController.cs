﻿using AutoMapper;
using ECommerceSkinet.Core.DTO;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.WebAPI.Errors;
using ECommerceSkinet.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceSkinet.Core.Entities.Identity;
using System.Security.Claims;

namespace ECommerceSkinet.WebAPI.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByUserByClaimsPrincipleEmailWithAddressAsync(User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByUserByClaimsPrincipleEmailWithAddressAsync(User);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            return BadRequest("Problem update the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email address already exists" }
                });
            }
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
