using System;

namespace Example.Services
{
    public class SingletonExService
    {
        private int counter = 0;
        public int count
        {
            get
            {
                return counter;
            }
        }

        public SingletonExService()
        {
            counter += 1;
        }
    }
}