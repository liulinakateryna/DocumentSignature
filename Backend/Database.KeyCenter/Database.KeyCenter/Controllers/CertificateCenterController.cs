using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.KeyCenter.Data;
using Database.KeyCenter.Entity;
using Database.KeyCenter.KeyCenter;
using Database.KeyCenter.Model;

namespace Database.KeyCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateCenterController : ControllerBase
    {
        private readonly DataContext _context;

        public CertificateCenterController(DataContext context)
        {
            _context = context;
        }

        // POST: api/Contracts
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task<IActionResult> SuggestContract([FromRoute]string userId)
        {
            try
            {
                var isUserAlreadyRegistered = _context.PrivateData.Where(x => x.UserId == userId).Any();
                if (!isUserAlreadyRegistered)
                {
                    var fingerprint = await GetFingerprint();
                    var keys = RsaCenter.GetKeys();
                    var record = new PrivateData()
                    {
                        UserId = userId,
                        Fingerprint = fingerprint,
                        RsaParameters = keys
                    };

                    _context.PrivateData.Add(record);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return Ok("The user is already registered in the system.");
                }
            }
            catch { }

            return BadRequest("The user is not successfully registered.");
        }

        // POST: api/CertificateCenter
        [HttpPost]
        public async Task<IActionResult> SignContract([FromBody] string userId, byte[] hash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUserAuthorized = await VerifyUser(userId);
            if (isUserAuthorized)
            {
                var rsaParameters = _context.PrivateData.Where(x => x.UserId == userId).FirstOrDefault().RsaParameters;
                var sign = RsaCenter.Sign(hash, rsaParameters);
                return Ok(sign);
            }
            return BadRequest("The user is not authorized. The contract can not be signed.");
        }

        // POST: api/CertificateCenter
        [HttpPost("VerifySign")]
        public async Task<IActionResult> VerifySign([FromBody] SignatureModel signature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rsaParameters = _context.PrivateData.Where(x => x.UserId == signature.UserId).FirstOrDefault().RsaParameters;
            var isValid = RsaCenter.Verify(signature.Hash, signature.Sign, rsaParameters);
            return Ok(isValid);
        }

        private async Task<byte[]> GetFingerprint()
        {
            return new byte[100];
        }

        private async Task<bool> VerifyUser(string userId)
        {
            return true;
        }

        private bool PrivateDataExists(int id)
        {
            return _context.PrivateData.Any(e => e.PrivateDataId == id);
        }
    }
}