# Sample Unit Test Project

This repository contains two projects with unit test examples.

In the first project the class `ValueSamples` is defined with various data and operations. Unit tests are located in the test project `ValueSamples.Tests.UnitTest`.

The other project is an API application that provides a service for user management. It includes a `UserService` class that performs operations such as creating, updating, deleting, and listing user data. The unit tests for this class are found in the test project named `Users.API.Tests.Unit`.

## Project Structure

### 1. Fundamentals
- `ValueSample`: Main application project.
- `ValueSample.Tests.UnitTest`: Project containing unit tests.

### 2. Real World
- `UserService`: Main application project.
- `Users.API.Tests.Unit`: Project containing unit tests.

## Technologies

- **FluentValidation**: Used for data validation operations.
- **FluentAssertions**: Used to check expected results in tests.
- **XUnit**: Test framework.
- **NSubstitute**: Used for creating mock objects in unit tests.

---


## RealWorld.WebAPI Project Structure

The project includes the following components:

- **Dtos**: Data transfer objects.
- **Logging**: Interfaces and implementations necessary for logging operations.
- **Models**: Model classes used within the application.
- **Repositories**: Data access layer.
- **Services**: Contains business logic.
- **Validators**: Classes used to validate input data.

### User Service Class

The `UserService` class is the basic service class that performs user operations. It contains the following methods:

- **GetAllAsync**: Retrieves all users.
- **CreateAsync**: Creates a new user.
- **DeleteByIdAsync**: Deletes a user with a specific ID.
- **UpdateAsync**: Updates an existing user.

### Error Management

Errors that occur during operations are logged using the `ILogger` interface, making the debugging process easier.

### Used Testing Libraries

1. **XUnit**: 
   - **Description**: A popular test framework for .NET applications with a simple and user-friendly structure.
   - **Purpose**: Used to define test classes and methods and to report the results of tests.

2. **NSubstitute**: 
   - **Description**: A mock object library for .NET. It is used to simulate dependencies in tests.
   - **Purpose**: Helps isolate dependencies by creating fake objects instead of using real ones for testing classes like `UserService`.


### Test Scenarios

- `GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist`: Tests that the GetAllAsync method returns an empty list when there are no users.
- `GetAllAsync_ShouldReturnUsers_WhenSomeUserExist`: Tests that the GetAllAsync method returns the existing users when some are present.
- `GetAllAsync_ShouldLogMessages_WhenInvoked`: Verifies that certain log messages are recorded when the GetAllAsync method is called.
- `GetAllAsync_ShouldLogMessageAnException_WhenExceptionIsThrown`: Tests that the error log message is recorded correctly when the GetAllAsync method throws an exception.
- `CreateAsync_ShouldThrowAnError_WhenUserCreateDetailAreNotValid`: Tests that the CreateAsync method throws an error when user creation details are invalid.
- `CreateAsync_ShouldThrownAnError_WhenUserNameExist`: Tests that the CreateAsync method throws an error when the username already exists.
- `CreateAsync_ShouldCreateUserDtoToUserObject`: Verifies that the CreateUserDto object is correctly converted to a User object.
- `CreateAsync_ShouldCreateUser_WhenDetailsAreValidAndUnique`: Tests that the CreateAsync method successfully creates a user when the information is valid and unique.
- `CreateAsync_ShouldLogMessages_WhenInvoked`: Verifies that certain log messages are recorded when the CreateAsync method is called.
- `CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown`: Tests that the error log message is recorded correctly when the CreateAsync method throws an exception.
- `DeleteByIdAsync_ShouldThrownAnError_WhenUserNotExist`: Tests that the DeleteByIdAsync method throws an error when the user to be deleted does not exist.
- `DeleteByIdAsync_ShouldDeleteUser_WhenUserExist`: Tests that the DeleteByIdAsync method successfully deletes the user when it exists.
- `DeleteByIdAsync_ShouldLogMessages_WhenInvoked`: Verifies that certain log messages are recorded when the DeleteByIdAsync method is called.
- `DeleteByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown`: Tests that the error log message is recorded correctly when the DeleteByIdAsync method throws an exception.
- `UpdateAsync_ShouldThrowAndError_WhenUserNotExist`: Tests that the UpdateAsync method throws an error when the user to be updated does not exist.
- `UpdateAsync_ShouldThrownAnError_WhenUserUpdateDetailAreNotValid`: Tests that the UpdateAsync method throws an error when user update details are invalid.
- `UpdateAsync_ShouldThrownError_WhenUserNameExist`: Tests that the UpdateAsync method throws an error when the username already exists.
- `UpdateAsync_ShouldCreateUpdateUserDtoToObject`: Verifies that the UpdateUserDto object is correctly converted to a User object.
- `UpdateAsync_ShouldUpdateUser_WhenDetailsAreValidAndUnique`: Tests that the UpdateAsync method successfully updates the user when the update information is valid and unique.
- `UpdateAsync_ShouldLogMessages_WhenInvoked`: Verifies that certain log messages are recorded when the UpdateAsync method is called.
- `UpdateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown`: Tests that the error log message is recorded correctly when the UpdateAsync method throws an exception.

These tests are designed to verify the expected behavior of the `UserService` class under various scenarios.

---

## ValueSample Project Unit Tests

Unit tests have been written using the `FluentAssertions` library to test various scenarios. The tests perform different types of validations on the properties and methods of the `ValueSamples` class.

### Test Scenarios

1. **StringAssertionExample**: Checks the correct value of the `FullName` property and some conditions.
2. **NumberAssertionExample**: Checks the correct value of the `Age` property and some conditions.
3. **ObjectAssertionExample**: Checks whether the `user` object matches the expected `User` object.
4. **EnumerableObjectAssertionExample**: Checks for a specific user in the `Users` collection and the size of the collection.
5. **EnumerableNumberAssertionExample**: Checks for the presence of a specific number in the `Numbers` collection.
6. **ExceptionThrownAssertionExample**: Checks whether the `Divide` method throws a `DivideByZeroException` in the case of division by zero.
7. **EventRaisedAssertionExample**: Checks whether the `ExampleEvent` event is triggered.
8. **TestingInternalMembersExample**: Checks the value of the `InternalSecretNumber` property.
