

using Mars.Rover.Domain.Repository;

namespace Mars.Rover.Domain.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly Context.Context.MarsRoverContext context;
        //private RefTypeRepository RefTypeRepository;

        public RepositoryWrapper(Context.Context.MarsRoverContext Context)
        {
            context = Context;
        }

        //public IRefTypeRepository RefTypeRepository
        //{
        //    get
        //    {
        //        if (RefTypeRepository == null) RefTypeRepository = new RefTypeRepository(context);

        //        return RefTypeRepository;
        //    }
        //}
    }
}