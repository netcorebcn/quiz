import React, { Component } from 'react';

import { RadialChart } from 'react-vis';

import './QuestionResults.css';

class QuestionResults extends Component {
  render() {
    const { question } = this.props;
    const data = [
      {
        angle: question.rightAnswersPercent || 0,
        color: '#66BB6A',
        label: 'Correct Answers'
      },
      {
        angle: question.wrongAnswersPercent || 0,
        color: '#EF5350',
        label: 'Wrong Answers'
      }
    ];
    return (
      <div className="QuestionResult">
        <h3>{question.description}</h3>
        <div>
          <RadialChart
            colorType="literal"
            data={data}
            width={300}
            height={300}
          />
        </div>
      </div>
    );
  }
}

export default QuestionResults;
