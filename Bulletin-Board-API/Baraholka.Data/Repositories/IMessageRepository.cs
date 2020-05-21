using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<MessageDto> CreateMessage(MessageDto newMessageDto);

        Task<MessageDto> FindMessage(int id);

        Task<MessageDto> GetMessage(int id);

        Task<List<MessageDto>> GetMessagesInbox(int userId);

        Task<List<MessageDto>> GetMessagesOutbox(int userId);

        Task<List<MessageDto>> GetMessagesUnread(int userId);

        Task<List<MessageDto>> GetMessageThread(int userOneId, int userTwoId);

        Task<MessageDto> UpdateMessage(MessageDto messageDto);

        Task UpdateMessages(IEnumerable<MessageDto> messages);
    }
}