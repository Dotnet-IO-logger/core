using Diol.Share.Services;
using System.Text.RegularExpressions;

namespace Units;

/// <summary>
/// Test class for SqlQueryParser.
/// </summary>
[TestClass]
public class SqlQueryParserTest
{
    [DataTestMethod]
    [DataRow(
        "SELECT * FROM table1 LEFT JOIN (SELECT * FROM table2) tt WHERE tt.name IN (SELECT name FROM table3)",
        "table1")]
    [DataRow(
        "SELECT [p].[Id], [p].[CategoryId], [p].[Name], [c].[Id], [c].[Name] FROM [Products] AS [p] LEFT JOIN [Categories] AS [c] ON [p].[CategoryId] = [c].[Id]",
        "Products")]
    [DataRow(
        "SELECT [p].[Id], [p].[CategoryId], [p].[Name]\r\nFROM [Products] AS [p]",
        "Products")]
    public void TestExtractTableNameFromSelectQuery(string sqlQuery, string expectedTableName)
    {
        var res = SqlQueryService.ExtractTableNameFromSelectQuery(sqlQuery);

        Assert.AreEqual(expectedTableName, res);
    }

    [DataTestMethod]
    [DataRow(
        "SET IMPLICIT_TRANSACTIONS OFF;\r\nSET NOCOUNT ON;\r\nINSERT INTO [Categories] ([Name])\r\nOUTPUT INSERTED.[Id]\r\nVALUES (@p0);",
        "Categories")]
    [DataRow(
        "SET IMPLICIT_TRANSACTIONS OFF;\r\nSET NOCOUNT ON;\r\nINSERT INTO [Products] ([CategoryId], [Name])\r\nOUTPUT INSERTED.[Id]\r\nVALUES (@p0, @p1);",
        "Products")]
    public void TestExtractTableNameFromInsertQuery(string sqlQuery, string expectedTableName)
    {
        var res = SqlQueryService.ExtractTableNameFromInsertQuery(sqlQuery);

        Assert.AreEqual(expectedTableName, res);
    }

    [DataTestMethod]
    [DataRow(
        "SET IMPLICIT_TRANSACTIONS OFF;\r\nSET NOCOUNT ON;\r\nUPDATE [Categories] SET [Name] = @p0\r\nOUTPUT 1\r\nWHERE [Id] = @p1;",
        "Categories")]
    public void TestExtractTableNameFromUpdateQuery(string sqlQuery, string expectedTableName)
    {
        var res = SqlQueryService.ExtractTableNameFromUpdateQuery(sqlQuery);

        Assert.AreEqual(expectedTableName, res);
    }

    [DataTestMethod]
    [DataRow(
        "SET IMPLICIT_TRANSACTIONS OFF;\r\nSET NOCOUNT ON;\r\nDELETE FROM [Products]\r\nOUTPUT 1\r\nWHERE [Id] = @p0;",
        "Products")]
    public void TestExtractTableNameFromDeleteQuery(string sqlQuery, string expectedTableName)
    {
        var res = SqlQueryService.ExtractTableNameFromDeleteQuery(sqlQuery);

        Assert.AreEqual(expectedTableName, res);
    }

    [DataTestMethod]
    [DataRow(
        "select * FROM Products",
        "SELECT")]
    [DataRow(
        "SET IMPLICIT_TRANSACTIONS OFF;\r\nSET NOCOUNT ON;\r\nINSERT INTO [Categories] ([Name])\r\nOUTPUT INSERTED.[Id]\r\nVALUES (@p0);",
        "INSERT")]
    public void TestExtractOperationNameFromQuery(string sqlQuery, string expectedOperation)
    {
        var res = SqlQueryService.ExtractOperationNameFromQuery(sqlQuery);

        Assert.AreEqual(expectedOperation, res);
    }
}