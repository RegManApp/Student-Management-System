using RegMan.Backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMan.Backend.DAL.Contracts
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        Task <IEnumerable<ConversationParticipant>> GetConversationParticipantsAsync(int conversationId);
        Task<Conversation?> GetConversationByParticipantsAsync(string firstUserId, string secondUserId);
        Task<IEnumerable<Conversation>>? GetConversationsByUserIdAsync(string userId);
        Task<Conversation?> GetSpecificUserConversationAsync(string userId, int conversationId);
    }
}
