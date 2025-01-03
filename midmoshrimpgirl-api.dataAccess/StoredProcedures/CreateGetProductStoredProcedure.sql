USE [midmoshrimpgirl-db]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProduct]
	@ProductSearchString nvarchar(100)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Name], [Price], [ImageLink]
	FROM [dbo].[Products]
	INNER JOIN [dbo].[ProductSearch] ON Products.Id = ProductSearch.ProductId
	WHERE ProductSearch.SearchName = @ProductgSearchString

END
GO