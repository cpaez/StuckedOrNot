USE StuckedOrNot
GO

CREATE TABLE Highways
(Description VARCHAR(250), 
Name VARCHAR(50), 
SnapToRoads VARCHAR(50),
GeoJSON VARCHAR(1000))
GO


BULK INSERT Highways
	FROM 'C:\Users\carlos.paez\Documents\GitHub\StuckedOrNot\Docs\Info.Autopistas\ausa-autopistas.csv'
	WITH
	(
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n'
	)
GO