using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MonkeyMcp.Mcp.Prompts;

[McpServerPromptType]
public class MonkeyPrompts
{
    [McpServerPrompt]
    [Description("Generate a formatted table of all available monkeys with their key information.")]
    public static string GetMonkeysTablePrompt()
    {
        return """
        Please provide a comprehensive list of all available monkeys and organize them in a well-formatted table.
        Include the following columns: Name, Location, Population, and a brief description.
        Sort the monkeys alphabetically by name for easy reference.
        """;
    }

    [McpServerPrompt]
    [Description("Generate detailed information about a specific monkey species.")]
    public static string GetMonkeyDetailsPrompt(
        [Description("The name of the monkey species to get detailed information about")] string name)
    {
        return $"""
        Please provide comprehensive details about the {name} monkey species.
        Include information about:
        - Physical characteristics and appearance
        - Natural habitat and geographical location
        - Population status and conservation concerns
        - Behavioral traits and social structure
        - Diet and feeding habits
        - Any interesting facts or unique features
        
        Format the information in a clear, educational manner suitable for both general audiences and researchers.
        """;
    }

    [McpServerPrompt]
    [Description("Generate a comparison between multiple monkey species.")]
    public static string CompareMonkeysPrompt(
        [Description("Comma-separated list of monkey names to compare")] string monkeyNames)
    {
        return $"""
        Please provide a detailed comparison of the following monkey species: {monkeyNames}
        
        Create a comparison table that includes:
        - Physical characteristics (size, weight, distinctive features)
        - Habitat preferences and geographical distribution
        - Population numbers and conservation status
        - Social behavior and group dynamics
        - Diet and foraging patterns
        - Unique adaptations or interesting facts
        
        Highlight the key similarities and differences between these species.
        """;
    }
}