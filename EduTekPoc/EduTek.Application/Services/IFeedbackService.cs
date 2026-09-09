using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDto>> GetAllAsync();

        Task<FeedbackDto?> GetByIdAsync(int id);

        Task<FeedbackDto> AddAsync(CreateFeedbackDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateFeedbackDto dto);

        Task<bool> DeleteAsync(int id);
    }
}