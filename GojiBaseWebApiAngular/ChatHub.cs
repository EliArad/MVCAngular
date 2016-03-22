using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace SignalRChat
{
    
    public class ChatHub : Hub
    {
        public static ConcurrentDictionary<string, string> MyUsers = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> HomeUsers = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message)
        {
            Console.WriteLine(name, message);
            try
            {
                MyUsers[name] = message;
            }
            catch (Exception err)
            {

            }
        }
        public void registerEstimator(string connectionId, string fieldGuid)
        {
            //Console.WriteLine(connectionId, fieldGuid);
            try
            {
                //string[] s = ipAddress.Split(new Char[] { ' ' });
                MyUsers[connectionId] = fieldGuid; // s[0];
            }
            catch (Exception err)
            {

            }
        }

        public void registerHome(string connectionId, string userName)
        {
            //Console.WriteLine(connectionId, fieldGuid);
            try
            {
                //string[] s = ipAddress.Split(new Char[] { ' ' });
                HomeUsers[connectionId] = userName; // s[0];
            }
            catch (Exception err)
            {

            }
        }

        public override Task OnConnected()
        {
            MyUsers.TryAdd(Context.ConnectionId, string.Empty);
            HomeUsers.TryAdd(Context.ConnectionId, string.Empty);
            return base.OnConnected();
        }
        /*
        public override Task OnDisconnected()
        {
            string garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            HomeUsers.TryRemove(Context.ConnectionId, out garbage);

            return base.OnDisconnected();
        }
         */
    }
 
}