using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IMessageService
    {
        public Task<MessageForDetailDto> GetById(int id);

        public Task<List<MessageForDetailDto>> GetMessageThread(int userOneId, int userTwoId);

        public Task<MessageForDetailDto> CreateMessage(MessageForCreateDto message);

        public void MarkAsRead(int messageId);

        Task<List<MessageForDetailDto>> GetMessages(string messagebox, int tokenUserId);
    }
}