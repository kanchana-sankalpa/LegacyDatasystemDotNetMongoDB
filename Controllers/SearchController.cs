using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegacyDatasystemDotNetMongoB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace LegacyDatasystemDotNetMongoB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("{collection}/{searchtext}")]
        public IActionResult GetAllResults(string collection, string searchtext)
        {
            //var user = _searchService.FindSearch(collection, searchtext);
            List<BsonDocument> results = _searchService.FindSearch(collection, searchtext);
            var finalResult = results.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
          //  return Content(finalResult);
            if (finalResult == null)
                return NotFound();

            return Ok(finalResult);
        }
    }
}
