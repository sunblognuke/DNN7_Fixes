	TRUNCATE TABLE {databaseOwner}{objectQualifier}EventLog
	TRUNCATE TABLE {databaseOwner}{objectQualifier}SiteLog
	TRUNCATE TABLE {databaseOwner}{objectQualifier}schedulehistory 

  /* The tabless below were removed from DNN7 so scripts should be not executed any more. */
	--TRUNCATE TABLE {databaseOwner}{objectQualifier}SearchItemWordPosition
	--DELETE {databaseOwner}{objectQualifier}SearchItemWord
	--DELETE {databaseOwner}{objectQualifier}SearchWord
	--DELETE {databaseOwner}{objectQualifier}SearchItem
