using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using Topshelf.Logging;

namespace Bot.Services
{
    public class MainService
    {
        public static LogWriter Logger = HostLogger.Get<ILogger>();
        private static readonly TimeSpan UpdateInterval = TimeSpan.FromMinutes(20);
        public static readonly List<GroupModel> Groups = new List<GroupModel>();

        public static void Log(string message)
        {
            Logger.Log(LoggingLevel.Info, message);
        }
        private static readonly RestClient GroupClient = new RestClient("http://14.0.20.45:8888/groups.txt")
        {
            FollowRedirects = true,

        };
        private static readonly RestRequest GroupRequest = new RestRequest(Method.GET);

        public static async Task InitGroups()
        {
            GroupRequest.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            var result = await GroupClient.ExecuteGetTaskAsync(GroupRequest);
            if (string.IsNullOrEmpty(result.Content))
                return;
            Groups.Clear();
            var groups = Regex.Split(result.Content, "\r\n");
            foreach (var group in groups)
            {
                Groups.Add(new GroupModel()
                {
                    Id = group,
                    Interval = UpdateInterval
                });
            }
        }

    }
}
