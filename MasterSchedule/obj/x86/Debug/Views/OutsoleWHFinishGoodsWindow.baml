   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.OutsoleWHFinishGoodsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$-  'Master Schedule - Outsole WH FinishGood��5   	   $	��400��69   $
��1000��6I    � WindowStartupLocation$ CenterScreen=�6Y    � 	Resources 5         MyButton �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius	 �� Padding
 � Color �� Triggers P� Property P� Value �� 
TargetName��%r  &	        �� 5   
   " ��6!   .�5      ��  � �� 5      �� 5	      " ��6&   .R�5
      �� $ border��6!   $ 0,0,0,0��61   
���   �$��1q�6\   $	 5,0,5,0q�6p   ��5      
� 5   "   .��5   &   � $
 	LightBlue��63   5   #   ._�5      �� $��Center=�6/   $��Center=�5   )   5      5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ border��6?    � ��
 �����6S   �� 5   "    � ��$
 Bold"�6?   5      P� 5       � �� . . �5   "   �� $ border��6?    � ��
 �Gc��6S   5      5      5      5       5!      .��5"      � $��2q�6    � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $
��Auto �6   �� 5&      $��* �6   �� 5'      $
��Auto �6   .+�5)   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5*      �� $��
ProductNo:��6   $��14(�6*   $��Bottom=�68   $	��100��6S   �� 5+      -   5         ��Name$ txtProductNo��5+      $	��150��6,   $��18(�6;   $
��Bold"�6I   
���   � � ToolTip$ Press Enter��6o   �� 5,      -   5         ��Name$ 	btnSearch��5,      $��0��6(   $��  Search >>��63   $��5,0,0,0q�6I   $��14(�6Z    �� 	IsDefault . 6y   5-      �� 5.   
   $��1��6   $$��Outsole WareHouse Finish Goods��6!    �� TextAlignment$ Center=�6G   $��26(�6^   $��0,0,0,5q�6l   $
��Bold"�6}   � 5/   
   $��2��6    � ColumnDefinitions 50      �� 51      $
��Auto �6#   �� 52      .+�54      �� -   5        $ 	btnAddRow��54   8   #���   6   {�.  $��+��6]   $��25��6i   $��14(�6t   $��25��6�   $
��Bold"�6�   $	��Top=�6�   $��0,0,1,0q�6�   &   System.Windows.Controls.DataGrid  55      -   5          Name$ dgOutsoleWHFG��55   '   $��1��6      AutoGenerateColumns .     CanUserAddRows .     FrozenColumnCount$ 1��6q      SelectionUnit$ CellOrRowHeader=�6�    �� AlternationCount$ 2��6�      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����
���   �    ColumnHeaderStyle  57      �� 58      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�59      ��  � ��$
 Bold"�67   �� 5:       � ��$ Center=�6G   5;      ! � ContextMenu! 5=      �� 5>      .@�5?      m� -   5        " m�Name$" miDelete��5?   #   $��Delete��65   6E   5@      5B      5C      �� 5D   
   $��3��6   $ 
Horizontal=�6"   $��0,2,0,0q�6;   $��Right=�6L   .+�5E      �� -   5        $ 
btnBalance��5E   J   #���   6   {�.  $��25��6^   $��16(�6j   $��Balance��6x   $	��120��6�   $��0,0,10,0q�6�   6�   �� 5F      -   5        $ btnSave��5F   8   #���   6   $��25��6I   $��16(�6U   $
��Save��6c   $	��120��6r   6�   5G      5H      5I      