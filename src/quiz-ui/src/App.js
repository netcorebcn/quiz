import React, { Component } from 'react';
import Loader from './Loader';
import Voting from './Voting';
import Results from './Results';
import { get, getResults, answer, start, close, subscribe} from './api';
import { bindClass } from './utils';

import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      quizResults: { quizId: 0, questions: []},
      quiz: { quizId: 0, questions: []},
      isProcessing: true,
      isSubmitted: false,
      showResults: window.location.search.indexOf('results') !== -1
    };

    bindClass(this);
  }

  componentDidMount() {
    get().then(quiz => this.setState({ isProcessing: false, quiz }));
    getResults().then(quizResults =>  this.setState({ quizResults, isProcessing: false }));
    subscribe(quizResults => this.setState({ quizResults }));
  }

  commandHandler(command){
    this.setState({ isProcessing: true });
    command
      .then(quiz => {
        this.setState({ quiz });
        getResults().then(quizResults =>  
          this.setState({ quizResults, isProcessing: false, isSubmitted: true })
        )
      })
  }
  
  startHandler(quiz) {
    this.commandHandler(start(quiz));
  }

  closeHandler(quizId) {
    this.commandHandler(close(quizId));
  }

  answerHandler(answers) {
    this.commandHandler(answer(this.state.quiz.quizId, answers));
  }

  render() {
    const {
      showResults,
      quiz,
      quizResults,
      isSubmitted,
      isProcessing,
    } = this.state;
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
              ? <Results
                  quizResults={quizResults}
                  quizState={quiz.quizState}
                  startHandler={this.startHandler}
                  closeHandler={this.closeHandler}
                />
              : <Voting
                  quiz={quiz}
                  answerHandler={this.answerHandler}
                  isSubmitted={isSubmitted}
                />}
          </div>
        </div>
      </div>
    );
  }
}

export default App;
