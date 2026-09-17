namespace EduTek.Web.Services
{
    public interface IApiService
    {
        Task<string> GetAsync(string endpoint);
    }
}