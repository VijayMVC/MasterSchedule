   M S B A M L   `   `   `  ����   MasterSchedule/    )MasterSchedule.Views.ProductionMemoWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Add Production Memo��$
��Auto��$	��640�� � WindowState$ 	Maximized=�.���� .��� $��5,5,5,5q�.+���  �� Orientation$ Vertical=�.+��� $��Section:���� -    ��Name$ 
cboSection�� �� SelectedValuePath$ 	SectionId�� �� DisplayMemberPath$
 Name���� $��0,10,0,0q�$��	Apply to:��&   System.Windows.Controls.DataGrid  -     Name$ dgProductionNumber��   CanUserAddRows .    CanUserDeleteRows . 	   AutoGenerateColumns	 .  
   CanUserResizeRows
 .  $	��200��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Production Number��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Value+ �� UpdateSourceTrigger$ PropertyChanged=��� $��0,10,0,0q�$��Picture:���� $
��Auto=�$��Disabled=�.���� $ 
Horizontal=�$	��320��.+��� 
�������$��1,1,1,1q�$	��320��
�������._��� -    ��Name$ 
imgPicture��$��Uniform=� � ContextMenu �� .@�m� -   $��Clear���� $��10,0,0,0q�
�������$��1,1,1,1q�$	��320��
�������._�� .+��� $��	Picture 1�� �� TextAlignment$ Center=�
��������� -   $ imgPicture1��$��Uniform=� �� .@�m� -   $��Clear���� $��10,0,0,0q�
�������$��1,1,1,1q�$	��320��
�������._�� .+��� $��	Picture 2��$ Center=�
��������� -   $ imgPicture2��$��Uniform=� �� .@�m� -	   $��Clear���� $��10,0,0,0q�
�������$��1,1,1,1q�$	��320��
�������._�� .+��� $��	Picture 3��$ Center=�
��������� -
   $ imgPicture3��$��Uniform=� �� .@�m� -   $��Clear���� $��10,0,0,0q�
�������$��1,1,1,1q�$	��320��
�������._�� .+��� $��	Picture 4��$ Center=�
��������� -   $ imgPicture4��$��Uniform=� �� .@�m� -   $��Clear���� -    ��Name$ btnBrowserPicture��$��Browser Picture��$	��150��$��Right=� �� .@�m� -    m�Name$ miBrowserPicture1��$��Browser Picture 1��m� -   $ miBrowserPicture2��$��Browser Picture 2��m� -   $ miBrowserPicture3��$��Browser Picture 3��m� -   $ miBrowserPicture4��$��Browser Picture 4���� -   $ btnSave��$��0,5,0,0q�$��	Save Memo��$
��Bold"�