import React, { Component } from 'react';
import { bindClass } from './utils';

import './QuestionOption.css';

class QuestionOption extends Component {
  constructor(props) {
    super(props);
    bindClass(this);
  }

  handleClick() {
    var { questionId, option, voteQuestion } = this.props;
    voteQuestion(questionId, option.id);
  }
  render() {
    var { option } = this.props;
    return (
      <div className="QuestionOption" onClick={this.handleClick}>
        {option.description}
      </div>
    );
  }
}

export default QuestionOption;
