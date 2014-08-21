USE StuckedOrNot
GO

-- add new information about highway signs

ALTER TABLE HighwaySigns
ADD 
	Direction VARCHAR(50), 
	Location VARCHAR(20)
GO