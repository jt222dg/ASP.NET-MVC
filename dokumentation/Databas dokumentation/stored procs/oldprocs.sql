USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_Area]    Script Date: 02/21/2013 14:28:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_Area] 

@text_id VARCHAR(40) = NULL,
@longitude VARCHAR(40) = NULL,
@lattitude VARCHAR(40) = NULL,
@stad VARCHAR(30) = NULL

AS
BEGIN
DECLARE @area_id INT
DECLARE @city_id INT

SELECT @area_id = area_id FROM Area WHERE text_id = @text_id;

	IF @area_id IS NULL
		BEGIN
			SELECT @city_id = city_id FROM City WHERE city = @stad;
			INSERT INTO Area (text_id, city_id, latitude, longitude)
			VALUES (@text_id, @city_id, @lattitude, @longitude)
			RETURN @@IDENTITY;
		END
	ELSE
		RETURN @area_id;
END


USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_Area]    Script Date: 02/21/2013 14:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_get_Area]

@byggnad_sv VARCHAR(40) = NULL

AS
BEGIN

DECLARE @Text_id INT
DECLARE @Area_id INT

	SELECT @Area_id = Area.area_id FROM Area
	INNER JOIN "Text" as TextTable
	ON Area.text_id = TextTable.text_id
	WHERE TextTable.text = @byggnad_sv
	
	RETURN @Area_id
	
END


USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_locations]    Script Date: 02/21/2013 14:29:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_get_locations]

AS
BEGIN
DECLARE @XYList varchar(MAX)
SET @XYList = ''

	SELECT
		Location.location_id,
		Location.latitude,
		Location.longitude,
		Location.floor_nr,
	
		stuff((select ', '+i.text
        from Text as i 
        where Area.text_id = i.text_id_u
        for xml path(''), type).value('text()[1]', 'nvarchar(max)'), 1, 2, '') as area_name,
        
        stuff((select ', '+i.text
        from Text as i 
        where LocationName.text_id = i.text_id_u
        for xml path(''), type).value('text()[1]', 'nvarchar(max)'), 1, 2, '') as location_name,
        
        stuff((select ', '+i.text
        from Text as i 
        where LocationType.text_id = i.text_id_u
        for xml path(''), type).value('text()[1]', 'nvarchar(max)'), 1, 2, '') as location_type
         
         
	FROM Location
	
	RIGHT JOIN (Area
		RIGHT JOIN Text as aText
		ON Area.text_id = aText.text_id_u
		INNER JOIN City
		ON Area.city_id = City.city_id
	)
	ON Location.area_id = Area.area_id
	
	RIGHT JOIN (LocationType
		RIGHT JOIN Text as ltText
		ON LocationType.text_id = ltText.text_id_u
	)
	ON Location.location_type_id = LocationType.location_type_id
	
	RIGHT JOIN (LocationName
		RIGHT JOIN Text as lnText
		ON LocationName.text_id = lnText.text_id_u
	)
	ON Location.location_id = LocationName.location_id
	WHERE aText.text = 'Hus B' 
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_insert_Area]    Script Date: 02/21/2013 14:29:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_insert_Area]

@stad VARCHAR(40) = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL,
@byggnad_sv VARCHAR(40) = NULL,
@byggnad_en VARCHAR(40) = NULL

AS
BEGIN

DECLARE @value INT
DECLARE @Text_id_u INT
DECLARE @Text_id INT
DECLARE @Area_id INT
	
	BEGIN TRY
		BEGIN TRANSACTION
		
			--Lägga till svenska namnet(lang_id = 1)
			EXEC @Text_id_u = usp_Text @byggnad_sv, 1;
			IF @byggnad_en IS NOT NULL
			BEGIN
				--Lägga till engelska namnet(lang_id = 2)
				EXEC @Text_id = usp_Text @byggnad_en, 2;
				--lägga till fk på sensate
				EXEC @Text_id = usp_insert_TextFK @Text_id, @Text_id_u;
			END
			-- Lägga till arean
			EXEC @Area_id = usp_Area @Text_id_u, @longitude, @latitude, @stad;
		
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Use RAISERROR inside the CATCH block to return error
    -- information about the original error that caused
    -- execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
	END CATCH;
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_insert_OldDATA]    Script Date: 02/21/2013 14:30:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_insert_OldDATA]

@latitud DECIMAL(9,6) = NULL,
@longitud DECIMAL(9,6) = NULL,
@ritningsnamn VARCHAR(30) = NULL,
@rumsnamn2 VARCHAR(30) = NULL,
@rumsnamn3 VARCHAR(30) = NULL,
@rumsnamn4 VARCHAR(30) = NULL,
@byggnad_sv VARCHAR(40) = NULL,
@vaning TINYINT = NULL,
@stad VARCHAR(30) = NULL, 
@rumstyp_sv VARCHAR(40) = NULL,
@rumstyp_en VARCHAR(40) = NULL

AS
BEGIN

DECLARE @Area_id SMALLINT;
DECLARE @Location_type_id INT;
DECLARE @Location_id INT;
DECLARE @Location_name_id INT;
DECLARE @Text_id INT;
DECLARE @Text_id_u INT;
	
	BEGIN TRY
		BEGIN TRANSACTION
			
			-- Hämta arean
			EXEC @Area_id = usp_get_Area @byggnad_sv;

			-- Lägga till platstyp texter svenska
			EXEC @Text_id_u = usp_text @rumstyp_sv, 1;
			
			-- Lägga till platstyp texter engelska
			EXEC @Text_id = usp_Text @rumstyp_en, 2;
			
			--lägga till fk på sensate
			EXEC @Text_id = usp_insert_TextFK @Text_id, @Text_id_u;
			
			-- Lägga till platstyp
			EXEC @Location_type_id = usp_LocationType @Text_id_u;
			
			-- Lägga till platsen med typid och area id.
			EXEC @Location_id = usp_Location @Location_type_id, @Area_id, @latitud, @longitud, @vaning;
			
			IF @ritningsnamn != 'null'
			BEGIN
				-- Lägga till platstyp texter svenska
				EXEC @Text_id = usp_Text @ritningsnamn, 1;
				
				-- Lägga till platsnamn
				EXEC @Location_name_id = usp_LocationName @Text_id, @Location_id, 1
			END
			
			
			-- Lägga till platstyp texter svenska
			IF @rumsnamn2 != 'null'
			BEGIN
				EXEC @Text_id = usp_Text @rumsnamn2, 1;
				-- Lägga till platsnamn
				EXEC @Location_name_id = usp_LocationName @Text_id, @Location_id, 0
			END
			
			-- Lägga till platstyp texter svenska
			IF @rumsnamn3 != 'null'
			BEGIN
				EXEC @Text_id = usp_Text @rumsnamn3, 1;
				-- Lägga till platsnamn
				EXEC @Location_name_id = usp_LocationName @Text_id, @Location_id, 0
			END
			
			-- Lägga till platstyp texter svenska
			IF @rumsnamn4 != 'null'
			BEGIN
				EXEC @Text_id = usp_Text @rumsnamn4, 1;
				-- Lägga till platsnamn
				EXEC @Location_name_id = usp_LocationName @Text_id, @Location_id, 0
			END
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Use RAISERROR inside the CATCH block to return error
    -- information about the original error that caused
    -- execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
	END CATCH;
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_insert_TextFK]    Script Date: 02/21/2013 14:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_insert_TextFK]

@text_id INT = NULL,
@fk_id INT = NULL

AS
BEGIN
		UPDATE Text
		SET text_id_u = @fk_id
		WHERE text_id = @text_id
		RETURN @@IDENTITY
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_Location]    Script Date: 02/21/2013 14:31:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_Location]

@location_type_id SMALLINT = NULL,
@area_id SMALLINT = NULL,
@latitud DECIMAL(9,6) = NULL,
@longitud DECIMAL(9,6) = NULL,	
@floor_nr INT = NULL

AS
BEGIN
	INSERT INTO Location (location_type_id, area_id, latitude, longitude, floor_nr)
	VALUES (@location_type_id, @area_id, @latitud, @longitud, @floor_nr);
	RETURN @@IDENTITY;
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_LocationName]    Script Date: 02/21/2013 14:31:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_LocationName]

@text_id INT = NULL,
@location_id INT = NULL,
@is_main INT = NULL

AS
BEGIN

	IF @text_id IS NOT NULL
		INSERT INTO LocationName (text_id, location_id, is_main)
		VALUES (@text_id, @location_id, @is_main)
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_LocationType]    Script Date: 02/21/2013 14:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_LocationType]

@text_id VARCHAR(40) = NULL

AS
BEGIN
DECLARE @location_type_id INT
-- Kolla om platstypen finns, om hämta id och lägg till
-- Annars lägg till typen ch lägg på id på platsen.
SELECT @location_type_id = location_type_id FROM LocationType WHERE text_id = @text_id

	IF @location_type_id IS NULL
		BEGIN
			INSERT INTO LocationType (text_id)
			VALUES (@text_id)
			RETURN @@IDENTITY;
		END
	ELSE
		RETURN @location_type_id;
END

USE [107229-map]
GO
/****** Object:  StoredProcedure [dbo].[usp_Text]    Script Date: 02/21/2013 14:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_Text]

@text VARCHAR(40) = NULL,
@lang_id TINYINT = NULL

AS
BEGIN
DECLARE @Text_id INT
-- Annars lägg till typen ch lägg på id på platsen.
	IF @text IS NOT NULL
		BEGIN
		SELECT @Text_id = text_id FROM "Text" WHERE "text" = @text
		IF @Text_id IS NULL
			BEGIN
				INSERT INTO Text (text, lang_id)
				VALUES (@text, @lang_id)
				SET @Text_id = @@IDENTITY
				--Om det är svenska så är det första inlagda och vi kan lägga fk på identity värdet
				IF @lang_id = 1
					BEGIN
						UPDATE Text
						SET text_id_u = @Text_id
						WHERE text_id = @Text_id
						RETURN @@IDENTITY
					END
				ELSE
					RETURN @@IDENTITY;
			END
			ELSE
				RETURN @Text_id
		END
	--Om det är svenska så är det första inlagda och vi kan lägga fk på identity värdet
END
