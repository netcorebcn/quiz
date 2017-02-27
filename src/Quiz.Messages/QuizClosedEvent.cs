namespace Quiz.Messages
{
    public class QuizClosedEvent
    {
        public int RightAnswers { get; }
        public int WrongAnswers { get; }

        public QuizClosedEvent(int righAnswers, int wrongAnswers)
        {
            RightAnswers = righAnswers;
            WrongAnswers = wrongAnswers;
        }
    }
}