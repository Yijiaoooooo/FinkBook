using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Api.Data;
using Microsoft.AspNetCore.JsonPatch;
using User.Api.Models;

namespace User.Api.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.AppUser
                .AsNoTracking()
                //.Include(c => c.Properties)
                .SingleOrDefaultAsync(c => c.Id == userIdentity.UserId);

            if (user == null)
                throw new UserOperationException($"Id {userIdentity.UserId}");

            return Json(user);

        }


        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> patch([FromBody] JsonPatchDocument<AppUser> patch)
        {
            var user = await _userContext.AppUser.SingleOrDefaultAsync(c => c.Id == userIdentity.UserId);

            patch.ApplyTo(user);

            foreach (var property in user.Properties)
            {
                _userContext.Entry(property).State = EntityState.Detached;
            }

            var originProperties = await _userContext.UserProperties.Where(c => c.AppUserId == userIdentity.UserId).AsNoTracking().ToListAsync();

            var allProperties = originProperties.Union(user.Properties).Distinct();

            var removeProperties = originProperties.Except(user.Properties);

            var newProperties = allProperties.Except(originProperties);

            foreach (var property in removeProperties)
            {
                _userContext.UserProperties.Remove(property);
            }

            foreach (var property in newProperties)
            {
                _userContext.UserProperties.Add(property);
            }

            _userContext.Update(user);
            _userContext.SaveChanges();

            return Json(user);
        }
    }
}
