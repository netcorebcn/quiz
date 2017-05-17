namespace Quiz.Domain
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
                return { 
                }
            },
            'QuestionRightAnsweredEvent': function (state, event) {
                processEvent(state, event, true);
            },
            'QuestionWrongAnsweredEvent': function (state, event) {
                processEvent(state, event, false);
            },
            'QuizStartedEvent': function(state, event) {
                state.quizId = event.data.QuizId;
                state.quizModel = event.data.QuizModel;
                state.questions = [];
            }
        });
        
        function processEvent(state, event, right){ 
            var questionId = event.data.QuestionId;
            var current = state.questions.find(q => q.questionId === questionId);
            
            if (!current){
                current = {
                    questionId,
                    wrongAnswers: 0,
                    wrongAnswersPercent: 0,
                    rightAnswers: 0,
                    rightAnswersPercent: 0
                }
                state.questions.push(current);
            }
            
            if (right)
                current.rightAnswers++;
            else
                current.wrongAnswers++;
            
            const total = current.rightAnswers + current.wrongAnswers;
            current.rightAnswersPercent = (current.rightAnswers * 100) / total;
            current.wrongAnswersPercent = (current.wrongAnswers * 100) / total;
            
            emit('QuestionAnswers', 'QuestionStatisticCreatedEvent', 
            {
                'quizId': state.quizId,
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