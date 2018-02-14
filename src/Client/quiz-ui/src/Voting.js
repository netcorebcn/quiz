import React, { Component } from 'react';
import Question from './Question';
import { bindClass } from './utils';

import './Voting.css';

class Voting extends Component {
  constructor(props) {
    super(props);
    this.state = {
      answers : []
    };

    bindClass(this);
  }

  answerHandler() {
    this.props.answerHandler(this.state.answers);
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
    const { quiz, isSubmitted } = this.props;
    const quizStarted = quiz.quizState === 'Started';
    return (
      <div className="Voting">
        <h2>Questions: {!quizStarted && 'Not available'}</h2>
        {quiz.questions.map(question => (
          <Question
            key={question.id}
            question={question}
            selectedOption={this.getSelectedOption(question.id)}
            selectAnswer={this.selectAnswer}
            isSubmitted={isSubmitted}
          />
        ))}
        {!isSubmitted && quizStarted &&
          <div className="buttons">
            <button className="submit-button button" onClick={this.answerHandler}>
              Submit
            </button>
          </div>}
      </div>
    );
  }
}

export default Voting;
