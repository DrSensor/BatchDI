using System;

namespace Example.Services
{
    public class ScopedExService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return counter;
            }
        }

        public ScopedExService()
        {
            counter += 1;
        }
    }
}