namespace Services.Infrastructure
{
    public interface IDatabaseFactory
    {
        ApplicationDb Get();
    }
}