using DDD.Domain.Repository;
using DDD.Domain.Result;
using DDD.Domain.EnumResults;
using DDD.Domain.Services;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserDomainService _userDomainService;

        public AuthController(UserDomainService userDomainService, IUserDomainRepository userDomainRepository)
        {
            _userDomainService = userDomainService;
        }

        /// <summary>
        /// 根据手机号和密码登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        [UnitOfWork(typeof(MyDbContext))] // 因为CheckLoginAsync可能有修改数据的操作
        public async Task<IActionResult> LoginByTelAndPwd(LoginByTelAndPwdRequest req)
        {
            if (req.password.Length <= 3)
            {
                return BadRequest("密码必须大于3");
            }

            var result = await _userDomainService.CheckLoginAsync(req.PhoneNumber,req.password);

            switch (result)
            {
                case UserAccessResult.Ok:
                    return Ok("登录成功");
                case UserAccessResult.PhoneNumberNotFound:
                case UserAccessResult.NoPassword:
                case UserAccessResult.PasswordError:
                    return BadRequest("登录失败");
                case UserAccessResult.Lockout:
                    return BadRequest("被锁定");
                default:
                    throw new ApplicationException("未知值");
            }
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("CheckSmsCode")]
        public async Task<IActionResult> CheckSmsCode(CheckSmsCodeRequest req)
        {
            var result = await _userDomainService.CheckPhoneNumberAsync(req.PhoneNumber,req.code);
            Console.WriteLine(req);
            Console.WriteLine(result);
            switch (result)
            {
                case CheckCodeResult.Ok:
                    return Ok("验证成功");
                case CheckCodeResult.PhoneNumberNotFound:
                case CheckCodeResult.CodeError:
                    return BadRequest("验证失败");
                case CheckCodeResult.Lockout:
                    return BadRequest("被锁定");
                default:
                    throw new ApplicationException("未知值");
            }
        }
    }

    public record LoginByTelAndPwdRequest(PhoneNumber PhoneNumber, string password);
    public record CheckSmsCodeRequest(PhoneNumber PhoneNumber, string code);
}
