fromCategory('QuizAggregate')
.when({
    $init : function(state, event)
    {
        return { wrongAnswers: 0, rightAnswers: 0}
    },
    "QuestionRightAnsweredEvent": function (state, event) {
        state.rightAnswers++;
    },
    "QuestionWrongAnsweredEvent": function (state, event) {
        state.wrongAnswers++;
    }
})