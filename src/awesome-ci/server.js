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
        // CONTINUOUS INTEGRATION
        createStatus(pull_request)
            .then(resp => runCI(pull_request));
      }

      if (action === 'merged')
      {
        // CONTINUOUS DELIVERY
        runCD(pull_request);
      }
    }
                
    res.sendStatus(200);
});

const runCI = (pull_request) =>
    runScript(`./run.sh ${pull_request.base.repo.full_name}.git ${pull_request.head.sha}`,
        () => createStatus(pull_request, 'success'),
        () => createStatus(pull_request, 'failure'));

const runCD = (pull_request) =>
    runScript(`./run.sh ${pull_request.base.repo.full_name}.git ${pull_request.head.sha}`,
        () => console.log('deployment success'),
        () => console.log("deployment failed"));

const createStatus = (pull_request, state = 'pending') =>
  github.repos.createStatus({
    owner:pull_request.base.repo.owner.login,
    repo:pull_request.base.repo.name,
    sha:pull_request.head.sha,
    state:state 
  });

var util = require('util'),
    exec = require('child_process').exec,
    child;

const runScript = (command, success, error) => {
    child = exec(command, // command line argument directly in string
    function (error, stdout, stderr) {      // one easy function to capture data/errors
        console.log('stdout: ' + stdout);
        console.log('stderr: ' + stderr);
        if (error !== null) {
            console.log('exec error: ' + error);
            error();
        } else {
            success();
        }
    });
};

// start the server
app.listen(port);
console.log('Server started! At http://localhost:' + port);