USE StuckedOrNot
GO

CREATE TABLE HighwaySigns
(Description VARCHAR(250), 
Name VARCHAR(50), 
SnapToRoads VARCHAR(50),
GeoJSON VARCHAR(1000))
GO

BULK INSERT HighwaySigns
	FROM 'C:\Users\carlos.paez\Documents\GitHub\StuckedOrNot\Docs\Info.Autopistas\Info.Carteles.Autopistas\ausa-carteles.csv'
	WITH
	(
		FIELDTERMINATOR = ',',
		ROWTERMINATOR = '\n'
	)
GO