using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Services;
using Bot.Web;
using Hangfire;
using Microsoft.Bot.Connector;
using Microsoft.Owin;
using Owin;
using RestSharp;
using Topshelf.Logging;

[assembly: OwinStartup(typeof(Startup))]

namespace Bot.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire");

            app.UseHangfireDashboard();
            app.UseHangfireServer();


            RecurringJob.AddOrUpdate(() => StartUpdateTask(), Cron.MinuteInterval(10));
        }
        public static readonly RestClient _client = new RestClient("https://scontent.xx.fbcdn.net/")
        {
            FollowRedirects = true
        };
        public async Task StartUpdateTask()
        {
            var context = new Model1Container();
            var groups = context.Groups.ToList();
            var chats = context.Chats.ToList();
            FacebookService facebook = new FacebookService("EAAIdW72eQaoBAFqxZBxvnXXpx69AHJrqFlNB6R8RJB0jJi8ZClhvxF01OC9oUupFUpSEzgk9UZAmmn9unC1FSIV9JStDvGlfBOeYZBU0hU5n9bHhrOlEzEOpr6FvmkZC6mQ0IuNcdhaIlIUJN5f6WVIsZCv0WmZABMZD");

            foreach (var @group in groups)
            {
                var images = await facebook.GetLatestPhotoAsync(group.Key);
                if (images == null || images.Count == 0)
                {
                    return;
                }

                foreach (var chat in chats)
                {
                    try
                    {
                        foreach (var image in images)
                        {
                            try
                            {
                                var connector = new ConnectorClient(new Uri(chat.ServiceUrl));
                                IMessageActivity newMessage = Activity.CreateMessageActivity();
                                newMessage.ChannelId = chat.ChannelId;
                                newMessage.Type = ActivityTypes.Message;
                                newMessage.From = new ChannelAccount("angels2it");
                                newMessage.Conversation = new ConversationAccount(false, chat.ConversasionId);
                                newMessage.Recipient = new ChannelAccount(chat.SubcriberId);
                                newMessage.Text = $"Nice picture from {@group.Key}";
                                newMessage.Attachments = new List<Attachment>()
                                {
                                    new Attachment()
                                    {
                                        ContentType = "image/png",
                                        ContentUrl = image
                                    }
                                };
                                await connector.Conversations.SendToConversationAsync((Activity)newMessage);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }

                    }
                    
                    catch (Exception e)
                    {
                        MainService.Logger.Log(LoggingLevel.Error, e);
                    }
                }
            }
        }
    }
}
