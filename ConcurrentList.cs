using System.Collections.Generic;

namespace APS_MVC
{
    public class ConcurrentList<T>
    {
        private List<T> _list = new();
        private object _lock = new object();
        public void Add(T item)
        {
            lock(_lock)
            {
                _list.Add(item);
            }
        }
        public void Remove(T item)
        {
            lock(_lock)
            {
                var delete_value = _list.FirstOrDefault(i => i.Equals(item));
                if(delete_value != null)
                {
                    _list.Remove(delete_value);
                }
            }
        }
        public IEnumerable<T> Get()
        {
            foreach (var item in _list)
            {
                lock (_lock)
                {
                    yield return item;
                }
            }
        }
    }
}
