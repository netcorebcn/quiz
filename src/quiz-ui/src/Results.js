import React, { Component } from 'react';
import ResultListItem from './ResultListItem';

import './Results.css';

class Results extends Component {
  render() {
    const {
      questions,
      quizId,
      startQuizHandler
    } = this.props;
    const correctScore = questions
      .map(question => question.rightAnswersPercent)
      .reduce((acc, val) => acc + val, 0) / questions.length;

    const incorrectScore = questions
      .map(question => question.wrongAnswersPercent)
      .reduce((acc, val) => acc + val, 0) / questions.length;

    return (
      <div className="Results">
        <h2>
          Current quiz ID: <b>{quizId || 'No Quiz Available'}</b>
        </h2>
        <div className="Results-List">
          {questions.map(question => (
            <ResultListItem
              key={question.id}
              description={question.description}
              correct={question.rightAnswersPercent || 0}
              incorrect={question.wrongAnswersPercent || 0}
            />
          ))}
        </div>
        <div className="Results-List">
          <h2>
            Overall Score
          </h2>
          <ResultListItem
            correct={correctScore || 0}
            incorrect={incorrectScore || 0}
          />
        </div>
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
