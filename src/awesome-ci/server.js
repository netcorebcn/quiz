var express = require('express');
var app = express();
var port = process.env.PORT || 8080;

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // support json encoded bodies
app.use(bodyParser.urlencoded({ extended: true })); // support encoded bodies

var GitHubApi = require("github");
var github = new GitHubApi({
    debug: true,
    protocol: "https",
    host: "api.github.com", // should be api.github.com for GitHub
    headers: {
        "user-agent": "awesome-ci-server" // GitHub is happy with a unique user agent
    }
});

github.authenticate({
    type: "oauth",
    token: process.env.CI_TOKEN
});

app.post('/api/ci', function(req, res) {

    var event = req.headers['x-github-event'];
    
    if (event === 'pull_request')
    {
      var pull_request = req.body.pull_request;
      var action = req.body.action;

      if (action === 'opened')
      {
        // PROCESS PULL REQUEST
        createStatus(pull_request)
        .then(resp => setTimeout(
          x => createStatus(pull_request, 'success'), 10000));
      }
    }
                
    res.sendStatus(200);
});

const createStatus = (pull_request, state = 'pending') =>
  github.repos.createStatus({
    owner:pull_request.base.repo.owner.login,
    repo:pull_request.base.repo.name,
    sha:pull_request.head.sha,
    state:state 
  });

// start the server
app.listen(port);
console.log('Server started! At http://localhost:' + port);