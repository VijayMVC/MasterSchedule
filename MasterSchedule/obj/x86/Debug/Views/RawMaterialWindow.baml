   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.1.3, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   ,    &MasterSchedule.Views.RawMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   -   5          � Title$&   Master Schedule - WH Master File��5   	   $	��640��62   $
��Auto��6?    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�6E    J� InputBindings 5      �� 5	   
   
 �� Key$ S��6    �� 	Modifiers$ Controlf�6    �� Command$
 Save��61   6B    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$
 Save��5      6Q   .��5      � 	 � RowDefinitions	 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   o� 
�������@�5      m� 5      $
��File��6   .@�5      m� -   5        
 m�Name$
 btnSave��5      $��_Save��60   {�.   m� InputGestureText$ Ctrl+S��6Q    m� Command$
 Save��6k   6z   m� 5      $��_Exit��6   5      5      � 5   
   $��1��6   	 5      �� 5      $
��Auto �6    �� 5       $��2* �6    .+�5"      �� -   5         ��Name$ 
scrlHeader��5"      $��Visible=�6/   $��Hidden=�6U   .��5#      � $��0��6    � ColumnDefinitions 5$      �� 5%      ��6+   ��  �� ElementName$ dgRawMaterial�� �� Path$ BorderThickness.Left��� 5&      ��6+   �� $ dgRawMaterial��$ RowHeaderWidth��� 5'      ��6+   �� $ Column1��$ ActualWidth��� 5(      ��6+   �� $ Column2��$ ActualWidth��� 5)      ��6+   �� $ Column3��$ ActualWidth��� 5*      ��6+   �� $ Column4��$ ActualWidth��� 5+      ��6+   �� $ Column5��$ ActualWidth��� 5,      ��6+   �� $ 	Column5_1��$ ActualWidth��� 5-      ��6+   �� $ Column6��$ ActualWidth��� 5.      ��6+   �� $ Column7��$ ActualWidth��� 5/      ��6+   �� $ 	Column7_1��$ ActualWidth��� 50      ��6+   �� $ 	Column7_2��$ ActualWidth��� 51      ��6+   �� $ Column7_1_1��$ ActualWidth��� 52      ��6+   �� $ 
Column10_1��$ ActualWidth��� 53      ��6+   �� $ 
Column10_2��$ ActualWidth��� 54      ��6+   �� $ 
Column10_3��$ ActualWidth��� 55      ��6+   �� $ Column8��$ ActualWidth��� 56      ��6+   �� $ Column9��$ ActualWidth��� 57      ��6+   �� $ Column10��$ ActualWidth��� 58      ��6+   �� $ Column11��$ ActualWidth��� 59      ��6+   �� $ Column12��$ ActualWidth��� 5:      ��6+   �� $ Column13��$ ActualWidth��� 5;      ��6+   �� $ Column14��$ ActualWidth��� 5<      ��6+   �� $ Column15��$ ActualWidth��� 5=      ��6+   �� $ Column16��$ ActualWidth��� 5>      ��6+   �� $ Column17��$ ActualWidth��� 5?      ��6+   �� $ Column18��$ ActualWidth��� 5@      ��6+   �� $ Column19��$ ActualWidth��� 5A      ��6+   �� $ Column20��$ ActualWidth��� 5B      ��6+   �� $ Column21��$ ActualWidth��� 5C      ��6+   �� $ Column22��$ ActualWidth��� 5D      ��6+   �� $ Column23��$ ActualWidth��� 5E      ��6+   �� $ Column24��$ ActualWidth��� 5F      ��6+   �� $ Column25��$ ActualWidth��� 5G      ��6+   �� $ 
Column25_1��$ ActualWidth��� 5H      ��6+   �� $ 
Column25_2��$ ActualWidth��� 5I      ��6+   �� $ 
Column25_3��$ ActualWidth��� 5J      ��6+   �� $ 
Column25_4��$ ActualWidth��� 5K      ��6+   �� $ 
Column25_5��$ ActualWidth��� 5L      ��6+   �� $ 
Column25_6��$ ActualWidth��� 5M      ��6+   �� $ Column26��$ ActualWidth��� 5N      ��6+   �� $ Column27��$ ActualWidth��� 5O      ��6+   �� $ Column28��$ ActualWidth��� 5P      ��6+   �� $ Column29��$ ActualWidth��� 5Q      ��6+   �� $ Column30��$ ActualWidth��� 5R      ��6+   �� $ Column31��$ ActualWidth��� 5S      ��6+   �� $ Column32��$ ActualWidth��� 5T      ��6+   �� $ Column33��$ ActualWidth��� 5U      ��6+   �� $ Column34��$ ActualWidth��� 5V      ��6+   �� $ Column35��$ ActualWidth��� 5W      ��6+   �� $ Column36��$ ActualWidth��� 5X      ��6+   �� $ Column37��$ ActualWidth��� 5Y      ��6+   �� $ Column38��$ ActualWidth�.+�5[      �� $��13��6   $��3��6.   
���   �$��1,1,1,0q�6V   ._�5\      �� $��0,3,0,3q�6$   $��TAIWAN��65    �� TextAlignment$ Center=�6C   $
��Bold"�6Z   5]      �� 5^      $��16��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5_      �� $��0,3,0,3q�6$   $��
LAMINATION��65   $ Center=�6G   $
��Bold"�6^   5`      �� 5a      $��19��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5b      �� $��0,3,0,3q�6$   $��CUTTING��65   $ Center=�6D   $
��Bold"�6[   5c      �� 5d      $��22��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5e      �� $��0,3,0,3q�6$   $��LEATHER��65   $ Center=�6D   $
��Bold"�6[   5f      �� 5g      $��25��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5h      �� $��0,3,0,3q�6$   $��SEMI-PROCESS��65   $ Center=�6I   $
��Bold"�6`   5i      �� 5j      $��28��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5k      �� $��0,3,0,3q�6$   $��SEWING��65   $ Center=�6C   $
��Bold"�6Z   5l      �� 5m      $��31��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5n      �� $��0,3,0,3q�6$   $��OUTSOLE��65   $ Center=�6D   $
��Bold"�6[   5o      �� 5p      $��34��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5q      �� $��0,3,0,3q�6$   $��UPPER COMPONENT��65   $ Center=�6L   $
��Bold"�6c   5r      �� 5s      $��37��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5t      �� $��0,3,0,3q�6$   $��INSOCK��65   $ Center=�6C   $
��Bold"�6Z   5u      �� 5v      $��40��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5w      �� $��0,3,0,3q�6$   $��SECURITY LABEL��65   $ Center=�6K   $
��Bold"�6b   5x      �� 5y      $��43��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5z      �� $��0,3,0,3q�6$   $��ASSEMBLY��65   $ Center=�6E   $
��Bold"�6\   5{      �� 5|      $��46��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5}      �� $��0,3,0,3q�6$   $��SOCK LINING��65   $ Center=�6H   $
��Bold"�6_   5~      �� 5      $��49��6   $��3��6.   
���   �$��0,1,1,0q�6V   ._�5�      �� $��0,3,0,3q�6$   $��CARTON��65   $ Center=�6C   $
��Bold"�6Z   5�      5�      5�      +   %MasterSchedule.Customs.CustomDataGrid  5�      -   5          Name$ dgRawMaterial��5�   ?   +   %MasterSchedule.Customs.CustomDataGrid   CellEditEnding$" dgRawMaterial_CellEditEnding��5�   #      KeyDown$ dgRawMaterial_KeyDown��6Q      MouseLeftButtonDown$' !dgRawMaterial_MouseLeftButtonDown��5�   #      Sorting$ dgRawMaterial_Sorting��6[      SelectedCellsChanged$( "dgRawMaterial_SelectedCellsChanged��5�   l   &   System.Windows.Controls.DataGrid   AutoGenerateColumns .  $��1��5�   V      RowHeaderWidth$ 10Q�6c      CanUserAddRows .     SelectionUnit$ CellOrRowHeader=�6�      AlternatingRowBackground
 ����� �� AlternationCount$ 2��6�      FrozenColumnCount$ 10��5�   q      CanUserResizeRows .      HorizontalGridLinesBrush
  �����!   VerticalGridLinesBrush
! �����" � 	Resources" 5�      *  $System.Windows.Controls.DataGridCell# �� 
TargetType$ �� Triggers% P� Property& P� Value' �� Property( �� Value��%�   '        �� 5�      "#  6   $ 5�      P� 5�      % �@ 
IsSelected& . . �5�   "   �� ' � ��
( ��iA�6?   �� 5�   "   ' � ��
( �����6?   5�      5�      5�      ) � ContextMenu) 5�      �� 5�      .@�5�      m� -   5        $
 	miDisable��5�   #   $��Disable��66   6G   5�      *   Columns* 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      -   5        Column1��,  &System.Windows.Controls.DataGridColumn+   Header$+ 	PROD.
NO.��5�   :   ,   Width5  /System.Windows.Controls.DataGridLengthConverter$
, Auto 6Q   -   
FontWeight$
- Bold"�6|   .   
IsReadOnly. . 1	  +System.Windows.Controls.DataGridBoundColumn/ 	  Binding/ 6^   �� *	ProductNo+0   	CellStyle0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *ProductNoBackground+5�      5�        5�      -   5        Column2��$+ COUNTRY��5�   :   $
, Auto 6K   . . / 6X   �� *	Country+6t     5�      -	   5        Column3��$+ STYLE��5�   :   $
, Auto 6I   . . / 6V   �� *
ShoeName+6s     5�      -
   5        Column4��$+ ARTICLE
NO.��5�   :   $
, Auto 6S   . . / 6`   �� *	ArticleNo+6~     5�      -   5        Column5��$+ 	PATT. NO.��5�   :   $
, Auto 6M   . . / 6Z   �� *	PatternNo+6x     5�      -   5        	Column5_1��$+ OUTSOLE
CODE��5�   <   $
, Auto 6V   . . / 6c   �� *OutsoleCode+6�     5�      -   5        Column6��$+ QUANTITY��5�   :   $
, Auto 6L   . . / 6Y   �� *
Quantity+6v     5�      -   5        Column7��$	+ EFD��5�   :   $
, Auto 6G   . . / 6T   �� *ETD+1 �� StringFormat$1 dd-MMM��2 �� ConverterCulture$2 en-US��6�     5�      -   5        	Column7_1��$+ Cut A
Start
Date��5�   t   . . / 6<   �� *CutAStartDate+$	1 M/d��0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *CutAStartDateForeground+5�      5�        5�      -   5        Column7_1_1��$+ Assy
Start
Date��5�   v   . . / 6>   �� *AssyStartDate+$	1 M/d��0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *AssyStartDateForeground+5�      5�        5�      -   5        	Column7_2��$+ Memo No.��5�   <   $
, Auto 6N   . . / 6[   �� *MemoId+6v     5�      -   5        
Column10_1��$	+ EFD��5�   =   $
, Auto 6J   . . / 6W   �� *
TAIWAN_ETD+3 �� UpdateSourceTrigger$3 PropertyChanged=�6�     5�      -   5        
Column10_2��$+ Actual
Date��5�   =   $
, Auto 6V   . . / 6c   �� *TAIWAN_ActualDate+$3 PropertyChanged=�6�     5�      -   5        
Column10_3��$+ REMARKS��5�   =   $
, Auto 6N   . . / 6[   �� *TAIWAN_Remarks+$3 PropertyChanged=�6�     5�      -   5        Column8��$	+ EFD��5�   :   $
, Auto 6G   . . / 6T   �� *LAMINATION_ETD+$3 PropertyChanged=�6�     5�      -   5        Column9��$+ Actual
Date��5�   :   $
, Auto 6S   . . / 6`   �� *LAMINATION_ActualDate+$3 PropertyChanged=�6�     5�      -   5        Column10��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *LAMINATION_Remarks+$3 PropertyChanged=�6�     5�      -   5        Column11��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *CUTTING_ETD+$3 PropertyChanged=�6�     5�      -   5        Column12��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *CUTTING_ActualDate+$3 PropertyChanged=�6�     5�      -   5        Column13��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *CUTTING_Remarks+$3 PropertyChanged=�6�     5�      -   5        Column14��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *LEATHER_ETD+$3 PropertyChanged=�6�     5�      -   5        Column15��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *LEATHER_ActualDate+$3 PropertyChanged=�6�     5�      -   5        Column16��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *LEATHER_Remarks+$3 PropertyChanged=�6�     5�      -   5        Column17��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *SEMIPROCESS_ETD+$3 PropertyChanged=�6�     5�      -   5        Column18��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *SEMIPROCESS_ActualDate+$3 PropertyChanged=�6�     5�      -    5        Column19��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *SEMIPROCESS_Remarks+$3 PropertyChanged=�6�     5�      -!   5        Column20��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *
SEWING_ETD+$3 PropertyChanged=�6�     5�      -"   5        Column21��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *SEWING_ActualDate+$3 PropertyChanged=�6�     5�      -#   5        Column22��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *SEWING_Remarks+$3 PropertyChanged=�6�     5�      -$   5        Column23��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *OUTSOLE_ETD+$3 PropertyChanged=�6�     5�      -%   5        Column24��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *OUTSOLE_ActualDate+$3 PropertyChanged=�0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *OUTSOLE_ActualDate_BACKGROUND+$3 PropertyChanged=��� 5�   "   ' � ��
( �   �6?   5�      5�        5�      -&   5        Column25��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *OUTSOLE_Remarks+$3 PropertyChanged=�6�     5�      -'   5        
Column25_1��$	+ EFD��5�   =   $
, Auto 6J   . . / 6W   �� *UPPERCOMPONENT_ETD+$3 PropertyChanged=�6�     5�      -(   5        
Column25_2��$+ Actual
Date��5�   =   $
, Auto 6V   . . / 6c   �� *UPPERCOMPONENT_ActualDate+$3 PropertyChanged=�6�     5�      -)   5        
Column25_3��$+ REMARKS��5�   =   $
, Auto 6N   . . / 6[   �� *UPPERCOMPONENT_Remarks+$3 PropertyChanged=�6�     5�      -*   5        
Column25_4��$	+ EFD��5�   =   $
, Auto 6J   . . / 6W   �� *
INSOCK_ETD+$3 PropertyChanged=�6�     5�      -+   5        
Column25_5��$+ Actual
Date��5�   =   $
, Auto 6V   . . / 6c   �� *INSOCK_ActualDate+$3 PropertyChanged=�6�     5�      -,   5        
Column25_6��$+ Remarks��5�   =   $
, Auto 6N   . . / 6[   �� *INSOCK_Remarks+$3 PropertyChanged=�6�     5�      --   5        Column26��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *SECURITYLABEL_ETD+$3 PropertyChanged=�6�     5�      -.   5        Column27��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *SECURITYLABEL_ActualDate+$3 PropertyChanged=�6�     5�      -/   5        Column28��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *SECURITYLABEL_Remarks+$3 PropertyChanged=�6�     5�      -0   5        Column29��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *ASSEMBLY_ETD+$3 PropertyChanged=�6�     5�      -1   5        Column30��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *ASSEMBLY_ActualDate+$3 PropertyChanged=�6�     5�      -2   5        Column31��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *ASSEMBLY_Remarks+$3 PropertyChanged=�6�     5�      -3   5        Column32��$	+ EFD��5�   ;   $
, Auto 6H   . . / 6U   �� *SOCKLINING_ETD+$3 PropertyChanged=�6�     5�      -4   5        Column33��$+ Actual
Date��5�   ;   $
, Auto 6T   . . / 6a   �� *SOCKLINING_ActualDate+$3 PropertyChanged=�6�     5�      -5   5        Column34��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *SOCKLINING_Remarks+$3 PropertyChanged=�6�     5�      -6   5        Column35��$	+ EFD��5�   ;   $
, Auto 6H   4   SortMemberPath$4 CARTON_ETD_Sort��6�   . . / 6U   �� *
CARTON_ETD+$3 PropertyChanged=�0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *CARTON_ETD_Background+�� 5�   "   ' � ��
( �   �6?   5�      5�        5�      -7   5        Column36��$+ Actual
Date��5�   ;   $
, Auto 6T   $4 CARTON_ActualDate_Sort��6�   . . / 6a   �� *CARTON_ActualDate+$3 PropertyChanged=�0 5�      �� 5�      "#  6$   .�5�   "   �� ' � ��( 6?   �� *CARTON_ActualDate_Background+�� 5�   "   ' � ��
( �   �6?   5�      5�        5�      -8   5        Column37��$+ REMARKS��5�   ;   $
, Auto 6L   . . / 6Y   �� *CARTON_Remarks+$3 PropertyChanged=�6�     5      -9   5        Column38��$+ Loading
Date��5   ;   . . / 6U   �� *LoadingDate+$3 PropertyChanged=�6�   5     5     �� 5  
   -:   5        5 ��Name$5 	btnReload��5     $��2��6   $��_Reload WH Master File��61   $	��150��6R   $
��Left=�6a   {�.  
����  �$��0,2,0,0q�6�   5     5     