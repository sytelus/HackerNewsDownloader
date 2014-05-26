using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public static class MessagePipe
    {
        private class Listner
        {
            public int Channel { get; set; }
            public Action<object> Callback { get; set; }
        }

        private static readonly ConcurrentDictionary<string, Listner> listeners = new ConcurrentDictionary<string, Listner>();
        public static bool AddListner(Action<object> callback, int channel = -1, string listnerKey = null)
        {
            return listeners.TryAdd(listnerKey ?? (Guid.NewGuid().ToString()), new Listner() {Channel = channel, Callback = callback});
        }

        public static void SendMessage(object message, int channel = -1)
        {
            foreach (var listener in listeners.Values.Where(l => l.Channel.CompareTo(channel) == 0))
            {
                listener.Callback(message);
            }
        }

        public static bool RemoveListner(string listnerKey)
        {
            Listner listner;
            return listeners.TryRemove(listnerKey, out listner);
        }
    }
}
