using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDomainRepository _userDomainRepository;
        private readonly MyDbContext _dbContext;

        public UserController(IUserDomainRepository userDomainRepository, MyDbContext dbContext)
        {
            _userDomainRepository = userDomainRepository;
            _dbContext = dbContext;
        }

        [UnitOfWork(typeof(MyDbContext))]
        [HttpPost]
        public async Task<IActionResult> AddNewUser(AddUserRequest req)
        {
            if (await _userDomainRepository.FindOneAsync(req.PhoneNumber) != null)
            {
                return BadRequest("手机号已经存在");
            }

            User user = new User(req.PhoneNumber);
            user.ChangePassword(req.password);
            _dbContext.Users.Add(user);
            return Ok("成功");
        }
    }

    /// <summary>
    /// 添加用户的接收参数 
    /// </summary>
    /// <param name="PhoneNumber"></param>
    /// <param name="password"></param>
    public record AddUserRequest(PhoneNumber PhoneNumber, string password);
}
