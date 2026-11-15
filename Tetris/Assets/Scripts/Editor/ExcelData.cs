
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Excel
{
    /// <summary>
    /// Excelシートクラス
    /// </summary>
    public struct Sheet
    {
        /// <summary>
        /// シート名
        /// </summary>
        public string name;
        /// <summary>
        /// セル
        /// </summary>
        public Cell[] cells;

        public Cell GetCell(int row, int column)
        {
            return cells.FirstOrDefault(c => c.row == row && c.column == column);
        }

        public Cell[] GetRowCells(int row)
        {
            return cells.Where(c => c.row == row).ToArray();
        }

        public Cell[] GetColumnCells(int column)
        {
            return cells.Where(c => c.column == column).ToArray();
        }
    }

    /// <summary>
    /// セル情報の構造体
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// 行番号を表します。
        /// </summary>
        /// <remarks>公開フィールドは外部から変更可能です。変更管理や検証が必要な場合はプロパティへの置き換えを検討してください。</remarks>
        public int row;
        /// <summary>
        /// 列位置または列番号を表す整数。
        /// </summary>
        public int column;
        /// <summary>
        /// セルの値
        /// </summary>
        public string value;
    }

    /// <summary>
    /// エラークラス
    /// </summary>
    public string error
    {
        get
        {
            return _error;
        }
    }

    /// <summary>
    /// xls内のシート配列
    /// </summary>
    public Sheet[] Sheets
    {
        get
        {
            return _sheets;
        }
    }
    /// <summary>
    /// 読み取ったシート情報の配列
    /// </summary>
    private Sheet[] _sheets = null;
    /// <summary>
    /// 読み取った結果のエラー分
    /// エラーがなければ値は空
    /// </summary>
    private string _error = string.Empty;
    private string _name = string.Empty;

    /// <summary>
    /// 指定したパスから Excel を読み取り、読み取り結果を出力パラメータに設定して成功可否を返す。
    /// </summary>
    /// <remarks>内部で Read(path) を呼び出し、返された Excel のエラー情報の有無で成否を判定する。</remarks>
    /// <param name="path">読み取るファイルのパス。</param>
    /// <param name="excel">読み取り結果を格納する出力パラメータ。読み取り失敗時はエラー情報を含む Excel インスタンスを返す。</param>
    /// <returns>読み取りが成功しエラー情報がない場合は true、そうでなければ false。</returns>
    public static bool TryRead(string path, out Excel excel)
    {
        excel = Read(path);
        return string.IsNullOrEmpty(excel._error);
    }

    /// <summary>
    /// 指定したファイルパスから Open XML（.xlsx）形式のワークブックを読み込み、Excel オブジェクトを返す。
    /// </summary>
    /// <remarks>内部でファイルストリームを開き ExcelReader で DataSet を解析する。例外発生時は例外情報をログに出力し、エラー情報を保持した Excel を返す。</remarks>
    /// <param name="path">読み込む Excel ファイルのパス。</param>
    /// <returns>読み込まれたワークブックを表す Excel オブジェクト。読み込みに失敗した場合は空のシート配列とエラー情報を含む Excel を返す。</returns>
    private static Excel Read(string path)
    {
        var excel = new Excel();
        try
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                if (reader != null)
                {
                    excel.ParseDataSet(reader.AsDataSet());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            excel._sheets = new Sheet[] { };
            excel._error = e.ToString();
        }
        return excel;
    }

    /// <summary>
    /// 指定した名前と一致する最初のシートを返します。
    /// </summary>
    /// <remarks>名前の比較は大文字小文字を区別します。内部コレクションの先頭から順に検索します。</remarks>
    /// <param name="name">検索するシートの名前。</param>
    /// <returns>一致する Sheet オブジェクト。見つからない場合は null を返します。</returns>
    public Sheet GetSheet(string name)
    {
        return _sheets.FirstOrDefault(s => s.name == name);
    }

    /// <summary>
    /// DataSet を解析し、各 DataTable を Sheet に、各セルを Cell に変換して内部フィールドに設定します。
    /// </summary>
    /// <remarks>内部フィールド _sheets を上書きします。dataSet が null の場合は例外が発生します。</remarks>
    /// <param name="dataSet">解析対象の DataSet。各 DataTable はシートになり、各セルは行・列インデックスと文字列化された値を持ちます。</param>
    private void ParseDataSet(DataSet dataSet)
    {
        var sheetList = new List<Sheet>();
        foreach (DataTable table in dataSet.Tables)
        {
            var sheet = new Sheet();
            sheet.name = table.TableName;
            var cellList = new List<Cell>();
            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int column = 0; column < table.Columns.Count; column++)
                {
                    var cell = new Cell();
                    cell.row = row;
                    cell.column = column;
                    cell.value = table.Rows[row][column].ToString();
                    cellList.Add(cell);
                }
            }
            sheet.cells = cellList.ToArray();
            sheetList.Add(sheet);
        }
        _sheets = sheetList.ToArray();
    }
}


public static class ExcelDemo
{
    private const string kSampleExcelPath = "Assets/Excel/Book1.xlsx";

    [MenuItem("ExcelDemo/Read Sample Excel")]
    private static void ReadSampleExcel()
    {
        Excel excel;
        Debug.Log("EXCEL READING START");
        Debug.Log("==================");
        if (Excel.TryRead(kSampleExcelPath, out excel))
        {
            foreach (var sheet in excel.Sheets)
            {
                foreach (var cell in sheet.cells)
                {
                    Debug.Log(string.Format("{0}:{1}:{2} value={3}", sheet.name, cell.row, cell.column, cell.value));
                }
            }
        }
        Debug.Log("==================");
        Debug.Log("EXCEL READING END");
    }
}