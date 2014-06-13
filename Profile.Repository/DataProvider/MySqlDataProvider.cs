using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;

namespace Profile.Repository.DataProvider
{
    namespace MySqlDataProvider
    {
        internal class MySqlDataProvider : DataProviderBase
        {
            static MySqlDataProvider()
            {
                ConfigureOldStyle();
            }

            public override void Configure(NameValueCollection attributes)
            {
                string str = attributes["ParameterPrefix"];
                if (str != null)
                {
                    SprocParameterPrefix = str;
                    CommandParameterPrefix = str;
                }
                str = attributes["CommandParameterPrefix"];
                if (str != null)
                {
                    CommandParameterPrefix = str;
                }
                str = attributes["SprocParameterPrefix"];
                if (str != null)
                {
                    SprocParameterPrefix = str;
                }
                string str2 = attributes["ParameterSymbolConfig"];
                if (str2 != null)
                {
                    string str6 = str2;
                    if (str6 != null)
                    {
                        if (!(str6 == "OldStyle"))
                        {
                            if (str6 == "NewStyle")
                            {
                                ConfigureNewStyle();
                            }
                        }
                        else
                        {
                            ConfigureOldStyle();
                        }
                    }
                }
                string str3 = attributes["ParameterSymbol"];
                if ((str3 != null) && (str3.Length == 1))
                {
                    ParameterSymbol = str3[0];
                }
                string str4 = attributes["ConvertParameterSymbols"];
                if (str4 != null)
                {
                    ConvertParameterSymbols = new List<char>(str4.ToCharArray());
                }
                string p = attributes["TryConvertParameterSymbol"];
                if (p != null)
                {
                    TryConvertParameterSymbol = BLToolkit.Common.Convert.ToBoolean(p);
                }
                base.Configure(attributes);
            }

            public static void ConfigureNewStyle()
            {
                ParameterSymbol = '@';
                ConvertParameterSymbols = null;
                TryConvertParameterSymbol = false;
            }

            public static void ConfigureOldStyle()
            {
                ParameterSymbol = '?';
                ConvertParameterSymbols = new List<char>(new char[] { '@' });
                TryConvertParameterSymbol = true;
            }

            public override object Convert(object value, ConvertType convertType)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                switch (convertType)
                {
                    case ConvertType.ExceptionToErrorNumber:
                        if (value is MySqlException)
                        {
                            return ((MySqlException)value).Number;
                        }
                        break;

                    case ConvertType.ExceptionToErrorMessage:
                        if (value is MySqlException)
                        {
                            return ((MySqlException)value).Message;
                        }
                        break;
                }
                return base.SqlProvider.Convert(value, convertType);
            }

            public override DataExceptionType ConvertErrorNumberToDataExceptionType(int number)
            {
                switch (number)
                {
                    case 0x4bd:
                        return (DataExceptionType) 1;

                    case 0x4c0:
                    case 0x4c1:
                        return (DataExceptionType)3;

                    case 0x4b5:
                        return (DataExceptionType)2;

                    case 0x491:
                        return (DataExceptionType)4;
                }
                return 0;
            }

            private void ConvertParameterNames(IDbCommand command)
            {
                foreach (IDataParameter parameter in command.Parameters)
                {
                    if (parameter.ParameterName[0] != ParameterSymbol)
                    {
                        parameter.ParameterName = this.Convert(this.Convert(parameter.ParameterName, ConvertType.SprocParameterToName), (command.CommandType == CommandType.StoredProcedure) ? ConvertType.NameToSprocParameter : ConvertType.NameToCommandParameter).ToString();
                    }
                }
            }

            public override IDbConnection CreateConnectionObject()
            {
                return new MySqlConnection();
            }

            public override DbDataAdapter CreateDataAdapterObject()
            {
                return new MySqlDataAdapter();
            }

            public override ISqlProvider CreateSqlProvider()
            {
                return new MySqlSqlProvider();
            }

            public override bool DeriveParameters(IDbCommand command)
            {
                if (command is MySqlCommand)
                {
                    MySqlCommandBuilder.DeriveParameters((MySqlCommand)command);
                    if (TryConvertParameterSymbol && (ConvertParameterSymbols.Count > 0))
                    {
                        this.ConvertParameterNames(command);
                    }
                    return true;
                }
                return false;
            }

            public override IDbDataParameter GetParameter(IDbCommand command, NameOrIndexParameter nameOrIndex)
            {
                if (nameOrIndex.ByName)
                {
                    string str = (command.CommandType == CommandType.StoredProcedure) ? this.Convert(nameOrIndex.Name, ConvertType.NameToSprocParameter).ToString() : nameOrIndex.Name;
                    return (IDbDataParameter)command.Parameters[str];
                }
                return (IDbDataParameter)command.Parameters[nameOrIndex.Index];
            }

            public static string CommandParameterPrefix
            {
                get
                {
                    return MySqlSqlProvider.CommandParameterPrefix;
                }
                set
                {
                    MySqlSqlProvider.CommandParameterPrefix = value;
                }
            }

            public override Type ConnectionType
            {
                get
                {
                    return typeof(MySqlConnection);
                }
            }

            public static List<char> ConvertParameterSymbols
            {
                get
                {
                    return MySqlSqlProvider.ConvertParameterSymbols;
                }
                set
                {
                    MySqlSqlProvider.ConvertParameterSymbols = value;
                }
            }

            public override string Name
            {
                get
                {
                    return "MySql";
                }
            }

            [Obsolete("Use CommandParameterPrefix or SprocParameterPrefix instead.")]
            public static string ParameterPrefix
            {
                get
                {
                    return MySqlSqlProvider.SprocParameterPrefix;
                }
                set
                {
                    string text1 = string.IsNullOrEmpty(value) ? string.Empty : value;
                    CommandParameterPrefix = text1;
                    SprocParameterPrefix = text1;
                }
            }

            public static char ParameterSymbol
            {
                get
                {
                    return MySqlSqlProvider.ParameterSymbol;
                }
                set
                {
                    MySqlSqlProvider.ParameterSymbol = value;
                }
            }

            public static string SprocParameterPrefix
            {
                get
                {
                    return MySqlSqlProvider.SprocParameterPrefix;
                }
                set
                {
                    MySqlSqlProvider.SprocParameterPrefix = value;
                }
            }

            public static bool TryConvertParameterSymbol
            {
                get
                {
                    return MySqlSqlProvider.TryConvertParameterSymbol;
                }
                set
                {
                    MySqlSqlProvider.TryConvertParameterSymbol = value;
                }
            }
        }
    }

}
