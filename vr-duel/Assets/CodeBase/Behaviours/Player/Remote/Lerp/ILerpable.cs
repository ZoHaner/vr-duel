namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public interface ILerpable<T>
    {
        T CalculateLerpValue(float deltaTime);
        void SetNewCurrentAndTarget(T current, T target);
    }
}