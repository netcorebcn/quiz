import React, { Component } from 'react';
import { bindClass } from './utils';

import './QuestionOption.css';

class QuestionOption extends Component {
  constructor(props) {
    super(props);
    bindClass(this);
  }

  handleClick() {
    var { questionId, option, selectAnswer, isSubmitted } = this.props;
    if (!isSubmitted) {
      selectAnswer(questionId, option.id);
    }
  }
  render() {
    var { option, isSelected, isSubmitted } = this.props;
    let className = `QuestionOption ${!isSubmitted ? 'active' : ''} `;
    if (isSelected) {
      if (!isSubmitted) {
        className += 'selected';
      }
      if (isSubmitted) {
        className += option.isCorrect ? 'correct' : 'incorrect';
      }
    }
    return (
      <div className={className} onClick={this.handleClick}>
        {option.description}
      </div>
    );
  }
}

export default QuestionOption;
