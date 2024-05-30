using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DictionaryApp
{
    public class Search
    {
        private ObservableCollection<Word> words = new ObservableCollection<Word>();

        public ObservableCollection<Word> Words { get { return words; } }

        public void LoadWords(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 4)
                    {
                        string category = parts[0].Trim();
                        string word = parts[1].Trim();
                        string description = parts[2].Trim();
                        string imagePath = parts[3].Trim().Trim('"');
                        words.Add(new Word { Category = category, Name = word, Description = description, ImagePath = imagePath });
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Words file not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading words: {ex.Message}");
            }
        }
        public Dictionary<string, Word> GetDictionary()
        {
            return words.ToDictionary(w => w.Name, w => w);
        }

        public List<string> SearchWordsByCategory(string category)
        {
            return words.Where(w => w.Category == category).Select(w => w.Name).ToList();
        }

        public List<string> SearchWordsByPrefix(string prefix)
        {
            return words.Where(w => w.Name.StartsWith(prefix)).Select(w => w.Name).ToList();
        }

        public Word GetWordByName(string name)
        {
            return words.FirstOrDefault(w => w.Name == name);
        }
    }

    public class Word
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }

        public BitmapImage GetImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                {
                    return new BitmapImage(new Uri(ImagePath));
                }
                else
                {
                    return new BitmapImage(new Uri("C:\\date\\FACULTATE\\Anul 2 Sem 2\\MVP\\Tema1_Dictionar_PetcuAndora\\poze\\default.jpg"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        }

    }
}
