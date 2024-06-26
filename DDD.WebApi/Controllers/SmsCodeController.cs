﻿using DDD.Domain;
using DDD.Domain.Repository;
using DDD.Domain.ValueObjects;
using DDD.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsCodeController : ControllerBase
    {
        /// <summary>
        /// 注入防腐层发送短信接口
        /// </summary>
        private readonly ISmsCodeSender _smsSender;
        private readonly IUserDomainRepository _userDomainRepository;
        public SmsCodeController(ISmsCodeSender smsSender, IUserDomainRepository userDomainRepository)
        {
            _smsSender = smsSender;
            _userDomainRepository = userDomainRepository;
        }

        [HttpPost("Send")]
        public async Task<ActionResult<SmsCodeDto>> Send(PhoneNumber PhoneNumber)
        {
            var user = await _userDomainRepository.FindOneAsync(PhoneNumber);
            if (user == null)
            {
                return BadRequest("找不到手机号");
            }
            string code = await _smsSender.SendCodeAsync(PhoneNumber);
            return new SmsCodeDto { Code = code };
        }
    }
}
