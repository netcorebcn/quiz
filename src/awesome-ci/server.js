var express = require('express');
var app = express();
var port = process.env.PORT || 8080;

var bodyParser = require('body-parser');
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

const token = process.env.CI_TOKEN;
const registry = process.env.REGISTRY;

var GitHubApi = require("github");
var github = new GitHubApi({
    debug: true,
    protocol: "https",
    host: "api.github.com",
    headers: { "user-agent": "awesome-ci-server" }
});

github.authenticate({
    type: "oauth",
    token: token
});

app.post('/api/ci', function(req, res) {
    var event = req.headers['x-github-event'];
    console.log("Event:" + event);

    if (event === 'pull_request')
    {
      var pull_request = req.body.pull_request;
      var action = req.body.action;

      if (action === 'opened')
      {
        // CONTINUOUS INTEGRATION
        createStatus(pull_request)
            .then(resp => runCI(pull_request, registry));
      }

      if (action === 'closed' && pull_request.merged)
      {
        // CONTINUOUS DELIVERY
        runCD(pull_request, registry, token);
      }
    }
                    
    res.sendStatus(200);
});

// start the server
app.listen(port);
console.log('Server started! At http://localhost:' + port);

const createStatus = (pull_request, state = 'pending') =>
  github.repos.createStatus({
    owner:pull_request.base.repo.owner.login,
    repo:pull_request.base.repo.name,
    sha:pull_request.head.sha,
    state:state 
  });

const runCI = (pull_request, registry) =>
    runScript(ciScript(pull_request, registry),
        () => createStatus(pull_request, 'success'),
        () => createStatus(pull_request, 'failure'));

const runCD = (pull_request, registry, ci_token) =>
    runScript(ciScript(pull_request, registry, ci_token),
        () => console.log('deployment success'),
        () => console.log("deployment failed"));

const ciScript = (pull_request, registry, ci_token = '') => `./run.sh \
            ${pull_request.base.repo.full_name}.git \
            ${pull_request.head.sha.slice(0, 7)} \
            ${registry} \
            ${ci_token}`;

var util = require('util'),
    exec = require('child_process').exec,
    child;

const runScript = (command, successHandler, errorHandler) => {
    child = exec(command,
    function (error, stdout, stderr) {
        console.log('stdout: ' + stdout);
        console.log('stderr: ' + stderr);

        // hack to detect non-zero value returned because error variable does not work
        if(stderr.indexOf('non-zero code: 1') > -1) {
            errorHandler();
        } else {
            successHandler();
        }
    });
};