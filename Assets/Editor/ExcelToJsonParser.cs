using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ExcelParser;

public class ExcelToJsonParser : EditorWindow
{
    private string selectedPath;
    private static readonly string SavePath = Path.Combine(Application.dataPath, "Resources", "JsonData");

    [MenuItem("Tools/Excel to Json")]
    public static void WindowOpen()
    {
        GetWindow<ExcelToJsonParser>("Excel To Json");
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel to Json File", EditorStyles.boldLabel);

        GUILayout.Space(10);

        if (GUILayout.Button("Select Excel file path"))
        {
            selectedPath = EditorUtility.OpenFilePanel("xlsx file select", "", "xlsx");
        }

        if (!string.IsNullOrEmpty(selectedPath))
        {
            EditorGUILayout.HelpBox(selectedPath, MessageType.None);
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox($"저장 경로 (고정): {SavePath}", MessageType.Info);

        GUILayout.Space(20);

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Parse", GUILayout.Height(35)))
        {
            if (string.IsNullOrEmpty(selectedPath))
            {
                EditorUtility.DisplayDialog("오류", "엑셀 파일 경로를 선택해주세요.", "확인");
                return;
            }

            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            try
            {
                WorkbookParser workbookParser = new WorkbookParser();
                WorksheetParser worksheetParser = new WorksheetParser();
                SharedStringParser sharedStringParser = new SharedStringParser();
                JsonWriter jsonWriter = new JsonWriter();

                SharedStringTable sharedStringTable =
                    sharedStringParser.Parse(selectedPath);

                List<SheetInfo> sheetInfos =
                    workbookParser.Parse(selectedPath);

                foreach (SheetInfo info in sheetInfos)
                {
                    Worksheet worksheet =
                        worksheetParser.Parse(
                            selectedPath,
                            info,
                            sharedStringTable);

                    if (worksheet == null)
                        continue;

                    jsonWriter.ConvertExcelToJsonFile(
                        SavePath,
                        worksheet);
                }

                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("완료", "JSON 변환이 완료되었습니다.", "확인");
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("오류", e.Message, "확인");
            }
        }

        GUI.backgroundColor = Color.white;
    }
}