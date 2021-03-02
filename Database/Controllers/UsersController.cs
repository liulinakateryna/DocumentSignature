using Database.Data;
using Database.Entities;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task RegisterAsync([FromBody]UserRegistrationModel userRegisterRequestModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = userRegisterRequestModel.Email,
                    UserName = userRegisterRequestModel.Email,
                    Role = userRegisterRequestModel.Role,
                    Name = userRegisterRequestModel.Name
                };
                var result = await userManager.CreateAsync(user, userRegisterRequestModel.Password);
                if (result.Succeeded)
                {
                    await Login(new AuthModel
                    {
                        Email = userRegisterRequestModel.Email,
                        Password = userRegisterRequestModel.Password
                    });
                }
                else
                {
                    await Response.WriteAsync("Result validation failed!");
                }
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task Login(AuthModel model)
        {
            await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),

                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var response = new
                    {
                        userId = user.Id
                    };
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                    return;
                }

                await Response.WriteAsync("Wrong credentials!");
            }
        }

        [HttpPost("logout")]
        public async Task LogOff()
        {
            await signInManager.SignOutAsync();
        }


    }
}