   M S B A M L   `   `   `  ����  MasterSchedule1    +MasterSchedule.Views.UpperWHInventoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$*  $Master Schedule - Upper WH Inventory��5   	   $	��360��66   $	��640��6C    � WindowStartupLocation$ CenterScreen=�6f   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5
         RowHeaderWidth$ 5Q�6(      
IsReadOnly .    AutoGenerateColumns .   �� AlternationCount$ 2��5         AlternatingRowBackground
 �����	   HorizontalGridLinesBrush
	 �����
   VerticalGridLinesBrush

 �����   CanUserReorderColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$ Assembly Line��5   6      
FontWeight$
 Bold"�6n      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6M   �� *AssemblyLine+6�     5      -   5        Column2��$ Upper��5   6    .  6E   �� *SewingOutput+6f     5      -   5        Column3��$ Outsole��5   6    .  6G   �� *OutsoleOutput+6i     5      -   5        Column4��$ Matching��5   6    .  6H   �� *
Matching+6e   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInventory��$ RowHeaderWidth��� 5      ��6#   �� $ Column1��$ ActualWidth��� 5      ��6#   �� $ Column2��$ ActualWidth��� 5      ��6#   �� $ Column3��$ ActualWidth��� 5      ��6#   �� $ Column4��$ ActualWidth��� 5      ��6#   �� $ Column5��$ ActualWidth�.+�5       �� $��2��6   $��1,0,1,1q�6%   
���   �._�5!      �� $��1,0,0,0q�6   $��TOTAL��6-   $
��Bold"�6:   5"      �� 5#      $��3��6   $��0,0,1,1q�6%   
���   �._�5$      �� -   5         ��Name$ lblSewingOutput��5$      $��1,0,0,0q�65   $��0��6F   $
��Bold"�6O   5%      �� 5&      $��4��6   $��0,0,1,1q�6%   
���   �._�5'      �� -   5        $ lblOutsoleOutput��5'      $��1,0,0,0q�66   $��0��6G   $
��Bold"�6P   5(      �� 5)      $��5��6   $��0,0,1,1q�6%   
���   �._�5*      �� -	   5        $ lblMatching��5*      $��1,0,0,0q�61   $��0��6B   $
��Bold"�6K   5+      �� 5,      $��6��6   $��0,0,1,1q�6%   
���   �._�5-      �� -
   5         ��Name$ btnPrint��5-      $	��150��6+   $
��Left=�6:   $��Print Report��6U   {�. 6}   5.      5/      50      51      