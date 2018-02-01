  const apiGateway = `${window.location.hostname}`
  const apiUrl = (resource = 'quiz', id = '')  => 
       `//${apiGateway}/${resource}/${id}`;
  
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
      const webSocket = new WebSocket(`ws://${apiGateway}/ws`);
      webSocket.onmessage = ({ data }) => 
      data.indexOf('Connected') === -1 && action(JSON.parse(data)); 
  }
  
  export {
      get,
      getResults,
      start,
      close,
      answer,
      subscribe
  }