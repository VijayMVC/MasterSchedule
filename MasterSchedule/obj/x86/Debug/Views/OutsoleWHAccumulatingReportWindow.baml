   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	    MasterSchedule<   6MasterSchedule.Views.OutsoleWHAccumulatingReportWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$6  0Master Schedule - Outsole WH Accumulating Report��5   	   $	��500��6B   $	��800��6R    � WindowStartupLocation$ CenterScreen=�6a    � WindowState$ 	Maximized=�6�   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5	      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� $��Select Date: ��6   $��Center=�6-   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpFrom��5      $��1��6)   $��5,0,0,0q�69      SelectedDate$ 
11/16/1992X�6J   �� 5      $��2��6   $��5,0,0,0q�6(   $��->��69    �� TextAlignment$ Center=�6C   $��Center=�6Z     5      -   5        $
 dpTo��5      $��3��6'   $��5,0,0,0q�67   $ 
11/16/1992X�6H   �� 5      -   5         ��Name$ 
btnPreview��5      $	��100��6)   $��Preview��65   $��20,0,0,0q�6`   $
��Bold"�6r   5      7  1System.Windows.Forms.Integration.WindowsFormsHost  5   
   $��1��6   $��0,2,0,0q�6(   	   Child.	 5      /   )Microsoft.Reporting.WinForms.ReportViewer  -   5        reportViewer��
   ShowFindControls
 .  5   5   5      5      5      