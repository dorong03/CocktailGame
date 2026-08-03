using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ExcelParser;

public class ExcelToJsonParser : EditorWindow
{
    private string selectedPath;
    private string savePath;

    [MenuItem("Tools/엑셀 변환기")]
    public static void WindowOpen()
    {
        GetWindow<ExcelToJsonParser>("Excel To Json");
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel → Json 변환기", EditorStyles.boldLabel);

        GUILayout.Space(10);


        if (GUILayout.Button("엑셀 파일 선택"))
        {
            selectedPath = EditorUtility.OpenFilePanel("xlsx 파일 선택", "", "xlsx");



        }

        if (!string.IsNullOrEmpty(selectedPath))
        {
            EditorGUILayout.HelpBox(selectedPath, MessageType.None);
        }

        GUILayout.Space(10);

        // 저장 폴더 선택
        if (GUILayout.Button("저장 폴더 선택"))
        {
            savePath = EditorUtility.OpenFolderPanel("저장할 폴더 선택", "", "");



        }

        if (!string.IsNullOrEmpty(savePath))
        {
            EditorGUILayout.HelpBox(savePath, MessageType.None);
        }

        GUILayout.Space(20);

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("변환하기", GUILayout.Height(35)))
        {
            if (string.IsNullOrEmpty(selectedPath))
            {
                EditorUtility.DisplayDialog("오류", "엑셀 파일을 선택하세요.", "확인");



                return;
            }

            if (string.IsNullOrEmpty(savePath))
            {
                EditorUtility.DisplayDialog("오류", "저장 폴더를 선택하세요.", "확인");



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
