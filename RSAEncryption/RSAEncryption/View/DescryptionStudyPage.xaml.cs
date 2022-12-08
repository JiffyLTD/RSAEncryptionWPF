using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace RSAEncryption.View
{
    /// <summary>
    /// Логика взаимодействия для DescryptionPage.xaml
    /// </summary>
    public partial class DescryptionStudyPage : Page
    {
        private char[] characters = new char[] { ' ', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я'};
        public DescryptionStudyPage()
        {
            InitializeComponent();

            descryptedGrid.Visibility = Visibility.Hidden;
            descryptedGrid.Opacity = 0;

            encryptedGrid.Visibility = Visibility.Hidden;
            encryptedGrid.Opacity = 0;

            loadPrivateKeyBut.Visibility = Visibility.Hidden;
            loadPrivateKeyBut.Opacity = 0;

            howToDescryptExampleLabel.Opacity = 0;
            howToDescryptLabel.Opacity = 0;

            resetBut.Visibility = Visibility.Hidden;
            resetBut.Opacity = 0;
        }

        private void loadTextBut_Click(object sender, RoutedEventArgs e)
        {
            string encryptedTextFromFile = string.Empty;

            OpenFileDialog openFile = new ();
            openFile.Filter = "Text file (.txt)|*.txt";
            if (openFile.ShowDialog() == true)
            {
                StreamReader sr = new (openFile.OpenFile());
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    encryptedTextFromFile += sr.ReadLine() + " ";
                }
                sr.Close();

                if (!String.IsNullOrEmpty(encryptedTextFromFile))
                {
                    validationLabel.Content = "Расшифрование сообщения";
                    validationLabel.Foreground = Brushes.White;

                    encryptedText.Text = encryptedTextFromFile;

                    LoadButAnimation();
                }
                else
                {
                    validationLabel.Content = "Вы загрузили пустой файл!";
                    validationLabel.Foreground = Brushes.Red;
                }
            }
        }

        private void LoadButAnimation()
        {
            DoubleAnimation anim = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };
            DoubleAnimation but = new()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            loadTextBut.BeginAnimation(Button.OpacityProperty, but);
            loadTextBut.Visibility = Visibility.Hidden;

            loadPrivateKeyBut.BeginAnimation(Button.OpacityProperty, anim);
            loadPrivateKeyBut.Visibility = Visibility.Visible;

            encryptedGrid.BeginAnimation(Grid.OpacityProperty, anim);
            encryptedGrid.Visibility = Visibility.Visible;
        }

        private void loadPrivateKeyBut_Click(object sender, RoutedEventArgs e)
        {
            string privateKeyFromFile = string.Empty;

            OpenFileDialog openFile = new();
            openFile.Filter = "Text file (.txt)|*.txt";
            if (openFile.ShowDialog() == true)
            {
                StreamReader sr = new(openFile.OpenFile());
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    privateKeyFromFile += sr.ReadLine() + " ";
                }
                sr.Close();

                if (!String.IsNullOrEmpty(privateKeyFromFile))
                {
                    string[] key = privateKeyFromFile.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (Regex.Match(privateKeyFromFile, @"[]a-zA-ZА-Яа-я+-=()?&.,_*/\^%$#@!;:><[{}]").Success && key.Length == 2)
                    {
                        try
                        {
                            validationLabel.Content = "Расшифрование сообщения";
                            validationLabel.Foreground = Brushes.White;

                            howToDescryptExampleLabel.Content = "-> [d,mod] = [" + key[0] + "," + key[1] + "]";
                            howToDescryptExampleLabel.FontSize = 26;
                            howToDescryptLabel.Content = "x -> (x^" + key[0] + ") % " + key[1];
                            howToDescryptLabel.FontSize = 26;

                            var encText = encryptedText.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                            descryptedText.Text = DesryptText(encText, Convert.ToInt64(key[0]), Convert.ToInt64(key[1]));

                            LoadPrivateKeyButAnimation();
                        }
                        catch
                        {
                            validationLabel.Content = "Проверьте данные зашифрованного текста!";
                            validationLabel.FontSize = 28;
                            validationLabel.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        validationLabel.Content = "Проверьте privateKey в файле!";
                        validationLabel.FontSize = 28;
                        validationLabel.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    validationLabel.Content = "Вы загрузили пустой файл!";
                    validationLabel.Foreground = Brushes.Red;
                }
            }
        }

        private string DesryptText(List<string> input, long d, long mod)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)mod);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }

        private void LoadPrivateKeyButAnimation()
        {
            DoubleAnimation anim = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };
            DoubleAnimation but = new()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            loadPrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);
            loadPrivateKeyBut.Visibility = Visibility.Hidden;

            howToDescryptExampleLabel.BeginAnimation(Label.OpacityProperty, anim);
            howToDescryptLabel.BeginAnimation(Label.OpacityProperty, anim);

            descryptedGrid.BeginAnimation(Grid.OpacityProperty, anim);
            descryptedGrid.Visibility = Visibility.Visible;

            resetBut.BeginAnimation(Button.OpacityProperty, anim);
            resetBut.Visibility = Visibility.Visible;
        }

        private void resetBut_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };
            DoubleAnimation but = new()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            howToDescryptExampleLabel.BeginAnimation(Label.OpacityProperty, but);
            howToDescryptLabel.BeginAnimation(Label.OpacityProperty, but);

            resetBut.BeginAnimation(Button.OpacityProperty, but);
            resetBut.Visibility = Visibility.Hidden;

            loadTextBut.BeginAnimation(Button.OpacityProperty, anim);
            loadTextBut.Visibility = Visibility.Visible;

            descryptedGrid.BeginAnimation(Grid.OpacityProperty, but);
            descryptedGrid.Visibility = Visibility.Hidden;
            descryptedText.Clear();

            encryptedGrid.BeginAnimation(Grid.OpacityProperty, but);
            encryptedGrid.Visibility = Visibility.Hidden;
            encryptedText.Clear();
        }
    }
}
