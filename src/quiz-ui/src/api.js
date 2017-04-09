const url = `http://${window.location.hostname}:81/quiz/`;
const ws = `ws://${window.location.hostname}:82/ws`;

export const get = () => fetch(url).then(r => r.json());

export const put = () => fetch(url, {
  method: 'PUT'
}).then(r => r.json());

export const post = (quizId, questionId, optionId) => fetch(`${url}${quizId}`, {
  method: 'POST',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    questionId,
    optionId
  })
}).then(r => r.json());

export const startWs = cb => {
  const webSocket = new WebSocket(ws);
  webSocket.onopen = e => {
    console.log(e);
  };
  webSocket.onmessage = e => {
    console.log(e.data);
    cb(JSON.parse(e.data));
  };
};
