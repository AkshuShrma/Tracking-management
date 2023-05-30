using AutoMapper.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IdentityModel.Tokens.Jwt;
using Tracker.Data;
using Tracker.Models;
using Tracker.Models.DTO;
using Tracker.Repository;
using static Tracker.Models.Invite;

namespace Tracker.Invitations
{
    public class InvitationRepo:IInvitationRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _email;
        public InvitationRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender email)
        {
            _context = context;
            _userManager = userManager;
            _email = email;
        }

        public bool CreateInvitation(string senderId, string reciverId)
        {

            var validatasender = _userManager.FindByIdAsync(senderId).Result;
            var validateReceiver = _userManager.FindByIdAsync(reciverId).Result;
            if (validateReceiver == null || validatasender == null)
                return false;
            // now we will create invitation here ...............................
            Invite invitation = new Invite()
            {
                ReciverId = validateReceiver.Id,
                SenderId = validatasender.Id,
                UserStatus = Invite.Status.Pending,
                UserActions = Invite.Action.Enable
            };
            _context.Invites.Add(invitation);
            var mailrequest = new EmailDto()
            {
                To = "akshay-sharma@cssoftsolutions.com", //validateReceiver.Email,
                Subject = "Invitation for join in the table and edit the details of the table"
            };
            _email.SendEmail(mailrequest);
            return _context.SaveChanges() == 1 ? true : false;
        }

        public ICollection<Invite> GetAllRegisteredPersons(string userId)
        {
            var data = _context.Set<Invite>().Include(m=>m.User2).Where(u=>u.SenderId==userId).
                Select(u=> new Invite()
                {
                    ReciverId=u.ReciverId, 
                    SenderId=u.SenderId,
                    User2=new ApplicationUser()
                    {
                        UserName = u.User2.UserName
                    },
                    UserStatus= u.UserStatus,
                    UserActions= u.UserActions
                }).
                ToList();
            return data;
        }

        public string? GetIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var data = tokenHandler.ReadJwtToken(token);
            var userName = data.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            return userName;
        }

        public ICollection<FindUser> GetSpecificInvitations(string username,string senderId)
        {
            //var dbusers = _userManager.Users;
            //return dbusers.Where(u => u.UserName.Contains(username)).ToList();
            var data = _userManager.FindByIdAsync(senderId).Result;
            var dbusers = _userManager.Users;
            return dbusers.Where(u => u.UserName.Contains(username)).Select(m => new FindUser() { Id = m.Id, Name = m.UserName }).Where(u => u.Id != data.Id).ToList();
        }

        public bool UpdateAction(string senderId, string reciverId, int action)
        {
            var sender = _userManager.FindByIdAsync(senderId).Result;
            var reciver = _userManager.FindByIdAsync(reciverId).Result;
            if (reciver == null || sender == null) return false;
            var findInvitation = _context.Invites.FirstOrDefault(m => m.SenderId == senderId && m.ReciverId == reciverId);
            if (findInvitation == null) return false;
                findInvitation.UserActions = Invite.Action.Enable;

            _context.Update(findInvitation);
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateStatus(string senderId, string reciverId, int status)
        {
            var sender = _userManager.FindByIdAsync(senderId).Result;
            var reciver = _userManager.FindByIdAsync(reciverId).Result;
            if(reciver==null || sender==null) return false;
            var findInvitation = _context.Invites.FirstOrDefault(m => m.SenderId == senderId && m.ReciverId == reciverId);
            if (findInvitation == null) return false;
            findInvitation.UserStatus = (Invite.Status)status;
            if (findInvitation.UserStatus == Invite.Status.Approved)
                findInvitation.UserActions = Invite.Action.Enable;

            _context.Update(findInvitation);
            return _context.SaveChanges() == 1 ? true : false;
        }
    }
}
