using SimpleAuthSystem.Application.DTOs;

namespace SimpleAuthSystem.Infrastructure.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {Id}", id);
                    return Result<UserDTO>.Failure("User not found");
                }

                var userDto = _mapper.Map<UserDTO>(user);
                _logger.LogInformation("User fetched successfully: {Id}", id);
                return Result<UserDTO>.Success(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID: {Id}", id);
                throw new NotFoundException("Failed to fetch user: " + ex.Message);

            }
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                    return Result<UserDTO>.Failure("User not found");
                }

                var userDto = _mapper.Map<UserDTO>(user);
                _logger.LogInformation("User fetched successfully: {Email}", email);
                return Result<UserDTO>.Success(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with email: {Email}", email);
                throw new NotFoundException("Failed to fetch user: " + ex.Message);
            }
        }

        public async Task<Result<PageList<UserDTO>>> GetAllUserAsync(int pageSize, int currentPage)
        {
            try
            {
                _logger.LogInformation("Getting all users with pagination - Page: {Page}, Size: {Size}", currentPage, pageSize);
                var users = await _unitOfWork.Users.GetAllAsync();

                int effectivePageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 50);
                int effectivePage = currentPage <= 0 ? 1 : currentPage;

                var totalCount = users.Count();
                var pagedList = PageList<UserDTO>.Create(
                    users,
                    totalCount,
                    effectivePageSize,
                    effectivePage,
                    items => _mapper.Map<List<UserDTO>>(items)
                );
                return Result<PageList<UserDTO>>.Success(pagedList, "Users retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users: {Message}", ex.Message);
                return Result<PageList<UserDTO>>.Failure("Failed to retrieve users: " + ex.Message);
            }
        }
    }
}
