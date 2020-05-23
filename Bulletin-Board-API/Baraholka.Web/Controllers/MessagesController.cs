using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
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
            List<MessageDto> messageDtos = await _messageService.GetMessagesByType(messagebox, authorizedUserID);
            if (messageDtos != null)
            {
                List<MessageWebModel> messageModels = _mapper.Map<List<MessageDto>, List<MessageWebModel>>(messageDtos);
                return Ok(messageModels);
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

            List<MessageDto> messageDtos = await _messageService.GetMessageThread(userOneId: authorizedUserID, userTwoId: withuserId);

            if (messageDtos != null)
            {
                List<MessageWebModel> messageModels = _mapper.Map<List<MessageDto>, List<MessageWebModel>>(messageDtos);
                return Ok(messageModels);
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

            int tokenUserId = User.GetUserID();

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

            MessageDto messageDto = await _messageService.GetById(messageId);
            if (messageDto == null)
                return NotFound();

            if (messageDto.RecieverId != authorizedUserID)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, "You are not allowed to mark this message as read");
            }

            MessageDto messageUpdateDto = await _messageService.MarkMessageAsRead(messageId);
            MessageWebModel messageModel = _mapper.Map<MessageWebModel>(messageUpdateDto);
            return Ok(messageModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(MessageCreateModel messageCreateModel)
        {
            MessageDto messageCreateDto = _mapper.Map<MessageDto>(messageCreateModel);
            messageCreateDto.SenderId = User.GetUserID();

            MessageDto createdMessageDto = await _messageService.CreateMessage(messageCreateDto);
            MessageWebModel createdMessageModel = _mapper.Map<MessageWebModel>(createdMessageDto);

            return CreatedAtAction("GetById", new { id = createdMessageModel.MessageId }, createdMessageModel);
        }
    }
}