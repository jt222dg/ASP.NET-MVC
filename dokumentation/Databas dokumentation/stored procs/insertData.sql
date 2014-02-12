--radera alla nullvärden i namn
delete from LocationName Where location_name = 'Null'

delete from Location
delete from LocationName
delete from LocationType
delete from Area

DBCC CHECKIDENT('Location', RESEED, 0)
DBCC CHECKIDENT('LocationName', RESEED, 0)
DBCC CHECKIDENT('Area', RESEED, 0)
DBCC CHECKIDENT('LocationType', RESEED, 0)

--Lägga till plats från xml fil.
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_insert_OldDATA

@latitud DECIMAL(9,6) = NULL,
@longitud DECIMAL(9,6) = NULL,
@ritningsnamn VARCHAR(30) = NULL,
@rumsnamn2 VARCHAR(30) = NULL,
@rumsnamn3 VARCHAR(30) = NULL,
@rumsnamn4 VARCHAR(30) = NULL,
@byggnad_sv VARCHAR(40) = NULL,
@vaning TINYINT = NULL,
@stad VARCHAR(30) = NULL, 
@rumstyp_sv VARCHAR(40) = NULL

AS
BEGIN

DECLARE @Area_id INT;
DECLARE @LocationType_id INT;
DECLARE @Location_id INT;
DECLARE @LocationName_id INT;
	
	BEGIN TRY
		BEGIN TRANSACTION
		
			-- Lägga till arean
			EXEC @Area_id = usp_Area @byggnad_sv, @stad;
		
			-- Lägga till platstyp
			EXEC @LocationType_id = usp_LocationType @rumstyp_sv;
			
			-- Lägga till platsen med typid och area id.
			EXEC @Location_id = usp_Location @LocationType_id, @Area_id, @latitud, @longitud, @vaning;
			
			-- Lägga till platsnamn
			EXEC @LocationName_id = usp_LocationName @Location_id, @ritningsnamn ,@rumsnamn2 ,@rumsnamn3 ,@rumsnamn4;
		
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
GO

EXEC usp_insert_OldDATA 56.67882083417161, 16.35916829109192, 'G123EK', 'Ny159K', 'null', 'null', 'G-Huset', 1, 'Kalmar', 'Videoredigering';

--LÄGGA TILL OMRÅDE
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_Area 

@stad VARCHAR(40) = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL,
@byggnad_sv VARCHAR(40) = NULL,
@byggnad_en VARCHAR(40) = NULL

AS
BEGIN
DECLARE @area_id INT
DECLARE @city_id INT

SELECT @area_id = area_id FROM Area WHERE area_swe = @byggnad_sv;

-- Kolla om platstypen finns, om hämta id och lägg till
-- Annars lägg till typen ch lägg på id på platsen.
	IF @area_id IS NULL
		BEGIN
			SELECT @city_id = city_id FROM City WHERE city = @stad;
			IF @byggnad_en = 'null'
				SET @byggnad_en = @byggnad_sv
			INSERT INTO Area (city_id, area_swe, area_eng, latitude, longitude)
			VALUES (@city_id, @byggnad_sv, @byggnad_en, @latitude, @longitude)
			RETURN @@IDENTITY;
		END
	ELSE
		RETURN @area_id;
END
GO


--LÄGGA TILL PLATSTYP
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_LocationType

@rumstyp_sv VARCHAR(40) = NULL,
@rumstyp_en VARCHAR(40) = NULL

AS
BEGIN
DECLARE @location_type_id INT
-- Kolla om platstypen finns, om hämta id och lägg till
-- Annars lägg till typen ch lägg på id på platsen.
SELECT @location_type_id = location_type_id FROM LocationType WHERE location_type_swe = @rumstyp_sv

	IF @location_type_id IS NULL
		BEGIN
			INSERT INTO LocationType (location_type_swe, location_type_eng)
			VALUES (@rumstyp_sv, @rumstyp_en)
			RETURN @@IDENTITY;
		END
	ELSE
		RETURN @location_type_id;
END
GO

EXEC usp_LocationType 'type_sv';

--LÄGGA TILL LOCATIONAME
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_LocationName

@location_id INT = NULL,
@ritningsnamn VARCHAR(30) = NULL,
@rumsnamn2 VARCHAR(30) = NULL,
@rumsnamn3 VARCHAR(30) = NULL,
@rumsnamn4 VARCHAR(30) = NULL

AS
BEGIN
	-- För varje namn som finns, lägg till namn
	-- Om namnet är ritningsnamnet lägg is main till true
	IF (@ritningsnamn IS NOT NULL)
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @ritningsnamn, @ritningsnamn, 1)
	IF (@rumsnamn2 IS NOT NULL)
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn2,@rumsnamn2, 0)
	IF (@rumsnamn3 IS NOT NULL)
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn3,@rumsnamn3, 0)
	IF (@rumsnamn4 IS NOT NULL)
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn4,@rumsnamn4, 0)
END
GO

EXEC usp_LocationName 3, 'Name1', 'Name2', 'Name3', 'Name4';
EXEC usp_LocationName 3, 'Name1', 'Name2', 'Name3';
EXEC usp_LocationName 3, 'Name1', 'Name2';
EXEC usp_LocationName 3, 'Name1';

--LÄGGA TILL LOCATION
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_Location

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
GO

EXEC usp_Location 3,25, 00.000000, 00.000000, 2