-- before: 1 155 373,586 MB

ALTER TABLE [tweet_ftidx]
ADD CONSTRAINT [PK_tweet_ftidx] PRIMARY KEY  CLUSTERED 
(
    [word],
    [run_id],
    [tweet_id],
    [id]
) WITH (SORT_IN_TEMPDB = OFF, DATA_COMPRESSION = PAGE) ON [FTINDEX]

-- 7:1:53
--- after 512 GB

