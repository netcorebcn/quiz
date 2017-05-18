import React, { Component } from 'react';
import ResultListItem from './ResultListItem';
import { bindClass, prettyPrint } from './utils';
import { defaultQuiz } from './defaultQuiz'

import './Results.css';

class Results extends Component {
  constructor(props) {
    super(props);
    this.state = {
      quizModel: prettyPrint(defaultQuiz)
    };

    bindClass(this);
  }

  startHandler() {
    this.props.startQuizHandler(JSON.parse(this.state.quizModel));
  }

  handleChange(event) {
    this.setState({quizModel: event.target.value});
  }

  render() {
    const {
      questions,
      quizId,
      startQuizHandler
    } = this.props;
    const correctScore = questions
      .map(question => question.rightAnswersPercent)
      .reduce((acc, val) => acc + val, 0) / questions.length;

    const incorrectScore = questions
      .map(question => question.wrongAnswersPercent)
      .reduce((acc, val) => acc + val, 0) / questions.length;

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
              correct={question.rightAnswersPercent || 0}
              incorrect={question.wrongAnswersPercent || 0}
            />
          ))}
        </div>
        <div className="Results-List">
          <h2>
            Overall Score
          </h2>
          <ResultListItem
            correct={correctScore || 0}
            incorrect={incorrectScore || 0}
          />
        </div>
        <div className="buttons">
          <button className="start-button button" onClick={this.startHandler}>
            Start New Quiz
          </button>
        </div>
        <div className="buttons">
          <textarea cols="150" rows="50" value={this.state.quizModel} onChange={this.handleChange} />
        </div>
      </div>
    );
  }
}

export default Results;
