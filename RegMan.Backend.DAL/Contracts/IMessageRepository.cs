using RegMan.Backend.DAL.Entities;
using RegMan.Backend.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMan.Backend.DAL.Contracts
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetByConversationIdAsync(int ConversationId);
    }
}
