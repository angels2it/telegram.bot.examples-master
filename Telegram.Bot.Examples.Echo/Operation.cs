using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bot.Services;
using RestSharp;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Topshelf.Logging;

namespace Telegram.Bot.Examples.Echo
{
    public class Operation
    {
        private readonly DataService _dataService;
        private readonly FacebookService _facebookService;
        private readonly TelegramBotClient _bot;
        private readonly RestClient _client = new RestClient("https://scontent.xx.fbcdn.net/")
        {
            FollowRedirects = true
        };

        public Operation(DataService dataService, FacebookService facebookService, TelegramBotClient bot)
        {
            _dataService = dataService;
            _facebookService = facebookService;
            _bot = bot;
        }

        public async Task Update(string groupId)
        {
            var chatIds = await _dataService.GetChatIds();
            if (chatIds.Count == 0)
                return;
            var images = await _facebookService.GetLatestPhotoAsync(groupId);
            if (images == null || images.Count == 0)
            {
                return;
            }

            foreach (var chat in chatIds)
            {
                try
                {
                    await _bot.SendChatActionAsync(chat.Id, ChatAction.Typing);
                    foreach (var image in images)
                    {
                        try
                        {
                            var request = new RestRequest(image.Replace("https://scontent.xx.fbcdn.net/", ""), Method.GET);
                            var data = await _client.ExecuteGetTaskAsync(request);
                            var fts = new FileToSend("Test", new MemoryStream(data.RawBytes));
                            await _bot.SendPhotoAsync(chat.Id, fts, $"Nice Picture from {groupId}");
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }

                }
                catch (ApiRequestException e)
                {
                    await _dataService.RemoveChatId(chat.Id);
                    MainService.Logger.Log(LoggingLevel.Error, e);
                }
                catch (Exception e)
                {
                    MainService.Logger.Log(LoggingLevel.Error, e);
                }
            }
        }

        private async Task SendMessageAsync(long chatId, string text)
        {
            try
            {
                await _bot.SendTextMessageAsync(chatId, text);
            }
            catch (ApiRequestException e)
            {
                await _dataService.RemoveChatId(chatId);
                MainService.Logger.Log(LoggingLevel.Error, e);
            }
            catch (Exception e)
            {
                MainService.Logger.Log(LoggingLevel.Error, e);
            }
        }
    }
}
