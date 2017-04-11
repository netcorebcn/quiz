import React, { Component } from 'react';
import Loader from './Loader';
import Question from './Question';
import QuestionResults from './QuestionResults';
import { getQuiz, postOption, startNewQuiz, initWebsockets } from './api';
import { bindClass } from './utils';

import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      quizId: 0,
      questions: [],
      isProcessing: true,
      showResults: false
    };

    bindClass(this);
  }

  componentDidMount() {
    getQuiz().then(json => {
      this.setState({
        quizId: json.quizId,
        questions: json.quizModel.questions.map(q => ({
          ...q,
          ...json.questions.find(x => x.questionId === q.id)
        })),
        isProcessing: false
      });
    });

    initWebsockets(questionStats => this.setState({
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
    startNewQuiz().then(json => this.setState({ ...json }));
  }

  showResults() {
    this.setState({
      showResults: true
    });
  }

  showVoting() {
    this.setState({
      showResults: false
    });
  }

  voteQuestion(questionId, optionId) {
    this.setState({ isProcessing: true });

    postOption(this.state.quizId, questionId, optionId).then(json => {
      this.setState({
        isProcessing: false
      });
    });
  }

  render() {
    const { isProcessing, showResults, questions, quizId } = this.state;
    return (
      <div className="Container">
        {isProcessing &&
          <div className="overlay">
            <Loader />
          </div>}
        <div className="App-container">
          <div className="App">
            <h1>Welcome to Quiz App</h1>
            {showResults
              ? <div className="App-Results">
                  <h2>
                    Current quiz ID: <b>{quizId}</b>
                  </h2>
                  {questions.map(question => (
                    <QuestionResults question={question} key={question.id} />
                  ))}
                  <div className="buttons">
                    <button
                      className="voting-button button"
                      onClick={this.showVoting}
                    >
                      Show voting
                    </button>
                    <button
                      className="start-button button"
                      onClick={this.startQuiz}
                    >
                      Start New Quiz
                    </button>
                  </div>
                </div>
              : <div className="App-Voting">
                  <h2>Questions: </h2>
                  {questions.map(question => (
                    <Question
                      question={question}
                      voteQuestion={this.voteQuestion}
                      key={question.id}
                    />
                  ))}
                  <div className="buttons">
                    <button
                      className="results-button button"
                      onClick={this.showResults}
                    >
                      Show results
                    </button>
                  </div>
                </div>}
          </div>
        </div>
      </div>
    );
  }
}

export default App;
