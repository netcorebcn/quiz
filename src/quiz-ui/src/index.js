import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';

var webSocket = new WebSocket('ws://localhost:82/ws');
webSocket.onopen = function (event) {
  console.log(event);
};

webSocket.onmessage = function (event) {
  console.log(event.data);
}

ReactDOM.render(
  <App />,
  document.getElementById('root')
);
