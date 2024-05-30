using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryApp
{
    public partial class GameWindow : Window
    {
        private List<Word> allWords;
        private List<bool> answerStates;
        private int currentWordIndex = 0;
        private int correctAnswers = 0;
        private Button nextButton;
        private Button previousButton;
        private Button finishButton;

        public GameWindow()
        {
            InitializeComponent();
            LoadWords();
            ShuffleWords();
            DisplayWord();
            txtUserInput.TextChanged += TxtUserInput_TextChanged;

            Closing += GameWindow_Closing;
        }

        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (currentWordIndex < allWords.Count)
            {
                MessageBoxResult result = MessageBox.Show("Esti sigur ca vrei sa parasesti jocul?", "Iesi din joc", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        private void TxtUserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string prefix = txtUserInput.Text.Trim();
            if (!string.IsNullOrEmpty(prefix))
            {
                SearchByPrefix(prefix);
            }
            else
            {
                ClearMatchedWords();
            }
        }

        private void SearchByPrefix(string prefix)
        {
            List<string> matchedWords = allWords
                .Where(w => w.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(w => w.Name)
                .ToList();
            listBoxMatchedWords.ItemsSource = matchedWords;
        }

        private void ClearMatchedWords()
        {
            listBoxMatchedWords.ItemsSource = null;
        }

        private void listBoxMatchedWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWord = listBoxMatchedWords.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedWord))
            {
                txtUserInput.Text = selectedWord;
            }
        }

        private void LoadWords()
        {
            try
            {
                Search search = new Search();
                search.LoadWords("words.txt");

                allWords = search.Words.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
                answerStates = new List<bool>(allWords.Count);
                for (int i = 0; i < allWords.Count; i++)
                {
                    answerStates.Add(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading words: {ex.Message}");
            }
        }


        private void ShuffleWords()
        {
            Random rng = new Random();
            int n = allWords.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Word value = allWords[k];
                allWords[k] = allWords[n];
                allWords[n] = value;
            }
        }

        private void DisplayWord()
        {
            if (currentWordIndex >= 0 && currentWordIndex < allWords.Count)
            {
                txtUserInput.Text = "";
                txtUserInput.Visibility = Visibility.Visible;
                txtUserInput.Focus();
                txtUserInput.SelectAll();
                txtUserInput.IsEnabled = true;
                txtUserInput.IsReadOnly = false;
                txtUserInput.Margin = new Thickness(0, 20, 0, 0);

                Random rnd = new Random();
                bool showImage = rnd.Next(2) == 0;

                if (showImage && allWords[currentWordIndex].GetImage() != null)
                {
                    imgWord.Source = allWords[currentWordIndex].GetImage();
                    imgWord.Visibility = Visibility.Visible;
                    txtDescription.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtDescription.Text = allWords[currentWordIndex].Description;
                    txtDescription.Visibility = Visibility.Visible;
                    imgWord.Visibility = Visibility.Hidden;
                }

                UpdateButtons();
            }
            else
            {
                MessageBox.Show($"Sfarsit!\nRaspunsuri corecte: {correctAnswers}/{allWords.Count}");

                btnPrevious.Visibility = Visibility.Collapsed;
                btnNext.Visibility = Visibility.Collapsed;
                finishButton.Visibility = Visibility.Collapsed;

                btnPlayAgain.Visibility = Visibility.Visible;
            }
        }


        private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            currentWordIndex = 0;
            correctAnswers = 0;
            ShuffleWords();
            DisplayWord();

            btnPlayAgain.Visibility = Visibility.Collapsed;
        }

        private void UpdateButtons()
        {
            if (nextButton != null)
            {
                if (currentWordIndex == allWords.Count - 1)
                {
                    nextButton.Visibility = Visibility.Collapsed;
                    finishButton.Visibility = Visibility.Visible;
                }
                else
                {
                    nextButton.Visibility = Visibility.Visible;
                    finishButton.Visibility = Visibility.Collapsed;
                }
            }

            if (previousButton != null)
            {
                if (currentWordIndex == 0)
                {
                    previousButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    previousButton.Visibility = Visibility.Visible;
                }
            }
        }


        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentWordIndex > 0)
            {
                currentWordIndex--;

                if (answerStates[currentWordIndex])
                {
                    correctAnswers--;
                }

                DisplayWord();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();

            if (currentWordIndex < allWords.Count - 1)
            {
                currentWordIndex++;
                DisplayWord();
            }
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();

            MessageBox.Show($"Sfarsit!\nRaspunsuri corecte: {correctAnswers}/{allWords.Count}");

            btnPrevious.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;
            finishButton.Visibility = Visibility.Collapsed;

            btnPlayAgain.Visibility = Visibility.Visible;
        }

        private void CheckAnswer()
        {
            if (txtUserInput.Text.Trim().ToLower() == allWords[currentWordIndex].Name.ToLower())
            {
                correctAnswers++;
                answerStates[currentWordIndex] = true;
            }
            else
            {
                answerStates[currentWordIndex] = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nextButton = (Button)FindName("btnNext");
            previousButton = (Button)FindName("btnPrevious");
            finishButton = (Button)FindName("btnFinish");

            if (previousButton != null)
                previousButton.Visibility = Visibility.Collapsed;
            if (finishButton != null)
                finishButton.Visibility = Visibility.Collapsed;
        }
    }
}
