﻿#pragma checksum "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0B098561894161924D4BB00492EE0D1223925867"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace MasterSchedule.Views {
    
    
    /// <summary>
    /// SelectAssemblyReleaseWindow
    /// </summary>
    public partial class SelectAssemblyReleaseWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.AutoCompleteBox txtReportId;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearchExpand;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridSearch;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.AutoCompleteBox txtProductNo;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvReportId;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/selectassemblyreleasewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            ((MasterSchedule.Views.SelectAssemblyReleaseWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtReportId = ((System.Windows.Controls.AutoCompleteBox)(target));
            return;
            case 3:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSearchExpand = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            this.btnSearchExpand.Click += new System.Windows.RoutedEventHandler(this.btnSearchExpand_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.gridSearch = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.txtProductNo = ((System.Windows.Controls.AutoCompleteBox)(target));
            return;
            case 7:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lvReportId = ((System.Windows.Controls.ListView)(target));
            
            #line 37 "..\..\..\..\Views\SelectAssemblyReleaseWindow.xaml"
            this.lvReportId.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lvReportId_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

