import React from 'react';

import './Loader.css';

const Loader = () => {
  return (
    <div className="Loader">
      <div className="Loader-icon-container">
        <div className="Loader-icon" />
      </div>
      <div className="Loader-label">Loading</div>
    </div>
  );
};

export default Loader;
