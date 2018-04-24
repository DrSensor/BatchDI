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

        ScopedExService()
        {
            counter += 1;
        }
    }
}