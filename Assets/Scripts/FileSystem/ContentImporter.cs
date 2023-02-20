using AnKuchen.KuchenLayout;
using AnKuchen.Map;
using Cysharp.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Path = System.IO.Path;

/// <summary>
/// ファイルをコピーする
/// </summary>
public class ContentImporter : MonoBehaviour
{
    [SerializeField] UICache root;
    ContentManager22UiElements ui;

    string contentPath = "";

    string[] files;
    string currentPath;

    void Start()
    {
        contentPath = $"{Application.dataPath}/{GM.db.contentPath}";
        currentPath = contentPath;

        ui = new ContentManager22UiElements(root);
        var input = root.Get<TMP_InputField>("DirectoryInputField");
        input.onEndEdit.AddListener(OnDirectoryChanged);
        input.text = currentPath;
    }

    void OnDirectoryChanged(string input)
    {
        currentPath = input;
        if (!Directory.Exists(currentPath)) return;

        // ディレクトリ内のファイル一覧を取得
        files = Directory.GetFiles(currentPath);
        using (var editor = ui.ItemList.Edit())
        {
            // ファイル一覧を表示
            int i = 0;
            foreach (string file in files)
            {
                var i1 = i;
                var button = editor.Create();
                button.Text.text = Path.GetFileName(file);
                button.Button.onClick.AddListener(() => OnSubmit(i1));
                i++;
            }
        }
    }

    void OnSubmit(int i)
    {
        // ファイルをコピーする
        string source = files[i];
        string destination = $"{contentPath}/{Path.GetFileName(source)}";

        if (File.Exists(destination)) return;

        File.Copy(source, destination);
    }
}

public class ContentManager22UiElements : IMappedObject
{
    public IMapper Mapper { get; private set; }
    public GameObject Root { get; private set; }
    public Layout<FileButton46UiElements> ItemList { get; private set; }

    public ContentManager22UiElements() { }
    public ContentManager22UiElements(IMapper mapper) { Initialize(mapper); }


    public void Initialize(IMapper mapper)
    {
        Mapper = mapper;
        Root = mapper.Get();
        ItemList = new Layout<FileButton46UiElements>(mapper.GetChild<FileButton46UiElements>("FileButton [4:6]"));
    }
}

public class FileButton46UiElements : IMappedObject
{
    public IMapper Mapper { get; private set; }
    public Button Button { get; private set; }
    public TMP_Text Text { get; private set; }
    public GameObject Root { get; private set; }

    public FileButton46UiElements() { }
    public FileButton46UiElements(IMapper mapper) { Initialize(mapper); }

    public void Initialize(IMapper mapper)
    {
        Mapper = mapper;
        Root = mapper.Get();
        Button = mapper.Get<Button>();
        Text = mapper.Get<TMP_Text>("FileText");
    }
}
