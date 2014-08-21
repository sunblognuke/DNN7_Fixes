DNN7_Fixes
==========

##Fixes

1) Simple ControlBar 

Just copy them into the folder $DNN_ROOT/admin/ControlPanel/. That it works.

##Tricks

1) Sidebar menu

2) Script to reduce db size

3) CleanDBlog.cs:

It is a small utility I created as a DNN Scheduler task in order to automate the task of cleaning the DNN log files.
All it does is to run the sql commands as follows:
(-- is for comments here)
ALTER DATABASE thisDBname SET RECOVERY simple; --To change the recovery model
dbcc shrinkdatabase (thisDBname, TRUNCATEONLY); -- shrink database
TRUNCATE TABLE eventlog; -- clean eventlog table
TRUNCATE TABLE sitelog; -- clean sitelog
DBCC SHRINKDATABASE (thisDBname]);

When the task is finished it gives a report of the DB size before and after.
You can set the task intervals in the Scheduler page (under Host menu)
Please see the documentation for details.

You may use the command sp_spaceused to see the DB size 
(via host menu - SQL) 

more details - http://dnncleandblogsched.codeplex.com/
