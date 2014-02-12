------------------------------------------------------
PLATSER
------------------------------------------------------
--usp_admin_update_location		(location_id, locationType_id, area_id, lattitude, longitude, floorNR)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_update_location	

@location_id INT = NULL,
@location_type_id SMALLINT = NULL,
@area_id SMALLINT = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL,
@floor_nr TINYINT = NULL

AS
BEGIN
	UPDATE Location
	SET
	location_type_id = @location_type_id,
	area_id = @area_id,
	latitude = @latitude,
	longitude = @longitude,
	floor_nr = @floor_nr
	
	WHERE location_id = @location_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_update_location 3,61, 71, 00.000000,00.000000, 1
------------------------------------------------------
--usp_admin_create_location		(locationType_id, area_id, lattitude, longitude, floorNR)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_create_location	

@location_type_id SMALLINT = NULL,
@area_id SMALLINT = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL,
@floor_nr TINYINT = NULL

AS
BEGIN
	INSERT INTO Location (location_type_id, area_id, latitude, longitude, floor_nr)
	VALUES (@location_type_id, @area_id, @latitude, @longitude, @floor_nr)
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_create_location 61,71,00.000000,00.000000,6
------------------------------------------------------
--usp_admin_delete_location		(location_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_delete_location	

@location_id INT = NULL

AS
BEGIN
	DELETE 
	FROM Location
	WHERE location_id = @location_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_delete_location 2
------------------------------------------------------
--usp_admin_get_locations			()
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_locations

AS
BEGIN
	SELECT * FROM Location;
END
GO
EXEC usp_admin_get_locations
------------------------------------------------------
--usp_admin_get_location_by_id	(location_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_location_by_id

@location_id INT = NULL

AS
BEGIN
	SELECT * FROM Location WHERE location_id = @location_id;
END
GO
EXEC usp_admin_get_location_by_id 1
------------------------------------------------------
PLATSNAMN
------------------------------------------------------
--usp_admin_update_locationName		(locationName_id, location_id, locationName, isMain)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_update_locationName

@location_name_id INT = NULL,
@location_id INT = NULL,
@location_name VARCHAR(30) = NULL,
@is_main BIT = NULL

AS
BEGIN
	UPDATE LocationName
	SET location_id = @location_id,
	location_name = @location_name,
	is_main = @is_main
	
	WHERE location_name_id = @location_name_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_update_locationName 25, 2, '142(mer)', 0
------------------------------------------------------
--usp_admin_create_locationName		(locationType_id, area_id, lattitude, longitude, floorNR)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_create_locationName	

@location_id INT = NULL,
@location_name VARCHAR(30) = NULL,
@is_main BIT = NULL

AS
BEGIN
	INSERT INTO LocationName (location_id, location_name, is_main)
	VALUES (@location_id, @location_name, @is_main)
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_create_locationName 2, 'NyttNamn', 1
------------------------------------------------------
--usp_admin_delete_locationName		(locationName_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_delete_locationName	

@location_name_id INT = NULL

AS
BEGIN
	DELETE 
	FROM LocationName
	WHERE location_name_id = @location_name_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_delete_locationName 489
------------------------------------------------------
--usp_admin_get_locationNames_by_location_id	(location_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_locationNames_by_location_id

@location_id INT = NULL

AS
BEGIN
	SELECT * FROM LocationName WHERE location_id = @location_id;
END
GO
EXEC usp_admin_get_locationNames_by_location_id 13
------------------------------------------------------
--usp_admin_get_locationName_by_id(locationName_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_locationName_by_id

@location_name_id SMALLINT = NULL

AS
BEGIN
	SELECT * FROM Location_name WHERE location_name_id = @location_name_id;
END
GO
EXEC usp_admin_get_locationName_by_id 25
------------------------------------------------------
PLATSTYP
------------------------------------------------------
--usp_admin_update_locationType	(locationType_id, locationType)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_update_locationType

@location_type_id SMALLINT = NULL,
@location_type VARCHAR(40) = NULL

AS
BEGIN
	UPDATE LocationType
	SET location_type = @location_type
	WHERE location_type_id = @location_type_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_update_locationType 29, 'Byggnadd'
------------------------------------------------------
--usp_admin_create_locationType	(locationType)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_create_locationType

@location_type VARCHAR(40) = NULL

AS
BEGIN
	INSERT INTO LocationType (location_type)
	VALUES (@location_type)
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_create_locationType 'TestTyp'
------------------------------------------------------
--usp_admin_delete_locationType	(locationType_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_delete_locationType

@location_type_id SMALLINT = NULL

AS
BEGIN
	DELETE 
	FROM LocationType
	WHERE location_type_id = @location_type_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_delete_locationType 36
------------------------------------------------------
--usp_admin_get_locationTypes		()
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_locationTypes

AS
BEGIN
	SELECT * FROM Location_type;
END
GO
EXEC usp_admin_get_locationTypes
------------------------------------------------------
--usp_admin_get_locationType_by_id(locationType_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_locationType_by_id

@location_type_id SMALLINT = NULL

AS
BEGIN
	SELECT * FROM LocationType WHERE location_type_id = @location_type_id;
END
GO
EXEC usp_admin_get_locationType_by_id 29
------------------------------------------------------
OMRÅDE
------------------------------------------------------
--usp_admin_update_area			(area_id, city_id, area, lattitude, longitude)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_update_area	

@area_id SMALLINT = NULL,
@city_id TINYINT = NULL,
@area VARCHAR(40) = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL

AS
BEGIN
	UPDATE Area
	SET
		city_id = @city_id,
		area = @area,
		latitude = @latitude,
		longitude = @longitude
	WHERE area_id = @area_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_update_area 52, 1, 'Arenan2', '56.8395090210194', null
------------------------------------------------------
--usp_admin_create_area			(city_id, area, lattitude, longitude)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_create_area	

@city_id TINYINT = NULL,
@area VARCHAR(40) = NULL,
@latitude DECIMAL(9,6) = NULL,
@longitude DECIMAL(9,6) = NULL

AS
BEGIN
	INSERT INTO Area (city_id, area, latitude, longitude)
	VALUES (@city_id, @area, @latitude, @longitude)
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_create_area 1, 'TestArea', '56.8395090210194', null
------------------------------------------------------
--usp_admin_delete_area			(area_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_delete_area

@area_id SMALLINT = NULL

AS
BEGIN
	DELETE 
	FROM Area
	WHERE area_id = @area_id
	RETURN @@IDENTITY;
END
GO
EXEC usp_admin_delete_area 56
------------------------------------------------------
--usp_admin_get_areas				()
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_areas

AS
BEGIN
	SELECT * FROM Area
END
GO
EXEC usp_admin_get_areas
------------------------------------------------------
--usp_admin_get_area_by_id		(area_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_area_by_id

@area_id SMALLINT = NULL

AS
BEGIN
	SELECT * FROM Area WHERE area_id = @area_id
END
GO
EXEC usp_admin_get_area_by_id 52
------------------------------------------------------
STAD
------------------------------------------------------
--usp_admin_get_cities			()
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_cities

AS
BEGIN
	SELECT * FROM City
END
GO
EXEC usp_admin_get_cities
------------------------------------------------------
--usp_admin_get_city_by_id		(city_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_admin_get_city_by_id

@city_id TINYINT = NULL

AS
BEGIN
	SELECT * FROM City WHERE city_id = @city_id
END
GO
EXEC usp_admin_get_city_by_id 3
------------------------------------------------------
