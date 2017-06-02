using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Topshelf.Logging;

namespace Bot.Services
{
    public class DataService
    {
        readonly Model1Container _container = new Model1Container();

        public async Task AddPost(string groupId, string postId)
        {
            _container.Posts.Add(new Post()
            {
                GroupId = groupId,
                PostId = postId
            });
            await _container.SaveChangesAsync();
        }

        private async Task<List<string>> GetPosts(string groupId)
        {
            try
            {
                var ids = await _container.Posts.Where(e => e.GroupId == groupId).Select(e=>e.PostId).ToListAsync();
                return ids;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public async Task<List<ChatModel>> GetChatIds()
        {
            try
            {
                var chats = await _container.Chats.ToListAsync() ?? new List<Chat>();
                return chats.Select(e => new ChatModel()
                {
                    Id = e.SubcriberId
                }).ToList();
            }
            catch (Exception e)
            {
                return new List<ChatModel>();
            }
        }
        public async Task RemoveChatId(string chatId)
        {
            var chatIds = await GetChatIds();
            chatIds.RemoveAll(e => e.Id == chatId);
        }

        public async Task AddChatId(IActivity activity)
        {
            var chatIds = await GetChatIds();
            if (chatIds.Any(e => e.Id == activity.From.Id))
                return;
            _container.Chats.AddOrUpdate(new Chat()
            {
                SubcriberId = activity.From.Id,
                ServiceUrl = activity.ServiceUrl,
                ConversasionId = activity.Conversation.Id,
                ChannelId = activity.ChannelId
            });
            _container.SaveChanges();
        }

        public async Task<string> GetLatestId(string groupId)
        {
            try
            {
                var ids = await GetPosts(groupId);
                return ids.LastOrDefault();
            }
            catch (Exception ex)
            {
                MainService.Logger.Log(LoggingLevel.Error, ex);
                return string.Empty;
            }
        }
    }
}
