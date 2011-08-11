package com.kalamon.mazio;

import com.atlassian.jira.issue.Issue;
import com.atlassian.jira.plugin.issueoperation.AbstractPluggableIssueOperation;
import com.atlassian.jira.plugin.issueoperation.IssueOperationModuleDescriptor;
import com.atlassian.jira.plugin.issueoperation.PluggableIssueOperation;
import com.atlassian.jira.security.JiraAuthenticationContext;
import com.atlassian.jira.security.PermissionManager;
import com.atlassian.jira.ManagerFactory;
import com.atlassian.jira.security.Permissions;
import webwork.action.ActionContext;
import webwork.action.ServletActionContext;

import javax.servlet.http.Cookie;
import java.util.HashMap;
import java.util.Map;

/**
 * User: kalamon
 * Date: 2010-02-01
 * Time: 15:45:47
 */
public class MazioJira extends AbstractPluggableIssueOperation implements PluggableIssueOperation
{
	public boolean showOperation(Issue issue) {
		return true;
	}

	private JiraAuthenticationContext authenticationContext;

	public MazioJira(JiraAuthenticationContext _authenticationContext) {
			authenticationContext = _authenticationContext;
	}

	public void init(IssueOperationModuleDescriptor issueOperationModuleDescriptor) {
		super.init(issueOperationModuleDescriptor);
    }

	public String getHtml(Issue issue) {
        PermissionManager mgr = ManagerFactory.getPermissionManager();
        if (mgr.hasPermission(Permissions.CREATE_ATTACHMENT, issue, authenticationContext.getUser())) {
            return getBullet() + descriptor.getHtml("view", getVelocityParams(issue));
        }
        return "";
    }

	private Map<String, Object> getVelocityParams(Issue issue) {
		Map<String, Object> velocityParams = new HashMap<String, Object>();
		if (authenticationContext != null && authenticationContext.getUser() != null) {
			velocityParams.put("user", authenticationContext.getUser().getName() + "@");
		} else {
			velocityParams.put("user", "");
		}
        Cookie[] cookies = ActionContext.getRequest().getCookies();
        String sessionCookie = null;
        for (Cookie cooky : cookies) {
            if (cooky.getName().equalsIgnoreCase("JSESSIONID")) {
                sessionCookie = cooky.getValue();
                break;
            }
        }
        String url = ServletActionContext.getRequest().getRequestURL().toString();
		url = url.substring(0, url.lastIndexOf("/browse"));
		velocityParams.put("serverUrl", url);
		velocityParams.put("issueKey", issue.getKey());
        velocityParams.put("issueId", "?id=" + issue.getId());
        if (sessionCookie != null) {
            velocityParams.put("cookie", "&JSESSIONID=" + sessionCookie);
        } else {
            velocityParams.put("cookie", "");
        }
		return velocityParams;
	}
}
