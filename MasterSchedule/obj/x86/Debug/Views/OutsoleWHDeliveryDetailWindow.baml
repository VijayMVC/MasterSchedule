   M S B A M L   `   `   `  ����  MasterSchedule8    2MasterSchedule.Views.OutsoleWHDeliveryDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$2  ,Master Schedule - Outsole WH Delivery Detail��5   	   $	��480��6>   $	��640��6N    � WindowState$ 	Maximized=�6]    � WindowStartupLocation$ CenterScreen=�6�   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5       $��0��6      
IsReadOnly .    RowHeaderWidth$ 5Q�6G      SelectionUnit$ CellOrRowHeader=�6Z      AutoGenerateColumns .  	   FrozenColumnCount$	 1��5   /   
 �� AlternationCount$
 2��6E      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .     ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$ Center=�6C   5      5      � 5   
   -   5         �Name$ 	gridTotal��5      $��1��6"    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInventory��$ RowHeaderWidth�5      � 5   
   $��2��6    5      �� 5      $
��Auto �6#   �� 5      .+�5       ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�62   $��5,0,0,0q�6M   .+�5!      �� $!��Product Background Legend: ��6   $��0,0,5,0q�6?   �� 5"      $��40��6   
��� � �6'   �� 5#      $��
: Finished��6   $��Italic$�6.   $��2,0,20,0q�6A   �� 5$      $��40��6   
��� ���6'   �� 5%      $��: In Process��6   $��Italic$�60   $��2,0,20,0q�6C   �� 5&      $��No color��6   �� 5'      $��: Not yet input��6   $��Italic$�63   $��2,0,20,0q�6F   5(      �� 5)      -   5         ��Name$ btnExcelFile��5)   %   $��1��6   $��70��6;   $��_Excel��6F   {�.  $��Right=�6i   6�   5*      5+      5,      