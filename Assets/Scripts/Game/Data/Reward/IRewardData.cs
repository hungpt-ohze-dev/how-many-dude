public interface IRewardData
{
    void Add(int amount);
    void Multiply(int mul);
    IRewardData Clone();
    void Log();
}
