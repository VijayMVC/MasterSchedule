   M S B A M L   `   `   `  ����   MasterSchedule7    1MasterSchedule.Views.UpperWHInventoryDetailWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$1  +Master Schedule - Upper WH Inventory Detail��$	��480��$	��640�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.���  � RowDefinitions �� $��* ��� $
��Auto �.+�&   System.Windows.Controls.DataGrid  -     Name$ dgInventory��   
IsReadOnly .    RowHeaderWidth$ 5Q�   AutoGenerateColumns .     FrozenColumnCount$ 1��	 �� AlternationCount$	 2��
   AlternatingRowBackground

 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  -   Column1��,  &System.Windows.Controls.DataGridColumn   Header$ PO No.��   
FontWeight$
 Bold"�   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	ProductNo+  -   Column2��$ Style�� .  �� *
ShoeName+  -   Column3��$ Article No.�� .  �� *	ArticleNo+  -   Column4��$	 EFD��$
 Bold"� .  �� *ETD+ �� StringFormat$ dd-MMM��  -   Column5��$ Upper�� .  �� *SewingOutput+  -   Column6��$ Outsole�� .  �� *OutsoleOutput+  -	   Column7��$ Matching�� .  �� *
Matching+� $��1�� � ColumnDefinitions �� ����  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� ���� $ dgInventory��$ RowHeaderWidth��� ���� $ Column1��$ ActualWidth��� ���� $ Column2��$ ActualWidth��� ���� $ Column3��$ ActualWidth��� ���� $ Column4��$ ActualWidth��� ���� $ Column5��$ ActualWidth��� ���� $ Column6��$ ActualWidth��� ���� $ Column7��$ ActualWidth�.+��� $��2��$��4��$��1,0,1,1q�
���   �._��� $��1,0,0,0q�$��TOTAL��$
��Bold"��� $��6��$��0,0,1,1q�
���   �._��� -
    ��Name$ lblSewingOutput��$��1,0,0,0q�$��0��$
��Bold"��� $��7��$��0,0,1,1q�
���   �._��� -   $ lblOutsoleOutput��$��1,0,0,0q�$��0��$
��Bold"��� $��8��$��0,0,1,1q�
���   �._��� -   $ lblMatching��$��1,0,0,0q�$��0��$
��Bold"�