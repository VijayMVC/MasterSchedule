   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.OutsoleOutputBalanceWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$,  &Master-Schedule Outsole Output Balance��5   	    � WindowStartupLocation$ CenterScreen=�68    � WindowState$ 	Maximized=�6]   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   �� 5	      $
��auto �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      $
��auto �6#   .+�5      �� $��0��6   $��Filter Information��6'   $��5q�6C   .��5      �  5      �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   .+�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� -   5         ��Name$ chboETD��5   #   $
��EFD:��64   $��Bottom=�6C   $��70��6^   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ 
dpETDStart��5   %   $	��120��69      SelectedDate$ 
07/12/1992X�6E   {�6_   �� 	 �� ElementName$	 chboETD��
 �� Path$
 	IsChecked��� 5      $��->��6$    �� TextAlignment$ Center=�6.   $��Center=�6E   $��80��6`     5      -   5        $ dpETDEnd��5   %   $	��120��67   $ 
07/12/1992X�6C   {�6]   �� $	 chboETD��$
 	IsChecked�5      �� 5      $��1��6!   $ 
Horizontal=�6.   $��0,5,0,0q�6G   .+�5      �� $��Country:��6$   $��Bottom=�64   $��70��6O   �� 5      -   5         ��Name$ 
txtCountry��5   "   
���   �$	��120��6J   �� 5       $��Article No:��6$   $��Bottom=�67   $��70��6R   $��10,0,0,0q�6]   �� 5!      -   5        $ txtArticleNo��5!   "   $	��120��68   
���   �6G   5"      �� 5#      $��3��6!   $ 
Horizontal=�6.   $��0,5,0,0q�6G   .+�5$      �� $��Style:��6$   $��Bottom=�62   $��70��6M   �� 5%      -   5        $ txtStyle��5%   "   $	��120��64   
���   �6C   �� 5'      $��Outsole Line:��6$   $��Bottom=�69   $��70��6T   $��10,0,0,0q�6`   �� 5(      -   5        $ txtOutsoleLine��5(   "   $	��120��6:   
���   �6I   5)      5*      5+      �� 5,      -	   5         ��Name$ 
btnPreview��5,   %   $��1��6   $��	20,10,0,0q�69   $��   _Preview   ��6L    �� 	IsDefault . {�.  6�   5-      &   System.Windows.Controls.DataGrid  5/   
   -
   5          Name$ dgOutsoleBalance��5/       $��1��6   $��0,5,0,0q�6:      AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .   �� AlternationCount$ 2��50   D      
IsReadOnly .    AlternatingRowBackground
 �����   CanUserReorderColumns .     SelectionUnit$ FullRow=�52   1      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources 54      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property  �� Value��%�   '        �� 55      "  6    56      P� 57       �@ 
IsSelected . . �58      ��  � ��
  ��iA�6;   �� 59       � ��
  �����6;   5:      5<      5=      5>      5?      5@      