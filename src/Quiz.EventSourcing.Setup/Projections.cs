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


        public const string QuestionAggregate = @"
        fromStream('QuizAggregate')
            .whenAny(function(state, ev) {
            linkTo('Question-' + ev.data.QuestionId, ev)
        })";

        public const string QuestionsAnswersPercent = @"
        fromStream('QuestionAnswers-2d882ad4-a0ef-4de3-bbec-8bf9e06553e2')
        .when({
            $init : function(state, event)
            {
                return { 
                    wrongAnswers: 0, 
                    wrongAnswersPercent: 0,
                    rightAnswersPercent: 0,
                    rightAnswers: 0, total:0}
            },
            'QuestionRightAnsweredEvent': function (state, event) {
                state.rightAnswers++;
                state.total = state.wrongAnswers + state.rightAnswers
                state.rightAnswersPercent = (state.rightAnswers * 100) / state.total;
                state.wrongAnswersPercent = (state.wrongAnswers * 100) / state.total;
            },
            'QuestionWrongAnsweredEvent': function (state, event) {
                state.wrongAnswers++;
                state.total = state.wrongAnswers + state.rightAnswers
                state.rightAnswersPercent = (state.rightAnswers * 100) / state.total;
                state.wrongAnswersPercent = (state.wrongAnswers * 100) / state.total;
            }
        })";
    }
}
