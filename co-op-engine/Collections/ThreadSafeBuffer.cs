using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Collections
{
    class ThreadSafeBuffer<T>
    {
        private List<T> data;
        object objectLock;

        public ThreadSafeBuffer()
        {
            data = new List<T>();
            objectLock = new object();
        }

        public void AddToList(T newData)
        {
            lock (objectLock)
            {
                data.Add(newData);
            }
        }

        public List<T> RetrieveData()
        {
            List<T> returnData = new List<T>();
            List<T> newDataList = new List<T>(); //saves time in locked space if it's JUST a ref swap
            lock (objectLock)
            {
                returnData = data;
                data = newDataList;
            }

            return returnData;
        }
    }
}
