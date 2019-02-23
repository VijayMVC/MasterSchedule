USE [SaovietMasterSchedule]
GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateProductionMemo_1]    Script Date: 04/28/2017 08:09:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateProductionMemo_1]
	@MemoId nvarchar(50),	
	@ProductionNumbers nvarchar(500),
	@Picture image,
	@Picture1 image,
	@Picture2 image,
	@Picture3 image,
	@Picture4 image
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE ProductionMemo
    SET ProductionNumbers = @ProductionNumbers, Picture = @Picture, Picture1 = @Picture1, Picture2 = @Picture2, Picture3 = @Picture3, Picture4 = @Picture4
    WHERE MemoId = @MemoId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertProductionMemo_1]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertProductionMemo_1]
	@SectionId nvarchar(50),
	@ProductionNumbers nvarchar(500),
	@Picture image,
	@Picture1 image,
	@Picture2 image,
	@Picture3 image,
	@Picture4 image
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @CHECK nvarchar(50)
    SELECT TOP 1 @CHECK = MemoId FROM ProductionMemo WHERE SectionId = @SectionId AND ProductionNumbers LIKE '%' + @ProductionNumbers + '%' AND IsVisible = 1
    DECLARE @MemoId nvarchar(50)
    IF(@CHECK IS NULL)
    BEGIN
		DECLARE @No int
		DECLARE @NoNext int
		SELECT TOP 1 @No = No FROM ProductionMemo WHERE SectionId = @SectionId ORDER BY CreatedTime DESC
		IF(@No IS NULL)
			SET @NoNext = 1
		ELSE
			SET @NoNext = @No + 1
		
		SET @MemoId = @SectionId + RIGHT('0000' + CONVERT(nvarchar(5), @NoNext), 5)
		
		INSERT INTO ProductionMemo(MemoId,No,SectionId,ProductionNumbers,Picture,Picture1,Picture2,Picture3,Picture4)
		VALUES (@MemoId,@NoNext,@SectionId,UPPER(@ProductionNumbers),@Picture,@Picture1,@Picture2,@Picture3,@Picture4)
	END
	ELSE
		SET @MemoId = ''
		
	SELECT 'MemoId' = @MemoId	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectProductionMemo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectProductionMemo]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MemoId,ProductionNumbers
	FROM ProductionMemo
	WHERE IsVisible = 1
	ORDER BY CreatedTime DESC
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectProductionMemoByProductionNumber]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectProductionMemoByProductionNumber]
	-- Add the parameters for the stored procedure here
	@ProductionNumber nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM ProductionMemo
	WHERE ProductionNumbers LIKE '%' + @ProductionNumber + '%' AND IsVisible = 1
	ORDER BY CreatedTime DESC
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectProductionMemoBySectionId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectProductionMemoBySectionId]
	-- Add the parameters for the stored procedure here
	@SectionId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MemoId,ProductionNumbers
	FROM ProductionMemo
	WHERE SectionId = @SectionId AND IsVisible = 1
	ORDER BY CreatedTime DESC
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteProductionMemo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteProductionMemo]
	@MemoId nvarchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE ProductionMemo
    SET IsVisible = 0
    WHERE MemoId = @MemoId AND IsVisible = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateProductionMemo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateProductionMemo]
	@MemoId nvarchar(50),	
	@ProductionNumbers nvarchar(500),
	@Picture image
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE ProductionMemo
    SET ProductionNumbers = @ProductionNumbers, Picture = @Picture
    WHERE MemoId = @MemoId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectProductionMemoByMemoId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectProductionMemoByMemoId]
	-- Add the parameters for the stored procedure here
	@MemoId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM ProductionMemo
	WHERE MemoId = @MemoId AND IsVisible = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertProductionMemo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertProductionMemo]
	@SectionId nvarchar(50),
	@ProductionNumbers nvarchar(500),
	@Picture image
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @CHECK nvarchar(50)
    SELECT TOP 1 @CHECK = MemoId FROM ProductionMemo WHERE SectionId = @SectionId AND ProductionNumbers LIKE '%' + @ProductionNumbers + '%' AND IsVisible = 1
    DECLARE @MemoId nvarchar(50)
    IF(@CHECK IS NULL)
    BEGIN
		DECLARE @No int
		DECLARE @NoNext int
		SELECT TOP 1 @No = No FROM ProductionMemo WHERE SectionId = @SectionId ORDER BY CreatedTime DESC
		IF(@No IS NULL)
			SET @NoNext = 1
		ELSE
			SET @NoNext = @No + 1
		
		SET @MemoId = @SectionId + RIGHT('0000' + CONVERT(nvarchar(5), @NoNext), 5)
		
		INSERT INTO ProductionMemo(MemoId,No,SectionId,ProductionNumbers,Picture)
		VALUES (@MemoId,@NoNext,@SectionId,UPPER(@ProductionNumbers),@Picture)
	END
	ELSE
		SET @MemoId = ''
		
	SELECT 'MemoId' = @MemoId	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSewingMasterCutAStartDate]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSewingMasterCutAStartDate]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ProductNo, CutAStartDate
	FROM SewingMaster
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyReleaseReportId_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyReleaseReportId_2]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT ReportId,ProductNo
	FROM AssemblyRelease	
	ORDER BY ReportId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOrderExtra]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOrderExtra]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@LoadingDate nvarchar(10)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		DECLARE @ID nvarchar(50)
		SELECT @ID = ProductNo FROM OrderExtra WHERE ProductNo = @ProductNo
		IF(@ID IS NULL)
			INSERT INTO OrderExtra(ProductNo,LoadingDate) VALUES (@ProductNo,@LoadingDate)
		ELSE
			UPDATE OrderExtra SET LoadingDate = @LoadingDate, ModifiedTime = GETDATE() WHERE ProductNo = @ID
	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrderExtra]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrderExtra]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OrderExtra
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialByOutsoleMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialByOutsoleMaster]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleReleaseMaterial
	WHERE ProductNo IN
	(	
		SELECT ProductNo
		FROM OutsoleMaster
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders WHERE IsEnable = 1
		)		
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialReportId_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialReportId_2]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT ReportId,ProductNo
	FROM OutsoleReleaseMaterial	
	ORDER BY ReportId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertRawMaterial_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertRawMaterial_2]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@MaterialTypeId int,
	@ETD date,
	@ActualDate date,
	@Remarks nvarchar(1000),
	
	@IsETDUpdate bit,
	@IsActualDateUpdate bit,
	@IsRemarksUpdate bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@IsETDUpdate = 1)
	BEGIN
		DECLARE @ID1 int
		SELECT @ID1 = RawMaterialId FROM RawMaterial WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
		IF(@ID1 IS NULL)
			INSERT INTO RawMaterial(ProductNo,MaterialTypeId,ETD) VALUES (@ProductNo,@MaterialTypeId,@ETD)
		ELSE
			UPDATE RawMaterial SET ETD = @ETD, ModifiedTime = GETDATE() WHERE RawMaterialId = @ID1
	END
	
	IF(@IsActualDateUpdate = 1)
	BEGIN
		DECLARE @ID2 int
		SELECT @ID2 = RawMaterialId FROM RawMaterial WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
		IF(@ID2 IS NULL)
			INSERT INTO RawMaterial(ProductNo,MaterialTypeId,ActualDate) VALUES (@ProductNo,@MaterialTypeId,@ActualDate)
		ELSE
			UPDATE RawMaterial SET ActualDate = @ActualDate, ModifiedTime = GETDATE() WHERE RawMaterialId = @ID2
	END
	
	IF(@IsRemarksUpdate = 1)
	BEGIN	
		UPDATE RawMaterial SET Remarks = @Remarks, ModifiedTime = GETDATE() WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spw_SelectSizeRunByProductNo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spw_SelectSizeRunByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SizeRun
	WHERE ProductNo = @ProductNo AND Quantity > 0
END

GO

/****** Object:  StoredProcedure [dbo].[spw_SelectOrdersByProductNo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spw_SelectOrdersByProductNo]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo = @ProductNo	
END

GO

/****** Object:  StoredProcedure [dbo].[spw_SelectOrders]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spw_SelectOrders]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE IsEnable = 1	
	ORDER BY CreatedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateOutsoleRawMaterialActualDate]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateOutsoleRawMaterialActualDate]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@OutsoleSupplierId int,
	@ActualDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = OutsoleRawMaterialId FROM OutsoleRawMaterial WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId
	IF(@ID IS NOT NULL)
		BEGIN
			UPDATE OutsoleRawMaterial SET ActualDate = @ActualDate WHERE OutsoleRawMaterialId = @ID
		END
	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByAssemblyMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByAssemblyMaster]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyMaster
	 )	 
	 AND IsEnable = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyReleaseByAssemblyMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyReleaseByAssemblyMaster]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM AssemblyRelease
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyMaster
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
		)		
	)	
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleOutputByAssemblyMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleOutputByAssemblyMaster]
	-- Add the parameters for the stored procedure here
	-- @ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleOutput
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyMaster
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
		)
	)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSewingOutputByAssemblyMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSewingOutputByAssemblyMaster]
	-- Add the parameters for the stored procedure here
	-- @ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SewingOutput
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyMaster
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
		)
	)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyReleaseReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyReleaseReportId]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ReportId
	FROM AssemblyRelease
	GROUP BY ReportId
	ORDER BY ReportId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialReportId]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ReportId
	FROM OutsoleReleaseMaterial
	GROUP BY ReportId
	ORDER BY ReportId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSizeRunByAssemblyReleaseByReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSizeRunByAssemblyReleaseByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SizeRun
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyRelease
		WHERE ReportId = @ReportId
	)	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleOutputByAssemblyReleaseByReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleOutputByAssemblyReleaseByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleOutput
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyRelease
		WHERE ReportId = @ReportId
	)	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSewingOutputByAssemblyReleaseByReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSewingOutputByAssemblyReleaseByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SewingOutput
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyRelease
		WHERE ReportId = @ReportId
	)	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyReleaseByReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyReleaseByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM AssemblyRelease
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyRelease
		WHERE ReportId = @ReportId
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByAssemblyReleaseByReportId]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByAssemblyReleaseByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM AssemblyRelease
		WHERE ReportId = @ReportId
	)
	--AND IsEnable = 1	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertAssemblyMaster_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertAssemblyMaster_2]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@Sequence int,
	@AssemblyLine nvarchar(50),
	@AssemblyStartDate date,
	@AssemblyFinishDate date,
	@AssemblyQuota int,
	@AssemblyActualStartDate nvarchar(50),
	@AssemblyActualFinishDate nvarchar(50),
	@AssemblyBalance nvarchar(100),
	
	@IsSequenceUpdate bit,
	@IsAssemblyLineUpdate bit,
	@IsAssemblyStartDateUpdate bit,
	@IsAssemblyFinishDateUpdate bit,
	@IsAssemblyQuotaUpdate bit,
	@IsAssemblyActualStartDateUpdate bit,
	@IsAssemblyActualFinishDateUpdate bit,
	@IsAssemblyBalanceUpdate bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = AssemblyMasterId FROM AssemblyMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO AssemblyMaster(ProductNo)
		VALUES (@ProductNo)
	END
	
	IF(@IsSequenceUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET Sequence = @Sequence
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyLineUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyLine = @AssemblyLine
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyStartDateUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyStartDate = @AssemblyStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyFinishDateUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyFinishDate = @AssemblyFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyQuotaUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyQuota = @AssemblyQuota
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyActualStartDateUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyActualStartDate = @AssemblyActualStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyActualFinishDateUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyActualFinishDate = @AssemblyActualFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsAssemblyBalanceUpdate = 1)
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyBalance = @AssemblyBalance
		WHERE ProductNo = @ProductNo
	END
	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSockliningMaster_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSockliningMaster_2]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,	
	@SockliningLine nvarchar(50),
	@SockliningStartDate date,
	@SockliningFinishDate date,
	@SockliningQuota int,
	@SockliningActualStartDate nvarchar(50),
	@SockliningActualFinishDate nvarchar(50),
	@InsoleBalance nvarchar(100),
	@InsockBalance nvarchar(100),
	
	@IsSequenceUpdate bit,
	@IsSockliningLineUpdate bit,
	@IsSockliningStartDateUpdate bit,
	@IsSockliningFinishDateUpdate bit,
	@IsSockliningQuotaUpdate bit,
	@IsSockliningActualStartDateUpdate bit,
	@IsSockliningActualFinishDateUpdate bit,
	@IsInsoleBalanceUpdate bit,
	@IsInsockBalanceUpdate bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SockliningMasterId FROM SockliningMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SockliningMaster(ProductNo)
		VALUES (@ProductNo)
	END
	
	IF(@IsSequenceUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET Sequence = @Sequence
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningLineUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningLine = @SockliningLine
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningStartDateUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningStartDate = @SockliningStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningFinishDateUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningFinishDate = @SockliningFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningQuotaUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningQuota = @SockliningQuota
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningActualStartDateUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningActualStartDate = @SockliningActualStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSockliningActualFinishDateUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET SockliningActualFinishDate  = @SockliningActualFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsInsoleBalanceUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET InsoleBalance = @InsoleBalance
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsInsockBalanceUpdate = 1)
	BEGIN
		UPDATE SockliningMaster
		SET InsockBalance = @InsockBalance
		WHERE ProductNo = @ProductNo
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleMaster_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleMaster_2]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@Sequence int,
	@OutsoleLine nvarchar(50),
	@OutsoleStartDate date,
	@OutsoleFinishDate date,
	@OutsoleQuota int,
	@OutsoleActualStartDate nvarchar(50),
	@OutsoleActualFinishDate nvarchar(50),
	@OutsoleBalance nvarchar(100),	
	
	@IsSequenceUpdate bit,
	@IsOutsoleLineUpdate bit,
	@IsOutsoleStartDateUpdate bit,
	@IsOutsoleFinishDateUpdate bit,
	@IsOutsoleQuotaUpdate bit,
	@IsOutsoleActualStartDateUpdate bit,
	@IsOutsoleActualFinishDateUpdate bit,
	@IsOutsoleBalanceUpdate bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = OutsoleMasterId FROM OutsoleMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO OutsoleMaster(ProductNo)
		VALUES (@ProductNo)
	END
	
	IF(@IsSequenceUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET Sequence = @Sequence
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleLineUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleLine = @OutsoleLine
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleStartDateUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleStartDate = @OutsoleStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleFinishDateUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleFinishDate = @OutsoleFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleQuotaUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleQuota = @OutsoleQuota
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleActualStartDateUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleActualStartDate = @OutsoleActualStartDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleActualFinishDateUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleActualFinishDate = @OutsoleActualFinishDate
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsOutsoleBalanceUpdate = 1)
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleBalance = @OutsoleBalance
		WHERE ProductNo = @ProductNo
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingMaster_2]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingMaster_2]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@SewingLine nvarchar(50),
	@SewingStartDate datetime,
	@SewingFinishDate datetime,
	@SewingQuota int,	
	@SewingActualStartDate nvarchar(50),
	@SewingActualFinishDate nvarchar(50),
	@SewingBalance nvarchar(100),
	@CutAStartDate datetime,
	@CutAFinishDate datetime, 
	@CutAQuota int,
	@CutAActualStartDate nvarchar(50),
	@CutAActualFinishDate nvarchar(50),
	@CutABalance nvarchar(100),
	@PrintingBalance nvarchar(100),
	@H_FBalance nvarchar(100),
	@EmbroideryBalance nvarchar(100),
	@CutBBalance nvarchar(100),	
	@AutoCut nvarchar(50),	
	
	@IsSequenceUpdate bit,
	@IsSewingLineUpdate bit,
	@IsSewingStartDateUpdate bit,
	@IsSewingFinishDateUpdate bit,
	@IsSewingQuotaUpdate bit,	
	@IsSewingActualStartDateUpdate bit,
	@IsSewingActualFinishDateUpdate bit,
	@IsSewingBalanceUpdate bit,
	@IsCutAStartDateUpdate bit,
	@IsCutAFinishDateUpdate bit, 
	@IsCutAQuotaUpdate bit,
	@IsCutAActualStartDateUpdate bit,
	@IsCutAActualFinishDateUpdate bit,
	@IsCutABalanceUpdate bit,
	@IsPrintingBalanceUpdate bit,
	@IsH_FBalanceUpdate bit,
	@IsEmbroideryBalanceUpdate bit,
	@IsCutBBalanceUpdate bit,
	@IsAutoCutUpdate bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SewingMasterId FROM SewingMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SewingMaster(ProductNo)
		VALUES (@ProductNo)
	END
	
	IF(@IsSequenceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET Sequence = @Sequence
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsSewingLineUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingLine = @SewingLine
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsSewingStartDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingStartDate = @SewingStartDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsSewingFinishDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingFinishDate = @SewingFinishDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsSewingQuotaUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingQuota = @SewingQuota
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsSewingActualStartDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingActualStartDate = @SewingActualStartDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsSewingActualFinishDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingActualFinishDate = @SewingActualFinishDate
		WHERE ProductNo = @ProductNo
	END		
	
	IF(@IsSewingBalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET SewingBalance = @SewingBalance
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAStartDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAStartDate = @CutAStartDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAFinishDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAFinishDate = @CutAFinishDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAQuotaUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAQuota = @CutAQuota
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAQuotaUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAQuota = @CutAQuota
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAActualStartDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAActualStartDate = @CutAActualStartDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutAActualFinishDateUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutAActualFinishDate = @CutAActualFinishDate
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsCutABalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutABalance = @CutABalance
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsPrintingBalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET PrintingBalance = @PrintingBalance
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsH_FBalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET H_FBalance = @H_FBalance
		WHERE ProductNo = @ProductNo
	END	
	
	IF(@IsEmbroideryBalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET EmbroideryBalance = @EmbroideryBalance
		WHERE ProductNo = @ProductNo
	END
	
	IF(@IsCutBBalanceUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET CutBBalance = @CutBBalance
		WHERE ProductNo = @ProductNo
	END
	IF(@IsAutoCutUpdate = 1)
	BEGIN
		UPDATE SewingMaster
		SET AutoCut = @AutoCut
		WHERE ProductNo = @ProductNo
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertAssemblyRelease]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertAssemblyRelease]
	-- Add the parameters for the stored procedure here
	@ReportId nvarchar(50),
	@ProductNo nvarchar(50),
	@Cycle int,
	@SizeNo nvarchar(10),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
	SELECT @ID = AssemblyReleaseId FROM AssemblyRelease
	WHERE ProductNo = @ProductNo AND Cycle = @Cycle AND SizeNo = @SizeNo AND ReportId = @ReportId
	IF(@ID IS NULL AND @Quantity > 0)
		BEGIN
			INSERT INTO AssemblyRelease(ReportId,ProductNo,Cycle,SizeNo,Quantity)
			VALUES (@ReportId,@ProductNo,@Cycle,@SizeNo,@Quantity)
		END
	ELSE
		BEGIN
			IF(@Quantity <= 0)
				DELETE AssemblyRelease WHERE AssemblyReleaseId = @ID
			ELSE
				UPDATE AssemblyRelease SET Quantity = @Quantity, ModifiedTime = GETDATE() WHERE AssemblyReleaseId = @ID
		END
		
	--UPDATE Orders
	--SET IsEnable = 0
	--WHERE ProductNo = @ProductNo
	--AND Quantity <=
	--(
		--SELECT SUM(Quantity)
		--FROM AssemblyRelease
		--WHERE ProductNo = Orders.ProductNo
		--GROUP BY ProductNo
	--)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteAssemblyReleaseByReportIdProductNo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteAssemblyReleaseByReportIdProductNo]
	-- Add the parameters for the stored procedure here
	@ReportId nvarchar(50),
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE AssemblyRelease
	WHERE ReportId = @ReportId AND ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyReleaseByProductNo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyReleaseByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM AssemblyRelease
	WHERE ProductNo = @ProductNo
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertAssemblyMasterAssembly]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertAssemblyMasterAssembly]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@AssemblyLine nvarchar(50),
	@AssemblyStartDate date,
	@AssemblyFinishDate date,
	@AssemblyQuota int,
	@AssemblyActualStartDate nvarchar(50),
	@AssemblyActualFinishDate nvarchar(50),
	@AssemblyBalance nvarchar(100)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = AssemblyMasterId FROM AssemblyMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO AssemblyMaster(ProductNo,AssemblyLine,AssemblyStartDate,AssemblyFinishDate,AssemblyQuota,AssemblyActualStartDate,AssemblyActualFinishDate,AssemblyBalance)
		VALUES (@ProductNo,@AssemblyLine,@AssemblyStartDate,@AssemblyFinishDate,@AssemblyQuota,@AssemblyActualStartDate,@AssemblyActualFinishDate,@AssemblyBalance)
	END
	ELSE
	BEGIN
		UPDATE AssemblyMaster
		SET AssemblyLine = @AssemblyLine,AssemblyStartDate = @AssemblyStartDate,AssemblyFinishDate = @AssemblyFinishDate,AssemblyQuota = @AssemblyQuota,AssemblyActualStartDate = @AssemblyActualStartDate,AssemblyActualFinishDate = @AssemblyActualFinishDate,AssemblyBalance=@AssemblyBalance
		WHERE AssemblyMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertAssemblyMasterSequence]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertAssemblyMasterSequence]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@AssemblyStartDate date,
	@AssemblyFinishDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = AssemblyMasterId FROM AssemblyMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO AssemblyMaster(ProductNo,Sequence,AssemblyStartDate,AssemblyFinishDate)
		VALUES (@ProductNo,@Sequence,@AssemblyStartDate,@AssemblyFinishDate)
	END
	ELSE
	BEGIN
		UPDATE AssemblyMaster		
		SET Sequence = @Sequence, AssemblyStartDate = @AssemblyStartDate, AssemblyFinishDate = @AssemblyFinishDate
		WHERE AssemblyMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAssemblyMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAssemblyMaster]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM AssemblyMaster
	WHERE ProductNo IN
	(
		SELECT ProductNo 
		FROM Orders 
		WHERE IsEnable = 1		
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSockliningMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSockliningMaster]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SockliningMaster
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSockliningMasterSocklining]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSockliningMasterSocklining]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@SockliningLine nvarchar(50),
	@SockliningStartDate date,
	@SockliningFinishDate date,
	@SockliningQuota int,
	@SockliningActualStartDate nvarchar(50),
	@SockliningActualFinishDate nvarchar(50),
	@InsoleBalance nvarchar(100),
	@InsockBalance nvarchar(100)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SockliningMasterId FROM SockliningMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SockliningMaster(ProductNo,SockliningLine,SockliningStartDate,SockliningFinishDate,SockliningQuota,SockliningActualStartDate,SockliningActualFinishDate,InsoleBalance,InsockBalance)
		VALUES (@ProductNo,@SockliningLine,@SockliningStartDate,@SockliningFinishDate,@SockliningQuota,@SockliningActualStartDate,@SockliningActualFinishDate,@InsoleBalance,@InsockBalance)
	END
	ELSE
	BEGIN
		UPDATE SockliningMaster
		SET SockliningLine = @SockliningLine,SockliningStartDate = @SockliningStartDate,SockliningFinishDate = @SockliningFinishDate,SockliningQuota = @SockliningQuota,SockliningActualStartDate = @SockliningActualStartDate,SockliningActualFinishDate = @SockliningActualFinishDate,InsoleBalance=@InsoleBalance,InsockBalance=@InsockBalance
		WHERE SockliningMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSockliningMasterSequence]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSockliningMasterSequence]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@SockliningStartDate date,
	@SockliningFinishDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SockliningMasterId FROM SockliningMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SockliningMaster(ProductNo,Sequence,SockliningStartDate,SockliningFinishDate)
		VALUES (@ProductNo,@Sequence,@SockliningStartDate,@SockliningFinishDate)
	END
	ELSE
	BEGIN
		UPDATE SockliningMaster		
		SET Sequence = @Sequence, SockliningStartDate = @SockliningStartDate, SockliningFinishDate = @SockliningFinishDate
		WHERE SockliningMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleOutputByProductNo]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleOutputByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleOutput
	WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleOutput]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleOutput]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@SizeNo nvarchar(10),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = OutsoleOutputId FROM OutsoleOutput WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo
	IF(@ID IS NULL)
		BEGIN
			INSERT INTO OutsoleOutput(ProductNo,SizeNo,Quantity)
			VALUES (@ProductNo,@SizeNo,@Quantity)
		END
	ELSE
		UPDATE OutsoleOutput SET Quantity = @Quantity, ModifiedTime = GETDATE() WHERE OutsoleOutputId = @ID
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleMasterOutsole]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleMasterOutsole]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@OutsoleLine nvarchar(50),
	@OutsoleStartDate date,
	@OutsoleFinishDate date,
	@OutsoleQuota int,
	@OutsoleActualStartDate nvarchar(50),
	@OutsoleActualFinishDate nvarchar(50),
	@OutsoleBalance nvarchar(100)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = OutsoleMasterId FROM OutsoleMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO OutsoleMaster(ProductNo,OutsoleLine,OutsoleStartDate,OutsoleFinishDate,OutsoleQuota,OutsoleActualStartDate,OutsoleActualFinishDate,OutsoleBalance)
		VALUES (@ProductNo,@OutsoleLine,@OutsoleStartDate,@OutsoleFinishDate,@OutsoleQuota,@OutsoleActualStartDate,@OutsoleActualFinishDate,@OutsoleBalance)
	END
	ELSE
	BEGIN
		UPDATE OutsoleMaster
		SET OutsoleLine = @OutsoleLine,OutsoleStartDate = @OutsoleStartDate,OutsoleFinishDate = @OutsoleFinishDate,OutsoleQuota = @OutsoleQuota,OutsoleActualStartDate = @OutsoleActualStartDate,OutsoleActualFinishDate = @OutsoleActualFinishDate,OutsoleBalance=@OutsoleBalance
		WHERE OutsoleMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleMasterSequence]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleMasterSequence]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@OutsoleStartDate date,
	@OutsoleFinishDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = OutsoleMasterId FROM OutsoleMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO OutsoleMaster(ProductNo,Sequence,OutsoleStartDate,OutsoleFinishDate)
		VALUES (@ProductNo,@Sequence,@OutsoleStartDate,@OutsoleFinishDate)
	END
	ELSE
	BEGIN
		UPDATE OutsoleMaster		
		SET Sequence = @Sequence, OutsoleStartDate = @OutsoleStartDate, OutsoleFinishDate = @OutsoleFinishDate
		WHERE OutsoleMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaster]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaster]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaster
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingMasterCutPrep]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingMasterCutPrep]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@CutAStartDate date,
	@CutAFinishDate date,
	@CutAQuota int,
	@CutAActualStartDate nvarchar(50),
	@CutAActualFinishDate nvarchar(50),
	@CutABalance nvarchar(100),
	@PrintingBalance nvarchar(100),
	@H_FBalance nvarchar(100),
	@EmbroideryBalance nvarchar(100),
	@CutBBalance nvarchar(100),
	@AutoCut nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SewingMasterId FROM SewingMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SewingMaster(ProductNo,CutAStartDate,CutAFinishDate,CutAQuota,CutAActualStartDate,CutAActualFinishDate,CutABalance,PrintingBalance,H_FBalance,EmbroideryBalance,CutBBalance,AutoCut)
		VALUES (@ProductNo,@CutAStartDate,@CutAFinishDate,@CutAQuota,@CutAActualStartDate,@CutAActualFinishDate,@CutABalance,@PrintingBalance,@H_FBalance,@EmbroideryBalance,@CutBBalance,@AutoCut)
	END
	ELSE
	BEGIN
		UPDATE SewingMaster
		SET CutAStartDate=@CutAStartDate,CutAFinishDate=@CutAFinishDate,CutAQuota=@CutAQuota,CutAActualStartDate=@CutAActualStartDate,CutAActualFinishDate=@CutAActualFinishDate,CutABalance=@CutABalance,PrintingBalance=@PrintingBalance,H_FBalance=@H_FBalance,EmbroideryBalance=@EmbroideryBalance,CutBBalance=@CutBBalance,AutoCut=@AutoCut
		WHERE SewingMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingMasterSewing]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingMasterSewing]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@SewingLine nvarchar(50),
	@SewingStartDate date,
	@SewingFinishDate date,
	@SewingQuota int,
	@SewingActualStartDate nvarchar(50),
	@SewingActualFinishDate nvarchar(50),
	@SewingBalance nvarchar(100)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SewingMasterId FROM SewingMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SewingMaster(ProductNo,SewingLine,SewingStartDate,SewingFinishDate,SewingQuota,SewingActualStartDate,SewingActualFinishDate,SewingBalance)
		VALUES (@ProductNo,@SewingLine,@SewingStartDate,@SewingFinishDate,@SewingQuota,@SewingActualStartDate,@SewingActualFinishDate,@SewingBalance)
	END
	ELSE
	BEGIN
		UPDATE SewingMaster
		SET SewingLine = @SewingLine,SewingStartDate = @SewingStartDate,SewingFinishDate = @SewingFinishDate,SewingQuota = @SewingQuota,SewingActualStartDate = @SewingActualStartDate,SewingActualFinishDate = @SewingActualFinishDate,SewingBalance=@SewingBalance
		WHERE SewingMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingMasterSequence]    Script Date: 04/28/2017 08:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingMasterSequence]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@SewingStartDate date,
	@SewingFinishDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SewingMasterId FROM SewingMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SewingMaster(ProductNo,Sequence,SewingStartDate,SewingFinishDate)
		VALUES (@ProductNo,@Sequence,@SewingStartDate,@SewingFinishDate)
	END
	ELSE
	BEGIN
		UPDATE SewingMaster		
		SET Sequence = @Sequence, SewingStartDate = @SewingStartDate, SewingFinishDate = @SewingFinishDate
		WHERE SewingMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByOutsoleMaterialReject]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByOutsoleMaterialReject]
	-- Add the parameters for the stored procedure here
	-- @ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(	
		SELECT ProductNo
		FROM OutsoleMaterial
		WHERE QuantityReject > 0
	)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingMaster]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingMaster]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@Sequence int,
	@SewingLine nvarchar(50),
	@SewingQuota int,
	@SewingActualStartDate nvarchar(50),
	@SewingActualFinishDate nvarchar(50),
	@SewingBalance nvarchar(100),
	@CutAQuota int,
	@CutAActualStartDate nvarchar(50),
	@CutAActualFinishDate nvarchar(50),
	@CutABalance nvarchar(100),
	@PrintingBalance nvarchar(100),
	@H_FBalance nvarchar(100),
	@EmbroideryBalance nvarchar(100),
	@CutBBalance nvarchar(100),
	@AutoCut nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
    SELECT @ID = SewingMasterId FROM SewingMaster WHERE ProductNo = @ProductNo
	IF(@ID IS NULL)
	BEGIN
		INSERT INTO SewingMaster(ProductNo,Sequence,SewingLine,SewingQuota,SewingActualStartDate,SewingActualFinishDate,SewingBalance,CutAQuota,CutAActualStartDate,CutAActualFinishDate,CutABalance,PrintingBalance,H_FBalance,EmbroideryBalance,CutBBalance,AutoCut)
		VALUES (@ProductNo,@Sequence,@SewingLine,@SewingQuota,@SewingActualStartDate,@SewingActualFinishDate,@SewingBalance,@CutAQuota,@CutAActualStartDate,@CutAActualFinishDate,@CutABalance,@PrintingBalance,@H_FBalance,@EmbroideryBalance,@CutBBalance,@AutoCut)
	END
	ELSE
	BEGIN
		UPDATE SewingMaster
		SET Sequence = @Sequence,SewingLine = @SewingLine,SewingQuota = @SewingQuota,SewingActualStartDate = @SewingActualStartDate,SewingActualFinishDate = @SewingActualFinishDate,SewingBalance=@SewingBalance,CutAQuota=@CutAQuota,CutAActualStartDate=@CutAActualStartDate,CutAActualFinishDate=@CutAActualFinishDate,CutABalance=@CutABalance,PrintingBalance=@PrintingBalance,H_FBalance=@H_FBalance,EmbroideryBalance=@EmbroideryBalance,CutBBalance=@CutBBalance,AutoCut=@AutoCut
		--SET Sequence = @Sequence
		WHERE SewingMasterId = @ID
	END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaterialReject]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaterialReject]
	-- Add the parameters for the stored procedure here
	-- @ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaterial
	WHERE QuantityReject > 0
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleMaterial_1]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleMaterial_1]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@OutsoleSupplierId int,
	@SizeNo nvarchar(10),
	@Quantity int,
	@QuantityReject int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = OutsoleMaterialId FROM OutsoleMaterial WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId AND SizeNo = @SizeNo
	IF(@ID IS NULL)
		BEGIN
			INSERT INTO OutsoleMaterial(ProductNo,OutsoleSupplierId,SizeNo,Quantity,QuantityReject)
			VALUES (@ProductNo,@OutsoleSupplierId,@SizeNo,@Quantity,@QuantityReject)
		END
	ELSE
		UPDATE OutsoleMaterial SET Quantity = @Quantity, QuantityReject = @QuantityReject, ModifiedTime = GETDATE() WHERE OutsoleMaterialId = @ID
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSewingMaster]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSewingMaster]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SewingMaster
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSewingOutput]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSewingOutput]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),	
	@SizeNo nvarchar(10),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = SewingOutputId FROM SewingOutput WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo
	IF(@ID IS NULL)
		BEGIN
			INSERT INTO SewingOutput(ProductNo,SizeNo,Quantity)
			VALUES (@ProductNo,@SizeNo,@Quantity)
		END
	ELSE
		UPDATE SewingOutput SET Quantity = @Quantity, ModifiedTime = GETDATE() WHERE SewingOutputId = @ID
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSewingOutputByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSewingOutputByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SewingOutput
	WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersBySewingMaster]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersBySewingMaster] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @C int
    SELECT @C = COUNT(SewingMasterId) FROM SewingMaster
    IF(@C = 0)
		SELECT * FROM Orders WHERE IsEnable = 1 ORDER BY ModifiedTime
	ELSE
		BEGIN
			SELECT Orders.* FROM Orders, SewingMaster
			WHERE Orders.ProductNo = SewingMaster.ProductNo AND IsEnable = 1
			ORDER BY Sequence
		END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectReportId]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectReportId]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ReportId
	FROM OutsoleReleaseMaterial
	GROUP BY ReportId
	ORDER BY ReportId
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteOffDay]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteOffDay]
	-- Add the parameters for the stored procedure here
	@Date datetime	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE OffDay WHERE CONVERT(date, Date) = CONVERT(date, @Date)    
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOffDay]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOffDay]
	-- Add the parameters for the stored procedure here
	@Date datetime,
	@Remarks nvarchar(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = OffDayId FROM OffDay WHERE CONVERT(date, Date) = CONVERT(date, @Date)
	IF(@ID IS NULL)
		BEGIN
			INSERT INTO OffDay(Date,Remarks)
			VALUES (CONVERT(date,@Date),UPPER(@Remarks))
		END
	ELSE
		EXEC spm_UpdateOffDay @Date,@Remarks
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateOffDay]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateOffDay]
	-- Add the parameters for the stored procedure here
	@Date datetime,
	@Remarks nvarchar(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here   
		UPDATE OffDay
		SET Remarks = UPPER(@Remarks)
		WHERE CONVERT(date, Date) = CONVERT(date, @Date)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOffDay]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOffDay]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OffDay
	WHERE YEAR(Date) = YEAR(GETDATE()) OR YEAR(Date) = YEAR(GETDATE()) + 1
	ORDER BY Date DESC
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@OutsoleSupplierId int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE OutsoleRawMaterial
	WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId
	DELETE OutsoleMaterial
	WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId
	
	DECLARE @ETDLast datetime
	SELECT TOP 1 @ETDLast = ETD
	FROM OutsoleRawMaterial
	WHERE ProductNo = @ProductNo
	ORDER BY ETD DESC
	IF(@ETDLast IS NULL)
		SET @ETDLast = '2000-1-1'    
	EXEC spm_InsertRawMaterial_2 @ProductNo, 6, @ETDLast, '2000-1-1', '', 1, 0, 0
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateOrdersOfIsEnable]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateOrdersOfIsEnable]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@IsEnable bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here   
		UPDATE Orders
		SET IsEnable = @IsEnable, ModifiedTime = GETDATE()
		WHERE ProductNo = @ProductNo	
		
		UPDATE ProductionMemo
		SET IsVisible = @IsEnable
		WHERE ProductionNumbers LIKE '%' + @ProductNo + '%'
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialByOutsoleMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialByOutsoleMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleReleaseMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleMaterial
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
			--AND  Quantity >
			--(
			--SELECT SUM(Quantity) 
			--FROM OutsoleReleaseMaterial
			--WHERE ProductNo = Orders.ProductNo
			--GROUP BY ProductNo
			--)
		)		
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByOutsoleMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByOutsoleMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleMaterial
	 )
	 --AND  Quantity >
	--(
		--SELECT SUM(Quantity) 
		--FROM OutsoleReleaseMaterial
		--WHERE ProductNo = Orders.ProductNo
		--GROUP BY ProductNo
	--)
	 AND IsEnable = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaterial]
	-- Add the parameters for the stored procedure here
	-- @ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo 
		FROM Orders
		WHERE IsEnable = 1
		--AND  Quantity >
		--(
		--SELECT SUM(Quantity) 
		--FROM OutsoleReleaseMaterial
		--WHERE ProductNo = Orders.ProductNo
		--GROUP BY ProductNo
		--)
	)
END
GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaterialByOutsoleReleaseMaterialByReportId]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaterialByOutsoleReleaseMaterialByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleReleaseMaterial
		WHERE ReportId = @ReportId
	)	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSizeRunByOutsoleReleaseMaterialByReportId]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSizeRunByOutsoleReleaseMaterialByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SizeRun
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleReleaseMaterial
		WHERE ReportId = @ReportId
	)	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByOutsoleReleaseMaterialByReportId]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByOutsoleReleaseMaterialByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleReleaseMaterial
		WHERE ReportId = @ReportId
	)
	--AND IsEnable = 1	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialByReportId]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialByReportId]
	-- Add the parameters for the stored procedure here	
	@ReportId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleReleaseMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleReleaseMaterial
		WHERE ReportId = @ReportId
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteOutsoleReleaseMaterialByReportIdProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteOutsoleReleaseMaterialByReportIdProductNo]
	-- Add the parameters for the stored procedure here
	@ReportId nvarchar(50),
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE OutsoleReleaseMaterial
	WHERE ReportId = @ReportId AND ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleReleaseMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleReleaseMaterial]
	-- Add the parameters for the stored procedure here
	@ReportId nvarchar(50),
	@ProductNo nvarchar(50),
	@Cycle int,
	@SizeNo nvarchar(10),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID int
	SELECT @ID = OutsoleReleaseMaterialId FROM OutsoleReleaseMaterial
	WHERE ProductNo = @ProductNo AND Cycle = @Cycle AND SizeNo = @SizeNo AND ReportId = @ReportId
	IF(@ID IS NULL AND @Quantity > 0)
		BEGIN
			INSERT INTO OutsoleReleaseMaterial(ReportId,ProductNo,Cycle,SizeNo,Quantity)
			VALUES (@ReportId,@ProductNo,@Cycle,@SizeNo,@Quantity)
		END
	ELSE
		BEGIN
			IF(@Quantity <= 0)
				DELETE OutsoleReleaseMaterial WHERE OutsoleReleaseMaterialId = @ID
			ELSE
				UPDATE OutsoleReleaseMaterial SET Quantity = @Quantity, ModifiedTime = GETDATE() WHERE OutsoleReleaseMaterialId = @ID
		END
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleReleaseMaterialByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleReleaseMaterialByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleReleaseMaterial
	WHERE ProductNo = @ProductNo
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectAccountByUserNamePassword]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectAccountByUserNamePassword]
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50),
	@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 *
	FROM Account
	WHERE UserName = @UserName AND Password = @Password AND IsEnable = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleRawMaterial
	 )
	 AND IsEnable = 1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaterialByOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaterialByOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleRawMaterial
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
		)
	 )
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSizeRunByOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSizeRunByOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SizeRun
	WHERE ProductNo IN
	(
		SELECT ProductNo
		FROM OutsoleRawMaterial
		WHERE ProductNo IN
		(
			SELECT ProductNo FROM Orders
			WHERE IsEnable = 1
		)
	 ) AND Quantity > 0
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleRawMaterial	
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders
		WHERE IsEnable = 1
	)
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleRawMaterialByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleRawMaterialByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleRawMaterial
	WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleRawMaterial]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@OutsoleSupplierId int,
	@ETD date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here    
	DECLARE @ID int
	SELECT @ID = OutsoleRawMaterialId FROM OutsoleRawMaterial WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId
	IF(@ID IS NULL)
		INSERT INTO OutsoleRawMaterial(ProductNo,OutsoleSupplierId,ETD)
		VALUES (@ProductNo, @OutsoleSupplierId, @ETD)
	ELSE
		UPDATE OutsoleRawMaterial SET ETD = @ETD, ModifiedTime = GETDATE() WHERE OutsoleRawMaterialId = @ID
	
	-- Auto add SizeRun for new OutsoleSupplier:
	DECLARE @CountSizeRun int
	SELECT @CountSizeRun = COUNT(SizeNo) FROM OutsoleMaterial WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId
	IF(@CountSizeRun <= 0)
	BEGIN
		INSERT INTO OutsoleMaterial(ProductNo,OutsoleSupplierId,SizeNo,Quantity)
		SELECT 'ProductNo' = @ProductNo, 'OutsoleSupplierId' = @OutsoleSupplierId, SizeNo, 'Quantity' = 0 
		FROM SizeRun 
		WHERE ProductNo = @ProductNo
	END
	-- End.	
	
	DECLARE @ETDLast datetime
	SELECT TOP 1 @ETDLast = ETD
	FROM OutsoleRawMaterial
	WHERE ProductNo = @ProductNo
	ORDER BY ETD DESC
	IF(@ETDLast IS NULL)
		SET @ETDLast = '2000-1-1'    
	EXEC spm_InsertRawMaterial_2 @ProductNo, 6, @ETDLast, '2000-1-1', '', 1, 0, 0
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectMaterialType]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectMaterialType]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM MaterialType
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectSizeRunByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectSizeRunByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM SizeRun
	WHERE ProductNo = @ProductNo AND Quantity > 0
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleSuppliers]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleSuppliers]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleSuppliers
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOutsoleMaterialByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOutsoleMaterialByProductNo]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM OutsoleMaterial
	WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOutsoleMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOutsoleMaterial]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@OutsoleSupplierId int,
	@SizeNo nvarchar(10),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = OutsoleMaterialId FROM OutsoleMaterial WHERE ProductNo = @ProductNo AND OutsoleSupplierId = @OutsoleSupplierId AND SizeNo = @SizeNo
	IF(@ID IS NULL)
		BEGIN
			INSERT INTO OutsoleMaterial(ProductNo,OutsoleSupplierId,SizeNo,Quantity)
			VALUES (@ProductNo,@OutsoleSupplierId,@SizeNo,@Quantity)
		END
	ELSE
		UPDATE OutsoleMaterial SET Quantity = @Quantity, ModifiedTime = GETDATE() WHERE OutsoleMaterialId = @ID
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectRawMaterial]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM RawMaterial
	WHERE ProductNo IN
	(
		SELECT ProductNo FROM Orders WHERE IsEnable = 1
	)
	ORDER BY ModifiedTime
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertRawMaterial]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertRawMaterial]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@MaterialTypeId int,
	@ETD date,
	@ActualDate date,
	@Remarks nvarchar(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(CONVERT(date, @ETD) != '1999-12-31')
	BEGIN
		DECLARE @ID1 int
		SELECT @ID1 = RawMaterialId FROM RawMaterial WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
		IF(@ID1 IS NULL)
			INSERT INTO RawMaterial(ProductNo,MaterialTypeId,ETD) VALUES (@ProductNo,@MaterialTypeId,@ETD)
		ELSE
			UPDATE RawMaterial SET ETD = @ETD, ModifiedTime = GETDATE() WHERE RawMaterialId = @ID1
	END
	
	IF(CONVERT(date, @ActualDate) != '1999-12-31')
	BEGIN
		DECLARE @ID2 int
		SELECT @ID2 = RawMaterialId FROM RawMaterial WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
		IF(@ID2 IS NULL)
			INSERT INTO RawMaterial(ProductNo,MaterialTypeId,ActualDate) VALUES (@ProductNo,@MaterialTypeId,@ActualDate)
		ELSE
			UPDATE RawMaterial SET ActualDate = @ActualDate, ModifiedTime = GETDATE() WHERE RawMaterialId = @ID2
	END
		
	UPDATE RawMaterial SET Remarks = @Remarks, ModifiedTime = GETDATE() WHERE ProductNo = @ProductNo AND MaterialTypeId = @MaterialTypeId
	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateSizeRun]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateSizeRun]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@SizeNo nvarchar(10),
	@Quantity int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF(@Quantity > 0)
    BEGIN	
		UPDATE SizeRun
		SET Quantity = @Quantity, ModifiedTime = GETDATE()
		WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo	
	END
	ELSE
	BEGIN		
		DELETE SizeRun WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo
		DELETE OutsoleMaterial WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo 
		DELETE SewingOutput WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo  
		DELETE OutsoleOutput WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo		   
		DELETE OutsoleReleaseMaterial WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo
		DELETE AssemblyRelease WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo	
	END
		
	UPDATE Orders
	SET Quantity = (SELECT SUM(Quantity) FROM SizeRun WHERE ProductNo = @ProductNo)	
	WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertSizeRun]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertSizeRun]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@SizeNo nvarchar(10),
	@Quantity int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ID int
	SELECT @ID = SizeRunId FROM SizeRun WHERE ProductNo = @ProductNo AND SizeNo = @SizeNo
	IF(@ID IS NULL)
	BEGIN
		IF(@Quantity > 0)
		BEGIN
			INSERT INTO SizeRun(ProductNo,SizeNo,Quantity)
			VALUES (UPPER(@ProductNo),UPPER(@SizeNo),@Quantity)
		END
	END
	ELSE
		EXEC spm_UpdateSizeRun @ProductNo,@SizeNo,@Quantity
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrdersByProductNo]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrdersByProductNo]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
	@ProductNo nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 *
	FROM Orders
	WHERE ProductNo = @ProductNo	
END

GO

/****** Object:  StoredProcedure [dbo].[spm_DeleteOrders]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_DeleteOrders]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE Orders WHERE ProductNo = @ProductNo
    DELETE SizeRun WHERE ProductNo = @ProductNo  
    DELETE RawMaterial WHERE ProductNo = @ProductNo
    DELETE OutsoleRawMaterial WHERE ProductNo = @ProductNo
    DELETE OutsoleMaterial WHERE ProductNo = @ProductNo 
    DELETE SewingOutput WHERE ProductNo = @ProductNo  
    DELETE OutsoleOutput WHERE ProductNo = @ProductNo 
    DELETE OutsoleMaster WHERE ProductNo = @ProductNo    
    DELETE OutsoleReleaseMaterial WHERE ProductNo = @ProductNo 
    DELETE SockliningMaster WHERE ProductNo = @ProductNo
    DELETE SewingMaster WHERE ProductNo = @ProductNo 
    DELETE AssemblyMaster WHERE ProductNo = @ProductNo 
    DELETE AssemblyRelease WHERE ProductNo = @ProductNo   
    DELETE OrderExtra WHERE ProductNo = @ProductNo
END

GO

/****** Object:  StoredProcedure [dbo].[spm_UpdateOrders]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_UpdateOrders]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@ETD datetime,	
	@ArticleNo nvarchar(50),
	@ShoeName nvarchar(100),
	@Quantity int,	
	@PatternNo nvarchar(50),
	@MidsoleCode nvarchar(50),
	@OutsoleCode nvarchar(50),
	@LastCode nvarchar(50),
	@Country nvarchar(100),
	@IsEnable bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here   
		UPDATE Orders
		SET ETD = CONVERT(date,@ETD),ArticleNo = @ArticleNo,ShoeName = UPPER(@ShoeName),Quantity = @Quantity,PatternNo = @PatternNo,MidsoleCode = @MidsoleCode,OutsoleCode = @OutsoleCode,LastCode = @LastCode,Country = @Country, IsEnable = @IsEnable, ModifiedTime = GETDATE()
		WHERE ProductNo = @ProductNo	
		
		UPDATE ProductionMemo
		SET IsVisible = @IsEnable
		WHERE ProductionNumbers LIKE '%' + @ProductNo + '%'
END

GO

/****** Object:  StoredProcedure [dbo].[spm_InsertOrders]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_InsertOrders]
	-- Add the parameters for the stored procedure here
	@ProductNo nvarchar(50),
	@ETD datetime,	
	@ArticleNo nvarchar(50),
	@ShoeName nvarchar(100),
	@Quantity int,	
	@PatternNo nvarchar(50),
	@MidsoleCode nvarchar(50),
	@OutsoleCode nvarchar(50),
	@LastCode nvarchar(50),
	@Country nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ID nvarchar(50)
    SELECT @ID = ProductNo FROM Orders WHERE ProductNo = @ProductNo
    
    IF(@ID IS NULL)
    BEGIN    
		INSERT INTO Orders(ProductNo,ETD,ArticleNo,ShoeName,Quantity,PatternNo,MidsoleCode,OutsoleCode,LastCode,Country)
		VALUES (@ProductNo,CONVERT(date,@ETD),@ArticleNo,UPPER(@ShoeName),@Quantity,@PatternNo,@MidsoleCode,@OutsoleCode,@LastCode,@Country)
	END
	ELSE
		EXEC spm_UpdateOrders @ProductNo,@ETD,@ArticleNo,@ShoeName,@Quantity,@PatternNo,@MidsoleCode,@OutsoleCode,@LastCode,@Country,1
END

GO

/****** Object:  StoredProcedure [dbo].[spm_SelectOrders]    Script Date: 04/28/2017 08:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spm_SelectOrders]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Orders
	WHERE IsEnable = 1	
	ORDER BY CreatedTime
END

GO

