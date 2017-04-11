import React, { Component } from 'react';

import './QuestionResults.css';

class QuestionResults extends Component {
  render() {
    var { question } = this.props;
    return (
      <div className="Question">
        <h3> {question.description}</h3>
        <div>
          <span className="correct">{question.rightAnswersPercent || 0}%</span>
          <span>/</span>
          <span className="incorrect">
            {question.wrongAnswersPercent || 0}%
          </span>
        </div>
      </div>
    );
  }
}

export default QuestionResults;
