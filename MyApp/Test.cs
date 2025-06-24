using System.Text.RegularExpressions;

class LogAnalyzer
{
    static void Main(string[] args)
    {
        string logFilePath = "sample_log.txt";
        //confirm file exists
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine("Error: File '" + logFilePath + "' not found.");
            return;
        }
        //separate lines in file into a list, reads all at once, bigger files may need to be read line by line
        var logLines = File.ReadAllLines(logFilePath).ToList();
        //make variables to hold event type counts and messages
        var eventTypeCounts = new Dictionary<string, int>();
        var eventTypeMessages = new Dictionary<string, Dictionary<string, int>>();
        var skippedCount = 0;
        //construct a regex pattern to match the log format
        string pattern = @"\[(.*?)\]\s+(\w+)\s+(.*)";
        foreach (var line in logLines)
        {
            var match = Regex.Match(line, pattern);
            //if the line matches the pattern, extract event type and message
            if (match.Success)
            {
                string eventType = match.Groups[2].Value;
                string message = match.Groups[3].Value;
                //create the event type in the dictionary if it does not exist yet
                if (!eventTypeCounts.ContainsKey(eventType)) eventTypeCounts[eventType] = 0;
                //increment count for this specific event type
                eventTypeCounts[eventType]++;
                //create the message dictionary for this event type if it does not exist yet
                if (!eventTypeMessages.ContainsKey(eventType)) eventTypeMessages[eventType] = new Dictionary<string, int>();
                //create actual message dictionary within the event type if it does not exist yet, repeat messages are ignored
                if (!eventTypeMessages[eventType].ContainsKey(message)) eventTypeMessages[eventType][message] = 0;
                //increment the message count for this event type to keep track of message count
                eventTypeMessages[eventType][message]++;
            }
            //if the line does not match the pattern, increment skipped count to account for bad format lines
            else skippedCount++;
        }
        Console.WriteLine("Event Type Counts:");
        //report the counts of each event type in descending order for better readability
        foreach (var kvp in eventTypeCounts.OrderByDescending(e => e.Value))
        {
            Console.WriteLine(kvp.Key + ":" + kvp.Value);
        }
        //report the top 3 messages for each event type, tied messages are sorted alphabetically because of .OrderByDescending and ThenBy
        Console.WriteLine("\nTop 3 Messages for Each Event Type:");
        foreach (var eventType in eventTypeMessages.Keys.OrderBy(k => k))
        {
            Console.WriteLine(eventType + ":");
            //sort messages by count in descending order, then alphabetically by message, then take top 3
            var topMessages = eventTypeMessages[eventType]
                .OrderByDescending(e => e.Value)
                .ThenBy(e => e.Key)
                .Take(3);
            //report top 3 messages for this event type
            foreach (var kvp in topMessages)
            {
                Console.WriteLine("  \"" + kvp.Key + "\" — " + kvp.Value + " time(s)");
            }
        }
        //could also store the skipped lines in order to view their format to create a better regex pattern, or a 2nd
        Console.WriteLine("\nNumber of lines skipped: " + skippedCount + "");
    }
}
