## このプロジェクトについて
多人数接続が可能なメタバースです。

アバターはVRMファイルを読み込みます。

ワールドにおいては、画像、動画等をランタイムで読み込むことができます。

また、スクリプトを記述して、メッセージや選択肢を表示することも可能です。

## 必要なアセット
ご使用の際は、以下のアセットをインポートしてください。
- [Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321)
- [Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547)

## 使用アセット
- [UniVRM](https://github.com/vrm-c/UniVRM)


## ワールド設定例
./World/ワールド名
ワールド名.yaml
```yaml
version: 1.0.0
updated: 2022-11-25-19-15
type: world
id: a55758ea31cd6770c449346dd8d13aa
name: World1
objects:  
  0001:
    file: 'fog.png'
    type: image
    position: "-5,5,20"
    rotation: "0,0,0"
    scale: "10,10,10"
    move: false
    gravity: false
    parent: ''
    child: ''
    custom: ''  
```
parentにidを入れることで親子関係の設定が可能

## 読み込み可能なファイル
| 種類 | type |
| --- | --- |
| png, jpg | image |
| mp4 | video |
| mp3 | audio |
| glb | object |
| ys | script |
| 空のオブジェクト | empty |
| 他のワールド | world |

## スクリプト
./World/ワールド名/スクリプト名.ys

### ワールド設定例
```yaml
~~~省略~~~
  0002:
    file: 'スクリプト名.ys'
    type: script
    position: "0,0,-0"
    rotation: "0,0,0"
    scale: "1.0,1.0,1.0"
    move: false
    gravity: false
    parent: ''
    child: ''
    custom: '{"exe": "key", "hint": "F:話す"}'
~~~省略~~~
```

### メッセージ表示

```jsx
say "Hello!"
say "Name" "Hello!"
//分行符號 「+」
say "Name" "Hello!+Where are you from?"
```

### 変数

```jsx
// 文字列型変数
name = "村人A"

// 整数型変数
money = 100

// 論理型変数
flagA
flagA = true
flagA = false
```

### 条件分岐

```jsx
// 条件分岐
//==, <, >, <=, >=, and, or
if flagA
    say "村人A" "これあげる！"

if gold >= 90
    say "90以上"
else if gold >= 10
    say "10以上"
else if 0 <= gold and gold < 10
    say "0以上10未満"
else
    say "0未満"
end

```

### 選択肢

```jsx
// 条件分岐
yesno
select "title" : "A" "B"
//or select "A" "B"
if ret == 0
    say "...A..."
else if ret == 1
    say "...B..."
else if ret == 2
    say "...C..."
end
```

### テキスト表示
```jsx
title "maintitle" "subtitle"
msg "text"
```

### 時間制御

```jsx

//停止
//wait {秒数}
wait 2.6
```

### オブジェクト表示・非表示機能
```jsx
// オブジェクト表示・非表示
active <id> <true or false>

// オブジェクトが表示されているか
is_active <id>
if ret == 0
   // 非表示になっている
else
   // 表示している
end
```
使用例
```jsx
is_active 330699wfe556wefaw6224e854fb1
if ret == 0
   active 330699wfe556wefaw6224e854fb1 true
else
   active 330699wfe556wefaw6224e854fb1 false
end
```

### オブジェクト親子関係の設定
子オブジェクトは、親オブジェクトの相対座標になります
```yaml
  330699wfe5566224ede8854fb1:
    file: logo3.png
    type: image
    position: "6,-2,0"
    rotation: "0,0,0"
    scale: "1,1,1"
    <省略>
    parent: ''
  330699wfe5566224e854fb1:
    file: text0.png
    type: image
    position: "6,-2,0"
    rotation: "0,0,0"
    scale: "1,1,1"
    <省略>
    parent: '330699wfe5566224ede8854fb1'
```
