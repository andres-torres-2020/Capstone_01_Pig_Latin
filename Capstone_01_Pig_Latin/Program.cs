/*
 * A.TORRES
 *
 * CAPSTONE # 1
 * Translate from English to Pig Latin
 *
 * What will the application do?
 * The application prompts the user for a word.
 * The application translates the text to Pig Latin and displays it on the console.
 * The application asks the user if he or she wants to translate another word.
 *
 * Build Specifications:
 * Convert each word to a lowercase before translating.
 * If a word starts with a vowel, just add “way” onto the ending.
 * if a word starts with a consonant, move all of the consonants that appear
 *     before the first vowel to the end of the word, then add “ay” to the end of the word.
 *
 * Additional Requirements:
 * For answering Lab Summary when submitting to the LMS
 * if there are any syntax errors or if the program does not run (for example, in a Main method).
 *
 * Extended Exercises (2 points maximum):
 * Keep the case of the word, whether its uppercase (WORD), title case (Word), or lowercase (word).
 * Allow punctuation in the input string.
 * Translate words with contractions. Ex: can’t become an’tcay
 * Don’t translate words that have numbers or symbols. Ex: 189 should be left
 *     as 189 and hello@grandcircus.co should be left as hello@grandcircus.co.
 * Check that the user has actually entered text before translating.
 * Make the application take a line instead of a single word, and then find the
 *     Pig Latin for each word in the line.
 *
 * Hints:
 *     Treat “y” as a consonant.
 */
using System;
using System.Collections;

namespace Capstone_01_Pig_Latin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Pig Latin Translator!\n\n");
            string canContinue = "";
            do
            {
                // prompt the user to enter an integer
                string input = ReadString("Enter a line to be translated: ");

                // translate the input text to Pig Latin
                string piglatin = TranslateToPigLatin(input);

                Console.WriteLine($"{piglatin}\n");

                // prompt the user to continue
                canContinue = ReadString("Translate another line? (y/n):  ").ToLower();
            } while (canContinue == "y");
        }
        public static string ReadString(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine().Trim();
        }
        public static int ReadInteger(string message)
        {
            string input;
            int number = -1;
            do
            {
                input = ReadString(message);
            } while (int.TryParse(input, out number) == false);
            return number;
        }
        public static ArrayList Tokenize(string input, char separator)
        {
            // manually split a string with the given separator
            // and include the separator as its own value
            ArrayList l = new ArrayList();
            char[] arr = input.ToCharArray();
            int startIndex;
            int nextIndex;
            string sub;
            for (int i=0; i < arr.Length; )
            {
                startIndex = i;
                nextIndex = i + 1;
                sub = "" + arr[i];
                if (arr[i] == separator)
                {
                    // make a string of the sequence of separators
                    while (nextIndex < arr.Length && arr[nextIndex] == separator)
                    {
                        sub += arr[nextIndex];
                        nextIndex++;
                    }
                }
                else
                {
                    // make a string of the sequence of non-separators
                    while (nextIndex < arr.Length && arr[nextIndex] != separator)
                    {
                        sub += arr[nextIndex];
                        nextIndex++;
                    }
                }
                i = nextIndex;
                l.Add(sub);
            }
            return l;
        }
        public static ArrayList SplitByPunctuation(ArrayList words, char separator)
        {
            ArrayList punc = new ArrayList();
            foreach (string w in words)
            {
                ArrayList moreWords = Tokenize(w, separator);
                punc.AddRange(moreWords);
            }
            return punc;
        }
        public static string TranslateToPigLatin(string input)
        {
            // separate words into a list of strings,
            // first by white space and then further by punctuators
            string plwords = "";
            ArrayList words = Tokenize(input, ' ');
            words = SplitByPunctuation(words, '\t');
            words = SplitByPunctuation(words, ',');
            words = SplitByPunctuation(words, '!');
            words = SplitByPunctuation(words, ':');
            words = SplitByPunctuation(words, '?');

            // translate each word; each punctuator is a separate word
            // and will be concantenated into the final translation stream
            foreach (string w in words)
            {
                if (w == null || w == "")
                {
                    plwords += " ";
                }
                else
                {
                    plwords += TranslateWordToPigLatin(w);
                }
            }
            return plwords;
        }
        public static bool HasNonAlphaChar(string s)
        {
            // return true if input string has at least one non-alphabet character
            foreach (char c in s.ToLower())
            {
                if ((c < 'a' || c > 'z') && c != '\'')
                {
                    return true;
                }
            }
            return false;
        }
        public static string TranslateWordToPigLatin(string input)
        {
           const string CONSONANTS = "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ";

            // convert each word to a lowercase before translating
            string word = input; //.ToLower();

            if (input.Length < 1 || HasNonAlphaChar(word))
            {
                return word;
            }

            string firstChar = word.Substring(0, 1);
            if ("aeiouAEIOU".Contains(firstChar))
            {
                // word has only letters and
                // word starts with a vowel, just add “way” onto the ending
                word += "way";
            }
            else if (CONSONANTS.Contains(firstChar))
            {
                // word has only letters and
                // if a word starts with a consonant,
                //    move all of the consonants that appear before the first vowel to the end of the word,
                //    then add “ay” to the end of the word.

                // find the end position of the consonant sequence
                string consonantPrefix;
                char[] inputWord = word.ToCharArray();
                int endPos = inputWord.Length;
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if ((CONSONANTS).Contains(inputWord[i]) == false)
                    {
                        // found the position of the first non-consonant
                        endPos = i;
                        break;
                    }
                }

                // the consonant prefix
                consonantPrefix = word.Substring(0, endPos);

                // remove prefix from input word (by extracting the rest of the word)
                word = word.Substring(consonantPrefix.Length);

                // make the pig latin version of the word
                word += consonantPrefix + "ay";
            }

            return word;
        }
    }
}
