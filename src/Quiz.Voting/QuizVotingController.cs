using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quiz.EventSourcing.Domain;
using Quiz.Messages;
using Quiz.Voting.Domain;

namespace Quiz.Api.Controllers
{
    [Route("[controller]/{id}")]
    public class QuizController
    {
        private readonly IRepository _quizRepository;

        public QuizController(IRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        public QuizModel Get(int id) => 
            QuizModelFactory.Create(id);

        [HttpPost]
        public async Task Vote(Guid id, [FromBody]QuestionAnswerCommand answer)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(id);
            quiz.Vote(answer.QuestionId, answer.OptionId);
            await _quizRepository.Save(quiz);
        }

        [HttpPut]
        public async Task Start(int id)
        {
            var quizModel = QuizModelFactory.Create(id);
            var quiz = new QuizAggregate();
            quiz.Start(quizModel);
            await _quizRepository.Save(quiz);
        }

        [HttpDelete]
        public async Task Close(Guid id)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(id);
            quiz.Close();
            await _quizRepository.Save(quiz);
        }
    }
}
