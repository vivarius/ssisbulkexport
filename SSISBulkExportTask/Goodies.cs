﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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
        public const string NETWORK_PACKET_SIZE = "NetworkPacketSize";
        public const string CODE_PAGE = "CodePage";
        public const string USE_REGIONAL_SETTINGS = "UseRegionalSettings";
        public const string MAX_ERRORS = "MaxErrors";

        public const string SET_QUOTED_IDENTIFIERS_ON = "SET_QUOTED_IDENTIFIERS_ON";
        public const string UNICODE_CHR = "UseUnicodeCharacters";
        public const string CHARACTER_DATA_TYPE = "UseCharacterDataType";

        public const string TRUE = "true";
        public const string FALSE = "false";

        public const string DATA_SOURCE = "DataSource";

        public const string TAB_SQL = "SQL Statement";
        public const string TAB_VIEW = "View";
        public const string TAB_SP = "Stored Procedure";
        public const string TAB_TABLES = "Tables";

        public const string SEND_FILE_BY_EMAIL = "SendFileByEmail";
        public const string SMTP_SERVER = "SmtpServer";
        public const string RECIPIENTS = "SmtpRecipients";
        public const string FROM = "SmtpFrom";
        public const string EMAIL_SUBJECT = "EmailSubject";
        public const string EMAIL_BODY = "EmailBody";

        public const string REGEX_EMAIL = @"^[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9]@[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9][\.][a-z0-9]{2,4}$";

        public static Dictionary<string, string> BodyKeys = new Dictionary<string, string>
                                                                {
                                                                    {"File path", "${FilePath}"},
                                                                    {"File size", "${FileSize}"},
                                                                    {"Start Time", "${StartTime}"},
                                                                    {"End Time", "${EndTime}"},
                                                                    {"Total Execution Time", "${TotalExecutionTime}"},
                                                                    {"Rows Copied", "${RowsCopied}"}
                                                                };

        public static Dictionary<string, string> BodyKeysConcreteValue = new Dictionary<string, string>
                                                                {
                                                                    {"${FilePath}", "Path"},
                                                                    {"${FileSize}", "Size"},
                                                                    {"${StartTime}", "StartTime"},
                                                                    {"${EndTime}", "EndTime"},
                                                                    {"${TotalExecutionTime}", "GetExecutionTime"},
                                                                    {"${RowsCopied}", "RowsCopied"}
                                                                };
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
        public string NetworkPacketSize { get; set; }
        public string UseRegionalSettings { get; set; }
        public string SET_QUOTED_IDENTIFIERS_ON { get; set; }
        public string UseUnicodeCharacters { get; set; }
        public string UseCharacterDataType { get; set; }
        public string MaxErrors { get; set; }
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
                        storedProcParams.Append(string.Format("{0} {1}",
                                                              (index > 0)
                                                                    ? ","
                                                                    : string.Empty,
                                                              (param.Type.ToLower().Contains("char") ||
                                                               param.Type.ToLower().Contains("date") ||
                                                               param.Type.ToLower().Contains("text"))
                                                                     ? string.Format("'{0}'", EvaluateExpression(param.Value, _variableDispenser))
                                                                     : EvaluateExpression(param.Value, _variableDispenser)));
                        index++;
                    }

                    _stringBuilder.Append(string.Format(@" ""exec {0}.{1} {2}"" queryout ", Database.Trim(), StoredProcedure.Trim(), storedProcParams));
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
                _stringBuilder.Append(string.Format(@" -S""{0}""", srvVal.Split('=')[1]));

            _stringBuilder.Append(TrustedConnection.Trim() == Keys.TRUE
                                     ? " -T "
                                     : string.Format(@" -U""{0}"" -P""{1}"" ",
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

            if (!string.IsNullOrEmpty(MaxErrors.Trim()))
            {
                _stringBuilder.Append(string.Format(" -m{0}", MaxErrors));
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
            //else
            //{
            //    _stringBuilder.Append(" -c ");
            //}

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

            if (!string.IsNullOrEmpty(NetworkPacketSize.Trim()))
            {
                _stringBuilder.Append(string.Format(" -a{0} ", NetworkPacketSize));
            }

            if (UseRegionalSettings == Keys.TRUE)
            {
                _stringBuilder.Append(" -R ");
            }

            if (SET_QUOTED_IDENTIFIERS_ON == Keys.TRUE)
            {
                _stringBuilder.Append(" -q ");
            }

            if (UseCharacterDataType == Keys.TRUE)
            {
                _stringBuilder.Append(" -c ");
            }

            if (UseUnicodeCharacters == Keys.TRUE)
            {
                _stringBuilder.Append(" -w ");
            }

            _stringBuilder.Append("'");

            return _stringBuilder.ToString();
        }

        #endregion
    }

    public class SQLGoodies
    {
        private static Hashtable _sqlDTypeTable;

        public static Type ConvertFromSqlDbType(SqlDbType type)
        {
            if (_sqlDTypeTable == null)
            {
                _sqlDTypeTable = new Hashtable
                                  {
                                      {SqlDbType.Bit,  typeof (Boolean)},
                                      {SqlDbType.SmallInt, typeof (Int16)},
                                      {SqlDbType.Int, typeof (Int32)},
                                      {SqlDbType.BigInt, typeof (Int64)},
                                      {SqlDbType.Float, typeof (Double)},
                                      {SqlDbType.Decimal, typeof (Decimal)},
                                      {SqlDbType.Money, typeof (Decimal)},
                                      {SqlDbType.VarChar, typeof (String)},
                                      {SqlDbType.DateTime, typeof (DateTime)},
                                      {SqlDbType.VarBinary, typeof (Byte[])},
                                      {SqlDbType.Binary, typeof (Byte[])},
                                      {SqlDbType.Image, typeof (Byte[])},
                                      {SqlDbType.UniqueIdentifier, typeof (Guid)},
                                      {SqlDbType.Char, typeof (string)},
                                      {SqlDbType.NChar, typeof (string)},
                                      {SqlDbType.NText, typeof (string)},
                                      {SqlDbType.NVarChar, typeof (string)},
                                      {SqlDbType.Real, typeof (Single)},
                                      {SqlDbType.SmallDateTime, typeof (DateTime)},
                                      {SqlDbType.SmallMoney, typeof (decimal)},
                                      {SqlDbType.Text, typeof (string)},
                                      {SqlDbType.Timestamp, typeof (Byte[])},
                                      {SqlDbType.TinyInt, typeof (Byte)},
                                      {SqlDbType.Variant, typeof (object)}
                                  };
            }

            Type dbtype;

            try
            {
                dbtype = (Type)_sqlDTypeTable[type];
            }
            catch
            {
                dbtype = typeof(string);
            }

            return dbtype;
        }
    }

    public class ExportedFileDetails
    {
        public double Size { get; set; }
        public string Path { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string RowsCopied { get; set; }
        public double GetExecutionTime
        {
            get
            {
                return (EndTime - StartTime).TotalMinutes;
            }
        }
    }

    public static class Tools
    {
        /// <summary>
        /// Converts the bytes to megabytes.
        /// </summary>
        /// <param name="bytes">The no of bytes.</param>
        /// <returns></returns>
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        /// <summary>
        /// Finds the string in combo box.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="searchTextItem">The search text item.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int FindStringInComboBox(ComboBox comboBox, string searchTextItem, int startIndex)
        {
            if (startIndex >= comboBox.Items.Count)
                return -1;

            int indexPosition = comboBox.FindString(searchTextItem, startIndex);

            if (indexPosition <= startIndex)
                return -1;

            return comboBox.Items[indexPosition].ToString() == searchTextItem
                                    ? indexPosition
                                    : FindStringInComboBox(comboBox, searchTextItem, indexPosition);
        }

        public static bool SendEmail(ExportedFileDetails exportedFileDetails,
                                     VariableDispenser variableDispenser,
                                     Connections connections,
                                     IDTSComponentEvents componentEvents,
                                     string filePath,
                                     string from,
                                     string to,
                                     string subject,
                                     string body,
                                     string smtp)
        {
            bool retVal = false;
            bool refire = false;
            try
            {

                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                "Build the e-mail...",
                                                string.Empty, 0, ref refire);

                using (MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(EvaluateExpression(from, variableDispenser).ToString()),
                    Subject = EvaluateExpression(subject, variableDispenser).ToString(),
                    Body = EvaluateExpression(EvaluateInternalExpression(body, exportedFileDetails), variableDispenser).ToString(),
                })
                {
                    var strTo = EvaluateExpression(to, variableDispenser).ToString().Split(';');

                    foreach (string item in strTo)
                    {
                        mailMessage.To.Add(new MailAddress(item));
                    }

                    try
                    {
                        componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                        string.Format("Send e-mail using {0}", GetConnectionParameter(connections[smtp].ConnectionString, "SmtpServer")),
                                                        string.Empty, 0, ref refire);

                        SmtpClient smtpClient = new SmtpClient(GetConnectionParameter(connections[smtp].ConnectionString, "SmtpServer"))
                        {
                            EnableSsl = Convert.ToBoolean(GetConnectionParameter(connections[smtp].ConnectionString, "EnableSsl")),
                            UseDefaultCredentials = Convert.ToBoolean(GetConnectionParameter(connections[smtp].ConnectionString, "UseWindowsAuthentication"))
                        };

                        componentEvents.FireInformation(0, "SSISBulkExportTask", "Send the e-mail", string.Empty, 0, ref refire);

                        smtpClient.Send(mailMessage);

                        componentEvents.FireInformation(0, "SSISBulkExportTask", "E-mail sended successfully", string.Empty, 0, ref refire);
                    }
                    catch (Exception exception)
                    {
                        componentEvents.FireError(0, "SSISBulkExportTask", string.Format("Problem: {0} {1}", exception.Message, exception.StackTrace), "", 0);
                        retVal = false;
                    }
                }

                retVal = true;
            }
            catch (Exception exception)
            {
                componentEvents.FireError(0, "SSISBulkExportTask", string.Format("Problem: {0} {1}", exception.Message, exception.StackTrace), "", 0);
                retVal = false;
            }

            return retVal;
        }

        /// <summary>
        /// Evaluates the internal expression.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <param name="exportedFileDetails">The exported file details.</param>
        /// <returns></returns>
        public static string EvaluateInternalExpression(string sourceText, ExportedFileDetails exportedFileDetails)
        {
            PropertyInfo[] propertyInfos = typeof(ExportedFileDetails).GetProperties();
            return Keys.BodyKeysConcreteValue.Aggregate(sourceText, (current, keyValue) => current.Replace(keyValue.Key, propertyInfos.Where(key => key.Name == keyValue.Value).FirstOrDefault().GetValue(exportedFileDetails, null).ToString()));
        }

        /// <summary>
        /// This method evaluate expressions like @([System::TaskName] + [System::TaskID]) or any other operation created using
        /// ExpressionBuilder
        /// </summary>
        /// <param name="mappedParam">The mapped param.</param>
        /// <param name="variableDispenser">The variable dispenser.</param>
        /// <returns></returns>
        public static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject;

            var regex = new Regex(Keys.REGEX_EMAIL, RegexOptions.IgnoreCase);

            if (regex.IsMatch(mappedParam))
                return mappedParam;

            if (mappedParam.Contains("@"))
            {
                var expressionEvaluatorClass = new ExpressionEvaluatorClass
                {
                    Expression = mappedParam
                };

                expressionEvaluatorClass.Evaluate(DtsConvert.GetExtendedInterface(variableDispenser),
                                                  out variableObject,
                                                  false);

            }
            else
            {
                variableObject = mappedParam;
            }

            return variableObject;
        }

        public static string GetConnectionParameter(string connectionString, string parameter)
        {
            string result = string.Empty;
            parameter += "=";

            int startOf = connectionString.IndexOf(parameter);

            if (startOf != -1)
            {
                startOf += parameter.Length;
                int endOf = connectionString.IndexOf(";", startOf);
                result = connectionString.Substring(startOf, endOf - startOf);
            }

            return result;
        }
    }
}