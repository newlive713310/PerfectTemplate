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
            Dictionary<string, int> _dict = wordsInMagazine.GroupBy(x =>x).ToDictionary(x => x.Key, x => x.Count());

            for (int i = 0; i < wordsInNote.Length; i++)
            {
                if (_dict.ContainsKey(wordsInNote[i]))
                {
                    if (_dict[wordsInNote[i]] == 0)
                        return Ok(false);

                    _dict[wordsInNote[i]]--;
                }
                else return Ok(false);
            }

            return Ok(true);
        }
    }
}
