using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Tilemaps;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PrefabBrush))]
public class PrefabBrushEditor : GridBrushEditor
{
    public VisualTreeAsset tree;

    private PrefabBrush prefabBrush;

    public override GameObject[] validTargets
    {
        get
        {
            StageHandle currentStageHandle = StageUtility.GetCurrentStageHandle();
            var results = currentStageHandle.FindComponentsOfType<GridLayout>().Where(x =>
            {
                GameObject gameObject;
                return (gameObject = x.gameObject).scene.isLoaded
                       && gameObject.activeInHierarchy;
            }).Select(x => x.gameObject);

            return results.ToArray();
        }
    }

    protected override void OnEnable()
    {
        prefabBrush = target as PrefabBrush;

        base.OnEnable();
    }

    public override VisualElement CreateInspectorGUI()
    {
        prefabBrush.TilemapHolder.Init(GameObject.Find("LevelGrid").GetComponent<Grid>());

        var root = new VisualElement();
        tree.CloneTree(root);

        var label = new Label($"Palette: {prefabBrush.paletteIndex} Prefab: {prefabBrush.prefabIndex} prefabBrush.map: {prefabBrush.TilemapHolder.Tilemap.tilemap.Count}");
        root.Add(label);

        var sp = serializedObject.FindProperty("prefabPalettes");
        var sf = new PropertyField(sp);
        root.Add(sf);

        if (prefabBrush.PalettesCount == 0) return root;

        for (int i = 0; i < prefabBrush.PalettesCount; i++)
        {
            var palette = prefabBrush.prefabPalettes[i];

            if (palette == null) continue;

            var paletteUI = new VisualElement();
            paletteUI.AddToClassList("palette");

            var foldout = new Foldout() { viewDataKey = "paletteUI", text = palette.name };
            foldout.AddToClassList("foldout");
            foldout.Add(paletteUI);
            root.Add(foldout);

            var prefabs = palette.prefabs;

            for (int j = 0; j < prefabs.Length; j++)
            {
                if (prefabs[j] == null) continue;

                var button = new Button
                {
                    text = prefabs[j].name
                };

                var paletteIndex = i;
                var prefabIndex = j;

                button.clicked += () =>
                {
                    prefabBrush.paletteIndex = paletteIndex;
                    prefabBrush.prefabIndex = prefabIndex;
                    label.text = $"Palette: {prefabBrush.CurrentPalette.name} Prefab: {prefabBrush.CurrentPalette.prefabs[prefabIndex].name}";
                };

                button.AddToClassList("button");

                paletteUI.Add(button);
            }
        }

        return root;
    }
}