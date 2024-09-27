using FluentValidation;
using RealWorld.WebAPI.Dtos;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Validators;
using System.Diagnostics;

namespace RealWorld.WebAPI.Services;

public sealed class UserService(
    IUserRepository userRepository, ILoggerAdapter<UserService> logger) : IUserService
{
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("All users are being retrieved.");

        try
        {
            return await userRepository.GetAllAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving the user list.");
            throw;
        }
        finally
        {
            logger.LogInformation("The entire user list has been fetched.");
        }
    }

    public async Task<bool> CreateAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        CreateUserDtoValidator validator = new();
        var result = validator.Validate(request);
        if(!result.IsValid)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(s => s.ErrorMessage)));
        }

        var nameIsExist = await userRepository.NameIsExists(request.Name, cancellationToken);
        if(nameIsExist)
        {
            throw new ArgumentException("This name has already been registered before.");
        }

        var user = CreateUserDtoToUserObject(request);

        logger.LogInformation("User registration has started for the username: {0}.", user.Name);

        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await userRepository.CreateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during user registration.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation("The user with User Id: {0} was created in {1}ms.", user.Id, stopWatch.ElapsedMilliseconds);
        }

    }

    public User CreateUserDtoToUserObject(CreateUserDto request)
    {
        return new User()
        {
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Age = request.Age,
        };
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found.");
        }

        logger.LogInformation("The user with ID {0} is being deleted...", id);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await userRepository.DeleteAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the user record.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation("The user record with User ID {0} was deleted in {1}ms.", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> UpdateAsync(UpdateUserDto request, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found.");
        }

        UpdateUserDtoValidator validator = new();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join("\n", result.Errors.Select(s => s.ErrorMessage)));
        }

        if (request.Name != user.Name)
        {
            var nameIsExist = await userRepository.NameIsExists(request.Name, cancellationToken);
            if (nameIsExist)
            {
                throw new ArgumentException("This name has already been registered.");
            }
        }

        CreateUpdateUserObject(ref user, request);

        logger.LogInformation("Updating process has started for user {0}.", request.Name);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the user update.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation("The update process for user with ID {0} was successfully completed in {1}ms.", user.Id, stopWatch.ElapsedMilliseconds);
        }

    }

    public void CreateUpdateUserObject(ref User user, UpdateUserDto request)
    {
        user.Name = request.Name;
        user.Age = request.Age;
        user.DateOfBirth = request.DateOfBirth;
    }
}
