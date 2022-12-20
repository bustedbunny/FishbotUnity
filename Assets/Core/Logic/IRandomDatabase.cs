namespace Fishbot
{
    public interface IRandomDatabase<T>
    {
        public T this[int index] { get; set; }
    }
}