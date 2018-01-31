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
    this.props.startHandler(JSON.parse(this.state.quizModel));
  }

  closeHandler() {
    this.props.closeHandler(this.props.quizResults.quizId);
  }

  handleChange(event) {
    this.setState({quizModel: event.target.value});
  }

  render() {
    const {
      quizResults,
      quizState
    } = this.props;
    const { quizId, questions } = quizResults;
    const quizStarted = quizState === 'Started';

    return (
      <div>
      {quizStarted 
      ? <div className="Results">
          <h2>
            Current quiz ID: <b>{quizId}</b>
          </h2>
          <div className="Results-List">
            {questions.map(q => (
              <ResultListItem
                key={q.id}
                description={q.description}
                correct={q.correctAnswersPercent || 0}
                incorrect={q.incorrectAnswersPercent || 0}
              />
            ))}
          </div>
          <div className="Results-List">
            <h2>
              Overall Score
            </h2>
            <ResultListItem
              correct={0}
              incorrect={0}
            />
          </div>
          <div className="buttons">
            <button className="submit-button button" onClick={this.closeHandler}>
              Finish
            </button>
          </div>
        </div>
      : <div className="Results">
          <div className="buttons">
            <button className="submit-button button" onClick={this.startHandler}>
              Start
            </button>
          </div>
          <div className="buttons">
            <textarea cols="150" rows="50" value={this.state.quizModel} onChange={this.handleChange} />
          </div>
        </div>}
      </div>
    );
  }
}

export default Results;
