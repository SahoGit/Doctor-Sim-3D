using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;


public class SOMenu : EditorWindow
{

    public static List<Texture2D> _AllTexture2D = new List<Texture2D>();
    // public static List<int> _TextSize = new List<int>();
    public static Texture2D Obb;
    public static TextureImporterFormat _TextureFormat = TextureImporterFormat.ASTC_6x6;
    //public static SpriteImportMode SpriteMode = SpriteImportMode.Single;
    public static string folderName = "";
    public static int Textursize;

    public static _platform PlatformName = _platform.Android;
    public static _UserSize UserSize = _UserSize._256;
    public static _UserSize UserSize2 = _UserSize._1024;


    public static bool toggleTextField = false;
    public static int MinimizeSize = 256, MaximumSize = 1024;

    static string[] Words;
    public enum _platform
    {
        Android = 1,
        iPhone = 2
    }

    public enum _UserSize
    {
        _32 = 32,
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048,
        _4096 = 4096,
    }

    GUIStyle guistyle = new GUIStyle();
    GUIStyle guistyle1 = new GUIStyle();
    GUIStyle LogoFont = new GUIStyle();
    private static Texture2D tt;


    [MenuItem("GameDistrict/TexturesOptimizer/StartManually", false, 100)]


    static void showWindow()
    {
#if UNITY_ANDROID
        PlatformName = _platform.Android;
#elif UNITY_IOS
        PlatformName = _platform.iPhone;
#endif

        SOMenu window = (SOMenu)EditorWindow.GetWindowWithRect(typeof(SOMenu), new Rect(0, 0, 450, 700));
        window.titleContent = new GUIContent("Optimizer");
        window.Show();

    }

    void OnGUI()
    {
        try
        {
            GUILayout.BeginHorizontal();


            GUILayout.Label("Version : 1.0", GUILayout.Width(100), GUILayout.Height(20));

            GUILayout.EndHorizontal();

            tt = EditorGUIUtility.Load("Assets/Plugins/TexturesOptimizer/logo.png") as Texture2D;

            GUI.Label(new Rect(145, 0, 173, 186), tt);

            GUILayout.Space(160);

            GUIStyle largeStyle = new GUIStyle(EditorStyles.boldLabel);
            largeStyle.fontSize = 25;


            GUI.Label(new Rect(2, 195, 300, 25), "Select Your Texture Folder :");

            guistyle1.fontSize = 15;
            if (GUI.Button(new Rect(210, 195, 90, 25), "Browse"))
            {
                folderName = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");

                if (folderName != "")
                {
                    if (folderName.Contains("Assets/"))
                    {
                        Words = folderName.Split(new string[] { "Assets/" }, StringSplitOptions.None);

                        GUI.Label(new Rect(50, 225, 350, 26), "Assets/" + Words[1]);
                    }
                    else
                    {
                        Debug.LogError("Directory Not Found!!!");

                    }
                }
            }


            if (folderName != "" && folderName.Contains("Assets/"))
            {
                GUI.Label(new Rect(50, 225, 350, 26), "Assets/" + Words[1]);
            }

            GUILayout.Space(70);

            PlatformName = (_platform)EditorGUILayout.EnumPopup("Select Your Platform:", PlatformName);

            GUILayout.Space(20);

            _TextureFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Select Texture Format:", _TextureFormat);

            GUILayout.Space(15);


            toggleTextField = EditorGUI.ToggleLeft(new Rect(5, 330, 300, 25), "Select Custom Size Range For Textures", toggleTextField);

            if (toggleTextField == true)
            {
                GUILayout.Space(42);
                UserSize = (_UserSize)EditorGUILayout.EnumPopup("Select Minimum Size:", UserSize);
                MinimizeSize = (int)UserSize;


                GUILayout.Space(20);
                UserSize2 = (_UserSize)EditorGUILayout.EnumPopup("Select Maximum Size:", UserSize2);
                MaximumSize = (int)UserSize2;



            }
            else
            {
                GUILayout.Space(39);
                UserSize = (_UserSize)EditorGUILayout.EnumPopup("Select Minimum Size:", UserSize);
                MinimizeSize = (int)UserSize;


                guistyle.fontSize = 10;
                guistyle.normal.textColor = new Color(1f, 0.091f, 0f);
                GUI.Label(new Rect(155, 390, 300, 25), "*Compression Start From Minimum Size", guistyle);




            }
            GUILayout.Space(60);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Optimize!", GUILayout.Width(175), GUILayout.Height(40))) //4
            {
                SetupAndTour();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(50);
            Splitter(new Color(0.35f, 0.35f, 0.35f));



            GUIStyle greyStyle = new GUIStyle(EditorStyles.label);
            greyStyle.fontSize = 12;

            DrawLabelWithFlexibleSpace("*Please Use Simple ASTC_*x* format for Android/iPhone Platform", greyStyle, 20);


            GUILayout.FlexibleSpace();

            GUIStyle SmallStyle = new GUIStyle(EditorStyles.label);
            SmallStyle.fontSize = 13;

            DrawLabelWithFlexibleSpace("By Team Koko Zone", SmallStyle, 50);
            EditorUtility.ClearProgressBar();
        }
        catch (Exception ex)
        {

            Debug.LogError("Unexpected Error, OnGui");
        }
    }

    static void SetupAndTour()
    {
        try
        {
            _AllTexture2D.Clear();

            if (folderName != "")
            {

                object[] t = AssetDatabase.FindAssets("t:texture2D", new string[] { "Assets/" + Words[1] });


                foreach (string guid in t)
                {
                    string test = AssetDatabase.GUIDToAssetPath(guid);

                    Obb = AssetDatabase.LoadAssetAtPath(test, typeof(Texture2D)) as Texture2D;

                    if (Obb.width > Obb.height)
                    {
                        Textursize = Obb.width;
                    }
                    else
                    {
                        Textursize = Obb.height;
                    }
                    if (toggleTextField == true)
                    {
                        if (Textursize >= MinimizeSize && Textursize <= MaximumSize)
                        {

                            _AllTexture2D.Add(AssetDatabase.LoadAssetAtPath(test, typeof(Texture2D)) as Texture2D);
                        }
                    }
                    else
                    {

                        if (Textursize >= MinimizeSize)
                        {
                            _AllTexture2D.Add(AssetDatabase.LoadAssetAtPath(test, typeof(Texture2D)) as Texture2D);
                        }

                    }
                }

            }
            else
            {
                Debug.LogError("Folder Name is Mixing !!");
            }


            InfoGet();

        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Setup and Tour");
        }
    }

    static void InfoGet()
    {
        if (_AllTexture2D.Count > 0)
        {

            Optimizer.Work(PlatformName.ToString(), _TextureFormat);

        }
        else
        {
            Debug.LogError("No Files Found! Please Try Again...");
        }
    }

    private static void DrawLabelWithFlexibleSpace(string text, GUIStyle style, int height)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text, style, new GUILayoutOption[] { GUILayout.Height(height) });
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }



    public static void Splitter(Color rgb, float thickness = 1, int margin = 0)
    {
        GUIStyle splitter = new GUIStyle();
        splitter.normal.background = EditorGUIUtility.whiteTexture;
        splitter.stretchWidth = true;
        splitter.margin = new RectOffset(margin, margin, 7, 7);

        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

        if (Event.current.type == EventType.Repaint)
        {
            Color restoreColor = GUI.color;
            GUI.color = rgb;
            splitter.Draw(position, false, false, false, false);
            GUI.color = restoreColor;
        }
    }

    string[] SplitingAddress()
    {

        string data = folderName;

        return folderName.Split(new string[] { "Assets" }, StringSplitOptions.None);



    }

}


public class Optimizer : MonoBehaviour
{
    static Texture2D _Texture;
    public static void Work(string Platform, TextureImporterFormat op)
    {


        for (int i = 0; i < SOMenu._AllTexture2D.Count; i++)
        {
            try
            {
                int size;

                _Texture = SOMenu._AllTexture2D[i];

                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(SOMenu._AllTexture2D[i]));

                if (SOMenu._AllTexture2D[i].width > SOMenu._AllTexture2D[i].height)
                {
                    size = SOMenu._AllTexture2D[i].width;
                }
                else
                {
                    size = SOMenu._AllTexture2D[i].height;
                }

                EditorUtility.DisplayProgressBar("Please Wait...", "Converting Textures : " + i + " Out Of " + SOMenu._AllTexture2D.Count, SOMenu._AllTexture2D.Count);

                //  importer.textureType = TextureImporterType.Sprite;

                importer.SetPlatformTextureSettings(Platform, size, op, 100, false);

                //importer.spriteImportMode = SpMode;

                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();

            }
            catch (System.Exception ex)
            {
                Debug.LogError(_Texture.name + " is not valid for this compression.Change the this texture folder and Try Again.");
                Debug.Log(ex);
            }
        }

        EditorUtility.ClearProgressBar();
        SOMenu._AllTexture2D.Clear();
        System.GC.Collect();

        Debug.Log("Successfuly Converted");



    }
}




