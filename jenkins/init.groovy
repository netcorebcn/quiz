import java.lang.reflect.Field
import jenkins.model.*
import org.jenkinsci.plugins.ghprb.*
import hudson.util.*
import org.jenkinsci.plugins.plaincredentials.impl.StringCredentialsImpl;
import org.jenkinsci.plugins.plaincredentials.impl.StringCredentialsImpl.DescriptorImpl;
import com.cloudbees.plugins.credentials.CredentialsScope;
import com.cloudbees.plugins.credentials.domains.Domain;
import com.cloudbees.plugins.credentials.SystemCredentialsProvider;
import org.jenkinsci.plugins.github.config.*
import java.net.URL
import javaposse.jobdsl.dsl.DslScriptLoader
import javaposse.jobdsl.plugin.JenkinsJobManagement

credentials = new StringCredentialsImpl(CredentialsScope.GLOBAL, "github-token", "github credentials", Secret.fromString(System.getenv("GITHUB_TOKEN")));
credentialsStore = Jenkins.instance.getExtensionList(com.cloudbees.plugins.credentials.SystemCredentialsProvider.class)[0];
credentialsStore.store.addCredentials(Domain.global(), credentials);

// configure github plugin
url = new URL('http', System.getenv("JENKINS_URL"), 80, '/github-webhook/')
def pluginConfig = Jenkins.instance.getExtensionList(GitHubPluginConfig.class)[0]
GitHubServerConfig serverConfig = new GitHubServerConfig('github-token')
pluginConfig.setConfigs([serverConfig])
pluginConfig.setOverrideHookUrl(true)
pluginConfig.setHookUrl(url)
pluginConfig.save()

// configure github pull request builder plugin
def descriptor = Jenkins.instance.getDescriptorByType(org.jenkinsci.plugins.ghprb.GhprbTrigger.DescriptorImpl.class)
Field auth = descriptor.class.getDeclaredField("githubAuth")
auth.setAccessible(true)

githubAuth = new ArrayList<GhprbGitHubAuth>()
githubAuth.add(new GhprbGitHubAuth("", "http://" + System.getenv("JENKINS_URL") + "/","github-token", "quiz-github", null, null))

auth.set(descriptor, githubAuth)

descriptor.save()

// create DSL Jobs
def jobDslScript = new File('/usr/share/jenkins/jobs.groovy')
def workspace = new File('.')

def jobManagement = new JenkinsJobManagement(System.out, [:], workspace)

new DslScriptLoader(jobManagement).runScript(jobDslScript.text)

// configure kubectl contexts for each environment
command = "kubectl config set-context staging --namespace=staging"
println command.execute().text

command = "kubectl config set-context production --namespace=production"
println command.execute().text