   M S B A M L   `   `   `  ����  MasterSchedule-    'MasterSchedule.Views.InsertOffDayWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Master Schedule - Off Day��5   	   $	��480��6+   $	��720��68    � WindowStartupLocation$ CenterScreen=�5   !   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5	      $��* �6   .+�5   
   � $��0��6   $��0,0,5,0q�6    5      �� 5      $
��Auto �6#   �� 5      $��* �6#    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��Date:��6   $��Bottom=�6%   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpDate��5      $��1��6)   $��5,0,0,0q�69      SelectedDate$ 2000/1/1X�6J   �� 5      $��1��6   $��0,5,0,0q�6%   $��Remarks:��66   $��Bottom=�6F   �� 5      -   5         ��Name$ 
txtRemarks��5      $��1��6*   $��1��6:   $��5,5,0,0q�6G   $	��100��6X    �� TextWrapping$
 Wrap=�6h   �� 5      -   5         ��Name$ 	btnInsert��5      $��2��6(   $��2��6<   $��0,5,0,0q�6I   $��Right=�6Z   $��75��6v   $��Insert��6�   $
��Bold"�6�   {�.  	 �� 	IsDefault	 . 6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5        
  Name$
 dgOffDay��5      $��1��6%   $��5,0,0,0q�66      AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     CanUserDeleteRows .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��5   8      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   Columns 5       0  *System.Windows.Controls.DataGridTextColumn  5!      ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *Date+ �� StringFormat$ 
dd/MM/yyyy��  5"      $ Remarks��6%      MinWidth$	 250Q�66    .    	FontStyle$ Italic$�6s    6W   �� *	Remarks+6s   5$      5%      5&      