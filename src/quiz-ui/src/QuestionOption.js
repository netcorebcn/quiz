import React, { Component } from 'react';
import { bindClass } from './utils';

import './QuestionOption.css';

class QuestionOption extends Component {
  constructor(props) {
    super(props);
    bindClass(this);
  }

  handleClick() {
    var { questionId, option, selectAnswer } = this.props;
    selectAnswer(questionId, option.id);
  }
  render() {
    var { option, isSelected } = this.props;
    const className = `QuestionOption ${isSelected ? 'selected' : ''}`;
    return (
      <div className={className} onClick={this.handleClick}>
        {option.description}
      </div>
    );
  }
}

export default QuestionOption;
