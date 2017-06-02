using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Akavache;
using Bot.Services;
using Telegram.Bot.Args;
using Telegram.Bot.Examples.Echo.CommandHandlers;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;

namespace Telegram.Bot.Examples.Echo
{
    public class TelegramService
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("335421811:AAHCc0fqp22ar-q5U9jIM3l1uVqdOQR_9Bc");
        private static readonly FacebookService Facebook = new FacebookService("EAAIdW72eQaoBAFqxZBxvnXXpx69AHJrqFlNB6R8RJB0jJi8ZClhvxF01OC9oUupFUpSEzgk9UZAmmn9unC1FSIV9JStDvGlfBOeYZBU0hU5n9bHhrOlEzEOpr6FvmkZC6mQ0IuNcdhaIlIUJN5f6WVIsZCv0WmZABMZD");
        private static readonly DataService DataService = new DataService();
        private static readonly Operation Operation = new Operation(DataService, Facebook, Bot);
        public static MainService MainService = new MainService();
        private readonly CommandList _commandList = new CommandList()
        {
            new StartCommand(DataService),
            new ResetGroupCommand(DataService),
            new ResetCommand(),
            new EndCommand(DataService),
            new UpdateGroupCommand(),
            new UpdateCommand(Operation)
        };

        async void StartApp()
        {

            BlobCache.ApplicationName = "TestApp";
            await MainService.InitGroups();
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;
            var me = await Bot.GetMeAsync();
            MainService.Log("Connected");
            Bot.StartReceiving();
            foreach (var group in MainService.Groups)
            {
                StartTimer(group);
            }
        }


        private async Task HandleTimer(string groupId)
        {
            await Operation.Update(groupId);
        }


        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };

            await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;
            _commandList.Process(Bot, message);
        }

        private void AddOrUpdateChatId(long chatId)
        {
            DataService.AddChatId(chatId).GetAwaiter();
        }

        public void StartTimer(GroupModel group)
        {
            Timer timer = new Timer(group.Interval.TotalMilliseconds);
            timer.Elapsed += async (sender, e) => await HandleTimer(group.Id);
            timer.Start();
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var chatId = callbackQueryEventArgs.CallbackQuery.Message.Chat.Id;
            AddOrUpdateChatId(chatId);
            await Bot.SendTextMessageAsync(chatId, "Ok, I'm added");
        }
        public void Start()
        {
            StartApp();
        }

        public void Stop()
        {

        }
    }
}