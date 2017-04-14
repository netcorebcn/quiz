var fetch = require('node-fetch');

const url = `http://${process.env.QUIZ_URL}:81/quiz/`;

const getQuiz = () => fetch(url).then(r => r.json());

const startNewQuiz = () =>
  fetch(url, { method: 'PUT' }).then(r => r.json());

const postOption = (quizId, questionId, optionId) =>
  fetch(`${url}${quizId}`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify([{ questionId, optionId }])
  }).catch(err => console.log(err));

const random = (max) => {
  min = Math.ceil(0);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

const randomOption = (options) => options[random(options.length-1)];

const vote = ({ quizId, quizModel }) => 
    quizModel.questions.forEach(
        q => {
            var option = randomOption(q.options);
            postOption(quizId, q.id, option.id);
            console.log(`voting for quizId:${quizId} question:${q.id} option:${option.id}`);
        }
    );

console.log('loading quiz...');
getQuiz().then(data => {
    console.log('start voting...');
    for (let i=0; i<=process.env.ITERATIONS; i++){
        vote(data);
    }    
});

