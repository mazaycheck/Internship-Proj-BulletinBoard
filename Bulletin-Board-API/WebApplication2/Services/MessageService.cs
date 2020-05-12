﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Services
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
            var messagesFromDb = MessagesIncludeSenderAndReciever().Where(x => x.RecieverId == userId).OrderByDescending(x => x.DateTimeSent);
            var messagesListDto = await messagesFromDb.Select(x => _mapper.Map<MessageForDetailDto>(x)).ToListAsync();
            return messagesListDto;
        }

        public async Task<List<MessageForDetailDto>> GetMessagesOutbox(int userId)
        {
            var messagesFromDb = MessagesIncludeSenderAndReciever().Where(x => x.SenderId == userId).OrderByDescending(x => x.DateTimeSent);
            var messagesListDto = await messagesFromDb.Select(x => _mapper.Map<MessageForDetailDto>(x)).ToListAsync();
            return messagesListDto;
        }

        public async Task<List<MessageForDetailDto>> GetMessagesUnread(int userId)
        {
            var messagesFromDb = MessagesIncludeSenderAndReciever()
                .Where(x => x.RecieverId == userId && x.IsRead == false)
                .OrderByDescending(x => x.DateTimeSent);
            var messagesListDto = await messagesFromDb.Select(x => _mapper.Map<MessageForDetailDto>(x)).ToListAsync();
            return messagesListDto;
        }

        public async void MarkAsRead(int messageId)
        {
            var message = await _repo.GetById(messageId);
            message.IsRead = true;
            message.DateTimeRead = DateTime.Now;
            await _repo.Save();
        }

        private IQueryable<Message> MessagesIncludeSenderAndReciever()
        {
            return _repo.GetQueryableSet()
               .Include(m => m.Sender)
               .Include(m => m.Reciever);
        }
    }
}