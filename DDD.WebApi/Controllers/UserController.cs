using AutoMapper;
using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using DDD.WebApi.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserDomainRepository userDomainRepository, MyDbContext dbContext,IMapper mapper,ILogger<UserController> logger)
        {
            _userDomainRepository = userDomainRepository;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<ActionResult<UserDto>> Add(User user)
        {
            var existingUser = await _userDomainRepository.FindOneAsync(user.PhoneNumber);

            if (existingUser != null)
            {
                return BadRequest("手机号已经存在");
            }

            _dbContext.Users.Add(user);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
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
            user.UserAccessFail!.IsDeleted = true;
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
        public async Task<ActionResult<UserDto>> Update(User user)
        {
            var dbUser = await _userDomainRepository.FindOneAsync(user.Id);

            if (dbUser == null)
            {
                return NotFound("用户不存在");
            }

            dbUser.ChangePhoneNumber(user.PhoneNumber);
            dbUser.UserName = user.UserName;
            _dbContext.Users.Update(dbUser);

            var userDto = _mapper.Map<UserDto>(dbUser);
            return userDto;
        }

        /*[HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userDomainRepository.FindAllAsync();

            if (users == null)
            {
                return NotFound("用户不存在");
            }

            return users;
        }*/

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _userDomainRepository.FindAllAsync();

            if (users == null)
            {
                return NotFound("用户不存在");
            }
            _logger.LogDebug("获取全部用户");

            // 使用 AutoMapper
            var userDto = _mapper.Map<List<UserDto>>(users);
            return userDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _userDomainRepository.FindOneAsync(id);

            if (user == null)
            {
                return NotFound("用户不存在");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        [HttpPost("PhoneNumber")]
        public async Task<ActionResult<UserDto>> GetPhone(PhoneNumber PhoneNumber)
        {
            var user = await _userDomainRepository.FindOneAsync(PhoneNumber);

            if (user == null)
            {
                return NotFound("用户不存在");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
