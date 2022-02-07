using UnityEditor;
using UnityEngine;
  
public class MyCustomGUI {
   
    [MenuItem("Tools/ShowMyCustomGUI")]
    public static void InitGUI() {
        MyCustomGUI MCG = new MyCustomGUI();
        SceneView.onSceneGUIDelegate += MCG.RenderSceneGUI;
    }
  
    float btnHeight = 50;
    float btnPadding = 10;
    public void RenderSceneGUI(SceneView sceneview) {
        //Scene gui stuff
        if (GUI.Button(new Rect(10, 10, 100, btnHeight), "Button"))
        {
            Debug.Log("Clicked the button with text");
            ShowIt();
        }

    }
    public void ShowIt()
    {
        Debug.Log("SHOW IT!");
    }
}