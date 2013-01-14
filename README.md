Better.Net
=================

Created by [Dan Nemec](http://github.com/nemec)

Convenience extensions for reducing friction when using C# built-in types.

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

* Allows reflowing of text. This strips all newlines out of a string, then
  partitions it into chunks of N characters. There is also an option to
  avoid splitting "words" across multiple lines. The resulting split words
  can even be right-aligned using `String.PadLeft`.

        foreach(var line in "Another string of text".Reflow(10))
        {
            Console.WriteLine(line);
        }
        // A long str
        // ing of tex
        // t

        foreach(var line in "Another string of text".ReflowWords(10))
        {
            Console.WriteLine(line);
        }
        // A long
        // string of
        // text

        foreach (var line in "A long string of text".ReflowWords(10))
        {
                Console.WriteLine(line.PadLeft(10));
        }
        //    A long
        // string of
        //      text

* Pluralization. Tests an integer value and if it equals 1, returns the
  singular (first) parameter. Otherwise, returns the plural parameter.

        Console.WriteLine(1.Pluralize("sock", "socks"));
        // sock

        Console.WriteLine(10.Pluralize("sock", "socks"));
        // socks

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
