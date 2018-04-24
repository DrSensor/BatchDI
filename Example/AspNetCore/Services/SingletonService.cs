using System;

namespace Example.Services
{
    public class SingletonService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return counter;
            }
        }

        public SingletonService()
        {
            counter += 1;
        }
    }
}