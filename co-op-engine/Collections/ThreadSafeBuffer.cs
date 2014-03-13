using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Collections
{
    /// <summary>
    /// A threadsafe list that allows for very fast data sharing among threads
    /// </summary>
    /// <typeparam name="T">type of object the list will hold</typeparam>
    public class ThreadSafeBuffer<T>
    {
        private List<T> data;
        object objectLock;

        public ThreadSafeBuffer()
        {
            data = new List<T>();
            objectLock = new object();
        }

        /// <summary>
        /// Adds a new object to the buffer
        /// </summary>
        /// <param name="newData">object to add</param>
        public void Add(T newData)
        {
            lock (objectLock)
            {
                data.Add(newData);
            }
        }

        /// <summary>
        /// Grabs all data from the list and 'clears' it
        /// </summary>
        /// <returns>list of objects in the buffer</returns>
        public List<T> Gather()
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
