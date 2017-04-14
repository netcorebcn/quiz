import React, { Component } from 'react';
import QuestionOption from './QuestionOption';

import './Question.css';

class Question extends Component {
  render() {
    var { question, selectAnswer, selectedOption, isSubmitted } = this.props;
    return (
      <div className="Question">
        <h3> {question.description}</h3>
        <div>
          {question.options.map(option => (
            <QuestionOption
              key={option.id}
              questionId={question.id}
              isSelected={option.id === selectedOption}
              option={option}
              selectAnswer={selectAnswer}
              isSubmitted={isSubmitted}
            />
          ))}
        </div>
      </div>
    );
  }
}

export default Question;
