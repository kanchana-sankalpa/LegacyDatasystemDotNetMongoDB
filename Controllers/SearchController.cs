using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetcondapackage.Entities;
using dotnetcondapackage.Services;
using LegacyDatasystemDotNetMongoB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace LegacyDatasystemDotNetMongoB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private SearchService _searchService;
        private IUserService _userService;

        public SearchController(SearchService searchService, IUserService userService)
        {
            _searchService = searchService;
            _userService = userService;
        }

        [HttpGet("{collection}/{searchtext}")]
        public IActionResult GetAllResults(string collection, string searchtext)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            //var user = _searchService.FindSearch(collection, searchtext);
            List<BsonDocument> results = _searchService.FindSearch(collection, searchtext);
            
            var finalResult = results.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
             //  return Content(finalResult);
            if (finalResult == null)
                return NotFound();

            return Ok(finalResult);
        }

        [HttpGet("textasynctest/{searchtext}")]
        public async Task<ActionResult<string>> Get(string searchtext)
        {
            List<BsonDocument> value = await _searchService.FindSearchAsync(searchtext);
           
            if (value == null) return new NotFoundResult();
            if (value.Count == 0) return new NotFoundResult();
           // var finalResult = value.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            //var obj = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(value.ToJson());
           
            return new OkObjectResult(value.ToJson());
        }


        [HttpGet("text/{searchtext}")]
        public async Task<ActionResult<string>> searchTextAsync(string searchtext)
        {

            var currentUserId = int.Parse(User.Identity.Name);
            List<Dataset> results = _userService.getUserRoleDataset(currentUserId);

            foreach (Dataset dataitem in results)
            {
                List<BsonDocument> docResults = await _searchService.FindSearchCollectionAsync(dataitem.SchemaDatasetName,searchtext);
                dataitem.Data = docResults;
            }
            return Ok(results.ToJson());

            /*
            List<BsonDocument> value = await _searchService.FindSearchAsync(searchtext);

            if (value == null) return new NotFoundResult();
            if (value.Count == 0) return new NotFoundResult();
            // var finalResult = value.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            //var obj = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(value.ToJson());

            return new OkObjectResult(value.ToJson());
            */
        }



        [HttpGet("textsync/{searchtext}")]
        public IActionResult searchText(string searchtext)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            List<Dataset> results = _userService.getUserRoleDataset(currentUserId);

            //List<BsonDocument>[] array1= new List<BsonDocument>[results.Count()+1];
            var arrayList = new List<List<BsonDocument>>();

            foreach (Dataset dataitem in results)
            {
                List<BsonDocument> docResults = _searchService.FindSearch(dataitem.SchemaDatasetName, searchtext);
                arrayList.Add(docResults);
            }
            return Ok(arrayList.ToJson());
        }

    }
}
