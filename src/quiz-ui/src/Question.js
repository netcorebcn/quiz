import React, { Component } from 'react';
import QuestionOption from './QuestionOption';
import { bindClass } from './utils';

import './Question.css';

class Question extends Component {
  render() {
    var { question, voteQuestion } = this.props;
    return (
      <div className="Question">
        <h3> {question.description}</h3>
        <div>
          {question.options.map(option => (
            <QuestionOption
              key={option.id}
              questionId={question.id}
              option={option}
              voteQuestion={voteQuestion}
            />
          ))}
        </div>
        <div>
          <span>Questions statistics: </span>
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

export default Question;
