## Decentralized Metaverse Project
This project creates a decentralized metaverse.

## Required Assets
Please import the following assets.
- [Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321)
- [Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547)

## How to set up a world
./World/`world name`
`world name`.yaml
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
Parent-child relationship can be set by setting the id at parent.

## Available file types
| type | tag |
| --- | --- |
| png, jpg | image |
| mp4 | video |
| mp3 | audio |
| glb | object |
| ys | script |

| type | tag |
| --- | --- |
| empty object | empty |
| other world | world |

## Script
./World/`world name`/`script name`.ys

### Examples
```yaml
~~~省略~~~
  0002:
    file: '<script name>.ys'
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

### Message indication

```jsx
say "Hello!"
say "Name" "Hello!"
//分行符號 「+」
say "Name" "Hello!+Where are you from?"
```

### Variable

```jsx
// String
name = "村人A"

// Integer
money = 100

// Boolean
flagA
flagA = true
flagA = false
```

### Conditional branch

```jsx
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

### Choice indication

```jsx
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

### Text indication
```jsx
title "maintitle" "subtitle"
msg "text"
```

### Time Control

```jsx

//停止
//wait {秒数}
wait 2.6
```

### Object active/inactive
```jsx
active <id> <true or false>

is_active <id>
if ret == 1
   // active
else
   // inactive
end
```
**Examples**
```jsx
is_active 330699wfe556wefaw6224e854fb1
if ret == 0
   active 330699wfe556wefaw6224e854fb1 true
else
   active 330699wfe556wefaw6224e854fb1 false
end
```

### Setting up object parent-child relationships
The child object will be relative to the parent object
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
