using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> Add(User user)
        {
            if (await _userDomainRepository.FindOneAsync(user.PhoneNumber) != null)
            {
                return BadRequest("手机号已经存在");
            }
            user.ChangePassword(user.passwordHash);
            _dbContext.Users.Add(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userDomainRepository.FindOneAsync(id);

            if (user == null)
            {
                return NotFound("用户不存在");
            }
            user.IsDeleted = true;
            _dbContext.Users.Update(user);
            return Ok("删除成功");
        }

        [HttpDelete("DeleteTrue/{id}")]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> DeleteTrue(Guid id)
        {
            var user = await _userDomainRepository.FindOneAsync(id);

            if (user == null)
            {
                return NotFound("用户不存在");
            }

            _dbContext.Users.Remove(user);
            return Ok("删除成功");
        }

        [HttpPut]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> Update(User user)
        {
            var dbUser = await _userDomainRepository.FindOneAsync(user.Id);

            if (dbUser == null)
            {
                return NotFound("用户不存在");
            }

            dbUser.ChangePhoneNumber(user.PhoneNumber);
            dbUser.UserName = user.UserName;

            _dbContext.Users.Update(dbUser);
            return Ok(dbUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userDomainRepository.FindAllAsync();

            if (users == null)
            {
                return NotFound("用户不存在");
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userDomainRepository.FindOneAsync(id);

            if (user == null)
            {
                return NotFound("用户不存在");
            }

            return Ok(user);
        }
    }
}
