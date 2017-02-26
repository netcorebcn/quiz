fromCategory('QuizAggregate')
.when({
    "QuestionRightAnsweredEvent": function (state, event) {
        linkTo('QuestionAnswers', event)
    },
    "QuestionWrongAnsweredEvent": function (state, event) {
        linkTo('QuestionAnswers', event)
    }
})