   M S B A M L   `   `   `  ����  MasterSchedule?    9MasterSchedule.Views.AddOutsoleMaterialRackPositionWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Add Rack��5   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�69    � WindowStyle$ 
ToolWindow=�6^    � 
ResizeMode$ NoResize=�6w   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5	      $
��auto �6   �� 5
      $
��auto �6   .+�5   
   �� -   5         ��Name$ txtSupplier��5      $��13(�6)    �� TextAlignment$ Center=�67   $
��Bold"�6N   $��Italic$�6`   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5      �� $��Rack No:��6   $��Bottom=�6(   $��60��6C   �� 5      -   5        	 ��Name$	 txtRackNumber��5      $	��120��6-   5      �� 5   
   $��2��6   $ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5      �� $��
Carton No:��6   $��Bottom=�6*   $��60��6E   �� 5      -   5        $	 txtCartonNumber��5      $	��120��6/   5      � 5   
   $��3��6   $��0,5,0,0q�6   
 � ColumnDefinitions
 5      �� 5      $��* �6#   �� 5      $
��auto �6#   .+�5      �� -   5         ��Name$ btnAdd��5      $
��_Add��6%   $
��Bold"�64   6F   �� 5      -   5        $ 	btnRemove��5      $��2,0,0,0q�6(   $��1��69   $
��Bold"�6I   
���  ��$��_Remove��6l   6~   5      5      5      