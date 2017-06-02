using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace Bot.Services
{
    public class FacebookService
    {
        private readonly string _token;
        readonly RestClient _client = new RestClient("https://graph.facebook.com/");
        private readonly DataService _dataService;
        public FacebookService(string token)
        {
            _token = token;
            _dataService = new DataService();
        }
        public async Task<List<string>> GetLatestPhotoAsync(string groupId)
        {
            var request = new RestRequest($"{groupId}/posts?fields=id,status_type&limit=10&access_token={_token}");
            var result = await _client.ExecuteGetTaskAsync<RootObject>(request);
            if (result?.Data == null || result.Data.Data.Count == 0)
                return null;
            var latestId = await _dataService.GetLatestId(groupId);
            PostData post;
            if (string.IsNullOrEmpty(latestId))
                post = result.Data.Data.LastOrDefault();
            else
            {
                var index = result.Data.Data.FindIndex(e => e.Id == latestId);
                post = index < 1 ? result.Data.Data.FirstOrDefault() : result.Data.Data[index - 1];
            }
            if (post == null || post.Id == latestId)
                return null;
            if (post.StatusType != "added_photos")
                return null;
            // get full picture
            var pictures = await GetPhotosByPostId(post.Id);
            if (pictures == null)
                return null;
            await _dataService.AddPost(groupId, post.Id);
            return pictures;
        }

        private async Task<List<string>> GetPhotosByPostId(string id)
        {
            var request = new RestRequest($"{id}/attachments?access_token={_token}");
            var result = await _client.ExecuteGetTaskAsync<PostAttachment>(request);
            List<string> photos = new List<string>();
            if (result?.Data?.Data == null)
                return photos;
            foreach (var data in result.Data.Data)
            {
                if (data.Subattachments?.Data == null)
                {
                    if (data.Type == "photo" && !string.IsNullOrEmpty(data.Media?.Image?.Src))
                        photos.Add(data.Media.Image.Src);
                    continue;
                }
                foreach (var subattachment in data.Subattachments.Data)
                {
                    if (subattachment.Type != "photo")
                        continue;
                    photos.Add(subattachment.Media.Image.Src);
                }
            }
            return photos;
        }
    }

    public class Image
    {
        public int Height { get; set; }
        public string Src { get; set; }
        public int Width { get; set; }
    }

    public class Media
    {
        public Image Image { get; set; }
    }

    public class Target
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class Datum2
    {
        public Media Media { get; set; }
        public Target Target { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class Subattachments
    {
        public List<Datum2> Data { get; set; }
    }

    public class Target2
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class PostAttachmentData
    {
        public Subattachments Subattachments { get; set; }
        public Media Media { get; set; }
        public Target2 Target { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class PostAttachment
    {
        public List<PostAttachmentData> Data { get; set; }
    }

    public class PostDetailData
    {
        public string FullPicture { get; set; }
        public string CreatedTime { get; set; }
        public string Id { get; set; }
    }

    public class From
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Id { get; set; }
    }

    public class Action
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Privacy
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string Friends { get; set; }
        public string Allow { get; set; }
        public string Deny { get; set; }
    }



    public class Cursors
    {
        public string Before { get; set; }
        public string After { get; set; }
    }

    public class Paging
    {
        public Cursors Cursors { get; set; }
    }

    public class PostData
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string StatusType { get; set; }
        public string ObjectId { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class Paging2
    {
        public string Previous { get; set; }
        public string Next { get; set; }
    }

    public class RootObject
    {
        public List<PostData> Data { get; set; }
    }
}
