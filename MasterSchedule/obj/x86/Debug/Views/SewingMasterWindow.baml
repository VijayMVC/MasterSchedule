   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.1.3, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   -    'MasterSchedule.Views.SewingMasterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   -   5          � Title$*  $Master Schedule - Sewing Master File��5   	   $	��640��66   $
��Auto��6C    � WindowStartupLocation$ CenterScreen=�5   
    � WindowState$ 	Maximized=�6F    J� InputBindings 5      �� 5   
   
 �� Key$ S��6    �� 	Modifiers$ Controlf�6    �� Command$
 Save��61   6B    J� CommandBindings 5
      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$
 Save��5      6Q   .��5      � 	 � RowDefinitions	 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� 
�������@�5      m� 5      $
��File��6   .@�5      m� -   5        
 m�Name$
 btnSave��5      $��_Save��60   {�.   m� InputGestureText$ Ctrl+S��6Q    m� Command$
 Save��6k   6z   m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ lblPopup��5      $y�	Collapsed=�6&   � 5   
   -   5         �Name$ popup��5      ��.  .��5      �� -   5         ��Name$ listView��5      ��5       �� 5!      .R�5"      � .+�5#      �� ._�5$   "   �� $
��Bold"�6G   ��6,   �� *	ProductNo+6G   5%      5&      5'      5)      5*      +   %MasterSchedule.Customs.CustomDataGrid  5+   
   -   5          Name$ dgSewingMaster��5+      +   %MasterSchedule.Customs.CustomDataGrid   KeyDown$ dgSewingMaster_KeyDown��5/         CellEditEnding$# dgSewingMaster_CellEditEnding��64      Sorting$ dgSewingMaster_Sorting��6c      MouseLeftButtonDown$( "dgSewingMaster_MouseLeftButtonDown��50         BeginningEdit$" dgSewingMaster_BeginningEdit��51   ]   !   PreviewMouseLeftButtonDown$/ )dgSewingMaster_PreviewMouseLeftButtonDown��52         	MouseMove$ dgSewingMaster_MouseMove��53         	DragLeave$ dgSewingMaster_DragLeave��68      Drop$ dgSewingMaster_Drop��54         DragOver$ dgSewingMaster_DragOver��6?      SelectedCellsChanged$) #dgSewingMaster_SelectedCellsChanged��6b   $��1��5+   7   &   System.Windows.Controls.DataGrid   AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     FrozenColumnCount$ 14��5,   D     �� AlternationCount$  2��6[   !   AlternatingRowBackground
! �����"   CanUserReorderColumns" .  #   SelectionUnit$# CellOrRowHeader=�5.   1   $   HorizontalGridLinesBrush
$ �����%   VerticalGridLinesBrush
% �����& J� 	AllowDrop& . ' � 	Resources' 55      *  $System.Windows.Controls.DataGridCell( �� 
TargetType) �� Triggers* P� Property+ P� Value, �� Property- �� Value��%�   '        �� 56      "(  6   ) 57      P� 58      * �@ 
IsSelected+ . . �59      �� , � ��
- ��iA�6;   �� 5:      , � ��
- �����6;   5;      5=      5>      . � ContextMenu. 5?      �� 5@      -   5        / ��Name$/ 
ctmTranfer��5@      $y�	Collapsed=�62   .@�5A      m� -	   5        $
 	miTranfer��5A      $��Tranfer to Sewing Master��62   6T   5B      0   Columns0 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      ,  &System.Windows.Controls.DataGridColumn1   Header$1 SEQUENCE��6B   2   
IsReadOnly2 . 3   
Visibility$3 	Collapsed=�6f   1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 6%   �� *
Sequence+6f     5F      $1 	PROD.
NO.��6C   5   
FontWeight$
5 Bold"�6Z   2 . 6   CanUserSort6 .  4 6%   �� *	ProductNo+7   	CellStyle7 5G      �� 5H      "(  6    .�5I      �� , � ��- 6;   �� *ProductNoBackground+5J      5L        5M      -
   5        
colCountry��$1 COUNTRY��5M   U   2 . 6 .  4 69   �� *	Country+6x     5N      -   5        colStyle��$1 STYLE��5N   T   2 . 6 .  4 67   �� *
ShoeName+6u     5O      $1 ART.
NO.��6C   2 . 6 .  4 6%   �� *	ArticleNo+6k     5P      $1 	PATT.
NO.��6C   2 . 6 .  4 6%   �� *	PatternNo+6l     5Q      $1 QUANTITY��6B   2 . 6 .  4 6%   �� *
Quantity+6f     5R      -   5        
colETD��$	1 EFD��5R   f   2 . 6 .  4 65   �� *ETD+8 �� StringFormat$8 dd-MMM��6�     5S      $1 Memo No.��6%   9   Width5	  /System.Windows.Controls.DataGridLengthConverter$
9 Auto	 67   2 . 4 6D   �� *MemoId+6_     5T      $1 Upper
Mat's
Arrival��6J   2 . 6 .  4 6%   �� *UpperMatsArrival+7 5U      �� 5V      "(  6    .�5W      �� , � ��- 6;   �� *UpperMatsArrivalForeground+�� 5X      , � ��- 6;   �� *UpperMatsArrivalBackground+5Y      5[        5\      $1 Sewing
Mat's
Arrival��6K   2 . 6 .  4 6%   �� *SewingMatsArrival+7 5]      �� 5^      "(  6    .�5_      �� , � ��- 6;   �� *SewingMatsArrivalForeground+�� 5`      , � ��- 6;   �� *SewingMatsArrivalBackground+5a      5c        5d      $1 OS
Mat's
Arrival��6G   2 . 6 .  4 6%   �� *OSMatsArrival+7 5e      �� 5f      "(  6    .�5g      �� , � ��- 6;   �� *OSMatsArrivalForeground+�� 5h      , � ��- 6;   �� *OSMatsArrivalBackground+5i      5k        5l      $1 Assy
Mat's
Arrival��6M   2 . 6 .  4 6%   �� *AssemblyMatsArrival+7 5m      �� 5n      "(  6    .�5o      �� , � ��- 6;   �� *AssemblyMatsArrivalForeground+�� 5p      , � ��- 6;   �� *AssemblyMatsArrivalBackground+5q      5s        5t      -   5        colSewingLine��2 . $1 Sewing
Line��5t   �   6 . 4 6<   �� *
SewingLine+: �� UpdateSourceTrigger$: PropertyChanged=�6�     5u      -   5        colSewingPrep��$1 Sewing
Prep��5u   [   2 . 6 .  4 6<   �� *
SewingPrep+6�     5v      $1 Sewing
Start
Date��6_   2 . 6 .  4 6%   �� *SewingStartDate+$	8 M/d��7 5w      �� 5x      "(  6    .�5y      �� , � ��- 6;   �� *SewingStartDateForeground+5z      5|        5}      $1 Sewing
Finish
Date��6`   2 . 6 .  4 6%   �� *SewingFinishDate+$	8 M/d��7 5~      �� 5      "(  6    .�5�      �� , � ��- 6;   �� *SewingFinishDateForeground+5�      5�        5�      -   5        colSewingQuota��$1 Sewing
Quota��5�   ]   2 . 6 .  4 6=   �� *SewingQuota+6�     5�      -   5        colSewingActualStartDate��$1 Sewing
Planned
Start
Date��5�   q   2 . 6 .  4 6G   �� *SewingActualStartDate+6�     5�      -   5        colSewingActualFinishDate��$ 1 Sewing
Planned
Finish
Date��5�   s   2 . 6 .  4 6H   �� *SewingActualFinishDate+6�     5�      -   5         colSewingActualStartDateAuto��$1 Sewing
Actual
Start
Date��5�   y   2 . 6 .  4 6K   �� *SewingActualStartDateAuto+6�     5�      -   5        !colSewingActualFinishDateAuto��$1 Sewing
Actual
Finish
Date��5�   {   2 . 6 .  4 6L   �� *SewingActualFinishDateAuto+6�     5�      -   5        colSewingBalance��$1 Sewing
Balance��5�   a   2 . 6 .  4 6?   �� *SewingBalance+6�     5�      $1 Stockfit
Finish
Date��6\   2 . 6 .  4 6%   �� *OSFinishDate+$	8 M/d��6�     5�      $1 Stockfit
Balance��6C   2 . 6 .  4 6%   �� *	OSBalance+6s     5�      $1 Cut A
Start
Date��6]   2 . 6 .  4 6%   �� *CutAStartDate+$	8 M/d��7 5�      �� 5�      "(  6    .�5�      �� , � ��- 6;   �� *CutAStartDateForeground+5�      5�        5�      $1 Cut A
Finish
Date��6^   2 . 6 .  4 6%   �� *CutAFinishDate+$	8 M/d��6�     5�      -   5        colCutAQuota��$1 Cut A
Quota��5�   Y   2 . 6 .  4 6;   �� *	CutAQuota+6�     5�      -   5        
colAutoCut��$1 Auto
Cut��5�   U   2 . 6 .  4 69   �� *	AutoCut+6}     5�      -   5        colLaserCut��$1 	Laser
Cut��5�   W   2 . 6 .  4 6:   �� *
LaserCut+6�     5�      -   5        colHuasenCut��$1 
Huasen
Cut��5�   Y   2 . 6 .  4 6;   �� *	HuasenCut+6�     5�      -   5        colCutAActualStartDate��$1 Cut A
Actual
Start��5�   m   2 . 6 .  4 6E   �� *CutAActualStartDate+6�     5�      -   5        colCutAActualFinishDate��$1 Cut A
Actual
Finish��5�   o   2 . 6 .  4 6F   �� *CutAActualFinishDate+6�     5�      -   5        colCutABalance��$1 Cut A
Balance��5�   ]   2 . 6 .  4 6=   �� *CutABalance+6�     5�      -   5        colPrintingBalance��$1 Printing
Balance��5�   e   2 . 6 .  4 6A   �� *PrintingBalance+6�     5�      -   5        colH_FBalance��$1 H/F
Balance��5�   [   2 . 6 .  4 6<   �� *
H_FBalance+6�     5�      -   5        colEmbroideryBalance��$1 Embro
Balance��5�   i   2 . 6 .  4 6C   �� *EmbroideryBalance+6�     5�      -   5        colCutBActualStartDate��$1 Cut B
Actual
Start��5�   m   2 . 6 .  4 6E   �� *CutBActualStartDate+6�     5�      -    5        colCutBBalance��$1 Cut B
Balance��5�   ]   2 . 6 .  4 6=   �� *CutBBalance+6�   5�      � 5�   
   $��2��6   .+�5�      �� -!   5        ; ��Name$; btnCaculate��5�      $	��150��6*   $
��Left=�69   $��	Calculate��6T   {�.  6z   5�      5�      5�      