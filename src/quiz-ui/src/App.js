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
        <div className="App-header">
          <h2>Welcome to Quiz</h2>
        </div>
            QuizId:{this.state.quizId}
            {this.state.questions.map(q =>
              <ul key={q.id}>    
                {q.id.slice(0,7)}-{q.description} 
                - Right {q.rightAnswersPercent || 0} 
                - Wrong {q.wrongAnswersPercent || 0}
                {q.options.map(o => 
                  <li key={o.id} onClick={() => this.voteQuestion(q.id,o.id)}>
                    {o.id.slice(0,7)}-{o.description}
                  </li>
                  )}
              </ul>
            )}

            <button onClick={() => this.startQuiz(1)}>
              Start Quiz
            </button>
      </div>
    );
  }
}

export default App;
