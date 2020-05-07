using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;
using WebApplication2.Helpers;
using System.Security.Claims;
using WebApplication2.Services;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class AnnoucementsController : ControllerBase
    {     
        private readonly IAnnoucementService _service;
       
        public AnnoucementsController(IAnnoucementService service)
        {    
            _service = service;
        }
        
        [AllowAnonymous]
        [HttpGet]               
        [Route("search")]        
        public async Task<IActionResult> GetAll([FromQuery]AnnoucementFilter filterOptions, 
            [FromQuery]PaginateParams paginateParams, [FromQuery]OrderParams orderByParams)
        {
            Paged<AnnoucementForViewDto> pagedObject = await _service.GetAnnoucements(filterOptions, paginateParams, orderByParams);
            if (pagedObject != null) 
            {                
                return Ok(pagedObject);
            }
            else
            {
                return NoContent();
            }            
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            AnnoucementForViewDto annoucementDto = await _service.GetAnnoucementById(id);
            if(annoucementDto == null)
            {
                return NotFound($"No annoucement with id: {id}");
            }
            return Ok(annoucementDto);
        }





        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Add([FromForm] AnnoucementForCreateDto annoucementDto) // id??
        {  
            var userId = GetUserIdentifierFromClaims();                  
            AnnoucementForViewDto annoucement = await _service.CreateNewAnnoucement(annoucementDto, userId);
            return CreatedAtAction("GetById", new { id = annoucement.Id }, annoucement);            
        }



        [Authorize(Roles = "Member")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            bool result = await _service.DeleteAnnoucementById(id);
            if (!result)
            {
                throw Exception($"Could not delete annoucement with id {id}");
            }
            return Ok();            
        }

        private Exception Exception(string v)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] AnnoucementForUpdateDto annoucementDto)
        {
            var userId = GetUserIdentifierFromClaims();

            AnnoucementForViewDto annoucement = await _service.UpdateAnnoucement(annoucementDto, userId);

            if (annoucement == null)
            {
                throw Exception($"Could not update annoucement with id {annoucementDto.AnnoucementId}");
            }

            return CreatedAtAction("GetById", new { id = annoucement.Id }, annoucement);
        }





        private int GetUserIdentifierFromClaims()
        {
            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);                       
            }
            else
            {
                throw new InvalidCredentialException($"User has no NameIdentifier claims");
            }
        }

    }
}