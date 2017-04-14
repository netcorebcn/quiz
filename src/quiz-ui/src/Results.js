import React, { Component } from 'react';
import QuestionResults from './QuestionResults';

class Results extends Component {
  render() {
    const {
      questions,
      quizId,
      startQuizHandler
    } = this.props;
    return (
      <div className="Results">
        <h2>
          Current quiz ID: <b>{quizId || 'No Quiz Available'}</b>
        </h2>
        {questions.map(question => (
          <QuestionResults question={question} key={question.id} />
        ))}
        <div className="buttons">
          <button className="start-button button" onClick={startQuizHandler}>
            Start New Quiz
          </button>
        </div>
      </div>
    );
  }
}

export default Results;
