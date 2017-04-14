import React, { Component } from 'react';
import Question from './Question';
import { bindClass } from './utils';

import './Voting.css';

class Voting extends Component {
  constructor(props) {
    super(props);
    this.state = {
      answers: []
    };

    bindClass(this);
  }

  voteHandler() {
    this.props.voteQuestionHandler(this.state.answers);
  }
  selectAnswer(questionId, optionId) {
    this.setState(prevState => {
      const answers = prevState.answers.filter(
        _ => _.questionId !== questionId
      );
      answers.push({
        questionId,
        optionId
      });
      return {
        answers
      };
    });
  }
  getSelectedOption(questionId) {
    var answer = this.state.answers.find(_ => _.questionId === questionId);
    if (answer) {
      return answer.optionId;
    }
    return null;
  }
  render() {
    const { questions, isSubmitted } = this.props;
    return (
      <div className="Voting">
        <h2>Questions: </h2>
        {questions.map(question => (
          <Question
            key={question.id}
            question={question}
            selectedOption={this.getSelectedOption(question.id)}
            selectAnswer={this.selectAnswer}
            isSubmitted={isSubmitted}
          />
        ))}
        {!isSubmitted &&
          <div className="buttons">
            <button className="submit-button button" onClick={this.voteHandler}>
              Submit
            </button>
          </div>}
      </div>
    );
  }
}

export default Voting;
