using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IMessageService
    {
        public Task<MessageDto> GetById(int id);

        public Task<List<MessageDto>> GetMessageThread(int userOneId, int userTwoId);

        public Task<MessageDto> CreateMessage(MessageDto message);

        public Task<MessageDto> MarkMessageAsRead(int messageId);

        Task<List<MessageDto>> GetMessagesByType(string messagebox, int tokenUserId);
    }
}