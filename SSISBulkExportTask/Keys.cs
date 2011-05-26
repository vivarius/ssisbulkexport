using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISBulkExportTask100
{
    internal static class Keys
    {
        public const string SQL_SERVER = "SQLServerInstance";
        public const string SQL_DATABASE = "Database";
        public const string SQL_STATEMENT = "SQLStatment";
        public const string SQL_VIEW = "View";
        public const string SQL_StoredProcedure = "StoredProcedure";
        public const string SQL_STORED_PROCEDURE_PARAMS = "StoredProcedureParameters";
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
        public const string FORMAT_FILE = "FormatFile";
        public const string FORMAT_FILE_CONNECTION = "FormatFileByFileConnection";
        public const string ACTIVATE_CMDSHELL = "ActivateCmdShell";

        public const string CODE_PAGE = "CodePage";

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
        #region Private members

        private const string activateCmdShell = " EXEC master..xp_cmdshell ";
        private readonly VariableDispenser _variableDispenser;
        private readonly Connections _connection;
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        #endregion

        #region Public Properties
        public string SQLServerInstance { get; set; }
        public string DataSource { get; set; }
        public string Database { get; set; }
        public string SQLStatment { get; set; }
        public string View { get; set; }
        public string StoredProcedure { get; set; }
        public object StoredProcedureParameters { get; set; }
        public string Tables { get; set; }
        public string FirstRow { get; set; }
        public string LastRow { get; set; }
        public string FieldTermiantor { get; set; }
        public string RowTermiantor { get; set; }
        public string NativeDatabaseDataType { get; set; }
        public string TrustedConnection { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DestinationPath { get; set; }
        public string DestinationByFileConnection { get; set; }
        public string FormatFile { get; set; }
        public string FormatFileByFileConnection { get; set; }
        public string ActivateCmdShell { get; set; }
        public string CodePage { get; set; }
        #endregion

        #region .ctor

        public BCP(VariableDispenser variableDispenser, Connections connections)
        {
            _variableDispenser = variableDispenser;
            _connection = connections;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method evaluate expressions like @([System::TaskName] + [System::TaskID]) or any other operation created using 
        /// ExpressionBuilder
        /// </summary>
        /// <param name="mappedParam"></param>
        /// <param name="variableDispenser"></param>
        /// <returns></returns>
        private static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject = null;
            try
            {
                var expressionEvaluatorClass = new ExpressionEvaluatorClass
                {
                    Expression = mappedParam
                };

                expressionEvaluatorClass.Evaluate(DtsConvert.GetExtendedInterface(variableDispenser), out variableObject, false);
            }
            catch
            {
                variableObject = mappedParam;
            }
            return variableObject;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public new string ToString()
        {

            _stringBuilder.Append(activateCmdShell);
            _stringBuilder.Append("'bcp ");

            switch (DataSource)
            {
                case Keys.TAB_SQL:
                    _stringBuilder.Append(string.Format(@" ""{0}"" queryout ", Regex.Replace(SQLStatment, "(\n|\r)+", string.Empty)));
                    break;
                case Keys.TAB_SP:
                    StringBuilder storedProcParams = new StringBuilder();

                    int index = 0;

                    foreach (var param in (MappingParams)StoredProcedureParameters)
                    {
                        storedProcParams.Append(string.Format("{0}, {1}",
                                                              (index > 0)
                                                                    ? ","
                                                                    : string.Empty,
                                                               EvaluateExpression(param.Value, _variableDispenser)));
                        index++;
                    }

                    _stringBuilder.Append(string.Format(@" ""set fmtonly off exec {0}.{1} {2}"" queryout ", Database.Trim(), StoredProcedure.Trim(), storedProcParams));
                    break;
                case Keys.TAB_VIEW:
                    _stringBuilder.Append(string.Format(@" ""{0}.{1}"" out ", Database.Trim(), View.Trim()));
                    break;
                case Keys.TAB_TABLES:
                    _stringBuilder.Append(string.Format(@" ""{0}.{1}"" out ", Database.Trim(), Tables.Trim()));
                    break;
            }

            _stringBuilder.Append(DestinationByFileConnection.Trim() == Keys.TRUE
                                     ? string.Format(@" ""{0}"" ", _connection[DestinationPath].ConnectionString)
                                     : string.Format(@" ""{0}"" ", EvaluateExpression(DestinationPath, _variableDispenser)));


            string srvVal = (from srv in _connection[SQLServerInstance].ConnectionString.Split(';')
                             where srv.Contains("Data Source")
                             select srv).FirstOrDefault();

            if (srvVal != null)
                _stringBuilder.Append(string.Format(" -S{0}", srvVal.Split('=')[1]));

            _stringBuilder.Append(TrustedConnection.Trim() == Keys.TRUE
                                     ? " -T "
                                     : string.Format(" -U{0} -P{1} ",
                                                     EvaluateExpression(Login, _variableDispenser),
                                                     EvaluateExpression(Password, _variableDispenser)));

            //if (!string.IsNullOrEmpty(Database))
            //{
            //    _stringBuilder.Append(string.Format(" -d [{0}]", Database));
            //}

            if (!string.IsNullOrEmpty(FirstRow.Trim()))
            {
                _stringBuilder.Append(string.Format(" -F{0}", FirstRow));
            }

            if (!string.IsNullOrEmpty(LastRow.Trim()))
            {
                _stringBuilder.Append(string.Format(" -L{0}", LastRow));
            }

            if (NativeDatabaseDataType == Keys.TRUE)
            {
                _stringBuilder.Append(" -N ");
            }

            if (!string.IsNullOrEmpty(FormatFile.Trim()))
            {
                _stringBuilder.Append(FormatFileByFileConnection == Keys.TRUE
                         ? string.Format(@" -f""{0}"" ", _connection[FormatFile].ConnectionString)
                         : string.Format(@" -f""{0}"" ", EvaluateExpression(FormatFile, _variableDispenser)));
            }
            else
            {
                _stringBuilder.Append(" -c ");
            }

            if (!string.IsNullOrEmpty(FieldTermiantor.Trim()))
            {
                _stringBuilder.Append(string.Format(" -t{0} ", FieldTermiantor));
            }

            if (!string.IsNullOrEmpty(RowTermiantor.Trim()))
            {
                _stringBuilder.Append(string.Format(" -r{0} ", RowTermiantor));
            }

            if (!string.IsNullOrEmpty(CodePage.Trim()))
            {
                _stringBuilder.Append(string.Format(" -C{0} ", CodePage));
            }

            _stringBuilder.Append("'");

            return _stringBuilder.ToString();
        }

        #endregion
    }
}