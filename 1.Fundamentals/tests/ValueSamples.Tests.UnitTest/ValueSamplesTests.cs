namespace ValueSamples.Tests.UnitTest;

using ValueSample;
using FluentAssertions;

public class ValueSamplesTests
{
    //Arrange
    private readonly ValueSamples _sut = new();

    [Fact]
    public void StringAssertionExample()
    {
        //Act
        var fullName = _sut.FullName;

        //Assert
        fullName.Should().Be("Gaye Tekin");
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Gaye");
        fullName.Should().EndWith("Tekin");
    }

    [Fact]
    public void NumberAssertionExample() 
    {
        var age = _sut.Age;

        age.Should().Be(24);
        age.Should().BePositive();
        age.Should().BeGreaterThan(20);
        age.Should().BeLessThanOrEqualTo(25);
        age.Should().BeInRange(20, 30);
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        //Arrange
        var expectedUser = new User()
        {
            FullName = "Gaye Tekin",
            Age = 24,
            DateOfBirth = new(2000, 01, 01)
        };

        //Act
        var user = _sut.user;

        //Assert
        //user.Should().Be(expectedUser); // Does not match because it checks by reference.
        user.Should().BeEquivalentTo(expectedUser); // Controls over values.
    }

    [Fact]
    public void EnumrableObjectAssertionExample()
    {
        //Arrange
        var expected = new User
        {
            FullName = "Gaye Tekin",
            Age = 24,
            DateOfBirth = new(2000, 01, 01)
        };

        //Act
        var users = _sut.Users.As<User[]>();

        //Assert
        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Ali") && x.Age > 5);
    }

    [Fact]
    public void EnumrableNumberAssertionExample()
    {
        //Act
        var numbers = _sut.Numbers.As<int[]>();

        //Assert
        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        //Act
        Action result = () => _sut.Divide(1, 0);

        //Assert
        result.Should().Throw<DivideByZeroException>().WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        //Arrange
        var monitorSubject = _sut.Monitor();

        //Act
        _sut.RaiseExampleEvent();

        //Assert
        monitorSubject.Should().Raise("ExampleEvent");
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        //Act
        var number = _sut.InternalSecretNumber;

        //Assert
        number.Should().Be(10);
    }
}

