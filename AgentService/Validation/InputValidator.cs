using System.Text.RegularExpressions;
using AgentService.Models;

namespace AgentService.Validation;

public abstract class InputValidator {
    public static bool validateAgent(Agent agent) {
        agent.codeName = agent.codeName?.Trim();
        agent.realName = agent.realName?.Trim();
        agent.burnerPhone = agent.burnerPhone?.Trim();
        
        if (agent.codeName?.Length is > 6 or < 2) return false;
        if (agent.realName == null || agent.realName.Length < 2 || agent.realName.Length > 30) return false;
        return isValidPhoneNumber(agent.burnerPhone) && isValidSecurityClearance(agent.securityClearance);
    }

    private static bool isValidSecurityClearance(string agentSecurityClearance) 
        => Enum.GetValues(typeof(SecurityClearance))
            .Cast<object>()
            .Any(clearance => agentSecurityClearance == clearance.ToString());
    
    private static bool isValidPhoneNumber(string phoneNumber) {
        const string phoneRegex = @"^\d{10}$";
        return Regex.IsMatch(phoneNumber, phoneRegex, RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }
}