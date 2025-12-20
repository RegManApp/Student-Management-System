using RegMan.Backend.BusinessLayer.DTOs.ChattingDTO;
using RegMan.Backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMan.Backend.BusinessLayer.Contracts
{
    public interface IChatService
    {
        Task<ViewConversationDTO> SendMessageAsync(string senderId, string recieverId, string textMessage);
        Task<ViewConversationsDTO> GetUserConversationsAsync(string userId);
        Task<ViewConversationDTO> ViewConversationAsync(string userId, int conversationId, int pageNumber, int pageSize = 20);
    }
}
