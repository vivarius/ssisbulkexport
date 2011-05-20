﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Microsoft.DataTransformationServices.Controls;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using TaskHost = Microsoft.SqlServer.Dts.Runtime.TaskHost;
using Variable = Microsoft.SqlServer.Dts.Runtime.Variable;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISBulkExportTask100
{
    public partial class frmEditProperties : Form
    {
        #region Private Properties
        private readonly TaskHost _taskHost;
        private readonly Connections _connections;
        private readonly Dictionary<string, string> _paramsManager = new Dictionary<string, string>();
        private bool _isFirstLoad = false;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>The variables.</value>
        private Variables Variables
        {
            get { return _taskHost.Variables; }
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <value>The connections.</value>
        private Connections Connections
        {
            get { return _connections; }
        }

        #endregion

        #region .ctor
        public frmEditProperties(TaskHost taskHost, Connections connections)
        {
            InitializeComponent();

            _taskHost = taskHost;
            _isFirstLoad = true;

            try
            {
                Cursor = Cursors.WaitCursor;

                LoadDBConnections();

                cmbSQLServer.SelectedIndex = FindStringInComboBox(cmbSQLServer, _taskHost.Properties[Keys.SQL_SERVER].GetValue(_taskHost).ToString(), -1);

                txFirstRow.Text = _taskHost.Properties[Keys.FIRSTROW].GetValue(_taskHost).ToString();
                txLastRow.Text = _taskHost.Properties[Keys.LASTROW].GetValue(_taskHost).ToString();
                txFieldTerminator.Text = _taskHost.Properties[Keys.FIELD_TERMINATOR].GetValue(_taskHost).ToString();
                txRowTerminator.Text = _taskHost.Properties[Keys.ROW_TERMINATOR].GetValue(_taskHost).ToString();

                txSQL.Text = _taskHost.Properties[Keys.SQL_STATEMENT].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.DESTINATION_FILE_CONNECTION].GetValue(_taskHost).ToString() == Keys.TRUE)
                    LoadFileConnections();
                else
                {
                    string selItem = string.Empty;
                    cmbDestination = LoadVariables("System.String", ref selItem);
                }

                if (_taskHost.Properties[Keys.TRUSTED_CONNECTION].GetValue(_taskHost).ToString() == Keys.TRUE)
                {
                    cmbLogin.Items.Clear();
                    cmbPassword.Items.Clear();
                }
                else
                {
                    string selItem = string.Empty;
                    cmbPassword = cmbLogin = LoadVariables("System.String", ref selItem);
                }

                LoadDataBaseObjects();

                cmbViews.SelectedIndex = FindStringInComboBox(cmbViews, _taskHost.Properties[Keys.SQL_VIEW].GetValue(_taskHost).ToString(), -1);
                cmbStoredProcedures.SelectedIndex = FindStringInComboBox(cmbStoredProcedures, _taskHost.Properties[Keys.SQL_StoredProcedure].GetValue(_taskHost).ToString(), -1);
                cmbTables.SelectedIndex = FindStringInComboBox(cmbTables, _taskHost.Properties[Keys.SQL_TABLE].GetValue(_taskHost).ToString(), -1);

                cmbDestination.SelectedIndex = FindStringInComboBox(cmbDestination, _taskHost.Properties[Keys.DESTINATION].GetValue(_taskHost).ToString(), -1);
                cmbLogin.SelectedIndex = FindStringInComboBox(cmbLogin, _taskHost.Properties[Keys.SRV_LOGIN].GetValue(_taskHost).ToString(), -1);
                cmbPassword.SelectedIndex = FindStringInComboBox(cmbDestination, _taskHost.Properties[Keys.SRV_PASSWORD].GetValue(_taskHost).ToString(), -1);
                _isFirstLoad = false;
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show(exception.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }


        #endregion

        #region Methods

        private ComboBox LoadVariables(string parameterInfo, ref string selectedText)
        {
            var comboBox = new ComboBox();

            comboBox.Items.Add(string.Empty);

            foreach (Variable variable in Variables.Cast<Variable>().Where(variable => Type.GetTypeCode(Type.GetType(parameterInfo)) == variable.DataType))
            {
                comboBox.Items.Add(string.Format("@[{0}::{1}]", variable.Namespace, variable.Name));
            }

            return comboBox;
        }

        public int FindStringInComboBox(ComboBox comboBox, string searchTextItem, int startIndex)
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

        private static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject;

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

        private void LoadFileConnections()
        {
            cmbDestination.Items.Clear();

            foreach (var connection in Connections)
            {
                cmbDestination.Items.Add(connection.Name);
            }
        }

        private void LoadDBConnections()
        {
            cmbSQLServer.Items.Clear();

            foreach (var connection in Connections)
            {
                cmbSQLServer.Items.Add(connection.Name);
            }
        }

        private void LoadFileVariables()
        {
            cmbDestination.Items.Clear();
            cmbDestination.Items.AddRange(LoadVariables("System.String").ToArray());
        }

        private List<string> LoadVariables(string parameterInfo)
        {
            return Variables.Cast<Variable>().Where(variable => Type.GetTypeCode(Type.GetType(parameterInfo)) == variable.DataType).Select(variable => string.Format("@[{0}::{1}]", variable.Namespace, variable.Name)).ToList();
        }

        private void LoadDataBaseObjects()
        {
            Cursor = Cursors.WaitCursor;

            using (var sqlConnection = new SqlConnection(EvaluateExpression(_connections[cmbSQLServer.Text].ConnectionString, _taskHost.VariableDispenser).ToString()))
            {
                {
                    using (var sqlCommand = new SqlCommand(QueryResources.TABLES, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbTables.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbTables.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }

                {
                    using (var sqlCommand = new SqlCommand(QueryResources.VIEWS, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbViews.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbViews.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }

                {
                    using (var sqlCommand = new SqlCommand(QueryResources.STORED_PROCEDURES, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbStoredProcedures.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbStoredProcedures.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }
            }

            Cursor = Cursors.Arrow;
        }

        private void LoadStoredProcedureParameters(string schema, string storedProcedureName)
        {
            Cursor = Cursors.WaitCursor;

            using (var sqlConnection = new SqlConnection(EvaluateExpression(_connections[cmbSQLServer.Text].ConnectionString, _taskHost.VariableDispenser).ToString()))
            {
                using (var sqlCommand = new SqlCommand(string.Format(QueryResources.STORED_PROCEDURE_PARAMETERS, schema, storedProcedureName), sqlConnection))
                {
                    using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                    {
                        grdParameters.Rows.Clear();

                        if (sqlDataReaderTables != null)
                            while (sqlDataReaderTables.Read())
                            {
                                int index = grdParameters.Rows.Add();

                                DataGridViewRow row = grdParameters.Rows[index];

                                row.Cells["grdColParams"] = new DataGridViewTextBoxCell
                                                                {
                                                                    Value = sqlDataReaderTables.GetString(1)
                                                                };

                                row.Cells["grdColDirection"] = new DataGridViewTextBoxCell
                                                                   {
                                                                       Value = sqlDataReaderTables.GetString(2)
                                                                   };

                                row.Cells["grdColVars"] = LoadCellVariables();

                                row.Cells["grdColExpression"] = new DataGridViewButtonCell();
                            }
                    }
                }

                if (_isFirstLoad)
                {
                    var mappingParams = (MappingParams)_taskHost.Properties[Keys.STORED_PROCEDURE_PARAMS].GetValue(_taskHost);

                    foreach (MappingParam mappingParam in mappingParams)
                    {
                        foreach (DataGridViewRow row in grdParameters.Rows.Cast<DataGridViewRow>().Where(row => row.Cells[0].Value.ToString() == mappingParam.Name))
                        {
                            row.Cells[2].Value = mappingParam.Value;
                        }
                    }
                }
            }

            Cursor = Cursors.Arrow;
        }

        #endregion

        #region Events

        void grdParameters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            _taskHost.Properties[Keys.SQL_SERVER].SetValue(_taskHost, cmbSQLServer.Text);
            _taskHost.Properties[Keys.DESTINATION_FILE_CONNECTION].SetValue(_taskHost, optFileConnection.Checked ? "true" : "false");
            _taskHost.Properties[Keys.DESTINATION].SetValue(_taskHost, cmbDestination.Text);

            _taskHost.Properties[Keys.TRUSTED_CONNECTION].SetValue(_taskHost, chkTrustedConnection.Checked ? "true" : "false");
            _taskHost.Properties[Keys.SRV_LOGIN].SetValue(_taskHost, !chkTrustedConnection.Checked ? cmbLogin.Text : string.Empty);
            _taskHost.Properties[Keys.SRV_PASSWORD].SetValue(_taskHost, !chkTrustedConnection.Checked ? cmbPassword.Text : string.Empty);

            _taskHost.Properties[Keys.NATIVE_DB_DATATYPE].SetValue(_taskHost, chkNativeDatabase.Checked ? "true" : "false");

            _taskHost.Properties[Keys.FIRSTROW].SetValue(_taskHost, txFirstRow.Text.Trim());
            _taskHost.Properties[Keys.LASTROW].SetValue(_taskHost, txLastRow.Text.Trim());

            _taskHost.Properties[Keys.FIELD_TERMINATOR].SetValue(_taskHost, txFieldTerminator.Text.Trim());
            _taskHost.Properties[Keys.ROW_TERMINATOR].SetValue(_taskHost, txRowTerminator.Text.Trim());

            _taskHost.Properties[Keys.ACTIVATE_CMDSHELL].SetValue(_taskHost, chkRightsCMDSHELL.Checked ? "true" : "false");

            _taskHost.Properties[Keys.SQL_STATEMENT].SetValue(_taskHost, txSQL.Text.Trim());
            _taskHost.Properties[Keys.SQL_VIEW].SetValue(_taskHost, cmbViews.Text);
            _taskHost.Properties[Keys.SQL_StoredProcedure].SetValue(_taskHost, cmbStoredProcedures.Text);
            _taskHost.Properties[Keys.SQL_TABLE].SetValue(_taskHost, cmbTables.Text);


            _taskHost.Properties[Keys.DATA_SOURCE].SetValue(_taskHost, tabControl.SelectedTab.Text);

            var mappingParams = new MappingParams();
            mappingParams.AddRange(from DataGridViewRow row in grdParameters.Rows
                                   select new MappingParam
                                   {
                                       Name = row.Cells[0].Value.ToString(),
                                       Type = row.Cells[1].Value.ToString(),
                                       Value = row.Cells[5].Value.ToString()
                                   });

            _taskHost.Properties[Keys.STORED_PROCEDURE_PARAMS].SetValue(_taskHost, mappingParams);

            DialogResult = DialogResult.OK;
            Close();
        }

        private DataGridViewComboBoxCell LoadCellVariables()
        {
            var comboBoxCell = new DataGridViewComboBoxCell();

            foreach (Variable variable in Variables)
            {
                comboBoxCell.Items.Add(string.Format("@[{0}::{1}]", variable.Namespace, variable.Name));
            }

            return comboBoxCell;
        }

        private void btGO_Click(object sender, EventArgs e)
        {
            LoadDataBaseObjects();
        }

        private void chkTrustedConnection_Click(object sender, EventArgs e)
        {
            cmbLogin.Enabled = cmbPassword.Enabled = (chkTrustedConnection.Checked) ? false : true;
        }

        private void optFileConnection_Click(object sender, EventArgs e)
        {
            LoadFileConnections();
        }

        private void optFileVariable_Click(object sender, EventArgs e)
        {
            LoadFileVariables();
        }

        private void cmdFileVariable_Click(object sender, EventArgs e)
        {
            using (var expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables, _taskHost.VariableDispenser, Type.GetType("Sysytem.String"), cmdFileVariable.Text))
            {
                if (expressionBuilder.ShowDialog() == DialogResult.OK)
                {
                    cmbDestination.Text = expressionBuilder.Expression;
                }
            }
        }

        private void cmbStoredProcedures_Click(object sender, EventArgs e)
        {
            var interValue = cmbStoredProcedures.Text.Split(new[] { "].[" }, StringSplitOptions.None);
            string schema = interValue[0].Replace("[", string.Empty).Replace("]", string.Empty);
            string storedProcedure = interValue[1].Replace("[", string.Empty).Replace("]", string.Empty); ;
            LoadStoredProcedureParameters(schema, storedProcedure);
        }

        private void optFileConnection_CheckedChanged(object sender, EventArgs e)
        {
            LoadFileConnections();
        }

        private void optFileVariable_CheckedChanged(object sender, EventArgs e)
        {
            string selItem = string.Empty;
            cmbDestination = LoadVariables("System.String", ref selItem);
        }

        private void grdParameters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 3:
                    {
                        using (ExpressionBuilder expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables,
                                                                                                _taskHost.VariableDispenser,
                                                                                                Type.GetType("System.Object"),
                                                                                                string.Empty))
                        {
                            if (expressionBuilder.ShowDialog() == DialogResult.OK)
                            {
                                ((DataGridViewComboBoxCell)grdParameters.Rows[e.RowIndex].Cells[e.ColumnIndex - 1]).Items.Add(expressionBuilder.Expression);
                                grdParameters.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = expressionBuilder.Expression;
                            }
                        }
                    }

                    break;
            }
        }

        #endregion
    }
}
