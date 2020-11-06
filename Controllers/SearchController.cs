using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegacyDatasystemDotNetMongoB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var results = _searchService.FindSearch(collection, searchtext);

            if (results == null)
                return NotFound();

            return Ok(results);
        }
    }
}
