  const apiGateway = (api) => `${window.location.hostname}/${api}`;
  const apiUrl = (api, id = '') => `//${apiGateway(api)}/quiz/${id}`;
  const commandsApiUrl = (id) => apiUrl('commands', id);
 
  const post = (url, method, body) =>
      fetch(url, { method, headers: { Accept: 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  
  const get = () =>
      fetch(commandsApiUrl())
      .then(r => r.json());

  const getResults = () =>
      fetch(apiUrl('queries'))
      .then(r => r.json());
        
  const start = (quiz) =>
      post(commandsApiUrl(), 'POST', quiz)
      .then(r => r.json());

  const close = (quizId) =>
      post(commandsApiUrl(quizId), 'DELETE')
      .then(r => r.json());

  const answer = (quizId, answers) =>
      post(commandsApiUrl(quizId) , 'PUT', {quizId, answers})
      .then(r => r.json());

  const subscribe = (action) => {
      const webSocket = new WebSocket(`ws://${apiGateway('queries')}/ws`);
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