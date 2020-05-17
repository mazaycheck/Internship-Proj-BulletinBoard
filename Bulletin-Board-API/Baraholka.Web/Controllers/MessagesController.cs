using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService service)
        {
            _messageService = service;
        }

        [HttpGet]
        [Route("box/{messagebox}")]
        public async Task<ActionResult> GetMessages([FromRoute]string messagebox)
        {
            int authorizedUserID = User.GetUserID();
            List<MessageForDetailDto> messages = await _messageService.GetMessages(messagebox, authorizedUserID);
            if (messages != null)
            {
                return Ok(messages);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("thread/{withuserId}")]
        public async Task<ActionResult> GetMessagesThread([FromRoute] int withuserId)
        {
            int authorizedUserID = User.GetUserID();

            List<MessageForDetailDto> messages = await _messageService.GetMessageThread(userOneId: authorizedUserID, userTwoId: withuserId);

            if (messages != null)
            {
                return Ok(messages);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute]int id)
        {
            var message = await _messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }

            int tokenUserId;

            tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (message.SenderId == tokenUserId || message.RecieverId == tokenUserId)
            {
                return Ok(message);
            }
            return Forbid("You are not allowed to get this message");
        }

        [HttpPut("{messageId}")]
        public async Task<IActionResult> MarkRead([FromRoute] int messageId)
        {
            int authorizedUserID = User.GetUserID();

            var messageFromDb = await _messageService.GetById(messageId);
            if (messageFromDb == null)
                return NotFound();
            if (messageFromDb.RecieverId == authorizedUserID)
            {
                _messageService.MarkAsRead(messageId);
                return Ok();
            }
            return Forbid("You are not allowed to mark this message as read");
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(MessageForCreateDto messageDto)
        {
            int authorizedUserID = User.GetUserID();
            messageDto.SenderId = authorizedUserID;

            var newMessage = await _messageService.CreateMessage(messageDto);
            return CreatedAtAction("GetById", new { id = newMessage.MessageId }, newMessage);
        }
    }
}