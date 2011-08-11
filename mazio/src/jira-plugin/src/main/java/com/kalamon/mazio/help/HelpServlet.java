package com.kalamon.mazio.help;

import com.atlassian.jira.ComponentManager;
import com.atlassian.jira.config.properties.ApplicationProperties;
import com.atlassian.velocity.VelocityManager;
import org.apache.log4j.Logger;
import org.apache.velocity.exception.VelocityException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.HashMap;
import java.util.Map;

/**
 * User: kalamon
 * Date: 2010-02-01
 * Time: 15:50:28
 */
public class HelpServlet extends HttpServlet {

    private static final String TEMPLATE_DIR = "templates/plugins/mazio/";
    private static final String VELOCITY_EXCEPTION = "Velocity exception: ";

    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {

        String backKey = request.getParameter("backTo");
        HashMap<String, Object> map = new HashMap<String, Object>();
        map.put("backTo", backKey);
        printString(request, response, "help.vm", map);
    }

    private static void printString(HttpServletRequest req, HttpServletResponse resp,
                                    String template, Map<String, Object> velocityParams) throws IOException {

        ApplicationProperties applicationProperties = getApplicationProperties();
        resp.setContentType(applicationProperties.getContentType());
        VelocityManager velocityManager = getVelocityManager();

        final PrintWriter writer = resp.getWriter();
        try {
            String body = velocityManager.getEncodedBody(TEMPLATE_DIR, template, req.getContextPath(),
                    applicationProperties.getEncoding(), velocityParams);
            writer.write(body);
        } catch (VelocityException e) {
            writer.write(VELOCITY_EXCEPTION + e.getMessage());
            Logger.getLogger(HelpServlet.class).error(e.getMessage(), e);
        }
    }

    private static ApplicationProperties getApplicationProperties() {
        return ComponentManager.getInstance().getApplicationProperties();
    }

    private static VelocityManager getVelocityManager() {
        return ComponentManager.getInstance().getVelocityManager();
    }
}

