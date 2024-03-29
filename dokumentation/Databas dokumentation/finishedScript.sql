USE [Karta]
GO
/****** Object:  User [KartaUser]    Script Date: 03/20/2013 10:11:03 ******/
CREATE USER [KartaUser] FOR LOGIN [KartaUser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [LNU\app_kartapi-students_gg]    Script Date: 03/20/2013 10:11:03 ******/
CREATE USER [LNU\app_kartapi-students_gg] FOR LOGIN [LNU\app_kartapi-students_gg]
GO
/****** Object:  Table [dbo].[Icon]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Icon](
	[icon_id] [tinyint] IDENTITY(1,1) NOT NULL,
	[icon_link] [varchar](50) NULL,
 CONSTRAINT [PK_Icon] PRIMARY KEY CLUSTERED 
(
	[icon_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[City]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[city_id] [tinyint] IDENTITY(1,1) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[latitude] [decimal](9, 6) NULL,
	[longitude] [decimal](9, 6) NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[city_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Area]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Area](
	[area_id] [smallint] IDENTITY(1,1) NOT NULL,
	[city_id] [tinyint] NOT NULL,
	[area_swe] [varchar](50) NOT NULL,
	[area_eng] [varchar](50) NOT NULL,
	[latitude] [decimal](9, 6) NOT NULL,
	[longitude] [decimal](9, 6) NOT NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[area_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_eng] UNIQUE NONCLUSTERED 
(
	[area_eng] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_swe] UNIQUE NONCLUSTERED 
(
	[area_swe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LocationType](
	[location_type_id] [smallint] IDENTITY(1,1) NOT NULL,
	[location_type_swe] [varchar](50) NOT NULL,
	[location_type_eng] [varchar](50) NOT NULL,
	[icon_id] [tinyint] NULL,
 CONSTRAINT [PK_LocationType] PRIMARY KEY CLUSTERED 
(
	[location_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_type_eng] UNIQUE NONCLUSTERED 
(
	[location_type_eng] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_type_swe] UNIQUE NONCLUSTERED 
(
	[location_type_swe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_LocationType]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_LocationType]

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
			IF @rumstyp_en = 'null'
			BEGIN
				SET @rumstyp_en = @rumstyp_sv
			END
			INSERT INTO LocationType (location_type_swe, location_type_eng)
			VALUES (@rumstyp_sv, @rumstyp_en)
			RETURN @@IDENTITY;
		END
	ELSE
		RETURN @location_type_id;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_Area]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Area] 

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
/****** Object:  StoredProcedure [dbo].[usp_get_Area]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_get_Area]

@byggnad_sv VARCHAR(40) = NULL

AS
BEGIN

DECLARE @area_id INT
DECLARE @city_id INT

	SELECT @area_id = area_id FROM Area WHERE area_swe = @byggnad_sv;
	RETURN @area_id;
	
END
GO
/****** Object:  Table [dbo].[Location]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[location_id] [int] IDENTITY(1,1) NOT NULL,
	[location_type_id] [smallint] NOT NULL,
	[area_id] [smallint] NOT NULL,
	[latitude] [decimal](9, 6) NOT NULL,
	[longitude] [decimal](9, 6) NOT NULL,
	[floor_nr] [tinyint] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationName]    Script Date: 03/20/2013 10:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LocationName](
	[location_name_id] [int] IDENTITY(1,1) NOT NULL,
	[location_id] [int] NOT NULL,
	[location_name_swe] [varchar](50) NOT NULL,
	[location_name_eng] [varchar](50) NOT NULL,
	[is_main] [bit] NOT NULL,
 CONSTRAINT [PK_LocationName] PRIMARY KEY CLUSTERED 
(
	[location_name_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_Location]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Location]

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
/****** Object:  StoredProcedure [dbo].[usp_area_locations]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_area_locations]

	@area_id INT = NULL
	
AS
BEGIN
	SELECT DISTINCT(Location.location_id), Location.location_type_id, Location.area_id, Location.latitude, Location.longitude, Location.floor_nr
	
	FROM Location
	WHERE Location.area_id = @area_id
END
GO
/****** Object:  StoredProcedure [dbo].[usp_LocationName]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_LocationName]

@location_id INT = NULL,
@ritningsnamn VARCHAR(30) = NULL,
@rumsnamn2 VARCHAR(30) = NULL,
@rumsnamn3 VARCHAR(30) = NULL,
@rumsnamn4 VARCHAR(30) = NULL

AS
BEGIN
	-- För varje namn som finns, lägg till namn
	-- Om namnet är ritningsnamnet lägg is main till true
	IF (@ritningsnamn != 'null')
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @ritningsnamn, @ritningsnamn, 1)
	IF (@rumsnamn2 != 'null')
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn2,@rumsnamn2, 0)
	IF (@rumsnamn3 != 'null')
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn3,@rumsnamn3, 0)
	IF (@rumsnamn4 != 'null')
		INSERT INTO LocationName (location_id, location_name_swe, location_name_eng, is_main)
		VALUES (@location_id, @rumsnamn4,@rumsnamn4, 0)
END
GO
/****** Object:  StoredProcedure [dbo].[test]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[test]

@search VARCHAR(50) = NULL

AS
BEGIN

	SELECT Location.location_id, location_type_id, area_id, latitude, longitude,floor_nr, location_name_swe, location_name_eng, is_main
	FROM Location
	RIGHT JOIN LocationName
	ON Location.location_id=LocationName.location_id
	WHERE LocationName.location_name_swe LIKE '%'+@search+'%' OR LocationName.location_name_eng LIKE '%'+@search+'%'

END
GO
/****** Object:  StoredProcedure [dbo].[usp_search_locations]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_search_locations]

@search VARCHAR(50) = NULL

AS
BEGIN
	SELECT DISTINCT(Location.location_id), Location.location_type_id, Location.area_id, Location.latitude, Location.longitude, Location.floor_nr
	
	FROM Location
	JOIN LocationName
	ON Location.location_id=LocationName.location_id
	WHERE LocationName.location_name_swe LIKE '%'+@search+'%' OR LocationName.location_name_eng LIKE '%'+@search+'%'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_insert_OldDATA]    Script Date: 03/20/2013 10:11:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_insert_OldDATA]

@latitud DECIMAL(9,6) = NULL,
@longitud DECIMAL(9,6) = NULL,
@ritningsnamn VARCHAR(30) = NULL,
@rumsnamn2 VARCHAR(30) = NULL,
@rumsnamn3 VARCHAR(30) = NULL,
@rumsnamn4 VARCHAR(30) = NULL,
@byggnad_sv VARCHAR(40) = NULL,
@byggnad_en VARCHAR(40) = NULL,
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
	
	BEGIN TRY
		BEGIN TRANSACTION
			
			-- Lägga till arean
			EXEC @Area_id = usp_get_Area @byggnad_sv;
		
			-- Lägga till platstyp
			EXEC @Location_type_id = usp_LocationType @rumstyp_sv, @rumstyp_en;
			
			-- Lägga till platsen med typid och area id.
			EXEC @Location_id = usp_Location @Location_Type_id, @Area_id, @latitud, @longitud, @vaning;
			
			-- Lägga till platsnamn
			EXEC @Location_name_id = usp_LocationName @Location_id, @ritningsnamn ,@rumsnamn2 ,@rumsnamn3 ,@rumsnamn4;
		
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
/****** Object:  ForeignKey [FK_Area_City1]    Script Date: 03/20/2013 10:10:45 ******/
ALTER TABLE [dbo].[Area]  WITH CHECK ADD  CONSTRAINT [FK_Area_City1] FOREIGN KEY([city_id])
REFERENCES [dbo].[City] ([city_id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Area] CHECK CONSTRAINT [FK_Area_City1]
GO
/****** Object:  ForeignKey [FK_Location_Area]    Script Date: 03/20/2013 10:10:45 ******/
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Area] FOREIGN KEY([area_id])
REFERENCES [dbo].[Area] ([area_id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Area]
GO
/****** Object:  ForeignKey [FK_Location_LocationType]    Script Date: 03/20/2013 10:10:45 ******/
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_LocationType] FOREIGN KEY([location_type_id])
REFERENCES [dbo].[LocationType] ([location_type_id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_LocationType]
GO
/****** Object:  ForeignKey [FK_LocationName_Location]    Script Date: 03/20/2013 10:10:45 ******/
ALTER TABLE [dbo].[LocationName]  WITH CHECK ADD  CONSTRAINT [FK_LocationName_Location] FOREIGN KEY([location_id])
REFERENCES [dbo].[Location] ([location_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocationName] CHECK CONSTRAINT [FK_LocationName_Location]
GO
/****** Object:  ForeignKey [FK_LocationType_Icon]    Script Date: 03/20/2013 10:10:45 ******/
ALTER TABLE [dbo].[LocationType]  WITH CHECK ADD  CONSTRAINT [FK_LocationType_Icon] FOREIGN KEY([icon_id])
REFERENCES [dbo].[Icon] ([icon_id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[LocationType] CHECK CONSTRAINT [FK_LocationType_Icon]
GO
