using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryApp
{

    public partial class MainWindow : Window
    {
        private Search search = new Search();
        private List<Administrator> administrators = new List<Administrator>();
        private List<string> categories = new List<string>();
        private MainWindow mainWindow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadAdministrators();
            search.LoadWords("words.txt");
            LoadCategories(); Dictionary<string, Word> dictionary = search.GetDictionary();
        }

        private void LoadAdministrators()
        {
            try
            {
                string[] lines = File.ReadAllLines("administrators.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        administrators.Add(new Administrator { Email = parts[0], Password = parts[1] });
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Administrators file not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading administrators: {ex.Message}");
            }
        }

        private void WordManagementWindow_WordsChanged(object sender, EventArgs e)
        {
            LoadCategories();
            if (cmbCategories.SelectedItem != null)
            {
                string selectedCategory = cmbCategories.SelectedItem as string;
                SearchByCategory(selectedCategory);
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            Console.WriteLine($"User: {username}");
            Console.WriteLine($"Parola: {password}");

            Administrator admin = administrators.FirstOrDefault(a => a.Email == username && a.Password == password);
            if (admin != null)
            {
                MessageBox.Show("Logarea a avut succes!");
                WordManagementWindow wordManagementWindow = new WordManagementWindow(this); 
                wordManagementWindow.Closed += WordManagementWindow_Closed; 
                wordManagementWindow.Show();
            }
            else
            {
                MessageBox.Show($"Parola sau user invalid!");
            }
        }

        private void WordManagementWindow_Closed(object sender, EventArgs e)
        {
            ReloadWords(); 
        }

        public void ReloadWords()
        {
            listBoxWords.ItemsSource = null; 
            search.LoadWords("words.txt");
            LoadCategories();
        }


        private void LoadCategories()
        {
            try
            {
                string[] lines = File.ReadAllLines("words.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 4)
                    {
                        string category = parts[0].Trim();
                        if (!categories.Contains(category))
                        {
                            categories.Add(category);
                        }
                    }
                }
                cmbCategories.ItemsSource = categories;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Words file not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = cmbCategories.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                SearchByCategory(selectedCategory);
            }
        }
        private void txtPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            string prefix = txtPrefix.Text.Trim();
            SearchByPrefix(prefix);
        }

        private void SearchByCategory(string category)
        {
            listBoxWords.ItemsSource = search.SearchWordsByCategory(category);
        }

        private void SearchByPrefix(string prefix)
        {
            listBoxWords.ItemsSource = search.SearchWordsByPrefix(prefix);
        }

        private void listBoxWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWord = listBoxWords.SelectedItem as string;
            if (selectedWord != null)
            {
                Word word = search.GetWordByName(selectedWord);
                if (word != null)
                {
                    txtWord.Text = word.Name;
                    txtDescription.Text = word.Description;
                    txtCategory.Text = word.Category;
                    imgWord.Source = word.GetImage();
                }
            }
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
        }

        private void txtWord_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    public class Administrator
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
