import React, { Component } from 'react';

import ResultChart from './ResultChart';

import './ResultListItem.css';

class ResultListItem extends Component {
  render() {
    const { description, correct, incorrect } = this.props;
    return (
      <div className="ResultListItem">
        {description && <h3>{description}</h3>}
        <div className="ResultListItem-Chart">
          <ResultChart correct={correct} incorrect={incorrect} />
        </div>
      </div>
    );
  }
}

export default ResultListItem;
