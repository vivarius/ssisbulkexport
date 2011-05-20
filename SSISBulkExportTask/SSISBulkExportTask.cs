using System;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using DTSExecResult = Microsoft.SqlServer.Dts.Runtime.DTSExecResult;
using DTSProductLevel = Microsoft.SqlServer.Dts.Runtime.DTSProductLevel;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISBulkExportTask100.SSIS
{
    [DtsTask(
        DisplayName = "Bulk Export Task",
        UITypeName = "SSISBulkExportTask100.SSISBulkExportTaskUIInterface" +
        ",SSISBulkExportTask100," +
        "Version=1.1.0.56," +
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
        [Category("Component specific"), Description("SQL Statment")]
        public string SQLStatment { get; set; }
        [Category("Component specific"), Description("Used View")]
        public string View { get; set; }
        [Category("Component specific"), Description("Stored Procedure")]
        public object StoredProcedure { get; set; }
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
        public object TrustedConnection { get; set; }
        [Category("Component specific"), Description("Login (for untrusted connection)")]
        public string Login { get; set; }
        [Category("Component specific"), Description("Password (for untrusted connection)")]
        public string Password { get; set; }
        [Category("Component specific"), Description("Destination path")]
        public string DestinationPath { get; set; }
        [Category("Component specific"), Description("Destination path is a File connector or a variable/expression? (O/1)")]
        public string DestinationByFileConnection { get; set; }

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

            //if (string.IsNullOrEmpty(ServiceUrl))
            //{
            //    componentEvents.FireError(0, "SSISBulkExportTask", "An URL is required.", "", 0);
            //    isBaseValid = false;
            //}

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
                //componentEvents.FireInformation(0,
                //                               "SSISBulkExportTask",
                //                               string.Format("Initialize WebService: {0}", EvaluateExpression(ServiceUrl, variableDispenser)),
                //                               string.Empty,
                //                               0,
                //                               ref refire);
                //object[] result = null;

                //if (result != null)
                //{
                //    if (IsValueReturned == "1")
                //    {
                //        componentEvents.FireInformation(0,
                //                                        "SSISBulkExportTask",
                //                                        string.Format("Get the Returned Value to: {0}", ReturnedValue),
                //                                        string.Empty,
                //                                        0,
                //                                        ref refire);

                //        string val = ReturnedValue.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                //        componentEvents.FireInformation(0,
                //                                        "SSISBulkExportTask",
                //                                        string.Format("Get the Returned Value to {0} and convert to {1}",
                //                                                      val.Substring(0, val.Length - 1),
                //                                                      _vars[val.Substring(0, val.Length - 1)].DataType),
                //                                        string.Empty,
                //                                        0,
                //                                        ref refire);

                //        _vars[val.Substring(0, val.Length - 1)].Value = Convert.ChangeType(result[0], _vars[val.Substring(0, val.Length - 1)].DataType);

                //        componentEvents.FireInformation(0,
                //                                        "SSISBulkExportTask",
                //                                        string.Format("The String Result is {0} ",
                //                                                      _vars[val.Substring(0, val.Length - 1)].Value),
                //                                        string.Empty,
                //                                        0,
                //                                        ref refire);
                //    }
                //    else
                //    {
                //        componentEvents.FireInformation(0,
                //                                        "SSISBulkExportTask",
                //                                        "Execution without return or no associated return variable",
                //                                        string.Empty,
                //                                        0,
                //                                        ref refire);
                //    }

                //}

            }
            catch (Exception ex)
            {
                componentEvents.FireError(0,
                                          "SSISBulkExportTask",
                                          string.Format("Problem: {0}",
                                                        ex.Message + "\n" + ex.StackTrace),
                                          "",
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
                //    var param = ServiceUrl;

                //    componentEvents.FireInformation(0, "SSISBulkExportTask", "ServiceUrl = " + ServiceUrl, string.Empty, 0, ref refire);

                //    if (param.Contains("@"))
                //    {
                //        var regexStr = param.Split('@');

                //        foreach (var nexSplitedVal in regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                //        {
                //            try
                //            {
                //                componentEvents.FireInformation(0, "SSISBulkExportTask", nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')), string.Empty, 0, ref refire);
                //                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                //            }
                //            catch (Exception exception)
                //            {
                //                throw new Exception(exception.Message);
                //            }
                //        }
                //    }
                //}
                //catch
                //{
                //    //We will continue...
                //}

                //try
                //{
                //    var param = Service;

                //    componentEvents.FireInformation(0, "SSISBulkExportTask", "Service = " + Service, string.Empty, 0, ref refire);

                //    if (param.Contains("@"))
                //    {
                //        var regexStr = param.Split('@');

                //        foreach (var nexSplitedVal in regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                //        {
                //            try
                //            {
                //                componentEvents.FireInformation(0, "SSISBulkExportTask", nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')), string.Empty, 0, ref refire);
                //                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                //            }
                //            catch (Exception exception)
                //            {
                //                throw new Exception(exception.Message);
                //            }
                //        }
                //    }
                //}
                //catch (Exception exception)
                //{
                //    throw new Exception(exception.Message);
                //}

                //try
                //{
                //    var param = WebMethod;

                //    componentEvents.FireInformation(0, "SSISBulkExportTask", "WebMethod = " + WebMethod, string.Empty, 0, ref refire);

                //    if (param.Contains("@"))
                //    {
                //        var regexStr = param.Split('@');

                //        foreach (var nexSplitedVal in regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                //        {
                //            try
                //            {
                //                componentEvents.FireInformation(0, "SSISBulkExportTask", nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')), string.Empty, 0, ref refire);
                //                variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                //            }
                //            catch (Exception exception)
                //            {
                //                throw new Exception(exception.Message);
                //            }
                //        }
                //    }
                //}
                //catch (Exception exception)
                //{
                //    throw new Exception(exception.Message);
                //}

                //try
                //{

                //    if (!string.IsNullOrEmpty(ReturnedValue))
                //    {
                //        var param = ReturnedValue;

                //        componentEvents.FireInformation(0, "SSISBulkExportTask", "ReturnedValue = " + ReturnedValue,
                //                                        string.Empty, 0, ref refire);

                //        if (param.Contains("@"))
                //        {
                //            var regexStr = param.Split('@');

                //            foreach (var nexSplitedVal in regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                //            {
                //                try
                //                {
                //                    componentEvents.FireInformation(0, "SSISBulkExportTask",
                //                                                    nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')),
                //                                                    string.Empty, 0, ref refire);
                //                    variableDispenser.LockForWrite(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                //                }
                //                catch (Exception exception)
                //                {
                //                    throw new Exception(exception.Message);
                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception exception)
                //{
                //    throw new Exception(exception.Message);
                //}

                //try
                //{
                //    componentEvents.FireInformation(0, "SSISBulkExportTask", "MappingParams ", string.Empty, 0, ref refire);

                //    //Get variables for MappingParams
                //    foreach (var mappingParams in (MappingParams)MappingParams)
                //    {

                //        try
                //        {
                //            if (mappingParams.Value.Contains("@"))
                //            {
                //                var regexStr = mappingParams.Value.Split('@');

                //                foreach (var nexSplitedVal in
                //                        regexStr.Where(val => val.Trim().Length != 0).Select(strVal => strVal.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)))
                //                {
                //                    try
                //                    {
                //                        componentEvents.FireInformation(0, "SSISBulkExportTask", nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')), string.Empty, 0, ref refire);
                //                        variableDispenser.LockForRead(nexSplitedVal[1].Remove(nexSplitedVal[1].IndexOf(']')));
                //                    }
                //                    catch (Exception exception)
                //                    {
                //                        throw new Exception(exception.Message);
                //                    }
                //                }
                //            }
                //        }
                //        catch (Exception exception)
                //        {
                //            throw new Exception(exception.Message);
                //        }
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

            XmlAttribute dataSource = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            dataSource.Value = DataSource;

            XmlAttribute sqlStatment = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            sqlStatment.Value = SQLStatment;

            XmlAttribute view = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            view.Value = View;

            XmlAttribute storedProcedure = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            storedProcedure.Value = StoredProcedure;

            XmlAttribute tables = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            tables.Value = Tables;

            XmlAttribute firstRow = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            firstRow.Value = FirstRow;

            XmlAttribute lastRow = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            lastRow.Value = LastRow;

            XmlAttribute fieldTermiantor = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            fieldTermiantor.Value = FieldTermiantor;

            XmlAttribute rowTermiantor = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            rowTermiantor.Value = RowTermiantor;

            XmlAttribute nativeDatabaseDataType = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            nativeDatabaseDataType.Value = NativeDatabaseDataType;


            XmlAttribute trustedConnection = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            trustedConnection.Value = TrustedConnection;

            XmlAttribute login = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            login.Value = Login;

            XmlAttribute password = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            password.Value = Password;

            XmlAttribute destinationPath = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            destinationPath.Value = DestinationPath;

            XmlAttribute destinationByFileConnection = doc.CreateAttribute(string.Empty, Keys.SQL_SERVER, string.Empty);
            destinationByFileConnection.Value = DestinationByFileConnection;

            taskElement.Attributes.Append(sqlServer);
            taskElement.Attributes.Append(dataSource);
            taskElement.Attributes.Append(sqlStatment);
            taskElement.Attributes.Append(view);
            taskElement.Attributes.Append(storedProcedure);
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

            doc.AppendChild(taskElement);
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="infoEvents">The info events.</param>
        void IDTSComponentPersist.LoadFromXML(XmlElement node, IDTSInfoEvents infoEvents)
        {
            //if (node.Name != "SSISBulkExportTask")
            //{
            //    throw new Exception("Wrong node name");
            //}

            //try
            //{
            //    ServiceUrl = node.Attributes.GetNamedItem(Keys.SQL_SERVER).Value;
            //    Service = node.Attributes.GetNamedItem(Keys.SQL_STATEMENT).Value;
            //    WebMethod = node.Attributes.GetNamedItem(Keys.WEBMETHOD).Value;
            //    MappingParams = Serializer.DeSerializeFromXmlString(typeof(MappingParams), node.Attributes.GetNamedItem(Keys.MAPPING_PARAMS).Value);
            //    ReturnedValue = node.Attributes.GetNamedItem(Keys.RETURNED_VALUE).Value;
            //    IsValueReturned = node.Attributes.GetNamedItem(Keys.IS_VALUE_RETURNED).Value;
            //}
            //catch
            //{
            //    throw new Exception("Unexpected task element when loading task.");
            //}
        }

        #endregion
    }
}

