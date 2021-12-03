using Mars.Rover.Domain.Aggregate.Base;

namespace Mars.Rover.Domain.Aggregate.Navigate
{
    public class Plataeu : BaseEntity
    {
        public virtual Surface Size { get; private set; }

        public Plataeu() { }

        public void Define(int width, int height)
        {
            Size = new Surface(width, height);
        }
    }
}
