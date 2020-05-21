using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IMessageService
    {
        public Task<MessageModel> GetById(int id);

        public Task<List<MessageModel>> GetMessageThread(int userOneId, int userTwoId);

        public Task<MessageModel> CreateMessage(MessageCreateModel message, int userId);

        public Task<MessageModel> MarkMessageAsRead(int messageId);

        Task<List<MessageModel>> GetMessagesByType(string messagebox, int tokenUserId);
    }
}