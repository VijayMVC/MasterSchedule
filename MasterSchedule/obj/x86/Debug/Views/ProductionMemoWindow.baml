   M S B A M L   `   `   `  ����  MasterSchedule/    )MasterSchedule.Views.ProductionMemoWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$*  $MasterSchedule - Add Production Memo��5   	   $
��Auto��66   $	��640��6D    � WindowState$ 	Maximized=�6g   .��5      �� .��5   
   � $��5,5,5,5q�6   .+�5      ��  �� Orientation$ Vertical=�6   .+�5      �� $��Section:��6   �� 5	      -   5         ��Name$ 
cboSection��5	       �� SelectedValuePath$ 	SectionId��6/    �� DisplayMemberPath$
 Name��6M   �� 5
      $��0,10,0,0q�6   $��	Apply to:��6.   &   System.Windows.Controls.DataGrid  5      -   5          Name$ dgProductionNumber��5         CanUserAddRows .    CanUserDeleteRows . 	   AutoGenerateColumns	 .  
   CanUserResizeRows
 .  $	��200��5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Production Number��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� *Value+ �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5      $��0,10,0,0q�6   $��Picture:��6.   �� 5      $
��Auto=�6   $��Disabled=�6D   .��5      �� $ 
Horizontal=�6!   $	��320��6:   .+�5      �� 
�������$��1,1,1,1q�64   $	��320��6N   
�������._�5      �� -   5         ��Name$ 
imgPicture��5   $   $��Uniform=�68    � ContextMenu 5   "   �� 5   &   .@�5   *   m� -   5        $��Clear��5   3   6B   5   '   5       5!      �� 5"      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5$      � .+�5%   "   �� $��	Picture 1��6,    �� TextAlignment$ Center=�6=   
�������6T   �� 5&   "   -   5        $ imgPicture1��5&   (   $��Uniform=�6=    5'   &   �� 5(   *   .@�5)   .   m� -   5        $��Clear��5)   7   6F   5*   +   5,   #   5-      5.      �� 5/      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�51      � .+�52   "   �� $��	Picture 2��6,   $ Center=�6=   
�������6T   �� 53   "   -   5        $ imgPicture2��53   (   $��Uniform=�6=    54   &   �� 55   *   .@�56   .   m� -	   5        $��Clear��56   7   6F   57   +   59   #   5:      5;      �� 5<      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5>      � .+�5?   "   �� $��	Picture 3��6,   $ Center=�6=   
�������6T   �� 5@   "   -
   5        $ imgPicture3��5@   (   $��Uniform=�6=    5A   &   �� 5B   *   .@�5C   .   m� -   5        $��Clear��5C   7   6F   5D   +   5F   #   5G      5H      �� 5I      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5K      � .+�5L   "   �� $��	Picture 4��6,   $ Center=�6=   
�������6T   �� 5M   "   -   5        $ imgPicture4��5M   (   $��Uniform=�6=    5N   &   �� 5O   *   .@�5P   .   m� -   5        $��Clear��5P   7   6F   5Q   +   5S   #   5T      5U      5V      5X      �� 5Y      -   5         ��Name$ btnBrowserPicture��5Y      $��Browser Picture��64   $	��150��6N   $��Right=�6Z    5Z      �� 5[      .@�5\      m� -   5         m�Name$ miBrowserPicture1��5\   '   $��Browser Picture 1��6B   6]   m� 5]      -   5        $ miBrowserPicture2��5]   '   $��Browser Picture 2��6B   6]   m� 5^      -   5        $ miBrowserPicture3��5^   '   $��Browser Picture 3��6B   6]   m� 5_      -   5        $ miBrowserPicture4��5_   '   $��Browser Picture 4��6B   6]   5`      5b      �� 5c      -   5        $ btnSave��5c      $��0,5,0,0q�6*   $��	Save Memo��6;   $
��Bold"�6O   6a   5d      5e      5f      5g      