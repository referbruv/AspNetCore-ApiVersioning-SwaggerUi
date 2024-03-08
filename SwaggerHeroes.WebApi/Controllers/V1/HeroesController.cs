using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwaggerHeroes.Core.Data.Entities;
using SwaggerHeroes.Core.Data.Repositories;
using SwaggerHeroes.Core.Data.Services;

namespace SwaggerHeroes.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    // [Route("api/[controller]")] // for backward compatibility
    public class HeroesController : ControllerBase
    {
        private readonly ILogger<HeroesController> _logger;
        private readonly IHeroesRepository _data;

        public HeroesController(ILogger<HeroesController> logger, IDataService data)
        {
            _logger = logger;
            _data = data.Heroes;
        }

        [MapToApiVersion("1.0")]
        [HttpGet, Route("alive")]
        public string Alive()
        {
            return "Captain, 1.0 Here. I'm Alive and Kicking!";
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IEnumerable<Hero> Get()
        {
            return _data.All();
        }

        [MapToApiVersion("1.0")]
        [HttpGet, Route("{id}")]
        public Hero Get(int id)
        {
            return _data.Single(x => x.Id == id);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public Hero Post(Hero model)
        {
            var item = new Hero
            {
                Category = model.Category,
                HasCape = model.HasCape,
                IsAlive = model.IsAlive,
                Name = model.Name,
                Powers = model.Powers
            };
            return _data.Create(item);
        }

        [MapToApiVersion("1.0")]
        [HttpGet, Route("searchbyname")]
        public IEnumerable<Hero> SearchByName(string name = "")
        {
            return _data.Search(x => x.Name.Contains(name));
        }
    }
}
