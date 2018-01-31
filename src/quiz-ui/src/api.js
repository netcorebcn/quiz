  const apiUrl = (resource = 'quiz', id = '')  => 
       `//${window.location.hostname}:5000/${resource}/${id}`;
  
  const post = (url, method, body) =>
      fetch(url, { method, headers: { Accept: 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  
  const get = () =>
      fetch(apiUrl())
      .then(r => r.json());

  const getResults = () =>
      fetch(apiUrl('quizResults'))
      .then(r => r.json());
        
  const start = (quiz) =>
      post(apiUrl(), 'POST', quiz)
      .then(r => r.json());

  const close = (quizId) =>
      post(apiUrl('quiz', quizId), 'DELETE')
      .then(r => r.json());

  const answer = (quizId, answers) =>
      post(apiUrl('quiz', quizId) , 'PUT', {quizId, answers})
      .then(r => r.json());

  const subscribe = (action) => {
      const webSocket = new WebSocket(`ws://${window.location.host}/ws`);
      webSocket.onmessage = ({ data }) => action(JSON.parse(data)); 
  }
  
  export {
      get,
      getResults,
      start,
      close,
      answer,
      subscribe
  }