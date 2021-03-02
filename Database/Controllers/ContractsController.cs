using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Data;
using Database.Entities;
using Database.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly DataContext context;

        public ContractsController(DataContext context)
        {
            this.context = context;
        }

        // GET: api/Contracts
        [HttpGet]
        [ProducesResponseType(typeof(ContractModel[]), StatusCodes.Status200OK)]
        public IEnumerable<ContractModel> GetContracts()
        {
            var contracts = context.Contracts.Select(x => new ContractModel()
            {
                ContractId = x.ContractId,
                UserId = x.User.Id,
                DocumentId = x.DocumentId,
                DocumentName = x.Document.FileName,
                Sign = x.Sign,
                Date = x.Date
            }).ToList();
            return contracts;
        }

        // GET: api/Contracts
        [HttpGet("statistics/declined")]
        [ProducesResponseType(typeof(ContractModel[]), StatusCodes.Status200OK)]
        public Statistics GetDeclinedContractsStatistics()
        {
            var contracts = context.Contracts.Where(x => x.IsRefused);

            var statisticsData = new Statistics();
            for(int i = 0; i < 200; i++)
            {
                statisticsData.Points.Add(i);
                var count = contracts.Where(x => x.User.Points == i).Count();
            }

            return statisticsData;
        }

        // GET: api/Contracts
        [HttpGet("statistics/accepted")]
        [ProducesResponseType(typeof(ContractModel[]), StatusCodes.Status200OK)]
        public Statistics GetAcceptedContractsStatistics()
        {
            var contracts = context.Contracts.Where(x => !x.IsRefused && x.Sign != null);

            var statisticsData = new Statistics();
            for (int i = 0; i < 200; i++)
            {
                statisticsData.Points.Add(i);
                var count = contracts.Where(x => x.User.Points == i).Count();
            }

            return statisticsData;
        }

        //GET: api/Contracts/5
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ContractModel[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContracts([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var contracts = user.Contracts.Select(x => new ContractModel()
            {
                ContractId = x.ContractId,
                UserId = x.User.Id,
                DocumentId = x.DocumentId,
                DocumentName = x.Document.FileName,
                Sign = x.Sign,
                Date = x.Date,
                IsRefused = x.IsRefused,
                DeclinedDate = x.DeclinedDate
            });

            return Ok(contracts);
        }

        //GET: api/Contracts/getFile/5
        [HttpGet("getFile/{contractId}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFile([FromRoute] int contractId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await context.Contracts.FindAsync(contractId);
            var document = contract.Document;

            if (document == null)
            {
                return NotFound();
            }

            Response.ContentType = document.ContentType;
            return File(document.Content, document.ContentType, document.FileName);
        }

        // POST: api/Contracts
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task SuggestContract([FromRoute]string userId, IFormFile file)
        {
            var user = await context.Users.FindAsync(userId);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var result = stream.ToArray();
                var document = new Document
                {
                    Content = result,
                    ContentType = file.ContentType,
                    FileName = file.FileName
                };

                var contract = new Contract()
                {
                    Date = DateTime.Now,
                    IsRefused = false,
                    Document = document
                };

                user.Contracts.Add(contract);
                await context.SaveChangesAsync();
            }
        }

        // PUT: api/Contracts/Sign/5
        [HttpPut("sign/{contractId}")]
        public async Task<IActionResult> SignContract([FromRoute] int contractId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await context.Contracts.FindAsync(contractId);

            if (contract == null)
            {
                return BadRequest();
            }

            //verify
            contract.Date = DateTime.Now;

            context.Entry(contract).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existsContract = context.Contracts.Any(x => x.ContractId == contractId);
                if (!existsContract)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Contracts/Sign/5
        [HttpPut("verifySigns/")]
        public async Task<IActionResult> VerifySigns()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // var contracts = context.Contracts.ToList();

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    var existsContract = context.Contracts.Any(x => x.ContractId == contractId);
            //    if (!existsContract)
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // PUT: api/Contracts/Decline/5
        [HttpPut("Decline/{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeclineContract([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return BadRequest();
            }

            contract.IsRefused = true;
            contract.DeclinedDate = DateTime.Now;

            context.Entry(contract).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existsContract = context.Contracts.Any(x => x.ContractId == id);
                if (!existsContract)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Contracts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteContract([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = context.Contracts.Where(x => x.ContractId == id).FirstOrDefault();
            if (contract == null)
            {
                return NotFound();
            }
            else if (this.IsContractRequired(contract))
            {
                return Conflict("The contract can not be deleted as the agreement is stil active.");
            }
            context.Contracts.Remove(contract);
            await context.SaveChangesAsync();

            return Ok(contract);
        }

        // DELETE: api/Contracts/5
        [HttpDelete("Irrelevant")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task<IActionResult> DeleteIrrelevantContract()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contracts = context.Contracts.Where(x => !IsContractRelevant(x)).ToList();
            
            foreach(var contract in contracts)
            {
                context.Contracts.Remove(contract);
            }
            
            await context.SaveChangesAsync();

            return Ok();
        }

        private bool IsContractRequired(Contract contract)
        {
            var canBeDeleted = contract.IsRefused || (contract.Sign == null);
            return !canBeDeleted;
        }

        private byte[] GetHash(byte[] file)
        {
            var sha = SHA256.Create();
            var hash = sha.ComputeHash(file);
            return hash;
        }

        private async Task GetSign(string userId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44308/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/CertificateCenter/{userId}", userId);
            response.EnsureSuccessStatusCode();
            var res = response.Content;
            //return response.Content;
        }

        private async Task<bool> VerifySigns(Contract contracts)
        {
            return true;
        }

        private bool IsContractRelevant(Contract contract)
        {
            var isIrrelevant = (DateTime.Now.Year - contract.Date.Year) >= 4 || contract.IsRefused;
            return !isIrrelevant;
        }
    }
}