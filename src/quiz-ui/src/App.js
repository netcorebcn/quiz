import React, { Component } from 'react';
import './App.css';
import {get, post, put, startWs} from './api';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      quizId: 0,
      questions: []
    };

    this.startQuiz = this.startQuiz.bind(this);
    this.voteQuestion = this.voteQuestion.bind(this);
  }

  componentDidMount() {
    get().then(json => {
        this.setState({ 
          quizId: json.quizId, 
          questions: json.quizModel.questions.map(q => 
            ({ ...q, 
              ...json.questions.find(x => x.questionId === q.id)
            }))
        })
    });

    startWs(questionStats =>
      this.setState({ ...this.state,
        questions: this.state.questions.map(
          question => question.id === questionStats.questionId 
            ? { ...question, ...questionStats  } 
            : question)
      })
    );
  }

  startQuiz() {
    put().then(json => this.setState({ ...json }));
  }

  voteQuestion(questionId, optionId) {
    post(this.state.quizId, questionId, optionId)
      .then(json => this.setState({ questions: json.questions }));
  }

  render() {
    return (
      <div className="App">
          <h2>Welcome to Quiz {this.state.quizId}</h2>
            {this.state.questions.map(q =>
              <div key={q.id}>
                {q.description} 
                <ul key={q.id}>    
                  {q.options.map(o => 
                    <li key={o.id} onClick={() => this.voteQuestion(q.id,o.id)}>
                      {o.description}
                    </li>
                    )}
                </ul>
                <p>
                  <span>Right {q.rightAnswersPercent || 0} </span>
                  <span>Wrong {q.wrongAnswersPercent || 0} </span>
                </p>
              </div>              
            )}
            <button onClick={() => this.startQuiz()}>
              Start Quiz
            </button>
      </div>
    );
  }
}

export default App;
