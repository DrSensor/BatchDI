using System;

namespace Example.Services
{
    public class TransientExService : ITransientService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return counter;
            }
        }

        TransientExService()
        {
            counter += 1;
        }
    }
}