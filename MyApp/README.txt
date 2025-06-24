Handling the log file is the initial problem to solve. I created an item group in the .csproj file, 
this keeps the log file in the directory with the executable, meaning it can be read without specifying
a complicated path. Next was separating the logs into lines to process, and separating those lines 
into groups, for the timestamp, event type, and message. I did this using regular expressions. 
Next issue was keeping track of event types and messages. Since we need counts of the event types 
and each message within them, I used a nested dictionary, then iterated through each line.

I assumed the log file format is consistent and that sample_log.text is the only one being used. 
Any additional text files would need to be added in the item group.

Some limitations I noticed was the situation where the top 3 messages for an event type have 
many with the same occurrence. Instead of simply listing every single tied message here, because 
that could get very long, I just kept it alphabetical for ties. There is also the potential of log 
lines not matching the given format. Even though I assumed for this project that the lines in the 
text file are consistent, but still made a count to ensure all lines are processed. 
So, hypothetically you could see that a line was skipped, analyze that line, and altar 
the regex pattern to work for it, or even create a 2nd regex to handle those cases. 
Another limitation could be extremely large log files, since we read them all at once, could fill up 
to fast, so a potential fix could be reading lines one at a time, and clearing memory after each. 