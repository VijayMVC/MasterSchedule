   M S B A M L   `   `   `  ����   MasterSchedule1    +MasterSchedule.Views.CompletionStatusWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$)  #Master Schedule - Completion Status��$	��300��$
��Auto�� � WindowState$ 	Maximized=�.���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��5,5,5,5q� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ chboETD��$
��EFD:��$��Center=�(  "System.Windows.Controls.DatePicker  -     Name$ 
dpETDStart��$��1��$��5,0,0,0q�   SelectedDate$ 1/1/2000X�{���  �� ElementName$ chboETD�� �� Path$ 	IsChecked��� $��2��$��5,0,0,0q�$��->��	 �� TextAlignment$	 Center=�$��Center=�  -   $ dpETDEnd��$��3��$��5,0,0,0q�$ 1/1/2000X�{��� $ chboETD��$ 	IsChecked��� $��1��$��0,5,0,0q�$��Article No.:��$��Center=��� -   
 ��Name$
 txtArticleNo��$��1��$��1��$	��175��$��5,5,0,0q��� $��2��$��1��$��5,5,0,0q�$��Style:��$��Center=��� -   $
 txtShoeName��$��3��$��1��$	��175��$��5,5,0,0q��� $��3��$��Is Finished?��$��0,5,0,0q�$��Center=��� -   $ chboFinished��$��1��$��3��$��5,5,0,0q�$��Finished���� -   $ chboUnfinished��$��3��$��3��$��5,5,0,0q�$��
Unfinished���� -	    ��Name$ btnView��$��4��$��0��$��3��$��50��$��10,0,0,0q�$
��View��{�.   �� 	IsDefault . &   System.Windows.Controls.DataGrid  -
     Name$ dgMain��$��1��   AutoGenerateColumns .     CanUserAddRows .   �� AlternationCount$ 2��   AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Product No.��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	ProductNo+  $ Country�� �� *	Country+  $ Article No.�� �� *	ArticleNo+  $ Style�� �� *
ShoeName+  $ Quantity�� �� *
Quantity+  $	 EFD�� �� *ETD+ �� StringFormat$ dd-MMM��  $ Loading Date�� �� *LoadingDate+  $ Cutprep�� �� *CutAFinishDate+$	 M/d��  $ Sewing Line�� �� *
SewingLine+  $ Sewing�� �� *SewingFinishDate+$	 M/d��  $ Outsole Line�� �� *OutsoleLine+  $ Outsole�� �� *OutsoleFinishDate+$	 M/d��  $ Assembly Line�� �� *AssemblyLine+  $ Assembly�� �� *AssemblyFinishDate+$	 M/d��