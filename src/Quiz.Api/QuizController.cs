using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    [Route("[controller]")]
    public class QuizController
    {
        private readonly QuizAppService _quizAppService;

        public QuizController(QuizAppService quizAppService) => _quizAppService = quizAppService;

        [HttpGet]
        public async Task<object> Get() => 
            await _quizAppService.GetState();

        [HttpGet("{quizId}")]
        public async Task<object> Get(Guid quizId) => 
            await _quizAppService.GetState(quizId);

        [HttpPut]
        [Route("{quizId}")]
        public async Task<object> Answer(Guid quizId, [FromBody]QuizAnswersCommand quizAnswersComand) =>
            await _quizAppService.Answer(new QuizAnswersCommand(quizId, quizAnswersComand.Answers));

        [HttpPost]
        public async Task<object> Start([FromBody]QuizModel quizModel) => 
            await _quizAppService.Start(quizModel);

        [HttpDelete]
        [Route("{quizId}")]
        public async Task Close(Guid quizId) =>
            await _quizAppService.Close(quizId);
    }
}
