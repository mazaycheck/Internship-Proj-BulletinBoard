using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
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
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("box/{messagebox}")]
        public async Task<ActionResult> GetMessages([FromRoute]string messagebox)
        {
            int authorizedUserID = User.GetUserID();
            List<MessageDto> messagesDto = await _messageService.GetMessagesByType(messagebox, authorizedUserID);
            if (messagesDto != null)
            {
                var messagesModel = _mapper.Map<List<MessageDto>, List<MessageWebModel>>(messagesDto);
                return Ok(messagesModel);
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

            List<MessageDto> messagesDto = await _messageService.GetMessageThread(userOneId: authorizedUserID, userTwoId: withuserId);

            if (messagesDto != null)
            {
                var messagesModel = _mapper.Map<List<MessageDto>, List<MessageWebModel>>(messagesDto);
                return Ok(messagesModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute]int id)
        {
            MessageDto messageDto = await _messageService.GetById(id);
            if (messageDto == null)
            {
                return NotFound();
            }

            int tokenUserId;

            tokenUserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (messageDto.SenderId == tokenUserId || messageDto.RecieverId == tokenUserId)
            {
                return Ok(_mapper.Map<MessageWebModel>(messageDto));
            }
            return StatusCode((int)HttpStatusCode.Forbidden, "You are not allowed to get this message");
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
                MessageDto messageDto = await _messageService.MarkMessageAsRead(messageId);
                var MessageWebModel = _mapper.Map<MessageWebModel>(messageDto);
                return Ok(MessageWebModel);
            }
            return StatusCode((int)HttpStatusCode.Forbidden, "You are not allowed to mark this message as read");
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(MessageCreateModel messageCreateModel)
        {
            MessageDto messageCreateDto = _mapper.Map<MessageDto>(messageCreateModel);
            messageCreateDto.SenderId = User.GetUserID();
            MessageDto newMessage = await _messageService.CreateMessage(messageCreateDto);
            MessageWebModel newMessageWebModel = _mapper.Map<MessageWebModel>(newMessage);
            return CreatedAtAction("GetById", new { id = newMessageWebModel.MessageId }, newMessageWebModel);
        }
    }
}