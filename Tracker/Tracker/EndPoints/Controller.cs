using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Tracker.Invitations;
using Tracker.Models;
using Tracker.Models.DTO;
using Tracker.Repository;

namespace Tracker.EndPoints
{
    public static class Controller
    {
        public static RouteGroupBuilder BooksAPI(this RouteGroupBuilder app)
        {

            app.MapGet("/Books", GetAllBooks);
            app.MapGet("/getBooks/{id}", GetBooksById);
            app.MapPost("/newBooks", NewBooks);
            app.MapPut("/updateBooks", UpdateBooks);
            app.MapDelete("/deleteBooks/{bookId}", DeleteBooks);
            
            return app;
        }

        public async static Task<IResult> GetAllBooks(IBookRepo bookRepo)
        {
            if (bookRepo == null) Results.BadRequest("First Add the bokks");
            return Results.Ok( await bookRepo.GetBooks());
        }

        public async static Task<IResult> GetBooksById(int id,IBookRepo bookRepo)
        {
            if (id == 0) Results.BadRequest("Id Not Matched");
            return Results.Ok(await bookRepo.GetBookById(id));
        }

        public async static Task<IResult> NewBooks(Book book, IBookRepo bookRepo, IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);
            if (getSenderId == null) return Results.BadRequest();
            book.UserId= getSenderId;
            await bookRepo.AddBook(book);
            if (book == null) return Results.BadRequest(new { Status = 0, Data = "not inserted data successfully"});
            return Results.Ok(new { Status = 1, Message = "data added successfully",Data=book });
        }

        public async static Task<IResult> UpdateBooks(Book book, IBookRepo bookRepo)
        {
            await bookRepo.UpdateBook( book);
            if (book == null) return Results.BadRequest(new { Data = "not updated data" });
            return Results.Ok(new { Status = 1, Message = "updated successfully",Data=book });
        }

        public async static Task<IResult> DeleteBooks(int bookId, IBookRepo bookRepo)
        {
            if (bookId == 0) return Results.BadRequest(new { Status = 0, Data = " data not deleted successfully" });
            await bookRepo.DeleteBook(bookId);
            return Results.Ok(new { Status = 1, Message = "data deleted successfully",Data=bookId });
        }

        public static RouteGroupBuilder LoginRegisterAPI(this RouteGroupBuilder app)
        {
            app.MapPost("/register", Register);
            app.MapPost("/login", Login);

            return app;
        }

        public async static Task<IResult> Register (UserDto book, IUserRepo _context, IMapper _mapper)
            {
                if (book == null) return Results.BadRequest();
                var ApplicationUser = _mapper.Map<ApplicationUser>(book);
                ApplicationUser.PasswordHash = book.Password;
                if (!await _context.IsUnique(book.Email)) return Results.NotFound("Go to login");
                var registerUser = await _context.RegisterUser(ApplicationUser);
                if (!registerUser) return Results.BadRequest("Register not successfully");
                return Results.Ok("Register Successfully");
            }

        public async static Task<IResult> Login (LoginDto book, IUserRepo _context)
            {
                if (await _context.IsUnique(book.Username)) return Results.BadRequest(new { Status = 1, Data = "Please Register" });
                var userAuthorize = await _context.AuthenticateUser(book.Username, book.Password);
                if (userAuthorize == null) return Results.NotFound(new { Status = 1, Data = "Invalid Attempt" });
                return Results.Ok( new {data="User login successfully",userAuthorize});
            }

        public static RouteGroupBuilder EmailVerification(this RouteGroupBuilder app)
        {
            app.MapPost("/email", Email);

            return app;
        }

        public static IResult Email(EmailDto request, IEmailSender _context)
        {
            _context.SendEmail(request);
            return Results.Ok(request);
        }

        public static RouteGroupBuilder SendInvitation(this RouteGroupBuilder app)
        {
            app.MapGet("/getAll", GetAll);
            app.MapGet("/invitation/{username}", Invitation);
            app.MapPost("/createinvitation", CreateInvitation).RequireAuthorization();
            app.MapGet("/status/{reciverId}/{status:int}", Status);
            app.MapGet("/action/{reciverId}/{action:int}", Action);
            app.MapGet("invitationcomesfrom", InvitationSender);

            return app;
        }

        public static IResult InvitationSender( IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = invitationRepo.GetIdFromToken(token);
            if (userId == null) return Results.BadRequest();
            var data = invitationRepo.InvitationComesFromUser(userId);
            return Results.Ok(data);
        }

        public static IResult GetAll(IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);
            var data = invitationRepo.GetAllRegisteredPersons(getSenderId);
            return Results.Ok(data);
        }

        public static IResult Invitation(string username,IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);
            var data = invitationRepo.GetSpecificInvitations(username,getSenderId);
            return Results.Ok(data);
        }

        public static IResult Status( string reciverId, int status, IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);
            if (getSenderId == null || reciverId == null || status == 0) return Results.BadRequest();

            if (!invitationRepo.UpdateStatus(reciverId, getSenderId, status))
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.Ok(new { Status = 1, Message = "Invitation Updated Successfully" });
        }

        public static IResult Action(string reciverId, int action, IInvitationRepo invitationRepo, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);
            if (getSenderId == null || reciverId == null || action == 0) return Results.BadRequest();

            if (!invitationRepo.UpdateAction(reciverId, getSenderId, action))
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.Ok(new { Status = 1, Message = "Invitation Updated Successfully" });
        }

        public static IResult CreateInvitation(FindUser invite,IHttpContextAccessor httpContextAccessor, IInvitationRepo invitationRepo)
        {
            if (string.IsNullOrEmpty(invite.Id))
                return Results.BadRequest();

            // here we will get the token form httpcontext ........
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var getSenderId = invitationRepo.GetIdFromToken(token);

            if (getSenderId == null)
                return Results.BadRequest(new { message = "your token doesnot contain user id " });


            var result = invitationRepo.CreateInvitation(getSenderId, invite.Id);
            return Results.Ok(result);
        }
    }
}
