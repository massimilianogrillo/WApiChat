using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Hubs
{
    /// <summary>
    /// Signalr Hub. This class serves as a web hook for clients that want to get notifications
    /// </summary>
    public class MessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Query["userId"].ToString();

            //add the connection to the group that corresponds to the user id
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            SignalRConnectionToGroupsMap.TryAddGroup(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            List<string> groups = new List<string>();
            if (SignalRConnectionToGroupsMap.TryRemoveConnection(Context.ConnectionId, out groups))
            {
                //remove connection from all the groups
                if (groups != null && groups.Count > 0)
                {
                    foreach (var groupName in groups)
                    {
                        Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                    }
                }
            }
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Maps connection Ids to groups
        /// </summary>
        public static class SignalRConnectionToGroupsMap
        {
            private static readonly ConcurrentDictionary<string, List<string>> Map = new ConcurrentDictionary<string, List<string>>();

            /// <summary>
            /// Tries to add a connectionid to a group.
            /// </summary>
            /// <param name="connectionId"></param>
            /// <param name="groupName"></param>
            /// <returns></returns>
            public static bool TryAddGroup(string connectionId, string groupName)
            {
                List<string> groups;

                if (!Map.TryGetValue(connectionId, out groups))
                {
                    return Map.TryAdd(connectionId, new List<string>() { groupName });
                }

                if (!groups.Contains(groupName))
                {
                    groups.Add(groupName);
                }

                return true;
            }

            // since for this use case we will only want to get the List of group names
            // when we're removing the mapping - we might as well remove the mapping while
            // we're grabbing the List
            public static bool TryRemoveConnection(string connectionId, out List<string> result)
            {
                return Map.TryRemove(connectionId, out result);
            }
        }
    }
}
