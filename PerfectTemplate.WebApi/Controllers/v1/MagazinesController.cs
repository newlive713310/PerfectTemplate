using Microsoft.AspNetCore.Mvc;

namespace PerfectTemplate.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MagazinesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Check(string[] wordsInMagazine, string[] wordsInNote)
        {
            var wordCount = wordsInMagazine.GroupBy(word => word)
                                    .ToDictionary(group => group.Key, group => group.Count());

            foreach (var word in wordsInNote)
            {
                if (wordCount.TryGetValue(word, out int count))
                {
                    if (count == 0)
                    {
                        return Ok(false);
                    }
                    wordCount[word]--;
                }
                else
                {
                    return Ok(false);
                }
            }

            return Ok(true);
        }
    }
}
