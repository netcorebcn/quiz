const url = `http://${window.location.hostname}:81/quiz/`;
const ws = `ws://${window.location.hostname}:82/ws`;

export const getQuiz = () => fetch(url).then(response => {
  if (response.status === 204) {
    return null;
  }
  if (response.status === 200) {
    return response.json();
  }
});

export const startNewQuiz = () =>
  fetch(url, { method: 'PUT' }).then(r => r.json());

export const postQuizAnswers = (quizId, answers) => fetch(`${url}${quizId}`, {
  method: 'POST',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({ answers })
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
