Better.Net
=================

Convenience extensions for reducing friction in C#

We're on [NuGet](https://nuget.org/packages/BetterDotNet)!

Anyone interested in proposing more features is welcome to submit a pull request or an issue.

## Better Strings

### Features

* Adds Python-like syntax for string formatting:

        "{0}:".Format("Title")

* Adds formatting syntax for dictionary keys:

        var stringDict = new Dictionary<string, string>{
          { "someKey", "value" }
        };

        Console.WriteLine("This is a {someKey}.".Format(stringDict));
        // This is a value.

        var dateDict = new Dictionary<string, DateTime>{
          { "currentDate", DateTime.Now }
        };

        Console.WriteLine("The current time is {currentDate : HH:mm:ss}"
            .Format(dateDict));
        // The current time is 04:10:55


## Better Dictionaries

### Features

  * Adds method to check an IDictionary for a certain key and, if the key
    exists, return the value and, if the key does not exist, return a default
    value.

        var dict = new Dictionary<string, int>{
          { "age", 20 }
        }

        Console.WriteLine(dict.Get("age", 18));
        // 20

        Console.WriteLine(dict.Get("height", 155));
        // 155
