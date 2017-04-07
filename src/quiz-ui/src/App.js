import React, { Component } from 'react';
import './App.css';

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
    var webSocket = new WebSocket('ws://localhost:82/ws');

    webSocket.onopen = (e) => {
      console.log(e);
    };

    webSocket.onmessage = (e) => {
      console.log(e.data);
      var questionStats = JSON.parse(e.data);

      this.setState({ ...this.state,
        questions: this.state.questions.map(
          question => question.id === questionStats.questionId 
            ? { ...question, ...questionStats  } 
            : question) 
      });
    }
  }

  startQuiz(quizId) {
    fetch(`http://localhost:81/quiz/${quizId}`, { method: 'PUT'})
      .then(r => r.json())
      .then(json => {
        console.log(json);
        this.setState({ ...json });
      });  
  }

  voteQuestion(questionId, optionId) {
    fetch(`http://localhost:81/quiz/${this.state.quizId}`, 
      { 
        method: 'POST', 
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({questionId, optionId})
      })
      .then(r => r.json())
      .then(json => { 
        console.log(json);
        this.setState({ questions: json.questions });
      })
      .catch(e => console.log(e));  
  }

  render() {
    return (
      <div className="App">
          <h2>Welcome to Quiz {this.state.quizId}</h2>
            {this.state.questions.map(q =>
              <div key={q.id}>
                <ul key={q.id}>    
                  {q.description} 
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

            <button onClick={() => this.startQuiz(1)}>
              Start Quiz
            </button>
      </div>
    );
  }
}

export default App;
