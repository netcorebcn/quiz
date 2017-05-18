using System;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Domain
{
    public class QuizAppService
    {
        private readonly IRepository _quizRepository;

        public QuizAppService(IRepository quizRepository) =>
            _quizRepository = quizRepository;

        public async Task Vote(QuizAnswersCommand quizAnswersComand)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(quizAnswersComand.QuizId);
            quizAnswersComand.Answers.ForEach(
                answer => quiz.Vote(answer.QuestionId, answer.OptionId));
            await _quizRepository.Save(quiz);
        }

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

        public async Task Close(Guid id)
        {
            var quiz = await _quizRepository.GetById<QuizAggregate>(id);
            quiz.Close();
            await _quizRepository.Save(quiz);
        }
    }
}
