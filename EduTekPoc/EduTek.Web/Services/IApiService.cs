namespace EduTek.Web.Services
{
    public interface IApiService
    {
        Task<string> GetAsync(string endpoint);
        Task<string> PostAsync(string endpoint, object data);
        Task<string> PutAsync(string endpoint, object data);

        Task<string> DeleteAsync(string endpoint);
    }
}