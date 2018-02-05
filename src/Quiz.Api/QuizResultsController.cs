using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Quiz.Domain;

namespace Quiz.Api
{
    [Route("api/[controller]")]
    public class QuizResultsController : Controller
    {
        private readonly QuizResultsAppService _appService;

        public QuizResultsController(QuizResultsAppService appService) => _appService = appService;

        [HttpGet]
        public async Task<object> Get() => await _appService.Get();

        [HttpGet("{quizId}")]
        public async Task<object> Get(Guid quizId) => await _appService.Get(quizId);
    }
}
