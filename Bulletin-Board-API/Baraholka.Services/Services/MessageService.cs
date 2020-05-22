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
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        private enum MessageboxType
        {
            inbox,
            outbox,
            unread,
        }

        public async Task<MessageDto> CreateMessage(MessageDto messageCreateDto)
        {
            messageCreateDto.DateTimeSent = DateTime.Now;
            MessageDto newMessage = await _messageRepository.CreateMessage(messageCreateDto);
            return newMessage;
        }

        public async Task<MessageDto> GetById(int id)
        {
            MessageDto messageFromDb = await _messageRepository.GetMessage(id);
            return messageFromDb;
        }

        public async Task<List<MessageDto>> GetMessagesByType(string messagesType, int userId)
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

            return messages;
        }

        public async Task<List<MessageDto>> GetMessageThread(int userOneId, int userTwoId)
        {
            List<MessageDto> messages = await _messageRepository.GetMessageThread(userOneId, userTwoId);
            var unreadMessagesByUserOne = messages.Where(m => m.SenderId == userTwoId && !m.IsRead).ToList();
            if (unreadMessagesByUserOne.Any())
            {
                await MarkThreadMessagesAsRead(unreadMessagesByUserOne);
            }

            return messages;
        }

        public async Task<MessageDto> MarkMessageAsRead(int messageId)
        {
            var message = await _messageRepository.FindMessage(messageId);
            message.IsRead = true;
            message.DateTimeRead = DateTime.Now;
            MessageDto updatedMessage = await _messageRepository.UpdateMessage(message);
            return updatedMessage;
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