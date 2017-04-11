import React, { Component } from 'react';
import Question from './Question';

import './Voting.css';

class Voting extends Component {
  render() {
    const { questions, voteQuestionHandler, showResultsHandler } = this.props;
    return (
      <div className="Voting">
        <h2>Questions: </h2>
        {questions.map(question => (
          <Question
            question={question}
            voteQuestion={voteQuestionHandler}
            key={question.id}
          />
        ))}
        <div className="buttons">
          <button
            className="results-button button"
            onClick={showResultsHandler}
          >
            Show results
          </button>
        </div>
      </div>
    );
  }
}

export default Voting;
