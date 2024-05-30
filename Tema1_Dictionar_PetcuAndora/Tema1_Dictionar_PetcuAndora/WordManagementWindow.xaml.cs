using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryApp
{
    public partial class WordManagementWindow : Window
    {
        private string wordsFilePath = "words.txt";
        private List<string> allWords;
        public event EventHandler WordsChanged;
        private MainWindow mainWindow;

        public WordManagementWindow(MainWindow main)
        {
            InitializeComponent();
            LoadWords();
            mainWindow = main;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.ReloadWords(); 
        }

        private void LoadWords()
        {
            try
            {
                string[] lines = File.ReadAllLines(wordsFilePath);
                allWords = lines.Select(line => line.Split('|')[1].Trim()).ToList();
                cmbWords.ItemsSource = allWords;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading words: {ex.Message}");
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            cmbWords.ItemsSource = allWords.Where(word => word.ToLower().Contains(searchText)).ToList();
        }

        private void cmbWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = cmbWords.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                string[] lines = File.ReadAllLines(wordsFilePath);
                string selectedLine = lines.FirstOrDefault(line => line.Split('|')[1].Trim() == selectedCategory);
                if (selectedLine != null)
                {
                    string[] parts = selectedLine.Split('|');
                    txtWord.Text = parts[0].Trim();
                    txtCategory.Text = parts[1].Trim();
                    txtDescription.Text = parts[2].Trim();
                    txtImagePath.Text = parts[3].Trim();
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string word = $"{txtWord.Text.Trim()}|{txtCategory.Text.Trim()}|{txtDescription.Text.Trim()}|{txtImagePath.Text.Trim()}";
                File.AppendAllLines(wordsFilePath, new[] { word });
                MessageBox.Show("Cuvant adaugat!");
                OnWordsChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adaugarea cuvantului: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string wordToDelete = txtWord.Text.Trim();
                string[] lines = File.ReadAllLines(wordsFilePath);
                File.WriteAllLines(wordsFilePath, lines.Where(line => !line.StartsWith(wordToDelete + "|")));
                MessageBox.Show("Cuvant sters!");
                OnWordsChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la stergerea cuvantului: {ex.Message}");
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string wordToUpdate = txtWord.Text.Trim();
                string newWordData = $"{txtWord.Text.Trim()}|{txtCategory.Text.Trim()}|{txtDescription.Text.Trim()}|{txtImagePath.Text.Trim()}";
                string[] lines = File.ReadAllLines(wordsFilePath);
                List<string> updatedLines = lines.Where(line => !line.StartsWith(wordToUpdate + "|")).ToList();
                updatedLines.Add(newWordData);
                File.WriteAllLines(wordsFilePath, updatedLines);
                MessageBox.Show("Cuvant editat!");
                OnWordsChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la editarea cuvantului: {ex.Message}");
            }
        }

        protected virtual void OnWordsChanged()
        {
            WordsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
