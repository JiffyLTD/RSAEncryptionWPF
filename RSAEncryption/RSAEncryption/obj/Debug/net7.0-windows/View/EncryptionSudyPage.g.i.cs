﻿#pragma checksum "..\..\..\..\View\EncryptionSudyPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "412C93AE049972E2A3A4EF65F584CF47DF552C59"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using RSAEncryption.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RSAEncryption.View {
    
    
    /// <summary>
    /// EncryptionStudyPage
    /// </summary>
    public partial class EncryptionStudyPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 51 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textForEncryption;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox pText;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label validationForP;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox qText;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label validationForQ;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\View\EncryptionSudyPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Encrypt;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RSAEncryption;V1.0.0.0;component/view/encryptionsudypage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\EncryptionSudyPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.textForEncryption = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.pText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.validationForP = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.qText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.validationForQ = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Encrypt = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\..\View\EncryptionSudyPage.xaml"
            this.Encrypt.Click += new System.Windows.RoutedEventHandler(this.Encrypt_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

