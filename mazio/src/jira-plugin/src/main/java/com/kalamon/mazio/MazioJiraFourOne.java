package com.kalamon.mazio;

import com.atlassian.jira.plugin.webfragment.contextproviders.AbstractJiraContextProvider;
import com.atlassian.jira.plugin.webfragment.model.JiraHelper;
import com.opensymphony.user.User;

import javax.servlet.http.Cookie;
import java.util.HashMap;
import java.util.Map;

/**
 * User: kalamon
 * Date: 2010-04-11
 * Time: 12:11:36
 */
public class MazioJiraFourOne extends AbstractJiraContextProvider {

    @Override
    public Map getContextMap(User user, JiraHelper jiraHelper) {
        Map<String, String> m = new HashMap<String, String>();
        String url = jiraHelper.getRequest().getRequestURL().toString();
        int index = url.lastIndexOf("/secure");
        if (index != -1) {
            url = url.substring(0, index);
        }
        Cookie[] cookies = jiraHelper.getRequest().getCookies();
        String sessionCookie = null;
        for (Cookie cooky : cookies) {
            if (cooky.getName().equalsIgnoreCase("JSESSIONID")) {
                sessionCookie = cooky.getValue();
                break;
            }
        }

        if (user != null) {
            m.put("user", user.getName() + "@");
        } else {
            m.put("user", "");
        }
        m.put("serverUrl", url);
        if (sessionCookie != null) {
            m.put("cookie", "&JSESSIONID=" + sessionCookie);
        } else {
            m.put("cookie", "");
        }
        return m;
    }
}
