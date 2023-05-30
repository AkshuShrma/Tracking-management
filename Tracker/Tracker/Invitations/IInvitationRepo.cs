using Tracker.Models;
using Tracker.Models.DTO;

namespace Tracker.Invitations
{
    public interface IInvitationRepo
    {
        ICollection<FindUser> GetSpecificInvitations(string username,string senderId);
        ICollection<Invite> GetAllRegisteredPersons(string userId);
        public bool CreateInvitation(string senderId, string reciverId);
        public bool UpdateAction(string senderId, string reciverId, int action);
        public string? GetIdFromToken(string token);
        public bool UpdateStatus(string senderId,string reciverId, int status);
    }
}
