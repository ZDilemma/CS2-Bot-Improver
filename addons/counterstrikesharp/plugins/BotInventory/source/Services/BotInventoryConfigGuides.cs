using System.Reflection;
using System.Text;

namespace InventorySimulator;

/// <summary>
/// Publishes the Markdown configuration guides embedded directly in BotInventory.dll
/// next to BotInventory.json.
///
/// Edit /docs/BotInventory.Chinese.md and /docs/BotInventory.English.md, rebuild,
/// and reload the plugin to publish updated guides as BotInventory.zh-CN.md and
/// BotInventory.en-US.md in the config directory.
/// </summary>
internal static class BotInventoryConfigGuides
{
    internal const string ChineseFileName = "BotInventory.zh-CN.md";
    internal const string EnglishFileName = "BotInventory.en-US.md";

    // Culture-neutral manifest names. This keeps both guides inside BotInventory.dll
    // instead of producing zh-CN/en-US satellite resource assemblies.
    private const string ChineseResourceName = "BotInventory.ConfigGuide.ChineseMarkdown";
    private const string EnglishResourceName = "BotInventory.ConfigGuide.EnglishMarkdown";

    internal static void WriteGuides(string directory)
    {
        Directory.CreateDirectory(directory);
        WriteEmbeddedGuide(ChineseResourceName, Path.Combine(directory, ChineseFileName));
        WriteEmbeddedGuide(EnglishResourceName, Path.Combine(directory, EnglishFileName));
    }

    private static void WriteEmbeddedGuide(string resourceName, string outputPath)
    {
        Assembly assembly = typeof(BotInventoryConfigGuides).Assembly;
        using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded BotInventory guide not found: {resourceName}. "
                    + $"Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}"
            );

        using var reader = new StreamReader(
            stream,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true
        );

        string content = reader.ReadToEnd().Replace("\r\n", "\n").TrimEnd() + "\n";

        if (File.Exists(outputPath))
        {
            string existing = File.ReadAllText(outputPath).Replace("\r\n", "\n");
            if (existing == content)
                return;
        }

        File.WriteAllText(outputPath, content, new UTF8Encoding(false));
    }
}
