import java.lang.reflect.Field
import jenkins.model.*
import org.jenkinsci.plugins.ghprb.*
import hudson.util.*
import org.jenkinsci.plugins.plaincredentials.impl.StringCredentialsImpl;
import org.jenkinsci.plugins.plaincredentials.impl.StringCredentialsImpl.DescriptorImpl;
import com.cloudbees.plugins.credentials.CredentialsScope;
import com.cloudbees.plugins.credentials.domains.Domain;
import com.cloudbees.plugins.credentials.SystemCredentialsProvider;

credentials = new StringCredentialsImpl(CredentialsScope.GLOBAL, "b1d836bd-25d3-4b58-bee8-4d9906ca1908", "Github credentials", Secret.fromString(System.getenv("GITHUB_TOKEN")));
credentialsStore = Jenkins.instance.getExtensionList(com.cloudbees.plugins.credentials.SystemCredentialsProvider.class)[0];
credentialsStore.store.addCredentials(Domain.global(), credentials);

def descriptor = Jenkins.instance.getDescriptorByType(org.jenkinsci.plugins.ghprb.GhprbTrigger.DescriptorImpl.class)
Field auth = descriptor.class.getDeclaredField("githubAuth")
auth.setAccessible(true)

githubAuth = new ArrayList<GhprbGitHubAuth>()
githubAuth.add(new GhprbGitHubAuth("", "", "", "quiz-github", null, null))

auth.set(descriptor, githubAuth)

descriptor.save()
