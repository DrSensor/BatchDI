namespace Example.Services
{
    public class TransientService : ITransientService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return ++counter;
            }
        }
    }
}