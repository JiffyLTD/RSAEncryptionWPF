using Microsoft.Win32;
using RSAEncryption.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSAEncryption.View
{
    /// <summary>
    /// Логика взаимодействия для EncryptionPage.xaml
    /// </summary>
    public partial class EncryptionStudyPage : Page
    {
        private char[] characters = new char[] { ' ', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я'};
        public EncryptionStudyPage()
        {
            InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            secondStepBut.Visibility = Visibility.Hidden;
            secondStepBut.Opacity = 0;

            labelKeys.Opacity = 0;

            labelEncryptText.Opacity = 0;

            savePrivateKeyBut.Visibility = Visibility.Hidden;
            savePrivateKeyBut.Opacity = 0;

            thirdStepBut.Visibility = Visibility.Hidden;
            thirdStepBut.Opacity = 0;

            textForEncrypt.Visibility = Visibility.Hidden;
            textForEncrypt.Opacity= 0;

            lastStepPanel.Visibility = Visibility.Hidden;
            lastStepPanel.Opacity = 0;
        }

        private void firstStepBut_Click(object sender, RoutedEventArgs e)
        {
            validationForP.Content = "";
            validationForQ.Content = "";
            long p, q;
            if (IsNumbP() && IsNumbQ())
            {
                p = Convert.ToInt64(pText.Text);
                q = Convert.ToInt64(qText.Text);
                if (IsPrimeNumbP(p) && IsPrimeNumbQ(q))
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

                        firstStepBut.BeginAnimation(Button.OpacityProperty, but);
                        firstStepBut.Visibility = Visibility.Hidden;

                        labelForPQ.BeginAnimation(Label.OpacityProperty, but);

                        secondStepBut.Visibility = Visibility.Visible;
                        secondStepBut.BeginAnimation(Button.OpacityProperty, anim);

                        labelForMod.Content = "-> mod = p * q = " + CalculateMod(p, q);
                        labelForMod.BeginAnimation(Label.OpacityProperty, anim);

                        var eiler = CalculateEiler(p, q);
                        labelForEiler.Content = "-> " + "\u03C6" + " = (p-1) * (q-1) = " + eiler;
                        labelForEiler.BeginAnimation(Label.OpacityProperty, anim);

                        var exp = CalculateExponent(eiler);
                        labelForExponent.Content = "-> e = " + exp;
                        labelForExponent.BeginAnimation(Label.OpacityProperty, anim);

                        labelForD.Content = "-> d = " + CalculateD(eiler,exp);
                        labelForD.BeginAnimation(Label.OpacityProperty, anim);

                        pText.IsReadOnly = true;
                        qText.IsReadOnly = true;
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
        private long CalculateD(long eiler,long exp)
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

            for (long i = 2;i <= eiler;i++)
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
            return (p-1) * (q-1);
        }
        private long CalculateMod(long p,long q)
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
                for(long i = 2; i < n; i++)
                {
                    if(n % i == 0)
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

        private void secondStepBut_Click(object sender, RoutedEventArgs e)
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

            labelKeys.BeginAnimation(Label.OpacityProperty,anim);

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty,anim);

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty,anim);
            savePrivateKeyBut.Visibility = Visibility.Visible;

            var modStr = labelForMod.Content.ToString();
            string[] mod = modStr.Split(' ');
            var eStr = labelForExponent.Content.ToString();
            string[] exp = eStr.Split(' ');
            var dStr = labelForD.Content.ToString();
            string[] d = dStr.Split(' ');

            labelForPublicKey.Content = "publicKey = {" + exp[^1] + "," + mod[^1] + "}";
            labelForPrivateKey.Content = "privateKey = {" + d[^1] + "," + mod[^1] + "}";

            secondStepBut.BeginAnimation(Button.OpacityProperty,but);
            secondStepBut.Visibility = Visibility.Hidden;
        }

        private void thirdStepBut_Click(object sender, RoutedEventArgs e)
        {
            var modStr = labelForMod.Content.ToString();
            string[] mod = modStr.Split(' ');
            var eStr = labelForExponent.Content.ToString();
            string[] exp = eStr.Split(' ');

            var textForRSAEncr = textForEncrypt.Text.ToUpper();

            if (!String.IsNullOrEmpty(textForRSAEncr))
            {
                if (!Regex.Match(textForRSAEncr, @"[]a-zA-Z0-9+-=()?&.,_*/\^%$#@!;:><[{}]").Success)
                {
                    List<string> encryptedText = EncryptText(textForRSAEncr, Convert.ToInt64(exp[^1]), Convert.ToInt64(mod[^1]));

                    SaveFileDialog saveFile = new();
                    saveFile.Filter = "Text file (.txt)|*.txt";
                    saveFile.FileName = "encryptedText";
                    if (saveFile.ShowDialog() == true)
                    {
                        StreamWriter sw = new(saveFile.OpenFile(), Encoding.GetEncoding(1251));
                        sw.Write("Зашифрованный текст: \n");
                        foreach (var item in encryptedText)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Dispose();
                        sw.Close();
                    }
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

                    labelHowEncrypt.Content = "-> (x < " + mod[^1] + ") -> (x^" + exp[^1] + ") % " + mod[^1];
                    encryptedTextBox.Text = "Зашифрованный текст: \n";

                    foreach (var item in encryptedText)
                    {
                        encryptedTextBox.Text += item + "\n";
                    }

                    lastStepPanel.BeginAnimation(StackPanel.OpacityProperty, anim);
                    lastStepPanel.Visibility = Visibility.Visible;

                    thirdStepBut.BeginAnimation(Button.OpacityProperty, but);
                    thirdStepBut.Visibility = Visibility.Hidden;

                    textForEncrypt.IsReadOnly = true;

                    labelEncryptText.Foreground = Brushes.White;
                    labelEncryptText.Content = "Текст для шифрования";
                }
                else
                {
                    labelEncryptText.Foreground = Brushes.Red;
                    labelEncryptText.Content = "Разрешенные символы 'А-Я' 'а-я' ' '";
                }
            }
            else
            {
                labelEncryptText.Foreground = Brushes.Red;
                labelEncryptText.Content = "Заполните поле!!!";
            }
        }

        private void savePrivateKey_Click(object sender, RoutedEventArgs e)
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

            labelEncryptText.BeginAnimation(Label.OpacityProperty, anim);

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);
            savePrivateKeyBut.Visibility = Visibility.Hidden;

            textForEncrypt.BeginAnimation(TextBox.OpacityProperty, anim);
            textForEncrypt.Visibility = Visibility.Visible;

            thirdStepBut.BeginAnimation(Button.OpacityProperty, anim);
            thirdStepBut.Visibility = Visibility.Visible;

            var modStr = labelForMod.Content.ToString();
            string[] mod = modStr.Split(' ');
            var dStr = labelForD.Content.ToString();
            string[] d = dStr.Split(' ');

            SaveFileDialog saveFile = new();
            saveFile.Filter = "Text file (.txt)|*.txt";
            saveFile.FileName = "privateKey";
            if (saveFile.ShowDialog() == true)
            {
                StreamWriter sw = new(saveFile.OpenFile(), Encoding.GetEncoding(1251));
                sw.Write("privateKey : \n");
                sw.Write(d[^1] + "\n" + mod[^1]);
                sw.Dispose();
                sw.Close();
            }
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

            firstStepBut.BeginAnimation(Button.OpacityProperty, anim);
            firstStepBut.Visibility = Visibility.Visible;

            labelForPQ.BeginAnimation(Label.OpacityProperty, anim);

            secondStepBut.Visibility = Visibility.Hidden;
            secondStepBut.BeginAnimation(Button.OpacityProperty, but);

            labelForMod.BeginAnimation(Label.OpacityProperty, but);

            labelForEiler.BeginAnimation(Label.OpacityProperty, but);

            labelForExponent.BeginAnimation(Label.OpacityProperty, but);

            labelForD.BeginAnimation(Label.OpacityProperty, but);

            textForEncrypt.Clear();
            pText.Clear();
            qText.Clear();
            pText.IsReadOnly = false;
            qText.IsReadOnly = false;
            textForEncrypt.IsReadOnly = false;

            labelKeys.BeginAnimation(Label.OpacityProperty, but);

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);

            savePrivateKeyBut.BeginAnimation(Button.OpacityProperty, but);
            savePrivateKeyBut.Visibility = Visibility.Hidden;

            lastStepPanel.BeginAnimation(StackPanel.OpacityProperty, but);
            lastStepPanel.Visibility = Visibility.Hidden;

            textForEncrypt.BeginAnimation(TextBox.OpacityProperty, but);
            textForEncrypt.Visibility = Visibility.Hidden;

            labelEncryptText.BeginAnimation(Label.OpacityProperty, but);
        }
    }
}
