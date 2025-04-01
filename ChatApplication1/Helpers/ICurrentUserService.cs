using ChatApplication1.Models;

namespace ChatApplication1.Helpers
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Task<AppUser> GetUser();
    }
}
