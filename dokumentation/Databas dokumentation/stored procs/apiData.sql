--------------------------------------------------------------
--/usp_API_get_locations(ingen parameter)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locations 

AS
BEGIN
	
	SELECT Location.location_id, location_type_id,area_id, latitude, longitude,floor_nr, location_name, is_main
	FROM Location
	JOIN LocationName
	ON Location.location_id=LocationName.location_id
	
END
GO

EXEC usp_API_get_locations ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_location_by_id		(location_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_location_by_id

@location_id INT = NULL

AS
BEGIN
	
	SELECT Location.location_id, location_type_id, area_id, latitude, longitude,floor_nr, location_name, is_main
	FROM Location
	JOIN LocationName
	ON Location.location_id=LocationName.location_id AND Location.location_id = @location_id
	
END
GO

EXEC usp_API_get_location_by_id 13 ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locations_by_city_id	(city_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locations_by_city_id

@city_id INT = NULL

AS
BEGIN

	SELECT Location.location_id, location_type_id, Location.area_id, Location.latitude, Location.longitude,floor_nr, location_name, is_main, city_id
	FROM Location
	
	JOIN LocationName
	ON Location.location_id=LocationName.location_id
	
	JOIN Area
	ON Location.area_id=Area.area_id AND Area.city_id = @city_id
	
END
GO

EXEC usp_API_get_locations_by_city_id 3 ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locations_by_name		(name)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locations_by_name

@location_name VarCHAR(30) = NULL

AS
BEGIN

	SELECT Location.location_id, location_type_id, Location.area_id, latitude, longitude,floor_nr, location_name, is_main
	FROM Location
	
	JOIN LocationName
	ON Location.location_id=LocationName.location_id AND LocationName.location_name LIKE '%'+@location_name+'%';
	
END
GO

EXEC usp_API_get_locations_by_name '142' ;
EXEC usp_API_get_locations_by_name 'A22:30CK' ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locations_by_area_id	(area_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locations_by_area_id

@area_id SMALLINT = NULL

AS
BEGIN

	SELECT Location.location_id, location_type_id, Location.area_id, latitude, longitude,floor_nr, location_name, is_main
	FROM Location
	
	JOIN LocationName
	ON Location.location_id=LocationName.location_id  AND Location.area_id = @area_id 
	
END
GO

EXEC usp_API_get_locations_by_area_id 78 ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locations_by_type_id	(type_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locations_by_type_id

@location_type_id SMALLINT = NULL

AS
BEGIN

	SELECT Location.location_id, location_type_id, Location.area_id, latitude, longitude,floor_nr, location_name, is_main
	FROM Location
	
	JOIN LocationName
	ON Location.location_id=LocationName.location_id  AND Location.location_type_id = @location_type_id 
	
END
GO

EXEC usp_API_get_locations_by_type_id 71 ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_areas 		(ingen)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_areas

AS
BEGIN

	SELECT * FROM Area;
	
END
GO

EXEC usp_API_get_areas;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_area_by_id		(area_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_area_by_id

@area_id SMALLINT = NULL

AS
BEGIN

	SELECT * FROM Area WHERE area_id = @area_id; 
	
END
GO

EXEC usp_API_get_area_by_id 54;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_areas_by_city_id	(city_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_areas_by_city_id

@city_id TINYINT = NULL

AS
BEGIN

	SELECT * FROM Area WHERE city_id = @city_id; 
	
END
GO

EXEC usp_API_get_areas_by_city_id 3;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_areas_by_name	(name)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_areas_by_name

@area VARCHAR(40) = NULL

AS
BEGIN

	SELECT * FROM Area WHERE area LIKE '%'+@area+'%'; 
	
END
GO

EXEC usp_API_get_areas_by_name 'drama';
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_cities		(ingen)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_cities

AS
BEGIN

	SELECT * FROM City;
	
END
GO

EXEC usp_API_get_cities;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_city_by_name 	(name)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_city_by_name

@city VARCHAR(50) = NULL

AS
BEGIN

	SELECT * FROM City WHERE city LIKE '%'+@city+'%'; 
	
END
GO

EXEC usp_API_get_city_by_name 'Kalmar';
EXEC usp_API_get_city_by_name 'pukeberg3';
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locationTypes		(ingen)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locationTypes

AS
BEGIN

	SELECT * FROM LocationType;
	
END
GO

EXEC usp_API_get_locationTypes ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locationTypes_by_id		(locationType_id)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locationTypes_by_id	

@location_type_id SMALLINT = NULL

AS
BEGIN

	SELECT * FROM LocationType WHERE location_type_id = @location_type_id;
	
END
GO

EXEC usp_API_get_locationTypes_by_id 38	 ;
--------------------------------------------------------------
--------------------------------------------------------------
--/usp_API_get_locationTypes_by_name 	(name)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_API_get_locationTypes_by_name 	

@location_type VARCHAR(40) = NULL

AS
BEGIN

	SELECT * FROM LocationType WHERE location_type LIKE '%'+@location_type+'%';
	
END
GO

EXEC usp_API_get_locationTypes_by_name  'föreläsning'	 ;
--------------------------------------------------------------