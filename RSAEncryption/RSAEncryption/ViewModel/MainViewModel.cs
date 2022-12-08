using CommunityToolkit.Mvvm.Input;
using RSAEncryption.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RSAEncryption.ViewModel
{
    internal class MainViewModel:ViewModelBase
    {
        private Page mainProg = new MainProgPage();
        private Page encryptionStudy = new EncryptionStudyPage();
        private Page descriptionStudy = new DescryptionStudyPage();
        private Page info = new InfoPage();
        private Page _currPage = new InfoPage();

        public Page CurPage
        {
            get => _currPage;
            set => Set(ref _currPage,value);
        }

        public ICommand OpenMainProgPage
        {
            get
            {
                return new RelayCommand(() => CurPage = mainProg);
            }
        }
        public ICommand OpenEncryptionStudyPage
        {
            get
            {
                return new RelayCommand(() => CurPage = encryptionStudy);
            }
        }
        public ICommand OpenDescryptionStudyPage
        {
            get
            {
                return new RelayCommand(() => CurPage = descriptionStudy);
            }
        }
        public ICommand OpenInfoPage
        {
            get
            {
                return new RelayCommand(() => CurPage = info);
            }
        }
    }
}
