To install the filter, copy files to SQL Server instance Binn directory, modify twftlib.reg accordingly and double click on it to add setting to the registry.

Run the following scripts to enable third-party filters:

exec sp_fulltext_service 'load_os_resources', 1
exec sp_fulltext_service 'verify_signature', 0     -- to load unsigned dll's

Test with the following script:

SELECT * FROM sys.dm_fts_parser(N'"Alma"', 1038, 0, 0)