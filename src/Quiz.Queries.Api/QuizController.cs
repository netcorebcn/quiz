using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quiz.Domain;

namespace Quiz.Results.Api
{
    [Route("[controller]")]
    public class QuizController : Controller
    {
        private readonly QuizResultsAppService _appService;

        public QuizController(QuizResultsAppService appService) => _appService = appService;

        [HttpGet]
        public object Get() => _appService.Get();
    }
}
