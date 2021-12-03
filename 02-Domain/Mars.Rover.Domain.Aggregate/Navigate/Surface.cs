using Mars.Rover.Domain.Aggregate.Base;

namespace Mars.Rover.Domain.Aggregate.Navigate
{
    public class Surface : BaseEntity
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Surface()
        {
        }

        public Surface(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
