CREATE TABLE [tweet_ftidx]
(
	[run_id] smallint NOT NULL,
	[tweet_id] bigint NOT NULL,
	[word] nvarchar(150) NOT NULL,
	[id] smallint NOT NULL,
	[pos] smallint NOT NULL,
	[created_at] datetime NOT NULL,
	[utc_offset] int NULL,
	[lon] float NULL,
	[lat] float NULL
)  ON [FTINDEX]
WITH (DATA_COMPRESSION = PAGE)