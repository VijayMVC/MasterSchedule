   M S B A M L   `   `   `  ���� I  EMasterSchedule, Version=1.1.5.3, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  .    (MasterSchedule.Views.OutsoleMasterWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6local$clr-namespace:MasterSchedule.Customs     -     � Title$+  %Master Schedule - Outsole Master File��$	��640��$
��Auto�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� J� InputBindings �� 
 �� Key$ S�� �� 	Modifiers$ Controlf� �� Command$
 Save�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$
 Save��.��� 	 � RowDefinitions	 �� $
��Auto ��� $��* ��� $
��Auto �.+�o� 
�������@�m� $
��File��.@�m� -   
 m�Name$
 btnSave��$��_Save��{�.   m� InputGestureText$ Ctrl+S�� m� Command$
 Save��m� $��_Exit���� -    ��Name$ lblPopup��$y�	Collapsed=�� -    �Name$ popup����.  .���� -    ��Name$ listView������ .R�� .+��� ._��� $
��Bold"����� *	ProductNo++   %MasterSchedule.Customs.CustomDataGrid  -     Name$ dgSewingMaster��+   %MasterSchedule.Customs.CustomDataGrid   KeyDown$ dgSewingMaster_KeyDown��   CellEditEnding$# dgSewingMaster_CellEditEnding��   Sorting$ dgSewingMaster_Sorting��   MouseLeftButtonDown$( "dgSewingMaster_MouseLeftButtonDown��   BeginningEdit$" dgSewingMaster_BeginningEdit��!   PreviewMouseLeftButtonDown$/ )dgSewingMaster_PreviewMouseLeftButtonDown��   	MouseMove$ dgSewingMaster_MouseMove��   	DragLeave$ dgSewingMaster_DragLeave��   Drop$ dgSewingMaster_Drop��   DragOver$ dgSewingMaster_DragOver��   SelectedCellsChanged$) #dgSewingMaster_SelectedCellsChanged��$��1��&   System.Windows.Controls.DataGrid   AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     FrozenColumnCount$ 14��  �� AlternationCount$  2��!   AlternatingRowBackground
! �����"   CanUserReorderColumns" .  #   SelectionUnit$# CellOrRowHeader=�$   HorizontalGridLinesBrush
$ �����%   VerticalGridLinesBrush
% �����& J� 	AllowDrop& . ' � 	Resources' *  $System.Windows.Controls.DataGridCell( �� 
TargetType) �� Triggers* P� Property+ P� Value, �� Property- �� Value��%j   '        �� "(  ) P� * �@ 
IsSelected+ . . ��� , � ��
- ��iA��� , � ��
- �����.   Columns. 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn/   Header$/ SEQUENCE��0   
IsReadOnly0 . 1   
Visibility$1 	Collapsed=�1  +System.Windows.Controls.DataGridBoundColumn2   Binding2 �� *
Sequence+  $/ 	PROD.
NO.��3   
FontWeight$
3 Bold"�0 . 4   CanUserSort4 .  2 �� *	ProductNo+5   	CellStyle5 �� "(  .��� , � ��- �� *ProductNoBackground+  $/ COUNTRY��0 . 4 .  2 �� *	Country+  $/ STYLE��0 . 4 .  2 �� *
ShoeName+  $/ ART.
NO.��0 . 4 .  2 �� *	ArticleNo+  $/ 	OS.
CODE.��0 . 4 .  2 �� *OutsoleCode+  $/ QUANTITY��0 . 4 .  2 �� *
Quantity+  $	/ EFD��0 . 4 .  2 �� *ETD+6 �� StringFormat$6 dd-MMM��  $/ Memo No.��7   Width5	  /System.Windows.Controls.DataGridLengthConverter$
7 Auto	 0 . 2 �� *MemoId+  $/ Sewing
Line��0 . 4 .  2 �� *
SewingLine+  $/ Sewing
Start
Date��0 . 4 .  2 �� *SewingStartDate+$	6 M/d��  $/ Sewing
Finish
Date��0 . 4 .  2 �� *SewingFinishDate+$	6 M/d��  $/ Sewing
Quota��0 . 4 .  2 �� *SewingQuota+  $/ Sewing
Balance��0 . 4 .  2 �� *SewingBalance+  $/ OS
Mat's
Arrival��0 . 4 .  2 �� *OSMatsArrival+5 �� "(  .��� , � ��- �� *OSMatsArrivalForeground+�� , � ��- �� *OSMatsArrivalBackground+  $/ WH
Outsole
Balance��0 . 4 .  2 �� *OutsoleWHBalance+  -   colOutsoleLine��0 . $/ Outsole
Line��4 . 2 �� *OutsoleLine+8 �� UpdateSourceTrigger$8 PropertyChanged=�  $/ Outsole
Start
Date��0 . 4 .  2 �� *OutsoleStartDate+$	6 M/d��5 �� "(  .��� , � ��- �� *OutsoleStartDateForeground+  $/ Outsole
Finish
Date��0 . 4 .  2 �� *OutsoleFinishDate+$	6 M/d��5 �� "(  .��� , � ��- �� *OutsoleFinishDateForeground+  -	   colOutsoleQuota��$/ OS
Quota��0 . 4 .  2 �� *OutsoleQuota+  -
   colOutsoleActualStartDate��$/ Outsole
Actual
Start
Date��0 . 4 .  2 �� *OutsoleActualStartDate+  -   colOutsoleActualFinishDate��$ / Outsole
Actual
Finish
Date��0 . 4 .  2 �� *OutsoleActualFinishDate+  -   colOutsoleBalance��$/ Outsole
Balance��0 . 4 .  2 �� *OutsoleBalance+  $/ Released
Quantity��0 . 4 .  2 �� *ReleasedQuantity+� $��2��.+��� -   9 ��Name$9 btnCaculate��$	��150��$
��Left=�$��	Calculate��{�.  