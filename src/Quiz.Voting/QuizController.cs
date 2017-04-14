using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.EventSourcing;
using Quiz.EventSourcing.Domain;
using Quiz.Messages;
using Quiz.Voting.Domain;

namespace Quiz.Api.Controllers
{
    [Route("[controller]")]
    public class QuizController
    {
        private readonly IRepository _quizRepository;
        private readonly IEventStoreProjections _projectionsClient;

        public QuizController(IRepository quizRepository, IEventStoreProjections projectionsClient)
        {
            _quizRepository = quizRepository;
            _projectionsClient = projectionsClient;
        }

        [HttpGet]
        public async Task<QuizReadModel> Get()
        {
            var result = await _projectionsClient.GetStateAsync(); 
            return JsonConvert.DeserializeObject<QuizReadModel>(result);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task Vote(Guid id, [FromBody]QuizAnswersCommand quizAnswersComand)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(id);

            quizAnswersComand.Answers.ForEach(answer =>
               quiz.Vote(answer.QuestionId, answer.OptionId)
            );

            await _quizRepository.Save(quiz);
        }

        [HttpPut]
        public async Task<object> Start()
        {
            var quizModel = QuizModelFactory.Create();
            var quiz = new QuizAggregate();
            quiz.Start(quizModel);
            await _quizRepository.Save(quiz);
            return new 
            {
                QuizId = quiz.Id,
                Questions = quiz.QuizModel.Questions
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Close(Guid id)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(id);
            quiz.Close();
            await _quizRepository.Save(quiz);
        }
    }
}
