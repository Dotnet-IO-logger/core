using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Diol.Share.Services
{
    public class SqlQueryService
    {
        public static string SELECT => "SELECT";

        public static string INSERT => "INSERT";

        public static string UPDATE => "UPDATE";

        public static string DELETE => "DELETE";

        private static readonly string[] operations = { SELECT, INSERT, UPDATE, DELETE };

        /// <summary>
        /// Extracts the table name from a SELECT query.
        /// </summary>
        /// <param name="sqlQuery">The SELECT query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromSelectQuery(string sqlQuery) =>
            ExtractFromQuery(sqlQuery, @"FROM\s+\[?(\w+)\]?");

        /// <summary>
        /// Extracts the table name from an INSERT query.
        /// </summary>
        /// <param name="sqlQuery">The INSERT query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromInsertQuery(string sqlQuery) =>
            ExtractFromQuery(sqlQuery, @"INTO\s+\[?(\w+)\]?");

        /// <summary>
        /// Extracts the table name from an UPDATE query.
        /// </summary>
        /// <param name="sqlQuery">The UPDATE query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromUpdateQuery(string sqlQuery) =>
            ExtractFromQuery(sqlQuery, @"UPDATE\s+\[?(\w+)\]?");

        /// <summary>
        /// Extracts the table name from a DELETE query.
        /// </summary>
        /// <param name="sqlQuery">The DELETE query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromDeleteQuery(string sqlQuery) =>
            ExtractFromQuery(sqlQuery, @"FROM\s+\[?(\w+)\]?");

        /// <summary>
        /// Extracts the operation name from a SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns>The operation name.</returns>
        public static string ExtractOperationNameFromQuery(string sqlQuery)
        {
            var splitedQuery = sqlQuery.ToUpperInvariant().Split('\n', ' ');

            foreach (var item in splitedQuery)
            {
                if (operations.Contains(item))
                {
                    return item;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Check if the query is a part of a transaction.
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static bool IsTransaction(string sqlQuery) 
        {
            return sqlQuery.Contains("IMPLICIT_TRANSACTIONS");
        }
        
        /// <summary>
        /// Extracts a value from a SQL query base on pattern.
        /// </summary>
        /// <param name="sqlQuery">The query.</param>
        /// <param name="pattern">Regex pattern.</param>
        /// <returns></returns>
        private static string ExtractFromQuery(string sqlQuery, string pattern)
        {
            Match match = Regex.Match(sqlQuery, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }
    }
}
