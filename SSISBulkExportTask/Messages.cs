using System;
using System.Collections.Generic;

namespace SSISBulkExportTask100
{
    internal static class Keys
    {
        public const string SQL_SERVER = "SQLServerInstance";
        public const string SQL_STATEMENT = "SQLStatment";
        public const string SQL_VIEW = "View";
        public const string SQL_StoredProcedure = "StoredProcedure";
        public const string SQL_TABLE = "Tables";
        public const string FIRSTROW = "FirstRow";
        public const string LASTROW = "LastRow";
        public const string FIELD_TERMINATOR = "FieldTermiantor";
        public const string ROW_TERMINATOR = "RowTermiantor";
        public const string NATIVE_DB_DATATYPE = "NativeDatabaseDataType";
        public const string TRUSTED_CONNECTION = "TrustedConnection";
        public const string SRV_LOGIN = "Login";
        public const string SRV_PASSWORD = "Password";
        public const string DESTINATION = "DestinationPath";
        public const string DESTINATION_FILE_CONNECTION = "DestinationByFileConnection";
        public const string ACTIVATE_CMDSHELL = "ActivateCmdShell";
        public const string STORED_PROCEDURE_PARAMS = "StoredProcedureParameters";

        public const string TRUE = "true";
        public const string FALSE = "false";

        public const string DATA_SOURCE = "DataSource";

        public const string TAB_SQL = "SQL Statement";
        public const string TAB_VIEW = "View";
        public const string TAB_SP = "Stored Procedure";
        public const string TAB_TABLES = "Tables";
    }

    [Serializable]
    public class MappingParam
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class MappingParams : List<MappingParam>
    {
    }

    public class QueryResources
    {
        public const string TABLES = "SELECT  '[' + sys.schemas.name + '].[' + sys.objects.name + ']' FROM sys.objects INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id WHERE sys.objects.type = 'U' ORDER BY sys.objects.name, sys.schemas.name";
        public const string VIEWS = "SELECT  '[' + sys.schemas.name + '].[' + sys.objects.name + ']' FROM sys.objects INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id WHERE sys.objects.type = 'V' ORDER BY sys.objects.name, sys.schemas.name";
        public const string STORED_PROCEDURES = "SELECT '[' + sys.schemas.name + '].[' + sys.objects.name + ']' FROM sys.objects INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id WHERE sys.objects.type = 'P' ORDER BY sys.objects.name, sys.schemas.name";
        public const string STORED_PROCEDURE_PARAMETERS = "SELECT sys.objects.name spName, sys.parameters.name paramName, sys.types.name typeName  FROM sys.parameters INNER JOIN sys.objects ON sys.parameters.object_id = sys.objects.object_id INNER JOIN sys.types ON sys.types.user_type_id = sys.parameters.user_type_id INNER JOIN sys.schemas  ON sys.objects.schema_id = sys.schemas.schema_id AND sys.schemas.name = '{0}' WHERE sys.objects.name = '{1}'";
        public const string ENABLE_CMDSHELL = "EXEC master.dbo.sp_configure 'show advanced options', 1;RECONFIGURE;EXEC master.dbo.sp_configure 'xp_cmdshell', 1;RECONFIGURE;";
    }

    public class BCP
    {
        private readonly string _bcp = " BCP ";

        public string SQLServerInstance { get; set; }
        public string DataSource { get; set; }
        public string SQLStatment { get; set; }
        public string View { get; set; }
        public object StoredProcedure { get; set; }
        public string Tables { get; set; }
        public string FirstRow { get; set; }
        public string LastRow { get; set; }
        public string FieldTermiantor { get; set; }
        public string RowTermiantor { get; set; }
        public string NativeDatabaseDataType { get; set; }
        public object TrustedConnection { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DestinationPath { get; set; }
        public string DestinationByFileConnection { get; set; }

        public new string ToString()
        {
            return _bcp;
        }
    }

}