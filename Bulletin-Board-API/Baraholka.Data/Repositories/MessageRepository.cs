using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly IMapper _mapper;

        public MessageRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<MessageDto> CreateMessage(MessageDto newMessageDto)
        {
            var messageForCreate = _mapper.Map<Message>(newMessageDto);
            var createdMessage = await CreateAndReturn(messageForCreate);
            return _mapper.Map<MessageDto>(createdMessage);
        }

        public async Task<MessageDto> GetMessage(int id)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };
            var conditions = new List<Expression<Func<Message, bool>>>
            {
                m => m.MessageId == id
            };
            var messageFromDb = await GetSingle(includes, conditions);
            return _mapper.Map<MessageDto>(messageFromDb);
        }

        public async Task<MessageDto> FindMessage(int id)
        {
            return _mapper.Map<MessageDto>(await GetFirst(x => x.MessageId == id));
        }

        private async Task<List<MessageDto>> GetMessages(List<Expression<Func<Message, bool>>> filters)
        {
            var includes = new string[] { $"{nameof(Message.Sender)}", $"{nameof(Message.Reciever)}" };

            var oderparams = new List<OrderParams<Message>>
            {
                new OrderParams<Message>{ OrderBy = (m) => m.DateTimeSent }
            };

            List<Message> messagesFromDb = await GetAll(includes, filters, oderparams);
            List<MessageDto> messages = _mapper.Map<List<Message>, List<MessageDto>>(messagesFromDb);
            return messages;
        }

        public async Task<List<MessageDto>> GetMessageThread(int userOneId, int userTwoId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message =>
                (message.SenderId == userOneId && message.RecieverId == userTwoId) ||
                (message.SenderId == userTwoId && message.RecieverId == userOneId)
            };

            return await GetMessages(filters);
        }

        public async Task<List<MessageDto>> GetMessagesInbox(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId,
            };

            return await GetMessages(filters);
        }

        public async Task<List<MessageDto>> GetMessagesOutbox(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.SenderId == userId,
            };
            return await GetMessages(filters);
        }

        public async Task<List<MessageDto>> GetMessagesUnread(int userId)
        {
            var filters = new List<Expression<Func<Message, bool>>>
            {
                message => message.RecieverId == userId && message.IsRead == false,
            };

            return await GetMessages(filters);
        }

        public async Task<MessageDto> UpdateMessage(MessageDto messageDto)
        {
            var messageToUpdate = _mapper.Map<Message>(messageDto);
            var updatedMessage = await UpdateAndReturn(messageToUpdate);
            return _mapper.Map<MessageDto>(updatedMessage);
        }

        public async Task UpdateMessages(IEnumerable<MessageDto> messages)
        {
            var messagesToUpdate = _mapper.Map<IEnumerable<MessageDto>, IEnumerable<Message>>(messages);
            await UpdateRange(messagesToUpdate);
        }
    }
}