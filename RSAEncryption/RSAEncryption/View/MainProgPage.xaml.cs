using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace RSAEncryption.View
{
    public partial class MainProgPage : Page
    {
        private char[] characters = new char[] { ' ', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я'};
        public MainProgPage()
        {
            InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            loadPrivateKeyBut.Opacity = 0;
            loadPrivateKeyBut.Visibility = Visibility.Hidden;

            resetButDescryption.Opacity = 0;
            resetButDescryption.Visibility = Visibility.Hidden;

            savePrivateKeyBut.Opacity = 0;
            savePrivateKeyBut.Visibility = Visibility.Hidden;

            resetButEncryption.Opacity = 0;
            resetButEncryption.Visibility = Visibility.Hidden;

            encryptPanel.Opacity = 0;
            encryptPanel.Visibility = Visibility.Collapsed;
        }

        private void loadTextBut_Click(object sender, RoutedEventArgs e)
        {
            string encryptedTextFromFile = string.Empty;

            OpenFileDialog openFile = new();
            openFile.Filter = "Text file (.txt)|*.txt";
            if (openFile.ShowDialog() == true)
            {
                StreamReader sr = new(openFile.OpenFile());
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    encryptedTextFromFile += sr.ReadLine() + " ";
                }
                sr.Close();
                var encText = encryptedTextFromFile.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (encText.Count() > 0)
                {
                    validationForEncryptText.Content = "Зашифрованный текст";
                    validationForEncryptText.Foreground = Brushes.White;
                    validationForEncryptText.FontSize = 22;

                    encryptedTextForDescrypt.Text = encryptedTextFromFile;

                    LoadTextButAnimation();
                }
                else
                {
                    validationForEncryptText.Content = "Вы загрузили пустой файл!";
                    validationForEncryptText.Foreground = Brushes.Red;
                    validationForEncryptText.FontSize = 14;
                }
            }
        }
        private void enterTextBut_Click(object sender, RoutedEventArgs e)
        {
            validationForEncryptText.Content = "Зашифрованный текст";
            validationForEncryptText.Foreground = Brushes.White;
            validationForEncryptText.FontSize = 22;

            encryptedTextForDescrypt.IsReadOnly = false;
            LoadTextButAnimation();
        }
        private void LoadTextButAnimation()
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

            loadTextBut.BeginAnimation(Button.OpacityProperty,but);
            loadTextBut.Visibility = Visibility.Hidden;

            enterTextBut.BeginAnimation(Button.OpacityProperty, but);
            enterTextBut.Visibility = Visibility.Hidden;

            loadPrivateKeyBut.BeginAnimation(Button.OpacityProperty, anim);
            loadPrivateKeyBut.Visibility = Visibility.Visible;
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

                if (!string.IsNullOrEmpty(privateKeyFromFile))
                {
                    string[] key = privateKeyFromFile.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (Regex.Match(privateKeyFromFile, @"[]a-zA-ZА-Яа-я+-=()?&.,_*/\^%$#@!;:><[{}]").Success && key.Length == 2) 
                    {
                        var encText = encryptedTextForDescrypt.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (encText.Count() > 0)
                        {
                            try
                            {
                                descryptedText.Text = DesryptText(encText, Convert.ToInt64(key[0]), Convert.ToInt64(key[1]));

                                LoadPrivateKeyButAnimation();

                                encryptedTextForDescrypt.IsReadOnly = true;

                                validationForEncryptText.Content = "Зашифрованный текст";
                                validationForEncryptText.Foreground = Brushes.White;
                                validationForEncryptText.FontSize = 22;
                            }
                            catch
                            {
                                validationForEncryptText.Content = "Проверьте данные зашифрованного текста!";
                                validationForEncryptText.Foreground = Brushes.Red;
                                validationForEncryptText.FontSize = 12;
                                resetButDescryption.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                encryptedTextForDescrypt.Clear();
                                loadPrivateKeyBut.Visibility = Visibility.Hidden;
                            }
                        }
                        else
                        {
                            validationForEncryptText.Content = "Введите текст!";
                            validationForEncryptText.Foreground = Brushes.Red;
                            validationForEncryptText.FontSize = 14;
                        }
                    }
                    else
                    {
                        validationForEncryptText.Content = "Проверьте privateKey в файле!";
                        validationForEncryptText.Foreground = Brushes.Red;
                        validationForEncryptText.FontSize = 14;
                    }
                }
                else
                {
                    validationForEncryptText.Content = "Вы загрузили пустой файл!";
                    validationForEncryptText.Foreground = Brushes.Red;
                    validationForEncryptText.FontSize = 14;
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

            resetButDescryption.BeginAnimation(Button.OpacityProperty, anim);
            resetButDescryption.Visibility = Visibility.Visible;
        }

        private void resetButDescryption_Click(object sender, RoutedEventArgs e)
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

            enterTextBut.BeginAnimation(Button.OpacityProperty, anim);
            enterTextBut.Visibility = Visibility.Visible;

            loadTextBut.BeginAnimation(Button.OpacityProperty, anim);
            loadTextBut.Visibility = Visibility.Visible;

            resetButDescryption.BeginAnimation(Button.OpacityProperty, but);
            resetButDescryption.Visibility = Visibility.Hidden;

            encryptedTextForDescrypt.Clear();
            descryptedText.Clear();
        }

        private void encryptBut_Click(object sender, RoutedEventArgs e)
        {
            validationForP.Content = "";
            validationForQ.Content = "";
            labelEncryptText.Foreground = Brushes.White;
            labelEncryptText.Content = "Текст для шифрования";
            labelEncryptText.FontSize = 22;
            long p, q;
            if (IsNumbP() && IsNumbQ())
            {
                p = Convert.ToInt64(pText.Text);
                q = Convert.ToInt64(qText.Text);

                var textForRSAEncr = textForEncrypt.Text.ToUpper();

                if (IsPrimeNumbP(p) && IsPrimeNumbQ(q))
                {
                    if (!String.IsNullOrEmpty(textForRSAEncr)) {
                        if (!Regex.Match(textForRSAEncr, @"[]a-zA-Z0-9+-=()?&.,_*/\^%$#@!;:><[{}]").Success) {
                            var mod = CalculateMod(p, q);
                            var eiler = CalculateEiler(p, q);
                            var exp = CalculateExponent(eiler);
                            var d = CalculateD(eiler, exp);

                            List<string> encryptedTextList = EncryptText(textForRSAEncr, exp, mod);

                            SaveFileDialog saveFile = new();
                            saveFile.Filter = "Text file (.txt)|*.txt";
                            saveFile.FileName = "encryptedText";
                            if (saveFile.ShowDialog() == true)
                            {
                                StreamWriter sw = new(saveFile.OpenFile(), Encoding.GetEncoding(1251));
                                sw.Write("Зашифрованный текст: \n");
                                foreach (var item in encryptedTextList)
                                {
                                    sw.WriteLine(item);
                                }
                                sw.Dispose();
                                sw.Close();

                                foreach (var item in encryptedTextList)
                                {
                                    encryptedText.Text += item + " ";
                                }

                                pText.IsReadOnly = true;
                                qText.IsReadOnly = true;
                                textForEncrypt.IsReadOnly = true;

                                EncryptButAnimation();
                            }
                        }
                        else
                        {
                            labelEncryptText.Foreground = Brushes.Red;
                            labelEncryptText.Content = "Разрешенные символы 'А-Я' 'а-я' ' '";
                            labelEncryptText.FontSize = 14;
                        }
                    }
                    else
                    {
                        labelEncryptText.Foreground = Brushes.Red;
                        labelEncryptText.Content = "Заполните поле!!!";
                    }
                }             
            }
        }
        private List<string> EncryptText(string textForEncrypt, long exp, long mod)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < textForEncrypt.Length; i++)
            {
                int index = Array.IndexOf(characters, textForEncrypt[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)exp);

                BigInteger n_ = new BigInteger((int)mod);

                bi = bi % n_;

                result.Add(bi.ToString());
                Console.WriteLine(bi.ToString() + " " + characters);
            }

            return result;
        }
        private long CalculateD(long eiler, long exp)
        {
            long d = 0;

            while (true)
            {
                if (((d * exp) % eiler == 1) && d > exp)
                    break;
                else
                    d++;
            }
            return d;
        }
        private long CalculateExponent(long eiler)
        {
            long exponent = eiler - 1;

            for (long i = 2; i <= eiler; i++)
            {
                if ((eiler % i == 0) && (exponent % i == 0))
                {
                    exponent--;
                    i = 1;
                }
            }
            return exponent;
        }
        public long CalculateEiler(long p, long q)
        {
            return (p - 1) * (q - 1);
        }
        private long CalculateMod(long p, long q)
        {
            return p * q;
        }
        private bool IsNumbP()
        {
            try
            {
                long p = Convert.ToInt64(pText.Text);
                return true;
            }
            catch
            {
                validationForP.Content = "Введите число";
                return false;
            }
        }
        private bool IsNumbQ()
        {
            try
            {
                long q = Convert.ToInt64(qText.Text);
                return true;
            }
            catch
            {
                validationForQ.Content = "Введите число";
                return false;
            }
        }
        private bool IsPrimeNumbQ(long n)
        {
            if (n > 1)
            {
                for (long i = 2; i < n; i++)
                {
                    if (n % i == 0)
                    {
                        validationForQ.Content = "Это не простое число";
                        return false;
                    }
                }
            }
            else
            {
                validationForQ.Content = "Это не простое число";
                return false;
            }
            return true;
        }
        private bool IsPrimeNumbP(long n)
        {
            if (n > 1)
            {
                for (long i = 2; i < n; i++)
                {
                    if (n % i == 0)
                    {
                        validationForP.Content = "Это не простое число";
                        return false;
                    }
                }
            }
            else
            {
                validationForP.Content = "Это не простое число";
                return false;
            }
            return true;
        }
        private void EncryptButAnimation()
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

            encryptBut.BeginAnimation(Button.OpacityProperty, but);
            encryptBut.Visibility = Visibility.Hidden;

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, anim);
            savePrivateKeyBut.Visibility = Visibility.Visible;
        }
        private void savePrivateKeyBut_Click(object sender, RoutedEventArgs e)
        {
            long p = Convert.ToInt64(pText.Text);
            long q = Convert.ToInt64(qText.Text);

            var mod = CalculateMod(p, q);
            var eiler = CalculateEiler(p, q);
            var exp = CalculateExponent(eiler);
            var d = CalculateD(eiler, exp);

            SaveFileDialog saveFile = new();
            saveFile.Filter = "Text file (.txt)|*.txt";
            saveFile.FileName = "privateKey";
            if (saveFile.ShowDialog() == true)
            {
                StreamWriter sw = new(saveFile.OpenFile(), Encoding.GetEncoding(1251));
                sw.Write("privateKey : \n");
                sw.Write(d + "\n" + mod);
                sw.Dispose();
                sw.Close();
                SavePrivateKeyAnimation();
            }
        }
        private void SavePrivateKeyAnimation()
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

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);
            savePrivateKeyBut.Visibility = Visibility.Hidden;

            resetButEncryption.BeginAnimation(Button.OpacityProperty, anim);
            resetButEncryption.Visibility = Visibility.Visible;
        }
        private void resetButEncryption_Click(object sender, RoutedEventArgs e)
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

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);
            savePrivateKeyBut.Visibility = Visibility.Hidden;

            resetButEncryption.BeginAnimation(Button.OpacityProperty, but);
            resetButEncryption.Visibility = Visibility.Hidden;

            encryptBut.BeginAnimation(Button.OpacityProperty, anim);
            encryptBut.Visibility = Visibility.Visible;

            pText.IsReadOnly = false;
            qText.IsReadOnly = false;
            textForEncrypt.IsReadOnly = false;

            labelEncryptText.Content = "";

            pText.Clear();
            qText.Clear();
            textForEncrypt.Clear();
            encryptedText.Clear();

            encryptPanel.BeginAnimation(StackPanel.OpacityProperty, but);
            encryptPanel.Visibility = Visibility.Collapsed;

            encryptPanelChoose.BeginAnimation(StackPanel.OpacityProperty, anim);
            encryptPanelChoose.Visibility = Visibility.Visible;
        }

        private void chooseEnterBut_Click(object sender, RoutedEventArgs e)
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

            labelEncryptText.Content = "Текст для шифрования";
            labelEncryptText.Foreground = Brushes.White;
            labelEncryptText.FontSize = 22;

            encryptPanel.BeginAnimation(StackPanel.OpacityProperty,anim);
            encryptPanel.Visibility = Visibility.Visible;

            encryptPanelChoose.BeginAnimation(StackPanel.OpacityProperty, but);
            encryptPanelChoose.Visibility = Visibility.Collapsed;
        }

        private void chooseLoadBut_Click(object sender, RoutedEventArgs e)
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
           
            string encryptedTextFromFile = string.Empty;

            OpenFileDialog openFile = new();
            openFile.Filter = "Text file (.txt)|*.txt";
            if (openFile.ShowDialog() == true)
            {
                StreamReader sr = new(openFile.OpenFile(), Encoding.GetEncoding(1251));
                while (!sr.EndOfStream)
                {
                    encryptedTextFromFile += sr.ReadLine() + " ";
                }
                sr.Close();
                    var encText = encryptedTextFromFile.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (encText.Count() > 0)
                    {
                        textForEncrypt.Text = encryptedTextFromFile;

                        labelEncryptText.Content = "Текст для шифрования";
                        labelEncryptText.Foreground = Brushes.White;
                        labelEncryptText.FontSize = 22;

                        encryptPanel.BeginAnimation(StackPanel.OpacityProperty, anim);
                        encryptPanel.Visibility = Visibility.Visible;

                        encryptPanelChoose.BeginAnimation(StackPanel.OpacityProperty, but);
                        encryptPanelChoose.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        labelEncryptText.Content = "Вы загрузили пустой файл!";
                        labelEncryptText.Foreground = Brushes.Red;
                        labelEncryptText.FontSize = 14;
                    }
            }
        }
    }
}
