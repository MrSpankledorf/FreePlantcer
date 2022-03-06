CREATE TABLE [dbo].[Time]
(
	[LoggedTime] NCHAR(10) NOT NULL, 
    [Rate] FLOAT NULL, 
    [Job] NCHAR(20) NULL, 
    [Deight] NCHAR(10) NOT NULL
	CONSTRAINT [PK_LoggedTime] PRIMARY KEY ([LoggedTime]), 
    CONSTRAINT [FK_Deight_Deight] FOREIGN KEY ([Deight]) REFERENCES [Deight]([Deight])
)
