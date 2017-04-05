import React, { Component } from 'react';
import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      questions: []
    };
  }

  componentDidMount() {
    fetch('http://localhost:81/quiz/1')
      .then(r => r.json())
      .then(json => { 
        console.log(json);
        this.setState({ questions: json.questions });
      });  
  }

  render() {
    return (
      <div className="App">
        <div className="App-header">
          <h2>Welcome to Quiz</h2>
        </div>
            {this.state.questions.map(q =>
              <ul key={q.id}>    
                {q.description}
                {q.options.map(o => 
                  <li key={o.id}>{o.description}</li>
                  )}
              </ul>
            )}
      </div>
    );
  }
}

export default App;
