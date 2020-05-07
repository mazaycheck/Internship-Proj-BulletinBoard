using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;
using WebApplication2.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MessagesController : ControllerBase
    {
        
        private readonly IMessageService _service;        

        public MessagesController(IMessageService service)
        {
            _service = service;            
        }

        
        [HttpGet]     
        [Route("box/{messagebox}")]
        public async Task<ActionResult> GetMessages([FromRoute]string messagebox)
        {
            
            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                var tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);                              
                List<MessageForDetailDto> messages;
                switch (messagebox)
                {
                    case "unread": messages = await _service.GetMessagesUnread(tokenUserId); break;
                    case "inbox": messages = await _service.GetMessagesInbox(tokenUserId); break;
                    case "outbox": messages = await _service.GetMessagesOutbox(tokenUserId); break;
                    default: messages = await _service.GetMessagesUnread(tokenUserId);break;                     
                }            
                if(messages != null)
                {
                
                    return Ok(messages);
                }
                else
                {
                    return NotFound();
                }                    
            }
            return Unauthorized();              
        }

        
        [HttpGet]
        [Route("thread/{withuserId}")]
        public async Task<ActionResult> GetMessagesThread([FromRoute] int withuserId)
        {

            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                var tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                List<MessageForDetailDto> messages;
                messages = await _service.GetMessageFromConversation(userOneId: tokenUserId, userTwoId: withuserId);
                
                if (messages != null)
                {
                    return Ok(messages);
                }
                else
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }



        
        [HttpGet("{id}")]       
        public async Task<ActionResult> GetById([FromRoute]int id)
        {

            var message = await _service.GetById(id);
            if(message == null)
            {
                return NotFound();
            }

            int tokenUserId;
            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if(message.SenderId == tokenUserId || message.RecieverId == tokenUserId)
                {
                    return Ok(message);
                }
            }
            return Unauthorized();            
        }

        
        [HttpPut("{messageId}")]
        public async Task<IActionResult> PutMessage([FromRoute] int messageId)
        {            
            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                int tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var messageFromDb = await _service.GetById(messageId);
                if (messageFromDb == null)
                    return NotFound();
                if(messageFromDb.RecieverId == tokenUserId) 
                {
                    _service.MarkAsRead(messageId);
                    return Ok();
                }                    
            }
            return Unauthorized();
        }

        
        [HttpPost]
        public async Task<ActionResult> PostMessage(MessageForCreateDto messageDto)
        {            
            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                int tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                messageDto.SenderId = tokenUserId;                
                var newMessage = await _service.CreateMessage(messageDto);
                return CreatedAtAction("GetById", new { id = newMessage.MessageId }, newMessage);                
            }
            else 
            { 
                return Unauthorized();
            }                  
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(int id)
        {
            return Ok();
        }

        private bool MessageExists(int id)
        {
            return true;
        }
    }
}
