namespace ValueSample;

public class ValueSamples
{
    public string FullName = "Gaye Tekin";

    public int Age = 24;

    public User user = new()
    {
        FullName = "Gaye Tekin",
        Age = 24,
        DateOfBirth = new(2000, 01, 01)
    };

    public IEnumerable<User> Users = new[]
    {
        new User()
        {
            FullName = "Gaye Tekin",
            Age = 24,
            DateOfBirth = new(2000, 01, 01)
        },
        new User()
        {
            FullName = "Ali Emir Tekin",
            Age = 10,
            DateOfBirth = new(2013, 01, 01)
        },
        new User()
        {
            FullName = "Cemre Tekin",
            Age = 19,
            DateOfBirth = new(2005, 01, 01)
        }
    };

    public IEnumerable<int> Numbers = new[] { 5, 10, 25, 50 };

    public float Divide(int a, int b)
    {
        if (a == 0 || b == 0)
        {
            throw new DivideByZeroException();
        }

        return a / b;
    }

    public event EventHandler ExampleEvent;
    public virtual void RaiseExampleEvent()
    {
        ExampleEvent(this, EventArgs.Empty);
    }

    internal int InternalSecretNumber = 10;
}

public sealed class User
{
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
