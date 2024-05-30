//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Windows;
//using Microsoft.Win32;

//namespace DictionaryApp
//{
//    public class Administrative
//    {
//        private List<Word> dictionary;
//        private Dictionary<string, Word> wordDictionary;

//        public Administrative(Dictionary<string, Word> dictionary)
//        {
//            wordDictionary = dictionary;
//        }

//        public Administrative(List<Word> dictionary)
//        {
//            this.dictionary = dictionary;
//        }

//        public void AddWord(string name, string description, string category, string imagePath = null)
//        {
//            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(category))
//            {
//                MessageBox.Show("Please provide all required information.");
//                return;
//            }

//            if (dictionary.Any(word => word.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
//            {
//                MessageBox.Show("Word already exists in the dictionary.");
//                return;
//            }

//            Word newWord = new Word
//            {
//                Name = name,
//                Description = description,
//                Category = category,
//                ImagePath = imagePath ?? "C:\\date\\FACULTATE\\Anul 2 Sem 2\\MVP\\Tema1_Dictionar_PetcuAndora\\poze\\default.jpg"
//        };

//            dictionary.Add(newWord);
//            MessageBox.Show("Word added successfully.");
//        }

//        public void UpdateWord(string name, string newDescription, string newCategory, string newImagePath = null)
//        {
//            Word existingWord = dictionary.FirstOrDefault(word => word.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
//            if (existingWord == null)
//            {
//                MessageBox.Show("Word not found in the dictionary.");
//                return;
//            }

//            existingWord.Description = newDescription;
//            existingWord.Category = newCategory;
//            if (!string.IsNullOrWhiteSpace(newImagePath))
//            {
//                existingWord.ImagePath = newImagePath;
//            }

//            MessageBox.Show("Word updated successfully.");
//        }

//        public void DeleteWord(string name)
//        {
//            Word wordToRemove = dictionary.FirstOrDefault(word => word.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
//            if (wordToRemove == null)
//            {
//                MessageBox.Show("Word not found in the dictionary.");
//                return;
//            }

//            dictionary.Remove(wordToRemove);
//            MessageBox.Show("Word deleted successfully.");
//        }

//        public void SaveDictionaryToFile(string filePath)
//        {
//            try
//            {
//                using (StreamWriter writer = new StreamWriter(filePath))
//                {
//                    foreach (Word word in dictionary)
//                    {
//                        writer.WriteLine($"{word.Name}|{word.Description}|{word.Category}|{word.ImagePath}");
//                    }
//                }
//                MessageBox.Show("Dictionary saved successfully.");
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error saving dictionary: {ex.Message}");
//            }
//        }

//        public string BrowseImage()
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog();
//            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
//            if (openFileDialog.ShowDialog() == true)
//            {
//                return openFileDialog.FileName;
//            }
//            return null;
//        }
//    }
//}
