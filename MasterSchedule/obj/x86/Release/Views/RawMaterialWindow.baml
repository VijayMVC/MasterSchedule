   M S B A M L   `   `   `  ���� I  EMasterSchedule, Version=1.1.5.3, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  ,    &MasterSchedule.Views.RawMaterialWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6local$clr-namespace:MasterSchedule.Customs     -     � Title$&   Master Schedule - WH Master File��$	��640��$
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
 Save��m� $��_Exit��� $��1��	 �� $
��Auto ��� $��2* �.+��� -    ��Name$ 
scrlHeader��$��Visible=�$��Hidden=�.��� $��0�� � ColumnDefinitions �� ����  �� ElementName$ dgRawMaterial�� �� Path$ BorderThickness.Left��� ���� $ dgRawMaterial��$ RowHeaderWidth��� ���� $ Column1��$ ActualWidth��� ���� $ Column2��$ ActualWidth��� ���� $ Column3��$ ActualWidth��� ���� $ Column4��$ ActualWidth��� ���� $ Column5��$ ActualWidth��� ���� $ 	Column5_1��$ ActualWidth��� ���� $ Column6��$ ActualWidth��� ���� $ Column7��$ ActualWidth��� ���� $ 	Column7_1��$ ActualWidth��� ���� $ 	Column7_2��$ ActualWidth��� ���� $ 
Column10_1��$ ActualWidth��� ���� $ 
Column10_2��$ ActualWidth��� ���� $ 
Column10_3��$ ActualWidth��� ���� $ Column8��$ ActualWidth��� ���� $ Column9��$ ActualWidth��� ���� $ Column10��$ ActualWidth��� ���� $ Column11��$ ActualWidth��� ���� $ Column12��$ ActualWidth��� ���� $ Column13��$ ActualWidth��� ���� $ Column14��$ ActualWidth��� ���� $ Column15��$ ActualWidth��� ���� $ Column16��$ ActualWidth��� ���� $ Column17��$ ActualWidth��� ���� $ Column18��$ ActualWidth��� ���� $ Column19��$ ActualWidth��� ���� $ Column20��$ ActualWidth��� ���� $ Column21��$ ActualWidth��� ���� $ Column22��$ ActualWidth��� ���� $ Column23��$ ActualWidth��� ���� $ Column24��$ ActualWidth��� ���� $ Column25��$ ActualWidth��� ���� $ Column26��$ ActualWidth��� ���� $ Column27��$ ActualWidth��� ���� $ Column28��$ ActualWidth��� ���� $ Column29��$ ActualWidth��� ���� $ Column30��$ ActualWidth��� ���� $ Column31��$ ActualWidth��� ���� $ Column32��$ ActualWidth��� ���� $ Column33��$ ActualWidth��� ���� $ Column34��$ ActualWidth��� ���� $ Column35��$ ActualWidth��� ���� $ Column36��$ ActualWidth��� ���� $ Column37��$ ActualWidth��� ���� $ Column38��$ ActualWidth�.+��� $��12��$��3��
���   �$��1,1,1,0q�._��� $��0,3,0,3q�$��TAIWAN�� �� TextAlignment$ Center=�$
��Bold"��� $��15��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��
LAMINATION��$ Center=�$
��Bold"��� $��18��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��CUTTING��$ Center=�$
��Bold"��� $��21��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��LEATHER��$ Center=�$
��Bold"��� $��24��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��SEMI-PROCESS��$ Center=�$
��Bold"��� $��27��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��SEWING��$ Center=�$
��Bold"��� $��30��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��OUTSOLE��$ Center=�$
��Bold"��� $��33��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��SECURITY LABEL��$ Center=�$
��Bold"��� $��36��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��ASSEMBLY��$ Center=�$
��Bold"��� $��39��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��SOCK LINING��$ Center=�$
��Bold"��� $��42��$��3��
���   �$��0,1,1,0q�._��� $��0,3,0,3q�$��CARTON��$ Center=�$
��Bold"�+   %MasterSchedule.Customs.CustomDataGrid  -     Name$ dgRawMaterial��+   %MasterSchedule.Customs.CustomDataGrid   CellEditEnding$" dgRawMaterial_CellEditEnding��   KeyDown$ dgRawMaterial_KeyDown��   MouseLeftButtonDown$' !dgRawMaterial_MouseLeftButtonDown��   SelectedCellsChanged$( "dgRawMaterial_SelectedCellsChanged��&   System.Windows.Controls.DataGrid   AutoGenerateColumns .  $��1��   RowHeaderWidth$ 10Q�   CanUserAddRows .     SelectionUnit$ CellOrRowHeader=�   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   FrozenColumnCount$ 10��   CanUserResizeRows .     HorizontalGridLinesBrush
 �����    VerticalGridLinesBrush
  �����! � 	Resources! *  $System.Windows.Controls.DataGridCell" �� 
TargetType# �� Triggers$ P� Property% P� Value& �� Property' �� Value��%j   '        �� ""  # P� $ �@ 
IsSelected% . . ��� & � ��
' ��iA��� & � ��
' �����( � ContextMenu( �� .@�m� -   $
 	miDisable��$��Disable��)   Columns) 0  *System.Windows.Controls.DataGridTextColumn  -   Column1��,  &System.Windows.Controls.DataGridColumn*   Header$* 	PROD.
NO.��+   Width5  /System.Windows.Controls.DataGridLengthConverter$
+ Auto ,   
FontWeight$
, Bold"�-   
IsReadOnly- . 1	  +System.Windows.Controls.DataGridBoundColumn. 	  Binding. �� *	ProductNo+/   	CellStyle/ �� ""  .��� & � ��' �� *ProductNoBackground+  -   Column2��$* COUNTRY��$
+ Auto - . . �� *	Country+  -	   Column3��$* STYLE��$
+ Auto - . . �� *
ShoeName+  -
   Column4��$* ARTICLE
NO.��$
+ Auto - . . �� *	ArticleNo+  -   Column5��$* 	PATT. NO.��$
+ Auto - . . �� *	PatternNo+  -   	Column5_1��$* OUTSOLE
CODE��$
+ Auto - . . �� *OutsoleCode+  -   Column6��$* QUANTITY��$
+ Auto - . . �� *
Quantity+  -   Column7��$	* EFD��$
+ Auto - . . �� *ETD+  -   	Column7_1��$* Cut A
Start
Date��- . . �� *CutAStartDate+0 �� StringFormat$	0 M/d��/ �� ""  .��� & � ��' �� *CutAStartDateForeground+  -   	Column7_2��$* Memo No.��$
+ Auto - . . �� *MemoId+  -   
Column10_1��$	* EFD��$
+ Auto - . . �� *
TAIWAN_ETD+1 �� UpdateSourceTrigger$1 PropertyChanged=�  -   
Column10_2��$* Actual
Date��$
+ Auto - . . �� *TAIWAN_ActualDate+$1 PropertyChanged=�  -   
Column10_3��$* REMARKS��$
+ Auto - . . �� *TAIWAN_Remarks+$1 PropertyChanged=�  -   Column8��$	* EFD��$
+ Auto - . . �� *LAMINATION_ETD+$1 PropertyChanged=�  -   Column9��$* Actual
Date��$
+ Auto - . . �� *LAMINATION_ActualDate+$1 PropertyChanged=�  -   Column10��$* REMARKS��$
+ Auto - . . �� *LAMINATION_Remarks+$1 PropertyChanged=�  -   Column11��$	* EFD��$
+ Auto - . . �� *CUTTING_ETD+$1 PropertyChanged=�  -   Column12��$* Actual
Date��$
+ Auto - . . �� *CUTTING_ActualDate+$1 PropertyChanged=�  -   Column13��$* REMARKS��$
+ Auto - . . �� *CUTTING_Remarks+$1 PropertyChanged=�  -   Column14��$	* EFD��$
+ Auto - . . �� *LEATHER_ETD+$1 PropertyChanged=�  -   Column15��$* Actual
Date��$
+ Auto - . . �� *LEATHER_ActualDate+$1 PropertyChanged=�  -   Column16��$* REMARKS��$
+ Auto - . . �� *LEATHER_Remarks+$1 PropertyChanged=�  -   Column17��$	* EFD��$
+ Auto - . . �� *SEMIPROCESS_ETD+$1 PropertyChanged=�  -   Column18��$* Actual
Date��$
+ Auto - . . �� *SEMIPROCESS_ActualDate+$1 PropertyChanged=�  -   Column19��$* REMARKS��$
+ Auto - . . �� *SEMIPROCESS_Remarks+$1 PropertyChanged=�  -    Column20��$	* EFD��$
+ Auto - . . �� *
SEWING_ETD+$1 PropertyChanged=�  -!   Column21��$* Actual
Date��$
+ Auto - . . �� *SEWING_ActualDate+$1 PropertyChanged=�  -"   Column22��$* REMARKS��$
+ Auto - . . �� *SEWING_Remarks+$1 PropertyChanged=�  -#   Column23��$	* EFD��$
+ Auto - . . �� *OUTSOLE_ETD+$1 PropertyChanged=�  -$   Column24��$* Actual
Date��$
+ Auto - . . �� *OUTSOLE_ActualDate+$1 PropertyChanged=�  -%   Column25��$* REMARKS��$
+ Auto - . . �� *OUTSOLE_Remarks+$1 PropertyChanged=�  -&   Column26��$	* EFD��$
+ Auto - . . �� *SECURITYLABEL_ETD+$1 PropertyChanged=�  -'   Column27��$* Actual
Date��$
+ Auto - . . �� *SECURITYLABEL_ActualDate+$1 PropertyChanged=�  -(   Column28��$* REMARKS��$
+ Auto - . . �� *SECURITYLABEL_Remarks+$1 PropertyChanged=�  -)   Column29��$	* EFD��$
+ Auto - . . �� *ASSEMBLY_ETD+$1 PropertyChanged=�  -*   Column30��$* Actual
Date��$
+ Auto - . . �� *ASSEMBLY_ActualDate+$1 PropertyChanged=�  -+   Column31��$* REMARKS��$
+ Auto - . . �� *ASSEMBLY_Remarks+$1 PropertyChanged=�  -,   Column32��$	* EFD��$
+ Auto - . . �� *SOCKLINING_ETD+$1 PropertyChanged=�  --   Column33��$* Actual
Date��$
+ Auto - . . �� *SOCKLINING_ActualDate+$1 PropertyChanged=�  -.   Column34��$* REMARKS��$
+ Auto - . . �� *SOCKLINING_Remarks+$1 PropertyChanged=�  -/   Column35��$	* EFD��$
+ Auto - . . �� *
CARTON_ETD+$1 PropertyChanged=�  -0   Column36��$* Actual
Date��$
+ Auto - . . �� *CARTON_ActualDate+$1 PropertyChanged=�  -1   Column37��$* REMARKS��$
+ Auto - . . �� *CARTON_Remarks+$1 PropertyChanged=�  -2   Column38��$* Loading
Date��- . . �� *LoadingDate+$1 PropertyChanged=�