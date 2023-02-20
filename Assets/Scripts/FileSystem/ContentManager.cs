using AnKuchen.KuchenLayout;
using AnKuchen.Map;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������Ă���R���e���c���琢�E�ɃI�u�W�F�N�g��z�u����UI
/// </summary>
public class ContentManager : MonoBehaviour
{
    [SerializeField] UICache root;
    ContentManagerUiElements ui;
    private string contentPath;
    private string[] files;

    void Start()
    {
        ui = new ContentManagerUiElements(root);
        contentPath = $"{Application.dataPath}/{GM.db.contentPath}";
        Show();
    }

    void Show()
    {
        ui.ui.active = true;
        // �f�B���N�g�����̃t�@�C���ꗗ���擾
        files = Directory.GetFiles(contentPath);
        using (var editor = ui.ItemList.Edit())
        {
            // �t�@�C���ꗗ��\��
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
        // �R���e���c�����[���h�ɐ�������
        string source = files[i];
        GM.Msg("GenerateObj", source);

        // ���������I�u�W�F�N�g��ݒ�t�@�C���ɕۑ�����

    }
}

public class ContentManagerUiElements : IMappedObject
{
    public IMapper Mapper { get; private set; }
    public UI_ShowCloseFade ui { get; private set; }
    public GameObject Root { get; private set; }
    public Button FileButton { get; private set; }
    public Layout<FileButtonUiElements> ItemList { get; private set; }

    public ContentManagerUiElements() { }
    public ContentManagerUiElements(IMapper mapper) { Initialize(mapper); }

    public void Initialize(IMapper mapper)
    {
        Mapper = mapper;
        Root = mapper.Get();
        FileButton = mapper.Get<Button>("FileButton");
        ItemList = new Layout<FileButtonUiElements>(mapper.GetChild<FileButtonUiElements>("FileButton"));
        ui = mapper.Get<UI_ShowCloseFade>();
    }
}

public class FileButtonUiElements : IMappedObject
{
    public IMapper Mapper { get; private set; }
    public GameObject Root { get; private set; }
    public Button Button { get; private set; }
    public TMP_Text Text { get; private set; }

    public FileButtonUiElements() { }
    public FileButtonUiElements(IMapper mapper) { Initialize(mapper); }

    public void Initialize(IMapper mapper)
    {
        Mapper = mapper;
        Root = mapper.Get();
        Button = mapper.Get<Button>();
        Text = mapper.Get<TMP_Text>("FileText");
    }
}
