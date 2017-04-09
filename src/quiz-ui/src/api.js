const url = `http://${window.location.hostname}:81/quiz/`;
const ws = `ws://${window.location.hostname}:82/ws`;

export const getQuiz = () => fetch(url).then(r => r.json());

export const startNewQuiz = () =>
  fetch(url, { method: 'PUT' }).then(r => r.json());

export const postOption = (quizId, questionId, optionId) =>
  fetch(`${url}${quizId}`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ questionId, optionId })
  }).catch(err => console.log(err));

export const initWebsockets = cb => {
  const webSocket = new WebSocket(ws);
  webSocket.onopen = e => {
    console.log(e);
  };
  webSocket.onmessage = e => {
    console.log(e.data);
    cb(JSON.parse(e.data));
  };
};
