   M S B A M L   `   `   `  ����   MasterSchedule6    0MasterSchedule.Views.UpdateAssemblyReleaseWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$+  %Master Schedule - Release to Assembly��$	��480��$	��900�� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0,10,0,0q�$��RELEASE(ASSEMBLY)�� �� TextAlignment$ Center=�$
��Bold"�$��15(���  �� Orientation$ 
Horizontal=�$��Right=�$��Bottom=�.+��� $��
Report ID:���� -    ��Name$ lblReportId��$��5,0,5,0q�$�� ��$
��Bold"��� -    ��Name$ svMain��$��1��.���� -    ��Name$ spMain��$ Vertical=�� $��2��$��0,5,0,0q� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+��� -   	 ��Name$	 
btnAddMore��$��0��$	��150��$��Other Product No.��{�.  �� -   $	 	btnExport��$��4��$��10,0,0,0q�$	��150��$��Report��{�.  �� -   $	 
btnRelease��$��3��$	��150��$��Release��$
��Bold"�{�.  � -   
 �Name$
 popupAddMore�� � AllowsTransparency . $��Slide=� � 	StaysOpen .  .���� 
�������$��1,1,1,1q�
�������._�� $��5,5,5,5q� �� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto �.+��� $��Product No.:��$��Bottom=��� -	    ��Name$ txtProductNo��$��1��$��5,0,0,0q�$�� ��$	��125���� -
   $	 btnAddMoreOk��$��1��$��1��$��5,5,0,0q�$��OK��$
��Bold"�$��50��$��Right=� �� 	IsDefault . 