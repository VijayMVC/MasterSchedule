   M S B A M L   `   `   `  ����  MasterSchedule&     MasterSchedule.Views.LoginWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Login��5   	   $
��Auto��6   $
��Auto��6%    � SizeToContent$ WidthAndHeight=�62    � WindowStartupLocation$ CenterScreen=�5   
    � 
ResizeMode$ CanMinimize=�6/   .��5      � 
�������.+�5   
   � $��5,5,5,5q�6    � RowDefinitions 5      �� 5	      $
��Auto �6    �� 5
      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��0,5,0,0q�6   $#��--- Master Schedule Login ---��6)   $��15(�6N    �� TextAlignment$ Center=�6\   $
��Bold"�6s   �� 5      $��1��6   $��0,5,0,0q�6&   $��User ID��67   �� 5      $��3��6   $��0,10,0,0q�6%   $��Password��67   $��Bottom=�6G   �� 5      -   5         ��Name$ lblConnectionStatus��5      $��6��65   $��0,5,0,0q�6B   $��Center=�6S   $��Connection Failed��6p   
���  ��6�   �� 5      -   5         ��Name$ txtUserName��5      $��2��6+   $��0,10,0,0q�68   $	��300��6J   $�� ��6Y   G� 5      -   5         G�Name$ txtPassword��5      $��4��6/   $��0,10,0,0q�6<   $	��300��6N   	 G� PasswordChar$	 *��6]   
 G� Password$
  ��6n   �� 5      -   5         ��Name$ btnOk��5      $��5��6$   $��0,5,0,0q�61   $��30��6B   
��� � �$��Log In��6a   $
��Bold"�6r   
������� �� 	IsDefault . {�.  6�   5      5      5      