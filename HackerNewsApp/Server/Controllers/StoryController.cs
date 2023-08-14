using Microsoft.AspNetCore.Mvc;
using IOFile = System.IO.File;
using HackerNewsApp.Shared;

namespace HackerNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private static readonly string path = "deleted.txt";


        public StoryController()
        {
            _httpClient = new();
        }

        [HttpGet("random/{size}")]
        [ProducesResponseType(typeof(List<Story>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int size)
        {
            var topStories = await _httpClient.GetFromJsonAsync<List<int>>("https://hacker-news.firebaseio.com/v0/topstories.json");

            var lst = new List<Story>();
            foreach (var id in topStories.Where(x => !IsDeleted(x)).Take(size))
            {
                var story = await _httpClient.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
                story.User = await _httpClient.GetFromJsonAsync<User>($"https://hacker-news.firebaseio.com/v0/user/{story.By}.json");
                lst.Add(story);
            }
            return Ok(lst.OrderBy(x => x.Score).ToList());

        }
 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete(int id)
        {
            if (!IsDeleted(id))
            {
                using StreamWriter writer = IOFile.AppendText(path);
                writer.WriteLine(id);
            }
            return Ok();
        }

        private static bool IsDeleted(int id)
        {
            IEnumerable<string> lines = IOFile.ReadLines(path);
            return lines.Contains(id.ToString());
        }
    }
}