TRUNCATE TABLE [tweet_ftidx]

INSERT INTO [tweet_ftidx] WITH (TABLOCKX)
SELECT
	t.run_id, t.tweet_id, w.word, w.id, w.pos,
	t.created_at, t.utc_offset, t.lon, t.lat
FROM tweet AS t
CROSS APPLY dbo.WordBreaker3(t.text, 3, 2, 0, 15) AS w

-- 9:48:56