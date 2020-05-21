using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        enum MessageboxType
        {
            inbox,
            outbox,
            unread,            
        }

        public async Task<MessageModel> CreateMessage(MessageCreateModel messageCreateModel, int userId)
        {
            var messageForCreate = _mapper.Map<MessageDto>(messageCreateModel);

            messageForCreate.DateTimeSent = DateTime.Now;
            messageForCreate.SenderId = userId;

            MessageDto newMessage = await _messageRepository.CreateMessage(messageForCreate);
            return _mapper.Map<MessageModel>(newMessage);
        }

        public async Task<MessageModel> GetById(int id)
        {
            var messageFromDb = await _messageRepository.GetMessage(id);
            var messageDto = _mapper.Map<MessageModel>(messageFromDb);
            return messageDto;
        }

        public async Task<List<MessageModel>> GetMessagesByType(string messagesType, int userId)
        {
            MessageboxType boxValue;
            Enum.TryParse(messagesType, true, out boxValue);

            List<MessageDto> messages;

            if (boxValue == MessageboxType.inbox)
                messages = await _messageRepository.GetMessagesInbox(userId);
            else if (boxValue == MessageboxType.outbox)
                messages = await _messageRepository.GetMessagesOutbox(userId);
            else if (boxValue == MessageboxType.unread)
                messages = await _messageRepository.GetMessagesUnread(userId);
            else
                throw new Exception("No such message box type");

            return _mapper.Map<List<MessageDto>, List<MessageModel>>(messages);
        }

        public async Task<List<MessageModel>> GetMessageThread(int userOneId, int userTwoId)
        {
            var messages = await _messageRepository.GetMessageThread(userOneId, userTwoId);
            var unreadMessagesByUserOne = messages.Where(m => m.SenderId == userTwoId && !m.IsRead).ToList();
            if (unreadMessagesByUserOne.Any())
            {
                await MarkThreadMessagesAsRead(unreadMessagesByUserOne);
            }

            return _mapper.Map<List<MessageDto>, List<MessageModel>>(messages);
        }

        public async Task<MessageModel> MarkMessageAsRead(int messageId)
        {
            var message = await _messageRepository.FindMessage(messageId);
            message.IsRead = true;
            message.DateTimeRead = DateTime.Now;
            MessageDto updatedMessage = await _messageRepository.UpdateMessage(message);
            return _mapper.Map<MessageModel>(updatedMessage);
        }

        public async Task MarkThreadMessagesAsRead(IEnumerable<MessageDto> messages)
        {
            foreach (var message in messages)
            {
                message.IsRead = true;
                message.DateTimeRead = DateTime.Now;
            }

            await _messageRepository.UpdateMessages(messages);
        }
    }
}