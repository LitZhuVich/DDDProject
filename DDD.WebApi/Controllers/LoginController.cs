using DDD.Domain.Repository;
using DDD.Domain.Services;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService _userDomainService;

        public LoginController(UserDomainService userDomainService, IUserDomainRepository userDomainRepository)
        {
            _userDomainService = userDomainService;
        }

        /// <summary>
        /// 根据手机号和密码登录
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(typeof(MyDbContext))] // 因为CheckLoginAsync可能有修改数据的操作
        [HttpPost]
        public async Task<IActionResult> LoginByTelAndPwd(LoginByTelAndPwdRequest req)
        {
            if (req.password.Length <= 3)
            {
                return BadRequest("密码必须大于3");
            }

            var result = await _userDomainService.CheckLoginAsync(req.PhoneNumber,req.password);
            switch (result)
            {
                case Domain.Result.UserAccessResult.Ok:
                    return Ok(new ApiResponse("1"));
                case Domain.Result.UserAccessResult.PhoneNumberNotFound:
                case Domain.Result.UserAccessResult.NoPassword:
                case Domain.Result.UserAccessResult.PasswordError:
                    return BadRequest("登录失败");
                case Domain.Result.UserAccessResult.Lockout:
                    return BadRequest("被锁定");
                default:
                    throw new ApplicationException("未知值");
            }
        }
    }

    public record LoginByTelAndPwdRequest(PhoneNumber PhoneNumber, string password);
}
