   M S B A M L   `   `   `  ����  MasterSchedule3    -MasterSchedule.Views.OutsoleRawMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$5  /Master Schedule - Outsole Raw Material Schedule��5   	   $	��300��6A   $	��480��6N    � WindowStartupLocation$ CenterScreen=�5   !   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgOutsoleMaterial��5       $��0��6      RowHeaderWidth$ 10Q�6;      AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     FrozenColumnCount$ 1��5   -   	   SelectionUnit$	 CellOrRowHeader=�6C   
 �� AlternationCount$
 2��5         AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$
 Bold"�63   �� 5       � ��$ Center=�6C   5         Columns 5      4  .System.Windows.Controls.DataGridComboBoxColumn  5      -   5        colSuppliers��,  &System.Windows.Controls.DataGridColumn   Header$ 	Suppliers��5   @      DisplayMemberPath$
 Name��6S      SelectedValuePath$ OutsoleSupplierId��6l      SelectedItemBinding 6�   �� *
Supplier+0  *System.Windows.Controls.DataGridTextColumn  5      $	 EFD��6&   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *ETD+5      �� 5   
   -   5         ��Name$ 
btnAddMore��5      $��1��6%   $��0,5,0,0q�62   $	��150��6C   $
��Left=�6R   $��Other Supplier��6m   {�.  6�   �� 5   
   -   5        $ btnSave��5      $��1��6"   $��0,5,0,0q�6/   $	��150��6@   $��Right=�6O   $
��Save��6k   $
��Bold"�6z   {�.  6�   5      5       