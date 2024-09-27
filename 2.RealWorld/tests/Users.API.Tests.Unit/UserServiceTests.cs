using Azure.Core;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using RealWorld.WebAPI.Dtos;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Services;

namespace Users.API.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> logger = Substitute.For<ILoggerAdapter<UserService>>();
    private readonly CreateUserDto createUserDto = new("Gaye Tekin", 24, new(2000, 01, 01));
    private readonly UpdateUserDto updateUserDto = new(1, "Cemre Tekin", 24, new DateOnly(2000, 01, 01));
    private User user = new()
    {
        Id = 1,
        Name = "Gaye Tekin",
        Age = 24,
        DateOfBirth = new(2000, 01, 01)
    };

    public UserServiceTests()
    {
        _sut = new(userRepository, logger);
    }

    [Fact]
    public async void GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        //Arrange
        userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers_WhenSomeUserExist()
    {
        //Arrange
        var gayeUser = new User
        {
            Id = 1,
            Age = 24,
            Name = "Gaye Tekin",
            DateOfBirth = new(2000, 01, 01)
        };

        var cemreUser = new User
        {
            Id = 2,
            Age = 19,
            Name = "Cemre",
            DateOfBirth = new(2005, 01, 01)
        };

        var users = new List<User>() { gayeUser, cemreUser };

        userRepository.GetAllAsync().Returns(users);

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().BeEquivalentTo(users);
        result.Should().HaveCount(2);
        result.Should().NotHaveCount(3);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        await _sut.GetAllAsync();

        //Assert
        logger.Received(1).LogInformation(Arg.Is("All users are being retrieved."));
        logger.Received(1).LogInformation(Arg.Is("The entire user list has been fetched."));
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAnExcection_WhenExceptionIsThrown()
    {
        var exception = new ArgumentException("An error occurred while retrieving the user list.");
        userRepository.GetAllAsync().Throws(exception);

        //Act
        var requestAction = async() => await _sut.GetAllAsync();

        await requestAction.Should()
            .ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(Arg.Is(exception), Arg.Is("An error occurred while retrieving the user list."));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowAnError_WhenUserCreateDetailAreNotValid()
    {
        //Arrange
        CreateUserDto request = new("", 0, new(2007, 01, 01));

        //Act
        var action = async() => await _sut.CreateAsync(request);

        //Assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserNameExist()
    {
        //Arrange
        userRepository.NameIsExists(Arg.Any<string>()).Returns(true);

        //Act
        var action = async () => await _sut.CreateAsync(new("Gaye Tekin", 24, new DateOnly(2000, 01, 01)));

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void CreateAsync_ShouldCreateUserDtoToUserObject()
    {
        //Arrange
        CreateUserDto request = new("Gaye Tekin", 24, new(2000, 01, 01));

        //Act
        var user = _sut.CreateUserDtoToUserObject(request);

        //Assert
        user.Name.Should().Be(request.Name);
        user.Age.Should().Be(request.Age);
        user.DateOfBirth.Should().Be(request.DateOfBirth);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDetailsAreValidAndUnique()
    {
        //Arrange
        userRepository.NameIsExists(createUserDto.Name).Returns(false);
        userRepository.CreateAsync(Arg.Any<User>()).Returns(true);

        //Act
        var result = await _sut.CreateAsync(createUserDto);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrage
        userRepository.NameIsExists(createUserDto.Name).Returns(false);
        userRepository.CreateAsync(Arg.Any<User>()).Returns(true);

        //Act
        await _sut.CreateAsync(createUserDto);

        //Asert
        logger.Received(1).LogInformation(
            Arg.Is("User registration has started for the username: {0}."),
            Arg.Any<string>());

        logger.Received(1).LogInformation(
            Arg.Is("The user with User Id: {0} was created in {1}ms."),
            Arg.Any<int>(),
            Arg.Any<long>());
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("An error occurred during user registration.");
        userRepository.CreateAsync(Arg.Any<User>()).Throws(exception);

        //Act
        var action = async () => await _sut.CreateAsync(createUserDto);

        //Assert
        await action.Should()
            .ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(Arg.Is(exception), Arg.Is("An error occurred during user registration."));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrownAnError_WhenUserNotExist()
    {
        //Arrange
        int userId = 1;
        userRepository.GetByIdAsync(userId).ReturnsNull();

        //Act
        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExist()
    {
        //Arrange
        int userId = 1;
        User user = new()
        {
            Id = userId,
            Name = "Gaye Tekin",
            Age = 24,
            DateOfBirth = new(2000, 01, 01)
        };
        userRepository.GetByIdAsync(userId).Returns(user);
        userRepository.DeleteAsync(user).Returns(true);

        //Act
        var result = await _sut.DeleteByIdAsync(userId);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        int userId = 1;
        var user = new User()
        {
            Id = userId,
            DateOfBirth = new(2000, 01, 01),
            Name = "Gaye Tekin",
            Age = 34
        };
        userRepository.GetByIdAsync(userId).Returns(user);
        userRepository.DeleteAsync(user).Returns(true);

        //Act
        await _sut.DeleteByIdAsync(userId);

        //Assert
        logger.Received(1).LogInformation(
            Arg.Is("The user with ID {0} is being deleted..."),
            Arg.Is(userId));

        logger.Received(1).LogInformation(
            Arg.Is("The user record with User ID {0} was deleted in {1}ms."),
            Arg.Is(userId),
            Arg.Any<long>());
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        int userId = 1;
        var user = new User()
        {
            Id = userId,
            Name = "Gaye Tekin",
            Age = 24,
            DateOfBirth = new(2000, 01, 01)
        };
        userRepository.GetByIdAsync(userId).Returns(user);
        var exception = new ArgumentException("An error occurred while deleting the user record.");
        userRepository.DeleteAsync(user).Throws(exception);

        //Act
        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(Arg.Is(exception), Arg.Is("An error occurred while deleting the user record."));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAndError_WhenUserNotExist()
    {
        //Arrange        
        userRepository.GetByIdAsync(updateUserDto.Id).ReturnsNull();

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrownAnError_WhenUserUpdateDetailAreNotValid()
    {
        //Arrange
        UpdateUserDto updateUserDto = new(1, "", 18, new DateOnly(1989, 09, 03));
        userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrownError_WhenUserNameExist()
    {
        //Arrange
        userRepository.NameIsExists(Arg.Any<string>()).Returns(true);
        userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);

        //Act
        var action = async ()=> await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void UpdateAsync_ShouldCreateUpdateUserDtoToObject()
    {
        //Act
        _sut.CreateUpdateUserObject(ref user, updateUserDto);

        //Assert
        user.Name.Should().Be(updateUserDto.Name);
        user.Age.Should().Be(updateUserDto.Age);
        user.DateOfBirth.Should().Be(updateUserDto.DateOfBirth);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenDetailsAreValidAndUnique()
    {
        //Arrange
        userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        userRepository.UpdateAsync(user).Returns(true);

        //Act
        var result = await _sut.UpdateAsync(updateUserDto);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task UpdateAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        userRepository.UpdateAsync(user).Returns(true);

        //Act
        await _sut.UpdateAsync(updateUserDto);

        //Assert
        logger.Received(1).LogInformation(
            Arg.Is("Updating process has started for user {0}."),
            Arg.Any<string>());

        logger.Received(1).LogInformation(
            Arg.Is("The update process for user with ID {0} was successfully completed in {1}ms."),
            Arg.Any<int>(),
            Arg.Any<long>());

    }

    [Fact]
    public async Task UpdateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("An error occurred during the user update.");
        userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        userRepository.UpdateAsync(Arg.Any<User>()).Throws(exception);

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should()
            .ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(
            Arg.Is(exception),
            Arg.Is("An error occurred during the user update."));
    }
}