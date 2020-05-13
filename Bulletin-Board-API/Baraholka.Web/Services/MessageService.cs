using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _repo;
        private readonly IMapper _mapper;

        public MessageService(IGenericRepository<Message> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<MessageForDetailDto> CreateMessage(MessageForCreateDto messageDto)
        {
            var messageEntity = _mapper.Map<Message>(messageDto);
            messageEntity.DateTimeSent = DateTime.Now;
            await _repo.Create(messageEntity);
            return _mapper.Map<MessageForDetailDto>(messageEntity);
        }

        public async Task<MessageForDetailDto> GetById(int id)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var messageFromDb = await _repo.GetSingle(m => m.MessageId == id, includes);
            var messageDto = _mapper.Map<MessageForDetailDto>(messageFromDb);
            return messageDto;
        }

        public async Task<List<MessageForDetailDto>> GetMessageFromConversation(int userOneId, int userTwoId)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message =>
                (message.SenderId == userOneId && message.RecieverId == userTwoId) ||
                (message.SenderId == userTwoId && message.RecieverId == userOneId)
            };
            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent }
            };

            List<Message> messagesFromDb = await _repo.GetAll(includes, filters, oderparams);
            List<MessageForDetailDto> messages = _mapper.Map<List<Message>, List<MessageForDetailDto>>(messagesFromDb);
            return messages;
        }

        public async Task<List<MessageForDetailDto>> GetMessagesInbox(int userId)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId,
            };
            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent, Descending = true }
            };

            List<Message> messagesFromDb = await _repo.GetAll(includes, filters, oderparams);
            List<MessageForDetailDto> messages = _mapper.Map<List<Message>, List<MessageForDetailDto>>(messagesFromDb);

            return messages;
        }

        public async Task<List<MessageForDetailDto>> GetMessagesOutbox(int userId)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.SenderId == userId,
            };
            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent, Descending = true }
            };

            List<Message> messagesFromDb = await _repo.GetAll(includes, filters, oderparams);
            List<MessageForDetailDto> messages = _mapper.Map<List<Message>, List<MessageForDetailDto>>(messagesFromDb);

            return messages;
        }

        public async Task<List<MessageForDetailDto>> GetMessagesUnread(int userId)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId && message.IsRead == false,
            };
            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent, Descending = true }
            };

            List<Message> messagesFromDb = await _repo.GetAll(includes, filters, oderparams);
            List<MessageForDetailDto> messages = _mapper.Map<List<Message>, List<MessageForDetailDto>>(messagesFromDb);

            return messages;
        }

        public async void MarkAsRead(int messageId)
        {
            var message = await _repo.GetById(messageId);
            message.IsRead = true;
            message.DateTimeRead = DateTime.Now;
            await _repo.Save();
        }
    }
}