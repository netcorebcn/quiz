import React, { Component } from 'react';
import ResultListItem from './ResultListItem';
import ResultChart from './ResultChart';

import './Results.css';

class Results extends Component {
  render() {
    const {
      questions,
      quizId,
      startQuizHandler
    } = this.props;
    console.log(questions);
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
              correct={question.rightAnswersPercent}
              incorrect={question.wrongAnswersPercent}
            />
          ))}
        </div>
        <div className="Results-List">
          <h2>
            Overall Score
          </h2>
          <ResultListItem correct={1} incorrect={2} />
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
