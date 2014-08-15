USE StuckedOrNot
GO

-- add new information about highways

ALTER TABLE Highways
ADD 
	SegmentCode int, 
	NameStart VARCHAR(50), 
	NameEnd VARCHAR(50), 
	Detail VARCHAR(300)
GO