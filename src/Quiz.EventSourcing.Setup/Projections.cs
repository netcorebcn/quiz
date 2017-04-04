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
            $init : function(state, event)
            {
                return { }
            },
            'QuestionRightAnsweredEvent': function (state, event) {
                processEvent(state, event, true);
            },
            'QuestionWrongAnsweredEvent': function (state, event) {
                processEvent(state, event, false);
            }
        });
        
        function processEvent(state, event, right){ 
            var questionId = event.data.QuestionId;
            if(!state[questionId])
                state[questionId] = { 
                    total: 0,
                    wrongAnswers: 0,
                    wrongAnswersPercent: 0,
                    rightAnswers: 0,
                    rightAnswersPercent: 0
                }
            
            var current = state[questionId];
            if (right)
                current.rightAnswers++;
            else
                current.wrongAnswers++;
                
            current.total++;
            current.rightAnswersPercent = (current.rightAnswers * 100) / current.total;
            current.wrongAnswersPercent = (current.wrongAnswers * 100) / current.total;
            emit('QuestionAnswers', 'QuestionStatisticCreatedEvent', 
            {
                'questionId': questionId,
                'rightAnswersPercent': current.rightAnswersPercent,
                'wrongAnswersPercent': current.wrongAnswersPercent
            });
        }";

        public const string QuestionAggregate = @"
        fromStream('QuizAggregate')
            .whenAny(function(state, ev) {
            linkTo('Question-' + ev.data.QuestionId, ev)
        })";
    }
}