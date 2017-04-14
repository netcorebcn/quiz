import React, { Component } from 'react';

import { RadialChart } from 'react-vis';

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
      <RadialChart
        colorType="literal"
        animation
        data={data}
        innerRadius={50}
        width={200}
        height={200}
      />
    );
  }
}

export default ResultChart;
