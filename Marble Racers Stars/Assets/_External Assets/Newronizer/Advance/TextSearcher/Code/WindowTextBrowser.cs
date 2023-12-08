using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

#if UNITY_EDITOR
public class WindowTextBrowser : EditorWindow
{
    [MenuItem("Newronizer/Text extensions/Find Text")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(WindowTextBrowser));
        window.Show();
    }

    static List<GameObject> bufferSearch = new List<GameObject>();
    static string textFieldValue = "";
    static int indexSearch = 0;
    static string lastSearchString = "";
    private void OnGUI()
    {
        textFieldValue = EditorGUILayout.TextField("Busqueda:", textFieldValue);

        if (GUILayout.Button("Buscar"))
        {
            SearchComponents(textFieldValue);
        }

        if (ExistsSearch)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
                DecreaseIndex();
            if (GUILayout.Button(">"))
                IncreaseIndex();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Limpiar"))
                ClearSearch();
            GUILayout.Label($"Existen {bufferSearch.Count} de la busqueda \"{lastSearchString}\"");
        }
        this.Repaint();
    }

    public static void SearchComponents(string str)
    {
        var items = GameObject.FindObjectsOfType<Text>(true);
        var items2 = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);
        
        var search = items.ToList().Where(t => t.text.Contains(str));
        var search2 = items2.ToList().Where(t => t.text.Contains(str));

        List<GameObject> list = new List<GameObject>();
        foreach (var item in search)
            list.Add(item.gameObject);
        foreach (var item in search2)
            list.Add(item.gameObject);
        bufferSearch = list;
        lastSearchString = str;
        IncreaseIndex();
    }


    public static void IncreaseIndex()
    {
        // inverse beacuse unity editor search element inversely
        if (!ExistsSearch)
            return;
        indexSearch--;
        if (indexSearch < 0)
            indexSearch = bufferSearch.Count - 1;
        ShowSearch();
    }

    public static void DecreaseIndex()
    {
        if (!ExistsSearch)
            return;
        indexSearch++;
        if (indexSearch == bufferSearch.Count)
            indexSearch = 0;
        ShowSearch();
    }
    private static bool ExistsSearch => bufferSearch.Count > 0 && !string.IsNullOrEmpty(lastSearchString);

    private void OnDestroy() => ClearSearch();
    public static void ShowSearch() => Selection.activeObject = bufferSearch[indexSearch];

    private static void ClearSearch()
    {
        bufferSearch.Clear();
        textFieldValue = lastSearchString = "";
        indexSearch = 0;
    }
}


public class TextToAllCaps 
{
    [MenuItem("Newronizer/Text extensions/ToUpper")]
    public static void TextToUpper()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.Log("Selecciona un objeto");
            return;
        }

        var text = Selection.activeGameObject.GetComponent<Text>();
        var textMesh =Selection.activeGameObject.GetComponent<TextMeshProUGUI>();
        if (text == null && textMesh == null)
        { 
            Debug.Log("Selecciona un objeto con un componente de texto");
        }


        if (text != null)
            text.text = text.text.ToUpper();
        if (textMesh != null)
            textMesh.text = textMesh.text.ToUpper();
    }

    [MenuItem("Newronizer/Text extensions/ToLower")]
    public static void TextToLower()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.Log("Selecciona un objeto");
            return;
        }

        var text = Selection.activeGameObject.GetComponent<Text>();
        var textMesh = Selection.activeGameObject.GetComponent<TextMeshProUGUI>();

        if (text == null && textMesh == null)
        {
            Debug.Log("Selecciona un objeto con un componente de texto");
        }

        if (text != null)
            text.text = text.text.ToLower();
        if (textMesh != null)
            textMesh.text = textMesh.text.ToLower();
    }
}
#endif