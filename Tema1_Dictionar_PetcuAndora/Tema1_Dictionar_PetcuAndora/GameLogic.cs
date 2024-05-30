//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;

//namespace DictionaryApp
//{
//    public class GameLogic
//    {
//        private List<Word> allWords;
//        private List<bool> answerStates;
//        private int currentWordIndex = 0;
//        private int correctAnswers = 0;

//        public GameLogic()
//        {
//            LoadWords();
//            ShuffleWords();
//        }

//        private void LoadWords()
//        {
//            try
//            {
//                Search search = new Search();
//                search.LoadWords("words.txt");

//                allWords = search.Words.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
//                answerStates = new List<bool>(allWords.Count);
//                for (int i = 0; i < allWords.Count; i++)
//                {
//                    answerStates.Add(false);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error loading words: {ex.Message}");
//            }
//        }

//        public void ShuffleWords()
//        {
//            Random rng = new Random();
//            int n = allWords.Count;
//            while (n > 1)
//            {
//                n--;
//                int k = rng.Next(n + 1);
//                Word value = allWords[k];
//                allWords[k] = allWords[n];
//                allWords[n] = value;
//            }
//        }

//        public void CheckAnswer(string userInput)
//        {
//            if (userInput.Trim().ToLower() == allWords[currentWordIndex].Name.ToLower())
//            {
//                correctAnswers++;
//                answerStates[currentWordIndex] = true;
//            }
//            else
//            {
//                answerStates[currentWordIndex] = false;
//            }
//        }

//        public void MoveToNextWord()
//        {
//            currentWordIndex++;
//            if (currentWordIndex >= allWords.Count)
//            {
//                ShowGameSummary();
//            }
//        }

//        public void MoveToPreviousWord()
//        {
//            currentWordIndex--;
//            if (currentWordIndex < 0)
//            {
//                currentWordIndex = 0;
//            }
//        }

//        private void ShowGameSummary()
//        {
//            MessageBox.Show($"Sfarsit!\nRaspunsuri corecte: {correctAnswers}/{allWords.Count}");
//        }

//        public List<string> SearchByPrefix(string prefix)
//        {
//            List<string> matchedWords = allWords
//                .Where(w => w.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
//                .Select(w => w.Name)
//                .ToList();

//            return matchedWords;
//        }

//        public void DecrementCorrectAnswersCount()
//        {
//            correctAnswers--;
//        }


//        public Word GetCurrentWord()
//        {
//            return allWords[currentWordIndex];
//        }

//        public int GetCurrentWordIndex()
//        {
//            return currentWordIndex;
//        }

//        public int GetCorrectAnswersCount()
//        {
//            return correctAnswers;
//        }

//        public int GetTotalWordsCount()
//        {
//            return allWords.Count;
//        }

//        public List<bool> GetAnswerStates()
//        {
//            return answerStates;
//        }
//        public void ResetGame()
//        {
//            correctAnswers = 0;
//        }

//    }
//}
