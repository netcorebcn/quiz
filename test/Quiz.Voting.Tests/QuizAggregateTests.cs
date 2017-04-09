using System.Linq;
using Quiz.Messages;
using Quiz.Voting.Domain;
using Xunit;

namespace Quiz.Voting.Tests
{
    public class QuizAggregateTests
    {
        [Fact]
        public void Given_A_Quiz_When_Closing_Then_QuizClosedEvent_Raised()
        {
            // Arrange
            var quiz = new QuizAggregate();

            // Act
            quiz.Close();

            // Assert
            var closedEvent = quiz.GetPendingEvents().FirstOrDefault();
            Assert.NotNull(closedEvent);
            Assert.IsAssignableFrom(typeof(QuizClosedEvent), closedEvent);
        }

        [Fact]
        public void Given_A_Quiz_When_Starting_Then_QuizStartedEvent_Raised()
        {
            // Arrange
            var quiz = new QuizAggregate();
            var quizModel = QuizModelFactory.Create();

            // Act
            quiz.Start(quizModel);

            // Assert
            var startEvent = quiz.GetPendingEvents().FirstOrDefault();
            Assert.NotNull(startEvent);
            Assert.IsAssignableFrom(typeof(QuizStartedEvent), startEvent);
        }

        [Fact]
        public void Given_An_Started_Quiz_When_Voting_For_RightAnswer_Then_QuestionRightAnsweredEvent_Raised()
        {
            // Arrange
            var quiz = new QuizAggregate();
            var quizModel = QuizModelFactory.Create();
            var selectedQuestion = quizModel.Questions.FirstOrDefault();
            var selectedOption = selectedQuestion.Options.FirstOrDefault(x => x.IsCorrect);

            // Act
            quiz.Start(quizModel);
            quiz.Vote(selectedQuestion.Id, selectedOption.Id);

            // Assert
            var startedEvent = quiz.GetPendingEvents().FirstOrDefault();
            var answeredEvent = quiz.GetPendingEvents().LastOrDefault();
            Assert.NotNull(startedEvent);
            Assert.IsAssignableFrom(typeof(QuizStartedEvent), startedEvent);
            Assert.NotNull(answeredEvent);
            Assert.IsAssignableFrom(typeof(QuestionRightAnsweredEvent), answeredEvent);            
        }

        [Fact]
        public void Given_An_Started_Quiz_When_Voting_For_WrongAnswer_Then_QuestionWrongAnsweredEvent_Raised()
        {
            // Arrange
            var quiz = new QuizAggregate();
            var quizModel = QuizModelFactory.Create();
            var selectedQuestion = quizModel.Questions.FirstOrDefault();
            var selectedOption = selectedQuestion.Options.FirstOrDefault(x => !x.IsCorrect);

            // Act
            quiz.Start(quizModel);
            quiz.Vote(selectedQuestion.Id, selectedOption.Id);

            // Assert
            var startedEvent = quiz.GetPendingEvents().FirstOrDefault();
            var answeredEvent = quiz.GetPendingEvents().LastOrDefault();

            Assert.NotNull(startedEvent);
            Assert.IsAssignableFrom(typeof(QuizStartedEvent), startedEvent);
            Assert.Null(answeredEvent);
            Assert.IsAssignableFrom(typeof(QuestionWrongAnsweredEvent), answeredEvent);            
        }
    }
}
