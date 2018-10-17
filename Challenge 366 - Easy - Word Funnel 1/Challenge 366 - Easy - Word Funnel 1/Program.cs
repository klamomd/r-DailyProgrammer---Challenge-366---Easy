using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge_366___Easy___Word_Funnel_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string choice;
            while (true)
            {
                Console.Write("Please choose mode to run in (options are funnel and bonus1): ");
                choice = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                if (choice == "funnel" || choice == "bonus1") break;
                else Console.WriteLine("Invalid selection: " + choice);
            }
            Console.WriteLine();

            if (choice == "funnel")
            {
                while (true)
                {
                    Console.Write("Please enter big string (\\q to quit): ");
                    string bigString = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                    if (bigString == "\\q") break;

                    Console.Write("Please enter small string (\\q to quit): ");
                    string smallString = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                    if (smallString == "\\q") break;

                    Console.WriteLine(string.Format("Results of funnel({0}, {1}): {2}\n", bigString, smallString, funnel(bigString, smallString)));
                }
            }
            else if (choice == "bonus1")
            {
                List<string> words = new List<string>();
                string line;

                // Parse all words from wordList.
                using (StreamReader reader = File.OpenText("./wordList.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        words.Add(line);
                    }
                }

                while (true)
                {
                    Console.Write("Please enter big string (\\q to quit): ");
                    string bigString = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                    if (bigString == "\\q") break;

                    // For each word in the word list that passes the funnel check, add it to the list of smallStrings.
                    List<string> smallStrings = new List<string>();
                    foreach (var word in words)
                    {
                        if (funnel(bigString, word) && !smallStrings.Contains(word))
                        {
                            smallStrings.Add(word);
                        }
                    }

                    // Build the results string showing all strings that can be created by removing a single letter from bigString.
                    StringBuilder outString = new StringBuilder(string.Format("Results of bonus1({0}): ", bigString));
                    for (int i=0; i<smallStrings.Count; i++)
                    {
                        outString.Append(smallStrings[i]);
                        if (i != smallStrings.Count - 1) outString.Append(", ");    // Append a comma only if we're not at the last string.
                    }
                    outString.AppendLine();
                    Console.WriteLine(outString.ToString());
                }
            }
        }


        private static bool funnel(string bigString, string smallString)
        {
            if (string.IsNullOrWhiteSpace(bigString)) throw new ArgumentException("bigString cannot be null or whitespace.", "bigString");
            if (string.IsNullOrWhiteSpace(smallString)) throw new ArgumentException("smallString cannot be null or whitespace.", "smallString");

            // smallString cannot be created from bigString by removing a single character if it is not 1 character shorter than bigString.
            if (smallString.Length != bigString.Length - 1) return false;

            // Keep track of whether we've come across a missing character.
            bool foundFirstDiscrepancy = false;

            int smallCtr = 0;
            for (int bigCtr = 0; bigCtr < bigString.Length; bigCtr++)
            {
                // If smallCtr has gone out of bounds, then we've matched the entirety of smallString to bigString so far, so the last character must be the one that is missing and thus satisfies the requirements.
                if (smallCtr >= smallString.Length) return true;

                // Check each string's current character (indicated by smallCtr and bigCtr). If they match, increment the counters (bigCtr is done automatically) and continue to check the rest of the string. If not, handle based on foundFirstDiscrepancy (FFD).
                if (bigString[bigCtr] == smallString[smallCtr])
                {
                    smallCtr++;
                    continue;
                }
                // If FFD is not set, then we've found the first discrepancy and only allow bigCtr to increment. If the rest of the string then matches, then smallString can be made by removing a single character from bigString.
                else if (!foundFirstDiscrepancy)
                {
                    foundFirstDiscrepancy = true;
                    continue;
                }
                // If FFD is set, then we've found a second discrepancy in the strings and we must return false.
                else return false;
            }

            // If we've made it out of the loop, then the strings have matched with only a single discrepancy, thus satisfying the requirements.
            return true;
        }
    }
}
