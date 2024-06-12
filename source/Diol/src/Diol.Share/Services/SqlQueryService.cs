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
        public static string ExtractTableNameFromSelectQuery(string sqlQuery)
        {
            string pattern = @"FROM\s+\[?(\w+)\]?";
            Match match = Regex.Match(sqlQuery, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts the table name from an INSERT query.
        /// </summary>
        /// <param name="sqlQuery">The INSERT query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromInsertQuery(string sqlQuery)
        {
            string pattern = @"INTO\s+\[?(\w+)\]?";
            Match match = Regex.Match(sqlQuery, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts the table name from an UPDATE query.
        /// </summary>
        /// <param name="sqlQuery">The UPDATE query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromUpdateQuery(string sqlQuery)
        {
            string pattern = @"UPDATE\s+\[?(\w+)\]?";
            Match match = Regex.Match(sqlQuery, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts the table name from a DELETE query.
        /// </summary>
        /// <param name="sqlQuery">The DELETE query.</param>
        /// <returns>The table name.</returns>
        public static string ExtractTableNameFromDeleteQuery(string sqlQuery)
        {
            string pattern = @"FROM\s+\[?(\w+)\]?";
            Match match = Regex.Match(sqlQuery, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

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
    }
}
