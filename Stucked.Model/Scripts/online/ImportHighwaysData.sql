CREATE TABLE Highways
(Description VARCHAR(250), 
Name VARCHAR(50), 
SnapToRoads VARCHAR(50),
GeoJSON VARCHAR(1000))
GO


BULK INSERT Highways
	FROM 'http://carlosapaez.com.ar/stuckedornot/ausa-autopistas.csv'
	WITH
	(
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n'
	)
GO