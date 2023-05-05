using Amazon;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using System.Diagnostics;

// Replace with your python exe path
string pythonExe = "C:\\Users\\YourUserName\\AppData\\Local\\Programs\\Python\\Python311\\python.exe";

// Replace with the path where you want to create code projects
string actionOutputDirectory = "C:\\MyCodeRootPath";

// Replace with your aws cli profile name
AWSConfigs.AWSProfileName = "your-aws-profile";
var comprehendClient = new AmazonComprehendClient();

string[] projectTypes = { "console", "webapi", "wpf", "sln" };
string[] verbs = { "create", "add", "remove" };

while (true)
{
    Console.WriteLine("\nEnter your prompt: ");
    var prompt = Console.ReadLine();

    if (string.IsNullOrEmpty(prompt))
        break;

    DetectSyntaxResponse detectedSyntaxResponse = await GetChomprehensionAnalysis(comprehendClient, prompt);

    // Initialize variables to hold the attribute values
    string action = "";
    string type = "";
    string name = "";

    foreach (var syntaxToken in detectedSyntaxResponse.SyntaxTokens)
    {
        ProcessSyntaxToken(ref action, ref type, ref name, syntaxToken);
    }

    // Print the attribute values
    Console.WriteLine("Action: " + action);
    Console.WriteLine("Type: " + type);
    Console.WriteLine("Name: " + name);

    Console.WriteLine("\nWould you like to proceed with these values?: y/n");
    prompt = Console.ReadLine();

    if (prompt != "y")
        break;

    ProcessAction(action, type, name);
}

async Task<DetectSyntaxResponse> GetChomprehensionAnalysis(AmazonComprehendClient comprehendClient, string? prompt)
{
    var detectedSyntaxRequest = new DetectSyntaxRequest
    {
        LanguageCode = SyntaxLanguageCode.En,
        Text = prompt
    };
    var detectedSyntaxResponse = await comprehendClient.DetectSyntaxAsync(detectedSyntaxRequest);
    return detectedSyntaxResponse;
}

void ProcessSyntaxToken(ref string action, ref string type, ref string name, SyntaxToken syntaxToken)
{
    var partOfSpeechTag = syntaxToken.PartOfSpeech.Tag;
    var syntaxTextLower = syntaxToken.Text.ToLower();

    if (partOfSpeechTag == PartOfSpeechTagType.VERB)
    {
        // thinks like "name it", "call it" need to be ignored
        if (verbs.Contains(syntaxTextLower))
        {
            action = syntaxToken.Text;
        }
    }

    if (partOfSpeechTag == PartOfSpeechTagType.NOUN)
    {
        foreach (var projectType in projectTypes)
        {
            if (syntaxTextLower.Contains(projectType))
            {
                type = projectType;
                break;
            }
        }
    }

    if (partOfSpeechTag == PartOfSpeechTagType.PROPN)
    {
        // Preserve casing
        name = syntaxToken.Text;
    }
}

void ProcessAction(string action, string type, string name)
{    
    string script = "dotnet-helper.py";
    string arguments = $"{action} {name} {type} {actionOutputDirectory}";

    ProcessStartInfo start = new ProcessStartInfo();
    start.FileName = pythonExe;
    start.Arguments = $"{script} {arguments}";
    start.UseShellExecute = false;
    start.RedirectStandardOutput = true;

    using (Process process = Process.Start(start))
    {
        using (StreamReader reader = process.StandardOutput)
        {
            string result = reader.ReadToEnd();
            Console.WriteLine(result);
        }
        process.WaitForExit();
    }
}