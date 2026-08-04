using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ExcelParser;

public class ExcelToJsonParser : EditorWindow
{
    private string selectedPath;
    private string savePath;

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
        
        if (GUILayout.Button("Select Save Path"))
        {
            savePath = EditorUtility.OpenFolderPanel("Save file path", "", "");
        }

        if (!string.IsNullOrEmpty(savePath))
        {
            EditorGUILayout.HelpBox(savePath, MessageType.None);
        }

        GUILayout.Space(20);

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Parse", GUILayout.Height(35)))
        {
            if (string.IsNullOrEmpty(selectedPath))
            {
                EditorUtility.DisplayDialog("Error", "select path is empty.", "Ȯ��");



                return;
            }

            if (string.IsNullOrEmpty(savePath))
            {
                EditorUtility.DisplayDialog("����", "���� ������ �����ϼ���.", "Ȯ��");



                return;
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
                        savePath,
                        worksheet);
                }

                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("�Ϸ�", "JSON ��ȯ�� �Ϸ�Ǿ����ϴ�.", "Ȯ��");



            }
            catch (System.Exception e)
            {
                Debug.LogException(e);

                EditorUtility.DisplayDialog("����", e.Message, "Ȯ��");



            }
        }

        GUI.backgroundColor = Color.white;
    }
}
