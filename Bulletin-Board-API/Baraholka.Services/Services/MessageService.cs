using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepo;
        private readonly IMapper _mapper;

        public MessageService(IGenericRepository<Message> repo, IMapper mapper)
        {
            _messageRepo = repo;
            _mapper = mapper;
        }

        public async Task<MessageForDetailDto> CreateMessage(MessageForCreateDto messageDto)
        {
            var messageEntity = _mapper.Map<Message>(messageDto);
            messageEntity.DateTimeSent = DateTime.Now;
            await _messageRepo.Create(messageEntity);
            return _mapper.Map<MessageForDetailDto>(messageEntity);
        }

        public async Task<MessageForDetailDto> GetById(int id)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var conditions = new List<Expression<Func<Message, bool>>>
            {
                m => m.MessageId == id
            };
            var messageFromDb = await _messageRepo.GetSingle(includes, conditions);
            var messageDto = _mapper.Map<MessageForDetailDto>(messageFromDb);
            return messageDto;
        }

        public async Task<List<MessageForDetailDto>> GetMessages(string messagesType, int userId)
        {
            MessageboxType boxValue;
            var parseResult = Enum.TryParse(messagesType, true, out boxValue);

            if (!parseResult)
            {
                throw new Exception("No such message box");
            }

            if (boxValue == MessageboxType.inbox)
                return await GetMessagesInbox(userId);
            else if (boxValue == MessageboxType.outbox)
                return await GetMessagesOutbox(userId);
            else if (boxValue == MessageboxType.unread)
                return await GetMessagesUnread(userId);
            else
                return await GetMessagesInbox(userId);            
        }

        private async Task<List<MessageForDetailDto>> GetMessages(List<Expression<Func<Message, bool>>> filters)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };

            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent }
            };

            List<Message> messagesFromDb = await _messageRepo.GetAll(includes, filters, oderparams);
            List<MessageForDetailDto> messages = _mapper.Map<List<Message>, List<MessageForDetailDto>>(messagesFromDb);
            return messages;
        }

        public async Task<List<MessageForDetailDto>> GetMessageThread(int userOneId, int userTwoId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message =>
                (message.SenderId == userOneId && message.RecieverId == userTwoId) ||
                (message.SenderId == userTwoId && message.RecieverId == userOneId)
            };

            return await GetMessages(filters);
        }

        private async Task<List<MessageForDetailDto>> GetMessagesInbox(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId,
            };

            return await GetMessages(filters);
        }

        public async Task<List<MessageForDetailDto>> GetMessagesOutbox(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.SenderId == userId,
            };
            return await GetMessages(filters);
        }

        private async Task<List<MessageForDetailDto>> GetMessagesUnread(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId && message.IsRead == false,
            };

            return await GetMessages(filters);
        }

        public async void MarkAsRead(int messageId)
        {
            var message = await _messageRepo.FindById(messageId);
            message.IsRead = true;
            message.DateTimeRead = DateTime.Now;
            await _messageRepo.Save();
        }
    }

    public enum MessageboxType
    {
        inbox,
        outbox,
        unread,
        thread
    }
}