   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.SearchProductionMemoWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$-  'MasterSchedule - Search Production Memo��5   	   $
��Auto��69   $	��640��6G    � WindowState$ 	Maximized=�6j   .��5      �� .��5   
   � $��5,5,5,5q�6   .+�5      ��  �� Orientation$ Vertical=�6   .+�5      �� $��Search��6   .��5	      �� $ Vertical=�6!   .+�5
      9� $!��Search by Production Number��6#   
�������.��5      �� $ Vertical=�6)   .+�5   "   �� $*��$Enter [Production Number] to Search:��6,   $��Bottom=�6X   �� 5   "   $ 
Horizontal=�6-   .+�5   &   �� -   5         ��Name$ txtProductionNumber��5   .   $	��200��6K   $��5,0,0,0q�6Z    �� CharacterCasing$ Upper=�6k   �� 5   &   -   5         ��Name$! btnSearchbyProductionNumber��5   -   $!��Search by Production Number��6R   $
��Bold"�6x   $��5,0,0,0q�6�   6�   5   #   �� 5   "   $��PRODUCTION MEMO FOUND��6,    �� TextAlignment$ Center=�6I   &   System.Windows.Controls.DataGrid  5   "   -   5          Name$ dgProductionMemo��5   +   $��5,5,5,5q�6E      
IsReadOnly . 	   AutoGenerateColumns	 .  
   Columns
 5   &   0  *System.Windows.Controls.DataGridTextColumn  5   *   -   5        	colMemoNo��,  &System.Windows.Controls.DataGridColumn   Header$ Memo No.��5   P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6b   �� *MemoId+  5   *   $ Section��6=    6N   �� *	SectionId+5   #   5      5      �� 5      $!��Enter [Memo No.] to Search:��6$   $��Bottom=�6G   $��0,10,0,0q�6b   �� 5      $ 
Horizontal=�6%   $��0,5,0,0q�6>   .+�5      �� -   5        $ 	txtMemoId��5   &   $	��200��69   $��5,0,0,0q�6H   $ Upper=�6Y   �� 5      -   5        $ 	btnSearch��5   %   $��Search by Memo No.��68   $
��Bold"�6U   $��5,0,0,0q�6g   6x   5      5       5!      �� 5"      $��0,10,0,0q�6   $��	Memo No.:��6.   �� 5#      -   5         ��Name$ 	lblMemoId��5#      $
��Bold"�6/   �� 5$      $��0,10,0,0q�6   $��Section:��6.   �� 5%      -	   5         ��Name$ 
cboSection��5%       �� SelectedValuePath$ 	SectionId��6/    �� DisplayMemberPath$
 Name��6M   {�.  6f   �� 5&      $��0,10,0,0q�6   $��	Apply to:��6.     5'      -
   5        $ dgProductionNumber��5'         CanUserAddRows .    CanUserDeleteRows . 	 .     CanUserResizeRows .  $	��200��5,      
 5-        5.      $ Production Number��6-    6H   �� *Value+ �� UpdateSourceTrigger$ PropertyChanged=�50      �� 51      $��0,10,0,0q�6   $��Picture:��6.   �� 52      $
��Auto=�6   $��Disabled=�6D   .��53      �� $ 
Horizontal=�6!   $	��320��6:   .+�54      �� 
�������$��1,1,1,1q�64   $	��320��6N   
�������._�56      �� -   5         ��Name$ 
imgPicture��56   $   $��Uniform=�68    � ContextMenu 57   "   �� 58   &   .@�59   *   m� -   5        $��Clear��59   3   6B   5:   '   5<      5=      �� 5>      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5@      � .+�5A   "   �� $��	Picture 1��6,   $ Center=�6=   
�������6T   �� 5B   "   -   5        $ imgPicture1��5B   (   $��Uniform=�6=    5C   &   �� 5D   *   .@�5E   .   m� -   5        $��Clear��5E   7   6F   5F   +   5H   #   5I      5J      �� 5K      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5M      � .+�5N   "   �� $��	Picture 2��6,   $ Center=�6=   
�������6T   �� 5O   "   -   5        $ imgPicture2��5O   (   $��Uniform=�6=    5P   &   �� 5Q   *   .@�5R   .   m� -   5        $��Clear��5R   7   6F   5S   +   5U   #   5V      5W      �� 5X      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5Z      � .+�5[   "   �� $��	Picture 3��6,   $ Center=�6=   
�������6T   �� 5\   "   -   5        $ imgPicture3��5\   (   $��Uniform=�6=    5]   &   �� 5^   *   .@�5_   .   m� -   5        $��Clear��5_   7   6F   5`   +   5b   #   5c      5d      �� 5e      $��10,0,0,0q�6!   
�������$��1,1,1,1q�6F   $	��320��6`   
�������._�5g      � .+�5h   "   �� $��	Picture 4��6,   $ Center=�6=   
�������6T   �� 5i   "   -   5        $ imgPicture4��5i   (   $��Uniform=�6=    5j   &   �� 5k   *   .@�5l   .   m� -   5        $��Clear��5l   7   6F   5m   +   5o   #   5p      5q      5r      5s      �� 5t      $ 
Horizontal=�6   $��Right=�66   .+�5u      �� -   5        $ btnSavePicture��5u      $��Download Picture��65   $	��150��6P    5v      �� 5w      .@�5x   "   m� -   5         m�Name$ miSavePicture1��5x   +   $��Download Picture 1��6C   6_   m� 5y   "   -   5        $ miSavePicture2��5y   +   $��Download Picture 2��6C   6_   m� 5z   "   -   5        $ miSavePicture3��5z   +   $��Download Picture 3��6C   6_   m� 5{   "   -   5        $ miSavePicture4��5{   +   $��Download Picture 4��6C   6_   5|      5~      �� 5�      -   5        $ btnBrowserPicture��5�      $��5,0,0,0q�68   $��Browser Picture��6I   $	��150��6c    5�      �� 5�      .@�5�   "   m� -   5        $ miBrowserPicture1��5�   +   $��Browser Picture 1��6F   6a   m� 5�   "   -   5        $ miBrowserPicture2��5�   +   $��Browser Picture 2��6F   6a   m� 5�   "   -   5        $ miBrowserPicture3��5�   +   $��Browser Picture 3��6F   6a   m� 5�   "   -   5        $ miBrowserPicture4��5�   +   $��Browser Picture 4��6F   6a   5�      5�      5�      �� 5�      -   5        $ btnSave��5�      $��0,5,0,0q�6*   $��	Save Memo��6;   $
��Bold"�6O   {�.  6w   �� 5�      -    5        $ 	btnDelete��5�      $��0,5,0,0q�6,   $��Delete?��6=   $
��Bold"�6O   $	��100��6a   $��Right=�6m   {�.  6�   5�      5�      5�      5�      