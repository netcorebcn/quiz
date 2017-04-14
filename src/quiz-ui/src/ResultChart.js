import React, { Component } from 'react';

import { RadialChart } from 'react-vis';

import './ResultChart.css';

class ResultChart extends Component {
  render() {
    const { correct, incorrect } = this.props;
    const data = [
      {
        angle: correct || 1,
        color: '#66BB6A'
      },
      {
        angle: incorrect || 1,
        color: '#EF5350'
      }
    ];
    return (
      <div>
        <RadialChart
          colorType="literal"
          animation
          data={data}
          innerRadius={40}
          radius={80}
          width={220}
          height={180}
        />
        <div className="ResultChart-Label">
          <span className="ResultChart-Label_correct">
            {correct.toFixed(1)}%
          </span>
          /
          <span className="ResultChart-Label_incorrect">
            {incorrect.toFixed(1)}%
          </span>
        </div>
      </div>
    );
  }
}

export default ResultChart;
