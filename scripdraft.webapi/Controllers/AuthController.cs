using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

using ScripDraft.Data;
using ScripDraft.Data.Entities;
using ScripDraft.WebApi.DTO;

namespace scripdraft.webapi.Controllers
{    
    [Route("api/[controller]")]     // api/auth
    public class AuthController : Controller
    {
        private IAuthRepository _repository = null;
        private readonly ILogger<AuthController> _logger;
        private IConfiguration _config = null;

        public AuthController(IAuthRepository repository, IConfiguration config, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
            _config = config;

            if(_repository.Database is null)
            {
                _repository.Database = SDDatabase.GetDatabase(_config);
            }
        }

        [HttpPost("login")]     // api/auth/login
        public IActionResult Login([FromBody]UserDto userDto)
        {
            // validate request
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (userDto == null) return BadRequest("Invalid client request");

            userDto.Username = userDto.Username.ToLower();    // we store username only in lowercase



            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);

            return Ok();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]UserDto userDto)
        {
            // validate request
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (userDto == null) return BadRequest("Invalid client request");

            userDto.Username = userDto.Username.ToLower();    // we store username only in lowercase

            if(await _repository.UserExists(userDto.Username)) 
            {
                return BadRequest(string.Format("User with username '{0}' already exists.", userDto.Username));
            }
            else
            {
                User userEntity = UserDto.CreateEntity(userDto);

                var result = _repository.Register(userEntity, userDto.Password);
            }

            return Ok();
        }
    }
}