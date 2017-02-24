namespace Quiz.Messages
{
    public class QuizStartedEvent
    {
        public QuizModel QuizModel { get; set; }

        public QuizStartedEvent(QuizModel quizModel)
        {
            QuizModel = quizModel;
        }
    }
}