namespace Quiz.EventSourcing.Setup
{
    public static class Projections
    {
        public const string Totals = @"
        fromCategory('QuizAggregate')
        .when({
            $init : function(state, event)
            {
                return { wrongAnswers: 0, rightAnswers: 0}
            },
            'QuestionRightAnsweredEvent': function (state, event) {
                state.rightAnswers++;
            },
            'QuestionWrongAnsweredEvent': function (state, event) {
                state.wrongAnswers++;
            }
        })";
    
        public const string QuestionAnswers = @"
        fromCategory('QuizAggregate')
        .when({
            'QuestionRightAnsweredEvent': function (state, event) {
                linkTo('QuestionAnswers', event)
            },
            'QuestionWrongAnsweredEvent': function (state, event) {
                linkTo('QuestionAnswers', event)
            }
        })";
    }
}
