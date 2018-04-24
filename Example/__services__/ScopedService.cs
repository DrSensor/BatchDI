using System;

namespace Example.Services
{
    public class ScopedService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return counter;
            }
        }

        ScopedService()
        {
            counter += 1;
        }
    }
}