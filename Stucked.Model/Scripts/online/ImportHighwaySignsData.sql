CREATE TABLE HighwaySigns
(Description VARCHAR(250), 
Name VARCHAR(50), 
SnapToRoads VARCHAR(50),
GeoJSON VARCHAR(1000))
GO


BULK INSERT HighwaySigns
	FROM 'http://carlosapaez.com.ar/stuckedornot/ausa-carteles.csv'
	WITH
	(
		FIELDTERMINATOR = ',',
		ROWTERMINATOR = '\n'
	)
GO