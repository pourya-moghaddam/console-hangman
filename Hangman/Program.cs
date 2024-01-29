using System;
using System.Diagnostics.Tracing;

namespace Hangman
{
    public class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Random random = new Random();
                List<string> words = GetWordsList();
                string word = words[random.Next(words.Count)];
                Play(word);
                Console.Write("\nWanna play again? (Y/N) ");
            } while (Console.ReadLine().ToUpper() == "Y");
        }

        static void Play(string word)
        {
            word = word.ToUpper();
            HashSet<char> wordLetters = new HashSet<char>(word.ToCharArray());
            char[] alphabet = Enumerable.Range('A', 26)
                .Select(asciiCode => (char)asciiCode).ToArray();
            HashSet<char> usedLetters = new HashSet<char>();

            int lives = 6;

            while ((wordLetters.Count > 0) && (lives > 0))
            {
                Console.WriteLine(
                    "You have {0} lives left and you have used these letters: {1}",
                    lives,
                    String.Join(", ", usedLetters));

                List<char> currentWordList = new List<char>();

                foreach (char c in word)
                {
                    if (usedLetters.Contains(c))
                    {
                        currentWordList.Add(c);
                    }
                    else
                    {
                        currentWordList.Add('-');
                    }
                }

                DisplayHangman(lives);
                Console.WriteLine("Current word: {0}",
                    String.Join(' ', currentWordList));
                char userLetter = '.';
                try
                {
                    Console.Write("Guess a letter: ");
                    userLetter = Convert.ToChar(Console.ReadLine().ToUpper());
                }
                catch (FormatException ex) {}

                if (alphabet.Except(usedLetters).Contains(userLetter))
                {
                    usedLetters.Add(userLetter);
                    if (wordLetters.Contains(userLetter))
                    {
                        wordLetters.Remove(userLetter);
                    }
                    else
                    {
                        lives--;
                        Console.WriteLine("Letter is not in word.");
                    }
                }
                else if (usedLetters.Contains(userLetter))
                {
                    Console.WriteLine("You have already used that letter. Please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid character. Please try again.");
                }
                Console.WriteLine();
            }

            if (lives == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DisplayHangman(lives);
                Console.WriteLine("You died. The word was '{0}'.", word);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You won!! You guessed the word '{0}'.", word);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static List<string> GetWordsList()
        {
            string GetFilePath(string folderName, string fileName, string currentDir)
            {
                int folderNameIndex = currentDir.Length - folderName.Length;
                if (currentDir.LastIndexOf(folderName) == folderNameIndex)
                {
                    return Path.Combine(currentDir, fileName); ;
                }
                else
                {
                    return GetFilePath(
                        folderName, fileName, Directory.GetParent(currentDir).ToString());
                }
            }

            List<string> words = new List<string>();
            string wordsFilePath = GetFilePath(
                "Hangman", "words.txt", Directory.GetCurrentDirectory());

            foreach (string word in File.ReadAllLines(wordsFilePath))
            {
                words.Add(word);
            }

            return words;
        }

        static void DisplayHangman(int lives)
        {
            string[] stages = {
                // final state: head, torso, both arms, and both legs
                @"
                   --------
                   |      |
                   |      O
                   |     \|/
                   |      |
                   |     / \
                   -
                ",
                // head, torso, both arms, and one leg
                @"
                   --------
                   |      |
                   |      O
                   |     \|/
                   |      |
                   |     / 
                   -
                ",
                // head, torso, and both arms
                @"
                   --------
                   |      |
                   |      O
                   |     \|/
                   |      |
                   |      
                   -
                ",
                // head, torso, and one arm
                @"
                   --------
                   |      |
                   |      O
                   |     \|
                   |      |
                   |     
                   -
                ",
                // head and torso
                @"
                   --------
                   |      |
                   |      O
                   |      |
                   |      |
                   |     
                   -
                ",
                // head
                @"
                   --------
                   |      |
                   |      O
                   |    
                   |      
                   |     
                   -
                ",
                // initial empty state
                @"
                   --------
                   |      |
                   |      
                   |    
                   |      
                   |     
                   -
                "};
            Console.WriteLine(stages[lives]);
        }
    }
}
