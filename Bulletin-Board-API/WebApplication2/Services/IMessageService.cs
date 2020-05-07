using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IMessageService
    {
        public Task<MessageForDetailDto> GetById(int id);
        public Task<List<MessageForDetailDto>> GetMessageFromConversation(int userOneId, int userTwoId);
        public Task<List<MessageForDetailDto>> GetMessagesInbox(int userId);
        public Task<List<MessageForDetailDto>> GetMessagesOutbox(int userId);
        public Task<List<MessageForDetailDto>> GetMessagesUnread(int userId);
        public Task<MessageForDetailDto> CreateMessage(MessageForCreateDto message);
        public void MarkAsRead(int messageId);
    }
}
