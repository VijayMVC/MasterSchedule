   M S B A M L   `   `   `  ����   MasterSchedule-    'MasterSchedule.Views.InsertOffDayWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Master Schedule - Off Day��$	��480��$	��720�� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $��* ��� $��* �.+�� $��0��$��0,0,5,0q� �� $
��Auto ��� $��* � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��Date:��$��Bottom=�(  "System.Windows.Controls.DatePicker  -     Name$ dpDate��$��1��$��5,0,0,0q�   SelectedDate$ 2000/1/1X��� $��1��$��0,5,0,0q�$��Remarks:��$��Bottom=��� -    ��Name$ 
txtRemarks��$��1��$��1��$��5,5,0,0q�$	��100�� �� TextWrapping$
 Wrap=��� -    ��Name$ 	btnInsert��$��2��$��2��$��0,5,0,0q�$��Right=�$��75��$��Insert��$
��Bold"�{�.  	 �� 	IsDefault	 . &   System.Windows.Controls.DataGrid  -   
  Name$
 dgOffDay��$��1��$��5,0,0,0q�   AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     CanUserDeleteRows .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Date+ �� StringFormat$ 
dd/MM/yyyy��  $ Remarks��   MinWidth$	 250Q� .    	FontStyle$ Italic$� �� *	Remarks+