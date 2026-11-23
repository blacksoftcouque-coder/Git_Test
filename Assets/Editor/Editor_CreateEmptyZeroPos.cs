using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.ShortcutManagement;
using System.Collections.Generic;

public class Editor_CreateEmptyZeroPos : MonoBehaviour
{
    private static bool _isExecuting = false;

    [MenuItem("GameObject/Create Empty Zero Pos", false, 0)]
    public static void CreateEmptyZeroPos(MenuCommand command)
    {
        if (_isExecuting) return;

        _isExecuting = true;
        EditorApplication.delayCall += () => _isExecuting = false;

        Scene nowScene = SceneManager.GetActiveScene();

        if (Selection.gameObjects.Length != 0)
        {

            List<GameObject> zeroList = new List<GameObject>();

            foreach (GameObject selectedInst in Selection.gameObjects)
            {
                GameObject zeroObject = ObjectFactory.CreateGameObject("GameObject");
                zeroObject.transform.SetParent(selectedInst.transform);
                zeroObject.transform.localPosition = Vector3.zero;
                zeroObject.transform.localEulerAngles = Vector3.zero;
                zeroObject.transform.localScale = Vector3.one;
                
                zeroList.Add(zeroObject);
            }

            foreach (GameObject posZero in zeroList)
            {
                Selection.activeObject = posZero;
            }
        }
        else
        {
            GameObject zeroObject = ObjectFactory.CreateGameObject("GameObject");

            Selection.activeObject = zeroObject;
        }
    }
}
