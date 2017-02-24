using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quiz.EventSourcing.Domain;
using Quiz.Messages;
using Quiz.Voting.Domain;

namespace Quiz.Api.Controllers
{
    [Route("[controller]")]
    public class QuizController
    {
        private readonly IRepository _quizRepository;

        public QuizController(IRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet("{id}")]
        public QuizModel Get(int id) => QuizModelFactory.Create(id);

        [HttpPost]
        public async Task Vote([FromBody]QuestionAnswerCommand answer)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(answer.QuizId);
            quiz.Vote(answer.QuestionId, answer.OptionId);
            await _quizRepository.Save(quiz);
        }

        [HttpPut("{id}")]
        public async Task Start(int id)
        {
            var quizModel = QuizModelFactory.Create(id);
            var quiz = new QuizAggregate();
            quiz.Start(quizModel);
            await _quizRepository.Save(quiz);
        }

        // [HttpDelete]
        // public async Task Close(Guid quizId)
        // {
        //     var quiz = await _quizRepository.GetById<QuizAggregate>(quizId);
        //     quiz.Close();
        //     await _quizRepository.Save(quiz);
        // }
    }
}