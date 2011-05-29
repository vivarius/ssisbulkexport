using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using DTSExecResult = Microsoft.SqlServer.Dts.Runtime.DTSExecResult;
using DTSProductLevel = Microsoft.SqlServer.Dts.Runtime.DTSProductLevel;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISBulkExportTask100.SSIS
{
    /// <summary>
    /// 
    /// </summary>
    [DtsTask(
        DisplayName = "Bulk Export Task",
        UITypeName = "SSISBulkExportTask100.SSISBulkExportTaskUIInterface" +
        ",SSISBulkExportTask100," +
        "Version=1.1.0.49," +
        "Culture=Neutral," +
        "PublicKeyToken=7660ecf4382af446",
        IconResource = "SSISBulkExportTask100.DownloadIcon.ico",
        TaskContact = "cosmin.vlasiu@gmail.com",
        RequiredProductLevel = DTSProductLevel.None
        )]
    public class SSISBulkExportTask : Task, IDTSComponentPersist
    {
        #region Constructor
        public SSISBulkExportTask()
        {
        }

        #endregion

        #region Public Properties
        [Category("Component specific"), Description("SQL Server Instance")]
        public string SQLServerInstance { get; set; }
        [Category("Component specific"), Description("Data Source: SQL Statement / View / Stored Procedure / Direct Table")]
        public string DataSource { get; set; }
        [Category("Component specific"), Description("Database")]
        public string Database { get; set; }
        [Category("Component specific"), Description("SQL Statment")]
        public string SQLStatment { get; set; }
        [Category("Component specific"), Description("Used View")]
        public string View { get; set; }
        [Category("Component specific"), Description("Stored Procedure")]
        public string StoredProcedure { get; set; }
        [Category("Component specific"), Description("Stored Procedure's parameters")]
        public object StoredProcedureParameters { get; set; }
        [Category("Component specific"), Description("Table")]
        public string Tables { get; set; }
        [Category("Component specific"), Description("First row")]
        public string FirstRow { get; set; }
        [Category("Component specific"), Description("Last row")]
        public string LastRow { get; set; }
        [Category("Component specific"), Description("Field terminator")]
        public string FieldTermiantor { get; set; }
        [Category("Component specific"), Description("Row terminator")]
        public string RowTermiantor { get; set; }
        [Category("Component specific"), Description("Use native database data type")]
        public string NativeDatabaseDataType { get; set; }
        [Category("Component specific"), Description("Trusted connection")]
        public string TrustedConnection { get; set; }
        [Category("Component specific"), Description("Login (for untrusted connection)")]
        public string Login { get; set; }
        [Category("Component specific"), Description("Password (for untrusted connection)")]
        public string Password { get; set; }
        [Category("Component specific"), Description("Destination path")]
        public string DestinationPath { get; set; }
        [Category("Component specific"), Description("Destination path is a File connector or a variable/expression? (true/false)")]
        public string DestinationByFileConnection { get; set; }
        [Category("Component specific"), Description("Format file")]
        public string FormatFile { get; set; }
        [Category("Component specific"), Description("Format file path is a File connector or a variable/expression? (true/false)")]
        public string FormatFileByFileConnection { get; set; }
        [Category("Component specific"), Description("Enable Command Shell? (true/false)")]
        public string ActivateCmdShell { get; set; }
        [Category("Component specific"), Description("Specifies the code page of the data in the data file. code_page is relevant only if the data contains char, varchar, or text columns with character values greater than 127 or less than 32.")]
        public string CodePage { get; set; }
        [Category("Component specific"), Description("Specifies the number of bytes, per network packet, sent to and from the server. A server configuration option can be set by using SQL Server Management Studio (or the sp_configure system stored procedure). However, the server configuration option can be overridden on an individual basis by using this option. packet_size can be from 4096 to 65535 bytes; the default is 4096.")]
        public string NetworkPacketSize { get; set; }
        [Category("Component specific"), Description("Specifies that currency, date, and time data is bulk copied into SQL Server using the regional format defined for the locale setting of the client computer. By default, regional settings are ignored.")]
        public string UseRegionalSettings { get; set; }
        [Category("Component specific"), Description(@"Executes the SET QUOTED_IDENTIFIERS ON statement in the connection between the bcp utility and an instance of SQL Server. Use this option to specify a database, owner, table, or view name that contains a space or a single quotation mark. Enclose the entire three-part table or view name in quotation marks ("").")]
        public string SET_QUOTED_IDENTIFIERS_ON { get; set; }
        [Category("Component specific"), Description("Performs the bulk copy operation using Unicode characters. This option does not prompt for each field; it uses nchar as the storage type, no prefixes, \t (tab character) as the field separator, and \n (newline character) as the row terminator.")]
        public string UseUnicodeCharacters { get; set; }
        [Category("Component specific"), Description("Performs the operation using a character data type. This option does not prompt for each field; it uses char as the storage type, without prefixes and with \t (tab character) as the field separator and \r\n (newline character) as the row terminator.")]
        public string UseCharacterDataType { get; set; }
        #endregion

        #region Private Properties

        Variables _vars = null;

        #endregion

        #region Validate

        /// <summary>
        /// Validate local parameters
        /// </summary>
        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            bool isBaseValid = true;

            if (base.Validate(connections, variableDispenser, componentEvents, log) != DTSExecResult.Success)
            {
                componentEvents.FireError(0, "SSISBulkExportTask", "Base validation failed", "", 0);
                isBaseValid = false;
            }

            if (string.IsNullOrEmpty(SQLServerInstance))
            {
                componentEvents.FireError(0, "SSISBulkExportTask", "A SQL Server Connection is mandatory", "", 0);
                isBaseValid = false;
            }

            if (string.IsNullOrEmpty(DestinationPath))
            {
                componentEvents.FireError(0, "SSISBulkExportTask", "Destination Path is mandatory ", "", 0);
                isBaseValid = false;
            }

            if (string.IsNullOrEmpty(SQLStatment) &&
                string.IsNullOrEmpty(View) &&
                string.IsNullOrEmpty(Tables) &&
                string.IsNullOrEmpty(StoredProcedure))
            {
                componentEvents.FireError(0, "SSISBulkExportTask", "You must specify an SQL Select Statelment / a View name / a Stored Procedure name / a Table Name", "", 0);
                isBaseValid = false;
            }

            return isBaseValid ? DTSExecResult.Success : DTSExecResult.Failure;
        }

        #endregion

        #region Execute

        /// <summary>
        /// This method is a run-time method executed dtsexec.exe
        /// </summary>
        /// <param name="connections"></param>
        /// <param name="variableDispenser"></param>
        /// <param name="componentEvents"></param>
        /// <param name="log"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            bool refire = false;

            componentEvents.FireInformation(0,
                                            "SSISBulkExportTask",
                                            "Prepare variables",
                                            string.Empty,
                                            0,
                                            ref refire);

            GetNeededVariables(variableDispenser, componentEvents);

            try
            {
                componentEvents.FireInformation(0,
                                               "SSISBulkExportTask",
                                               string.Format("Start BULK Export from server: {0}", EvaluateExpression(SQLServerInstance, variableDispenser)),
                                               string.Empty,
                                               0,
                                               ref refire);

                if (ActivateCmdShell == Keys.TRUE)
                {
                    componentEvents.FireInformation(0,
                               "SSISBulkExportTask",
                               "Enable Command Shell",
                               string.Empty,
                               0,
                               ref refire);
                    ExecuteNonSQL(QueryResources.ENABLE_CMDSHELL, variableDispenser, connections, 10);
                }

                componentEvents.FireInformation(0,
                               "SSISBulkExportTask",
                               "Prepare the BCP Bulk Command",
                               string.Empty,
                               0,
                               ref refire);

                var bcp = new BCP(variableDispenser, connections)
                              {
                                  SQLServerInstance = SQLServerInstance,
                                  DataSource = DataSource,
                                  Database = Database,
                                  SQLStatment = SQLStatment,
                                  View = View,
                                  StoredProcedure = StoredProcedure,
                                  StoredProcedureParameters = StoredProcedureParameters,
                                  Tables = Tables,
                                  FirstRow = FirstRow,
                                  LastRow = LastRow,
                                  FieldTermiantor = FieldTermiantor,
                                  RowTermiantor = RowTermiantor,
                                  NativeDatabaseDataType = NativeDatabaseDataType,
                                  TrustedConnection = TrustedConnection,
                                  Login = Login,
                                  Password = Password,
                                  DestinationPath = DestinationPath,
                                  DestinationByFileConnection = DestinationByFileConnection,
                                  FormatFile = FormatFile,
                                  FormatFileByFileConnection = FormatFileByFileConnection,
                                  ActivateCmdShell = ActivateCmdShell,
                                  CodePage = CodePage,
                                  NetworkPacketSize = NetworkPacketSize,
                                  UseRegionalSettings = UseRegionalSettings,
                                  SET_QUOTED_IDENTIFIERS_ON = SET_QUOTED_IDENTIFIERS_ON,
                                  UseUnicodeCharacters = UseUnicodeCharacters,
                                  UseCharacterDataType = UseCharacterDataType
                              };

                string commandBulkExport = bcp.ToString();

                componentEvents.FireInformation(0,
                               "SSISBulkExportTask",
                               string.Format("Command To execute: {0}", commandBulkExport),
                               string.Empty,
                               0,
                               ref refire);

                List<string> result = ExecuteReader(commandBulkExport, variableDispenser, connections, 0);

                componentEvents.FireInformation(0,
                               "SSISBulkExportTask",
                               "Execution response:",
                               string.Empty,
                               0,
                               ref refire);

                foreach (var item in result)
                {
                    if (item.Trim().StartsWith("Error"))
                        componentEvents.FireError(0,
                                         "SSISBulkExportTask",
                                         item.Trim(),
                                         string.Empty,
                                         0);
                    else
                        componentEvents.FireInformation(0,
                                   "SSISBulkExportTask",
                                   item.Trim(),
                                   string.Empty,
                                   0,
                                   ref refire);
                }

                componentEvents.FireInformation(0,
                                               "SSISBulkExportTask",
                                               "The Bulk export has been succesfully done!",
                                               string.Empty,
                                               0,
                                               ref refire);

            }
            catch (Exception ex)
            {
                componentEvents.FireError(0,
                                          "SSISBulkExportTask",
                                          string.Format("Problem: {0}",
                                                        ex.Message + "\n" + ex.StackTrace),
                                          string.Empty,
                                          0);
            }
            finally
            {
                if (_vars.Locked)
                {
                    _vars.Unlock();
                }
            }

            return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the non SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="variableDispenser">The variable dispenser.</param>
        /// <param name="connections">The connections.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public int ExecuteNonSQL(string sql, VariableDispenser variableDispenser, Connections connections, int commandTimeOut)
        {
            int retVal;

            using (var sqlConnection = new SqlConnection(connections[SQLServerInstance].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sql, sqlConnection)
                                                   {
                                                       CommandTimeout = commandTimeOut
                                                   })
                {
                    retVal = sqlCommand.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }

            return retVal;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="variableDispenser">The variable dispenser.</param>
        /// <param name="connections">The connections.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        public List<string> ExecuteReader(string sql, VariableDispenser variableDispenser, Connections connections, int commandTimeOut)
        {
            var retVal = new List<string>();

            using (var sqlConnection = new SqlConnection(connections[SQLServerInstance].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sql, sqlConnection)
                {
                    CommandTimeout = commandTimeOut
                })
                {
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader != null)
                            while (sqlDataReader.Read())
                            {
                                if (!sqlDataReader.IsDBNull(0))
                                    retVal.Add(sqlDataReader.GetString(0));
                            }
                    }
                }

                sqlConnection.Close();
            }

            return retVal;
        }

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
        /// Gets the needed variables.
        /// </summary>
        /// <param name="variableDispenser">The variable dispenser.</param>
        /// <param name="componentEvents">The component events.</param>
        private void GetNeededVariables(VariableDispenser variableDispenser, IDTSComponentEvents componentEvents)
        {
            bool refire = false;

            try
            {
                {
                    var param = SQLServerInstance;

                    componentEvents.FireInformation(0, "SSISBulkExportTask", "SQLServerInstance = " + SQLServerInstance, string.Empty,
                                                    0, ref refire);

                    if (param.Contains("@"))
                    {
                        var regexStr = param.Split('@');

                        foreach (
                            var nexSplitedVal in
                                regexStr.Where(val => val.Trim().Length != 0).Select(
                                    strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                        {
                            try
                            {
                                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                                nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                                                                string.Empty, 0, ref refire);
                                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }

                }

                {
                    var param = Login;

                    componentEvents.FireInformation(0, "SSISBulkExportTask", "Login = " + Login, string.Empty,
                                                    0, ref refire);

                    if (param.Contains("@"))
                    {
                        var regexStr = param.Split('@');

                        foreach (
                            var nexSplitedVal in
                                regexStr.Where(val => val.Trim().Length != 0).Select(
                                    strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                        {
                            try
                            {
                                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                                nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                                                                string.Empty, 0, ref refire);
                                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }

                }

                {
                    var param = Password;

                    componentEvents.FireInformation(0, "SSISBulkExportTask", "Password = " + Password, string.Empty,
                                                    0, ref refire);

                    if (param.Contains("@"))
                    {
                        var regexStr = param.Split('@');

                        foreach (
                            var nexSplitedVal in
                                regexStr.Where(val => val.Trim().Length != 0).Select(
                                    strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                        {
                            try
                            {
                                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                                nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                                                                string.Empty, 0, ref refire);
                                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }
                }

                {
                    var param = Password;

                    componentEvents.FireInformation(0, "SSISBulkExportTask", "Destination = " + DestinationPath, string.Empty,
                                                    0, ref refire);

                    if (param.Contains("@"))
                    {
                        var regexStr = param.Split('@');

                        foreach (
                            var nexSplitedVal in
                                regexStr.Where(val => val.Trim().Length != 0).Select(
                                    strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                        {
                            try
                            {
                                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                                nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                                                                string.Empty, 0, ref refire);
                                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }
                }

                {
                    var param = Password;

                    componentEvents.FireInformation(0, "SSISBulkExportTask", "Format File = " + FormatFile, string.Empty,
                                                    0, ref refire);

                    if (param.Contains("@"))
                    {
                        var regexStr = param.Split('@');

                        foreach (
                            var nexSplitedVal in
                                regexStr.Where(val => val.Trim().Length != 0).Select(
                                    strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                        {
                            try
                            {
                                componentEvents.FireInformation(0, "SSISBulkExportTask",
                                                                nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                                                                string.Empty, 0, ref refire);
                                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }
                }

                {
                    foreach (var mappingParams in (MappingParams)StoredProcedureParameters)
                    {
                        try
                        {
                            if (mappingParams.Value.Contains("@"))
                            {
                                var regexStr = mappingParams.Value.Split('@');

                                foreach (var nexSplitedVal in
                                        regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                                {
                                    try
                                    {
                                        componentEvents.FireInformation(0, "SSISBulkExportTask", nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')), string.Empty, 0, ref refire);
                                        variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch
                        {
                            //oops, it's a fix value
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                componentEvents.FireError(0, "SSISReportGeneratorTask", string.Format("Problem MappingParams: {0} {1}", ex.Message, ex.StackTrace), "", 0);
            }

            variableDispenser.GetVariables(ref _vars);
        }

        #endregion

        #region Implementation of IDTSComponentPersist

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="infoEvents">The info events.</param>
        void IDTSComponentPersist.SaveToXML(XmlDocument doc, IDTSInfoEvents infoEvents)
        {
            XmlElement taskElement = doc.CreateElement(string.Empty, "SSISBulkExportTask", string.Empty);

            XmlAttribute sqlServer = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            sqlServer.Value = SQLServerInstance;

            XmlAttribute dataSource = doc.CreateAttribute(string.Empty, Keys.DATA_SOURCE, string.Empty);
            dataSource.Value = DataSource;

            XmlAttribute dataBase = doc.CreateAttribute(string.Empty, Keys.SQL_DATABASE, string.Empty);
            dataBase.Value = Database;

            XmlAttribute sqlStatment = doc.CreateAttribute(string.Empty, Keys.SQL_STATEMENT, string.Empty);
            sqlStatment.Value = SQLStatment;

            XmlAttribute view = doc.CreateAttribute(string.Empty, Keys.SQL_VIEW, string.Empty);
            view.Value = View;

            XmlAttribute storedProcedure = doc.CreateAttribute(string.Empty, Keys.SQL_StoredProcedure, string.Empty);
            storedProcedure.Value = StoredProcedure;

            XmlAttribute storedProcedureParameters = doc.CreateAttribute(string.Empty, Keys.SQL_STORED_PROCEDURE_PARAMS, string.Empty);
            storedProcedureParameters.Value = Serializer.SerializeToXmlString(StoredProcedureParameters);

            XmlAttribute tables = doc.CreateAttribute(string.Empty, Keys.SQL_TABLE, string.Empty);
            tables.Value = Tables;

            XmlAttribute firstRow = doc.CreateAttribute(string.Empty, Keys.FIRSTROW, string.Empty);
            firstRow.Value = FirstRow;

            XmlAttribute lastRow = doc.CreateAttribute(string.Empty, Keys.LASTROW, string.Empty);
            lastRow.Value = LastRow;

            XmlAttribute fieldTermiantor = doc.CreateAttribute(string.Empty, Keys.FIELD_TERMINATOR, string.Empty);
            fieldTermiantor.Value = FieldTermiantor;

            XmlAttribute rowTermiantor = doc.CreateAttribute(string.Empty, Keys.ROW_TERMINATOR, string.Empty);
            rowTermiantor.Value = RowTermiantor;

            XmlAttribute nativeDatabaseDataType = doc.CreateAttribute(string.Empty, Keys.NATIVE_DB_DATATYPE, string.Empty);
            nativeDatabaseDataType.Value = NativeDatabaseDataType;

            XmlAttribute trustedConnection = doc.CreateAttribute(string.Empty, Keys.TRUSTED_CONNECTION, string.Empty);
            trustedConnection.Value = TrustedConnection;

            XmlAttribute login = doc.CreateAttribute(string.Empty, Keys.SRV_LOGIN, string.Empty);
            login.Value = Login;

            XmlAttribute password = doc.CreateAttribute(string.Empty, Keys.SRV_PASSWORD, string.Empty);
            password.Value = Password;

            XmlAttribute destinationPath = doc.CreateAttribute(string.Empty, Keys.DESTINATION, string.Empty);
            destinationPath.Value = DestinationPath;

            XmlAttribute destinationByFileConnection = doc.CreateAttribute(string.Empty, Keys.DESTINATION_FILE_CONNECTION, string.Empty);
            destinationByFileConnection.Value = DestinationByFileConnection;

            XmlAttribute formatFile = doc.CreateAttribute(string.Empty, Keys.FORMAT_FILE, string.Empty);
            formatFile.Value = FormatFile;

            XmlAttribute formatFileConnection = doc.CreateAttribute(string.Empty, Keys.FORMAT_FILE_CONNECTION, string.Empty);
            formatFileConnection.Value = FormatFileByFileConnection;

            XmlAttribute activateCmdShell = doc.CreateAttribute(string.Empty, Keys.ACTIVATE_CMDSHELL, string.Empty);
            activateCmdShell.Value = ActivateCmdShell;

            XmlAttribute codePage = doc.CreateAttribute(string.Empty, Keys.CODE_PAGE, string.Empty);
            codePage.Value = CodePage;

            XmlAttribute networkPacketSize = doc.CreateAttribute(string.Empty, Keys.NETWORK_PACKET_SIZE, string.Empty);
            networkPacketSize.Value = NetworkPacketSize;

            XmlAttribute useRegionalSettings = doc.CreateAttribute(string.Empty, Keys.USE_REGIONAL_SETTINGS, string.Empty);
            useRegionalSettings.Value = UseRegionalSettings;

            XmlAttribute setQuotedIdentifiersOn = doc.CreateAttribute(string.Empty, Keys.SET_QUOTED_IDENTIFIERS_ON, string.Empty);
            setQuotedIdentifiersOn.Value = SET_QUOTED_IDENTIFIERS_ON;

            XmlAttribute useUnicodeChr = doc.CreateAttribute(string.Empty, Keys.UNICODE_CHR, string.Empty);
            useUnicodeChr.Value = UseUnicodeCharacters;

            XmlAttribute characterDataType = doc.CreateAttribute(string.Empty, Keys.CHARACTER_DATA_TYPE, string.Empty);
            characterDataType.Value = UseCharacterDataType;

            taskElement.Attributes.Append(sqlServer);
            taskElement.Attributes.Append(dataSource);
            taskElement.Attributes.Append(dataBase);
            taskElement.Attributes.Append(sqlStatment);
            taskElement.Attributes.Append(view);
            taskElement.Attributes.Append(storedProcedure);
            taskElement.Attributes.Append(storedProcedureParameters);
            taskElement.Attributes.Append(tables);
            taskElement.Attributes.Append(firstRow);
            taskElement.Attributes.Append(lastRow);
            taskElement.Attributes.Append(fieldTermiantor);
            taskElement.Attributes.Append(rowTermiantor);
            taskElement.Attributes.Append(nativeDatabaseDataType);
            taskElement.Attributes.Append(trustedConnection);
            taskElement.Attributes.Append(login);
            taskElement.Attributes.Append(password);
            taskElement.Attributes.Append(destinationPath);
            taskElement.Attributes.Append(destinationByFileConnection);
            taskElement.Attributes.Append(formatFile);
            taskElement.Attributes.Append(formatFileConnection);
            taskElement.Attributes.Append(activateCmdShell);
            taskElement.Attributes.Append(codePage);
            taskElement.Attributes.Append(networkPacketSize);
            taskElement.Attributes.Append(useRegionalSettings);
            taskElement.Attributes.Append(setQuotedIdentifiersOn);
            taskElement.Attributes.Append(useUnicodeChr);
            taskElement.Attributes.Append(characterDataType);


            doc.AppendChild(taskElement);
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="infoEvents">The info events.</param>
        void IDTSComponentPersist.LoadFromXML(XmlElement node, IDTSInfoEvents infoEvents)
        {
            if (node.Name != "SSISBulkExportTask")
            {
                throw new Exception("Wrong node name");
            }

            try
            {
                SQLServerInstance = node.Attributes.GetNamedItem(Keys.SQL_SERVER).Value;
                DataSource = node.Attributes.GetNamedItem(Keys.DATA_SOURCE).Value;
                Database = node.Attributes.GetNamedItem(Keys.SQL_DATABASE).Value;
                SQLStatment = node.Attributes.GetNamedItem(Keys.SQL_STATEMENT).Value;
                View = node.Attributes.GetNamedItem(Keys.SQL_VIEW).Value;
                StoredProcedure = node.Attributes.GetNamedItem(Keys.SQL_StoredProcedure).Value;
                StoredProcedureParameters = Serializer.DeSerializeFromXmlString(typeof(MappingParams), node.Attributes.GetNamedItem(Keys.SQL_STORED_PROCEDURE_PARAMS).Value);
                Tables = node.Attributes.GetNamedItem(Keys.SQL_TABLE).Value;
                FirstRow = node.Attributes.GetNamedItem(Keys.FIRSTROW).Value;
                LastRow = node.Attributes.GetNamedItem(Keys.LASTROW).Value;
                FieldTermiantor = node.Attributes.GetNamedItem(Keys.FIELD_TERMINATOR).Value;
                RowTermiantor = node.Attributes.GetNamedItem(Keys.ROW_TERMINATOR).Value;
                NativeDatabaseDataType = node.Attributes.GetNamedItem(Keys.NATIVE_DB_DATATYPE).Value;
                TrustedConnection = node.Attributes.GetNamedItem(Keys.TRUSTED_CONNECTION).Value;
                Login = node.Attributes.GetNamedItem(Keys.SRV_LOGIN).Value;
                Password = node.Attributes.GetNamedItem(Keys.SRV_PASSWORD).Value;
                DestinationPath = node.Attributes.GetNamedItem(Keys.DESTINATION).Value;
                DestinationByFileConnection = node.Attributes.GetNamedItem(Keys.DESTINATION_FILE_CONNECTION).Value;
                FormatFile = node.Attributes.GetNamedItem(Keys.FORMAT_FILE).Value;
                FormatFileByFileConnection = node.Attributes.GetNamedItem(Keys.FORMAT_FILE_CONNECTION).Value;
                ActivateCmdShell = node.Attributes.GetNamedItem(Keys.ACTIVATE_CMDSHELL).Value;
                CodePage = node.Attributes.GetNamedItem(Keys.CODE_PAGE).Value;
                NetworkPacketSize = node.Attributes.GetNamedItem(Keys.NETWORK_PACKET_SIZE).Value;
                UseRegionalSettings = node.Attributes.GetNamedItem(Keys.USE_REGIONAL_SETTINGS).Value;
                UseCharacterDataType = node.Attributes.GetNamedItem(Keys.CHARACTER_DATA_TYPE).Value;
                UseUnicodeCharacters = node.Attributes.GetNamedItem(Keys.UNICODE_CHR).Value;
                SET_QUOTED_IDENTIFIERS_ON = node.Attributes.GetNamedItem(Keys.SET_QUOTED_IDENTIFIERS_ON).Value;
            }
            catch
            {
                throw new Exception("Unexpected task element when loading task.");
            }
        }

        #endregion
    }
}

