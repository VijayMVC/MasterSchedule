   M S B A M L   `   `   `  ����   MasterSchedule5    /MasterSchedule.Views.RawMaterialSearchBoxWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Master Schedule - Search��$	��180��$	��420�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � Topmost . .���  � RowDefinitions �� $��* ��� $
��Auto �.+��� .@��� $
��Find��.��� $��5,5,5,5q� � ColumnDefinitions �� $
��Auto ��� $��* � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��
Find What:��$��Bottom=��� -    ��Name$ txtFindWhat��$��1��$��5,0,0,0q�
��������� -    ��Name$ 
cboIsMatch��$��1��$��1��$��5,5,0,0q�$��
Match Case�� d� 	IsChecked$
 TrueY��� -   	 ��Name$	 
spShowHide��
 �� Orientation$
 
Horizontal=�$��1��$��2��$��5,5,0,0q�$y�	Collapsed=�.+�	� -    	�Name$ rbShow��$
��Show��$
 TrueY�	� $
��Hide��$��25,0,0,0q��� $��1��$��0,5,0,0q�$
 
Horizontal=�$��Right=�.+��� -    ��Name$ 
btnFindAll��$��Find All��$��75�� �� 	IsDefault . �� -   $ btnClose��$��10,0,0,0q�$��Close��$��75��