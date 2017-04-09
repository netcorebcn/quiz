import React, { Component } from 'react';
import Question from './Question';
import { get, post, put, startWs } from './api';
import { bindClass } from './utils';

import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      quizId: 0,
      questions: []
    };

    bindClass(this);
  }

  componentDidMount() {
    get().then(json => {
      this.setState({
        quizId: json.quizId,
        questions: json.quizModel.questions.map(q => ({
          ...q,
          ...json.questions.find(x => x.questionId === q.id)
        }))
      });
    });

    startWs(questionStats => this.setState({
      ...this.state,
      questions: this.state.questions.map(
        question =>
          question.id === questionStats.questionId
            ? { ...question, ...questionStats }
            : question
      )
    }));
  }

  startQuiz() {
    put().then(json => this.setState({ ...json }));
  }

  voteQuestion(questionId, optionId) {
    post(this.state.quizId, questionId, optionId).then(json =>
      this.setState({ questions: json.questions }));
  }

  render() {
    const { questions, quizId } = this.state;
    return (
      <div className="App-container">
        <div className="App">
          <h1>Welcome to Quiz App</h1>
          <h2>
            Current quiz ID: <b>{quizId}</b> <button
              className="start-button"
              onClick={this.startQuiz}
            >
              Start New Quiz
            </button>
          </h2>
          <h2>Questions: </h2>
          {questions.map(question => (
            <Question
              question={question}
              voteQuestion={this.voteQuestion}
              key={question.id}
            />
          ))}
        </div>
      </div>
    );
  }
}

export default App;
