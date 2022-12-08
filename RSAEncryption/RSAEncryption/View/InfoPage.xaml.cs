using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace RSAEncryption.View
{
    /// <summary>
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();

            string RelativeFilePath = System.IO.Path.GetFullPath("Docs/info.xps");

            XpsDocument doc = new XpsDocument(RelativeFilePath, FileAccess.Read);
            docViewer.Document = doc.GetFixedDocumentSequence();
            doc.Close();
        }
    }
}
