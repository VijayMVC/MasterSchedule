   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.1.3, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   7    1MasterSchedule.Views.ImportAvailableMachineWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   -   5          � Title$  Import Available Machine��5   	   $	��300��6*   $
��Auto��67   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 	scrHeader��5      $��Visible=�6*   $��Hidden=�6P   .��5      � $��0��6    � ColumnDefinitions 5      �� 5      $��11 �6'   �� 5      ��6'   ��  �� ElementName$ Column1�� �� Path$ ActualWidth��� 5      ��6'   �� $ 	Column4_2��$ ActualWidth��� 5      ��6'   �� $ 	Column4_3��$ ActualWidth��� 5      ��6'   �� $ 	Column4_4��$ ActualWidth��� 5      ��6'   �� $ 	Column4_5��$ ActualWidth��� 5      ��6'   �� $ Column4_5_1��$ ActualWidth��� 5      ��6'   �� $ 	Column4_6��$ ActualWidth��� 5      ��6'   �� $ 	Column5_2��$ ActualWidth��� 5      ��6'   �� $ 	Column5_3��$ ActualWidth��� 5      ��6'   �� $ Column5_3_1��$ ActualWidth��� 5      ��6'   �� $ 	Column5_4��$ ActualWidth��� 5      ��6'   �� $ 	Column5_5��$ ActualWidth��� 5      ��6'   �� $ 	Column5_6��$ ActualWidth��� 5!      ��6'   �� $ 	Column6_2��$ ActualWidth��� 5"      ��6'   �� $ 	Column6_3��$ ActualWidth��� 5#      ��6'   �� $ 	Column6_4��$ ActualWidth��� 5$      ��6'   �� $ 	Column6_5��$ ActualWidth��� 5%      ��6'   �� $ 	Column6_6��$ ActualWidth��� 5&      ��6'   �� $ 	Column6_7��$ ActualWidth��� 5'      ��6'   �� $ 	Column6_8��$ ActualWidth��� 5(      ��6'   �� $ 	Column6_9��$ ActualWidth��� 5)      ��6'   �� $ 
Column6_10��$ ActualWidth��� 5*      ��6'   �� $ 
Column6_11��$ ActualWidth��� 5+      ��6'   �� $ 
Column6_12��$ ActualWidth��� 5-      ��6'   �� $ 	Column7_2��$ ActualWidth��� 5.      ��6'   �� $ 	Column7_3��$ ActualWidth��� 5/      ��6'   �� $ 	Column7_4��$ ActualWidth��� 50      ��6'   �� $ 	Column7_5��$ ActualWidth��� 51      ��6'   �� $ 	Column7_6��$ ActualWidth��� 52      ��6'   �� $ 	Column7_7��$ ActualWidth��� 53      ��6'   �� $ 	Column7_8��$ ActualWidth��� 55      ��6'   �� $ 	Column8_2��$ ActualWidth��� 56      ��6'   �� $ 	Column8_3��$ ActualWidth��� 57      ��6'   �� $ 	Column8_4��$ ActualWidth��� 58      ��6'   �� $ 	Column8_5��$ ActualWidth��� 59      ��6'   �� $ 	Column8_6��$ ActualWidth��� 5:      ��6'   �� $ 	Column8_7��$ ActualWidth��� 5;      ��6'   �� $ 	Column8_8��$ ActualWidth��� 5<      ��6'   �� $ 	Column8_9��$ ActualWidth�.+�5?      �� $��2��6   $��6��6)   
���   �$��1,1,1,0q�6Q   ._�5@      �� $��0,3,0,3q�6    $��CUTTING��61    �� TextAlignment$ Center=�6@   $
��Bold"�6W   5A      �� 5B      $��8��6   $��6��6)   
���   �$��0,1,1,0q�6Q   ._�5C      �� $��0,3,0,3q�6    $
��PREP��61   $ Center=�6=   $
��Bold"�6T   5D      �� 5E      $��14��6   $��11��6*   
���   �$��0,1,1,0q�6S   ._�5F      �� $��0,3,0,3q�6    $��SEWING��61   $ Center=�6?   $
��Bold"�6V   5G      �� 5H      $��25��6   $��7��6*   
���   �$��0,1,1,0q�6R   ._�5I      �� $��0,3,0,3q�6    $��STOCKFIT��61   $ Center=�6A   $
��Bold"�6X   5J      �� 5K      $��32��6   $��8��6*   
���   �$��0,1,1,0q�6R   ._�5L      �� $��0,3,0,3q�6    $��ASSEMBLY��61   $ Center=�6A   $
��Bold"�6X   5M      5N      5O      +   %MasterSchedule.Customs.CustomDataGrid  5P   
   -   5          Name$ dgAvailableMachine��5P   ;   &   System.Windows.Controls.DataGrid   AutoGenerateColumns .  	   RowHeaderWidth$	 10Q�6W   
   CanUserAddRows
 .  $��1��6�      FrozenColumnCount$ 1��5Q         CanUserResizeRows .     HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources 5T      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 5U      "  6    5V      P� 5W       �@ 
IsSelected . . �5X      ��  � ��
 ��iA�6;   �� 5Y       � ��
 �����6;   5Z      5\      5]         Columns 5^      0  *System.Windows.Controls.DataGridTextColumn  5_      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$	  Id��5_   6      Width5  /System.Windows.Controls.DataGridLengthConverter$ 30 6C      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6N   �� *Id+6e     5a      -   5        	Column4_2��$ Arm
Clicker��5a   8   $
 Auto 6Q    .  6^   �� *CuttingArmClicker+6�     5b      -   5        	Column4_3��$
 Beam��5b   8   $
 Auto 6F    .  6S   �� *CuttingBeam+6s     5c      -   5        	Column4_4��$ CutStrap��5c   8   $
 Auto 6J    .  6W   �� *CuttingCutStrap+6{     5d      -   5        	Column4_5��$ Laser��5d   8   $
 Auto 6G    .  6T   �� *CuttingLaser+6u     5e      -	   5        Column4_5_1��$ Puncher
Hole��5e   :   $
 Auto 6T    .  6a   �� *CuttingPuncherHole+6�     5f      -
   5        	Column4_6��$ Skiving��5f   8   $
 Auto 6I    .  6V   �� *CuttingSkiving+6y     5h      -   5        	Column5_2��$ Vertical
HF��5h   8   $
 Auto 6Q    .  6^   �� *PrepVerticalHF+6�     5i      -   5        	Column5_3��$ Horizontal
HF��5i   8   $
 Auto 6S    .  6`   �� *PrepHorizontalHF+6�     5j      -   5        Column5_3_1��$ Online Heat
Press��5j   :   $
 Auto 6Y    .  6f   �� *PrepHorizontalHF+6�     5k      -   5        	Column5_4��$ Auto
HF��5k   8   $
 Auto 6M    .  6Z   �� *
PrepAutoHF+6y     5l      -   5        	Column5_5��$
 Inye��5l   8   $
 Auto 6F    .  6S   �� *
PrepInye+6p     5m      -   5        	Column5_6��$ Hotmelt
Machine��5m   8   $
 Auto 6U    .  6b   �� *PrepHotmeltMachine+6�     5o      -   5        	Column6_2��$ Small
Computer��5o   8   $
 Auto 6T    .  6a   �� *SewingSmallComputer+6�     5p      -   5        	Column6_3��$ Big
Computer��5p   8   $
 Auto 6R    .  6_   �� *SewingBigComputer+6�     5q      -   5        	Column6_4��$ 
Ultrasonic��5q   8   $
 Auto 6L    .  6Y   �� *SewingUltrasonic+6~     5r      -   5        	Column6_5��$ 4Needle
Flat��5r   8   $
 Auto 6R    .  6_   �� *Sewing4NeedleFlat+6�     5s      -   5        	Column6_6��$ 4Needle
Post��5s   8   $
 Auto 6R    .  6_   �� *Sewing4NeedlePost+6�     5t      -   5        	Column6_7��$ 
Long
Table��5t   8   $
 Auto 6P    .  6]   �� *SewingLongTable+6�     5u      -   5        	Column6_8��$ 	Eyeleting��5u   8   $
 Auto 6K    .  6X   �� *SewingEyeleting+6|     5v      -   5        	Column6_9��$ 
ZZ
Binding��5v   8   $
 Auto 6P    .  6]   �� *SewingZZBinding+6�     5w      -   5        
Column6_10��$ Holmelt
Machine��5w   9   $
 Auto 6V    .  6c   �� *SewingHotmeltMachine+6�     5x      -   5        
Column6_11��$ HandHeld
Hotmelt��5x   9   $
 Auto 6W    .  6d   �� *SewingHandHeldHotmelt+6�     5y      -   5        
Column6_12��$ Stationary
HHHotmelt��5y   9   $
 Auto 6[    .  6h   �� *SewingStationaryHHHotmelt+6�     5{      -   5        	Column7_2��$ Vertical
Buffing��5{   8   $
 Auto 6V    .  6c   �� *StockfitVerticalBuffing+6�     5|      -   5        	Column7_3��$ Horizontal
Buffing��5|   8   $
 Auto 6X    .  6e   �� *StockfitHorizontalBuffing+6�     5}      -   5        	Column7_4��$ Side
Buffing��5}   8   $
 Auto 6R    .  6_   �� *StockfitSideBuffing+6�     5~      -   5        	Column7_5��$ Outsole
Stitching��5~   8   $
 Auto 6W    .  6d   �� *StockfitOutsoleStitching+6�     5      -    5        	Column7_6��$ Auto
Buffing��5   8   $
 Auto 6R    .  6_   �� *StockfitAutoBuffing+6�     5�      -!   5        	Column7_7��$ Hydraulic
Cutting��5�   8   $
 Auto 6W    .  6d   �� *StockfitHydraulicCutting+6�     5�      -"   5        	Column7_8��$ Pad
Painting��5�   8   $
 Auto 6R    .  6_   �� *StockfitPadPrinting+6�     5�      -#   5        	Column8_2��$ Toe
Lasting��5�   8   $
 Auto 6Q    .  6^   �� *AssemblyToeLasting+6�     5�      -$   5        	Column8_3��$ Side
Lasting��5�   8   $
 Auto 6R    .  6_   �� *AssemblySideLasting+6�     5�      -%   5        	Column8_4��$ Heel
Lasting��5�   8   $
 Auto 6R    .  6_   �� *AssemblyHeelLasting+6�     5�      -&   5        	Column8_5��$ 
Side
Press��5�   8   $
 Auto 6P    .  6]   �� *AssemblySidePress+6�     5�      -'   5        	Column8_6��$ Top
Down��5�   8   $
 Auto 6N    .  6[   �� *AssemblyTopDown+6     5�      -(   5        	Column8_7��$ Hotmelt
Machine��5�   8   $
 Auto 6U    .  6b   �� *AssemblyHotmeltMachine+6�     5�      -)   5        	Column8_8��$ Sockliner
Hotmelt��5�   8   $
 Auto 6W    .  6d   �� *AssemblySocklinerHotmelt+6�     5�      -*   5        	Column8_9��$ VWrinkle
Remover��5�   8   $
 Auto 6V    .  6c   �� *AssemblyVWrinkleRemover+6�   5�      �� 5�   
   -+   5         ��Name$ 	btnImport��5�      $��2��6$   $��0,2,2,2q�61   $	��150��6B   $��Right=�6Q   $��Import��6m    � ToolTip$) #Click to import requirement machine��6~   $
��Bold"�6�   {�.  6�   �� 5�   
   $��3��6   .@�5�      �� $��Processing:��6   � 5�      -,   5         �Name$ progressBar��5�      $	��100��6/   $��15��6;    � Minimum$ 0Q�6G    � Value$ 0Q�6S   �� 5�      --   5          ��Name$  	lblStatus��5�      $�� ��6+   $
��Bold"�63   5�      5�      5�      