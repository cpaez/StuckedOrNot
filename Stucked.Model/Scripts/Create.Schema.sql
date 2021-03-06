
CREATE TABLE [dbo].[HighwaySigns](
	[Description] [varchar](250) NULL,
	[Name] [varchar](50) NULL,
	[SnapToRoads] [varchar](50) NULL,
	[GeoJSON] [varchar](1000) NULL,
	[HighwaySignId] [int] NOT NULL,
	[Direction] [varchar](50) NULL,
	[Location] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[HighwaySignId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Segments](
	[Description] [varchar](250) NULL,
	[Name] [varchar](50) NULL,
	[SnapToRoads] [varchar](50) NULL,
	[GeoJSON] [varchar](1000) NULL,
	[SegmentId] [int] NOT NULL,
	[SegmentCode] [int] NULL,
	[NameStart] [varchar](50) NULL,
	[NameEnd] [varchar](50) NULL,
	[Detail] [varchar](300) NULL,
	[HighwayId] [int] NULL,
 CONSTRAINT [PK__Segments__39DF5804D9F65D4C] PRIMARY KEY CLUSTERED 
(
	[SegmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Highways](
	[HighwayId] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[NameStart] [varchar](50) NULL,
	[NameEnd] [varchar](50) NULL,
 CONSTRAINT [PK__Highways__39DF5804D9F65D4C] PRIMARY KEY CLUSTERED 
(
	[HighwayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]